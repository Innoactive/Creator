using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VPG.Creator.Core.Attributes;
using VPG.Creator.Core.Utils;

namespace VPG.CreatorEditor.UI.Drawers
{
    internal static class DrawerLocator
    {
        private static readonly Dictionary<Type, ITrainingDrawer> allDrawers;
        private static readonly Dictionary<Type, ITrainingDrawer> defaultDrawers;
        private static readonly Dictionary<Type, ITrainingDrawer> instantiatorDrawers;

        static DrawerLocator()
        {
            defaultDrawers = new Dictionary<Type, ITrainingDrawer>();
            foreach (Type drawerType in ReflectionUtils.GetConcreteImplementationsOf<ITrainingDrawer>())
            {
                foreach (DefaultTrainingDrawerAttribute attribute in drawerType.GetAttributes<DefaultTrainingDrawerAttribute>(true))
                {
                    defaultDrawers[attribute.DrawableType] = (ITrainingDrawer) ReflectionUtils.CreateInstanceOfType(drawerType);
                }
            }

            allDrawers = new Dictionary<Type, ITrainingDrawer>();
            foreach (Type drawerType in ReflectionUtils.GetConcreteImplementationsOf<ITrainingDrawer>())
            {
                allDrawers[drawerType] = (ITrainingDrawer) ReflectionUtils.CreateInstanceOfType(drawerType);
            }

            instantiatorDrawers = new Dictionary<Type, ITrainingDrawer>();
            foreach (Type drawerType in ReflectionUtils.GetConcreteImplementationsOf<ITrainingDrawer>().Where(t => t.GetAttributes<InstantiatorTrainingDrawerAttribute>(true).Any()))
            {
                foreach (InstantiatorTrainingDrawerAttribute attribute in drawerType.GetAttributes<InstantiatorTrainingDrawerAttribute>(true))
                {
                    instantiatorDrawers[attribute.Type] = (ITrainingDrawer) ReflectionUtils.CreateInstanceOfType(drawerType);
                }
            }
        }

        /// <summary>
        /// Returns required drawer for a given object member.
        /// If the member has a <see cref="UsesSpecificTrainingDrawerAttribute"/>, returns the drawer defined in that attribute.
        /// If the member contains non-null value, tries to find drawers from most concrete type definition (which is actual value's type) and up through inheritance tree.
        /// If member's value is null, does the same starting from the property declared type instead.
        /// If drawer is still not found, checks drawers for interfaces.
        /// If everything else fails, returns the drawer for System.Object.
        /// </summary>
        /// <param name="memberInfo">Reflection information about the member for which drawer is needed.</param>
        /// <param name="owner">Object to which this member belongs to.</param>
        /// <returns>Returns suitable training drawer. Returns null only if the member is not a property or field, or the specified custom drawer is not found.</returns>
        public static ITrainingDrawer GetDrawerForMember(MemberInfo memberInfo, object owner)
        {
            if (ReflectionUtils.IsProperty(memberInfo) == false && ReflectionUtils.IsField(memberInfo) == false)
            {
                return null;
            }

            if (HasCustomDrawer(memberInfo))
            {
                return GetCustomDrawer(memberInfo);
            }

            object actualValue = GetValue(memberInfo, owner);
            return GetDrawerForValue(actualValue, ReflectionUtils.GetDeclaredTypeOfPropertyOrField(memberInfo));
        }

        /// <summary>
        /// Returns required drawer for a given value.
        /// If the value is not null, try to find drawers from most concrete type definition (which is actual value's type) and up through inheritance tree.
        /// If the value is null, does the same starting from the member declaring type instead.
        /// If drawer is still not found, checks drawers for interfaces.
        /// If everything else fails, returns the drawer for System.Object.
        /// </summary>
        /// <param name="value">Value to get drawer for.</param>
        /// <param name="declaredType">Declaring type of the class member that contains the value.</param>
        /// <returns>Returns suitable training drawer.</returns>
        public static ITrainingDrawer GetDrawerForValue(object value, Type declaredType)
        {
            if (value == null)
            {
                return GetDrawerForType(declaredType);
            }

            return GetDrawerForType(value.GetType());
        }

