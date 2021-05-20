﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using VPG.Core;
using VPG.Core.Attributes;
using VPG.Editor.UI.Drawers;
using UnityEditor.Callbacks;

namespace VPG.Editor
{
    /// <summary>
    /// Helper class for reflection.
    /// </summary>
    internal static class EditorReflectionUtils
    {
        private static readonly Dictionary<Type, IEnumerable<MemberInfo>> fieldAndPropertiesToDrawCache = new Dictionary<Type, IEnumerable<MemberInfo>>();
        private static readonly Dictionary<MemberInfo, HashSet<Attribute>> membersAttributesCache = new Dictionary<MemberInfo, HashSet<Attribute>>();
        private static readonly Dictionary<Type, HashSet<Attribute>> classAttributesCache = new Dictionary<Type, HashSet<Attribute>>();

        [DidReloadScripts]
        private static void OnScriptsReload()
        {
            fieldAndPropertiesToDrawCache.Clear();
            membersAttributesCache.Clear();
            classAttributesCache.Clear();
        }

        public static IEnumerable<Attribute> GetAttributes(this Type type, bool isInherited)
        {
            return type.GetAttributes<Attribute>(isInherited);
        }

        public static IEnumerable<Attribute> GetAttributes(this MemberInfo memberInfo, bool isInherited)
        {
            return memberInfo.GetAttributes<Attribute>(isInherited);
        }

        public static IEnumerable<T> GetAttributes<T>(Type type, bool isInherited) where T : Attribute
        {
            if (classAttributesCache.ContainsKey(type))
            {
                return classAttributesCache[type].OfType<T>();
            }

            classAttributesCache[type] = new HashSet<Attribute>(type.GetCustomAttributes(isInherited).Cast<Attribute>());

            return classAttributesCache[type].OfType<T>();
        }

        public static IEnumerable<T> GetAttributes<T>(this MemberInfo memberInfo, bool isInherited) where T : Attribute
        {
            if (membersAttributesCache.ContainsKey(memberInfo))
            {
                return membersAttributesCache[memberInfo].OfType<T>();
            }

            membersAttributesCache[memberInfo] = new HashSet<Attribute>(memberInfo.GetCustomAttributes(isInherited).Cast<Attribute>());

            return membersAttributesCache[memberInfo].OfType<T>();
        }

        public static string GetDisplayName(this MemberInfo memberInfo)
        {
            DisplayNameAttribute nameAttribute = memberInfo.GetCustomAttribute<DisplayNameAttribute>();
            return nameAttribute != null ? nameAttribute.Name : memberInfo.Name;
        }

        /// <summary>
        /// Checks if assembly exists in current domain space.
        /// </summary>
        public static bool AssemblyExists(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().Any(assembly => assembly.GetName().Name == name);
        }

        /// <summary>
        /// Checks if class exists in given assembly.
        /// </summary>
        public static bool ClassExists(string assemblyName, string className)
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == assemblyName);

            if (assembly == null)
            {
                return false;
            }

            return assembly.GetType(className) != null;
        }

        /// <summary>
        /// Returns all fields and properties of given object as <see cref="MemberInfo"/>.
        /// </summary>
        public static IEnumerable<MemberInfo> GetAllFieldsAndProperties(object value)
        {
            IEnumerable<MemberInfo> result = new List<MemberInfo>();

            if (value == null)
            {
                return result;
            }

            Type type = value.GetType();
            while (type != null)
            {
                result = result.Concat(type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(propertyInfo => propertyInfo.FieldType.GetInterfaces().Contains(typeof(IMetadata)) == false));

                type = type.BaseType;
            }

            type = value.GetType();
            while (type != null)
            {
                result = result.Concat(type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(propertyInfo => propertyInfo.PropertyType.GetInterfaces().Contains(typeof(IMetadata)) == false));

                type = type.BaseType;
            }

            return result;
        }

        /// <summary>
        /// Returns all properties and fields of the object that have to be drawn by training drawers.
        /// </summary>
        public static IEnumerable<MemberInfo> GetFieldsAndPropertiesToDraw(object value)
        {
            IEnumerable<MemberInfo> result = new List<MemberInfo>();

            if (value == null)
            {
                return result;
            }

            Type type = value.GetType();
            if (fieldAndPropertiesToDrawCache.ContainsKey(type))
            {
                return fieldAndPropertiesToDrawCache[type];
            }

            HashSet<MethodInfo> getOverloads = new HashSet<MethodInfo>();

            IEnumerable<PropertyInfo> properties = new List<PropertyInfo>();

            // Get all fields which do not have HideInInspectorAttribute and have DataMember attribute,
            // which aren't IMetadata fields.
            // Do it all the way up in the inheritance tree, skipping properties which were defined in more concrete implementation of the class.
            while (type != null)
            {
                result = result.Concat(type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .GroupBy(propertyInfo => propertyInfo.Name)
                    .SelectMany(groupByName => groupByName.GroupBy(propertyInfo => propertyInfo.DeclaringType).Select(groupByDeclaringType => groupByDeclaringType.First()))
                    .Where(fieldInfo => fieldInfo.FieldType.GetInterfaces().Contains(typeof(IMetadata)) == false)
                    .Cast<MemberInfo>());

                type = type.BaseType;
            }

            type = value.GetType();

            while (type != null)
            {
                properties = properties.Concat(type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(propertyInfo => propertyInfo.CanRead && propertyInfo.CanWrite)
                    .Where(propertyInfo => propertyInfo.PropertyType.GetInterfaces().Contains(typeof(IMetadata)) == false));

                type = type.BaseType;
            }

            result = result.Concat(properties.GroupBy(propertyInfo => propertyInfo.Name)
                .SelectMany(groupByName => groupByName.GroupBy(propertyInfo => propertyInfo.DeclaringType).Select(groupByDeclaringType => groupByDeclaringType.First()))
                .Where(propertyInfo =>
                {
                    MethodInfo get = propertyInfo.GetGetMethod(true);
                    if (getOverloads.Contains(get))
                    {
                        return false;
                    }

                    MethodInfo baseGet = get.GetBaseDefinition();

                    getOverloads.Add(baseGet);
                    return true;
                })
                .Cast<MemberInfo>());

            type = value.GetType();

            fieldAndPropertiesToDrawCache[type] = result.Where(memberInfo => memberInfo.GetAttributes<HideInTrainingInspectorAttribute>(true).Any() == false)
                .Where(memberInfo => memberInfo.GetAttributes<DataMemberAttribute>(true).Any() && DrawerLocator.GetDrawerForMember(memberInfo, value) != null)
                .OrderBy(memberInfo =>
                {
                    DrawingPriorityAttribute priorityAttribute = memberInfo.GetAttributes(true).FirstOrDefault(attribute => attribute is DrawingPriorityAttribute) as DrawingPriorityAttribute;

                    if (priorityAttribute == null)
                    {
                        return int.MaxValue;
                    }

                    return priorityAttribute.Priority;
                }).ToList();

            return fieldAndPropertiesToDrawCache[type];
        }
    }
}