        /// <summary>
        /// Get a drawer for a view that creates a new instance of <paramref name="declaredType"/>
        /// </summary>
        public static ITrainingDrawer GetInstantiatorDrawer(Type declaredType)
        {
            Type currentType = declaredType;
            // Get drawer for type, checking from the most concrete type definition to a most abstract one.
            while (currentType.IsInterface == false && currentType != typeof(object))
            {
                if (instantiatorDrawers.ContainsKey(currentType))
                {
                    return instantiatorDrawers[currentType];
                }

                currentType = currentType.BaseType;
            }

            if (declaredType.IsInterface && instantiatorDrawers.ContainsKey(declaredType))
            {
                return instantiatorDrawers[declaredType];
            }

            return declaredType.GetInterfaces().Where(i => instantiatorDrawers.ContainsKey(i)).Select(i => instantiatorDrawers[i]).FirstOrDefault(t => t != null);
        }

        private static ITrainingDrawer GetDrawerForType(Type type)
        {
            Type currentType = type;
            // Get drawer for type, checking from the most concrete type definition to a most abstract one.
            while (currentType.IsInterface == false && currentType != typeof(object))
            {
                ITrainingDrawer concreteTypeDrawer = GetTypeDrawer(currentType);
                if (concreteTypeDrawer != null)
                {
                    return concreteTypeDrawer;
                }

                currentType = currentType.BaseType;
            }

            ITrainingDrawer interfaceDrawer = null;
            if (type.IsInterface)
            {
                interfaceDrawer = GetTypeDrawer(type);
            }

            if (interfaceDrawer == null)
            {
                interfaceDrawer = GetInheritedInterfaceDrawer(type);
            }

            if (interfaceDrawer != null)
            {
                return interfaceDrawer;
            }

            return GetObjectDrawer();
        }

        private static object GetValue(MemberInfo memberInfo, object owner)
        {
            PropertyInfo propertyInfo = memberInfo as PropertyInfo;
            FieldInfo fieldInfo = memberInfo as FieldInfo;

            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(owner, null);
            }

            if (fieldInfo != null)
            {
                return fieldInfo.GetValue(owner);
            }

            return null;
        }

        private static ITrainingDrawer GetTypeDrawer(Type type)
        {
            if (defaultDrawers.ContainsKey(type))
            {
                return defaultDrawers[type];
            }

            return null;
        }

        private static ITrainingDrawer GetObjectDrawer()
        {
            return GetTypeDrawer(typeof(object));
        }

        private static ITrainingDrawer GetInheritedInterfaceDrawer(Type type)
        {
            return type.GetInterfaces().Select(GetTypeDrawer).FirstOrDefault(t => t != null);
        }

        private static bool HasCustomDrawer(MemberInfo memberInfo)
        {
            return memberInfo.GetAttributes<UsesSpecificTrainingDrawerAttribute>(true).Any();
        }

        private static ITrainingDrawer GetCustomDrawer(MemberInfo memberInfo)
        {
            UsesSpecificTrainingDrawerAttribute attribute = memberInfo.GetAttributes<UsesSpecificTrainingDrawerAttribute>(true).First();

            // ReSharper disable once PossibleNullReferenceException
            string drawerTypeName = attribute.DrawerType;
            string[] splittedName = drawerTypeName.Split('.').Reverse().ToArray();

            // Drawer name can be partially qualified
            Type drawerType = allDrawers.Keys.FirstOrDefault(key =>
            {
                string[] splittedKey = key.FullName.Split('.').Reverse().ToArray();

                if (splittedName.Length > splittedKey.Length)
                {
                    return false;
                }

                for (int i = 0; i < splittedName.Length; i++)
                {
                    if (splittedKey[i] != splittedName[i])
                    {
                        return false;
                    }
                }

                return true;
            });
            if (drawerType == null)
            {
                return null;
            }

            return allDrawers[drawerType];
        }
    }
}
