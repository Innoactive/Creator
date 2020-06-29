﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Properties;
using Innoactive.Creator.Core.RestrictiveEnvironment;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Utils;
using UnityEngine;

namespace Innoactive.Creator.Core
{
    public static class PropertyReflectionHelper
    {

        public static List<LockablePropertyData> ExtractLockablesFromStep(IStep step)
        {
            List<LockablePropertyData> result = new List<LockablePropertyData>();

            foreach (ITransition transition in step.Data.Transitions.Data.Transitions)
            {
                foreach (ICondition condition in transition.Data.Conditions)
                {
                    result.AddRange(ExtractLockablePropertiesFromConditions(condition.Data));
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts all scene and property references which have extend a LockableProperty.
        /// </summary>
        public static List<LockablePropertyData> ExtractLockablePropertiesFromConditions(IConditionData data)
        {
            List<MemberInfo> memberInfo = data.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(info =>
                    info.PropertyType.IsConstructedGenericType && info.PropertyType.GetGenericTypeDefinition() ==
                    typeof(ScenePropertyReference<>))
                .Cast<MemberInfo>()
                .ToList();

            memberInfo.AddRange(data.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(info =>
                    info.FieldType.IsConstructedGenericType && info.FieldType.GetGenericTypeDefinition() ==
                    typeof(ScenePropertyReference<>)));

            List<LockablePropertyData> result = new List<LockablePropertyData>();
            memberInfo.ForEach(info =>
            {
                UniqueNameReference reference =
                    (UniqueNameReference) ReflectionUtils.GetValueFromPropertyOrField(data, info);

                Type refType = ReflectionUtils
                    .GetConcreteImplementationsOf(reference.GetReferenceType())
                    .Where(typeof(LockableProperty).IsAssignableFrom)
                    .FirstOrDefault(type => Enumerable.All(type.Assembly.GetReferencedAssemblies(),
                        assemblyName => assemblyName.Name != "UnityEditor" && assemblyName.Name != "nunit.framework"));

                if (refType != null)
                {
                    result.AddRange(GetAllDependenciesFrom(refType).Select(type => new LockablePropertyData(GetProperty(reference, type))));
                }
            });

            return result;
        }

        private static LockableProperty GetProperty(UniqueNameReference reference, Type type)
        {
            ISceneObject sceneObject = RuntimeConfigurator.Configuration.SceneObjectRegistry.GetByName(reference.UniqueName);
            foreach (ISceneObjectProperty prop in sceneObject.Properties)
            {
                if (prop.GetType() == type)
                {
                    return (LockableProperty)prop;
                }
            }
            Debug.LogWarningFormat("Could not find fitting {0} type in SceneObject {1}", type.Name, reference.UniqueName);
            return null;
        }

        private static HashSet<Type> GetAllDependenciesFrom(Type trainingProperty)
        {
            HashSet<Type> dependencies = new HashSet<Type>();
            RequireComponent[] requireComponents = trainingProperty.GetCustomAttributes(typeof(RequireComponent), false) as RequireComponent[];

            if (requireComponents.Any())
            {
                foreach (RequireComponent requireComponent in requireComponents)
                {
                    AddTypeToList(requireComponent.m_Type0, ref dependencies);
                    AddTypeToList(requireComponent.m_Type1, ref dependencies);
                    AddTypeToList(requireComponent.m_Type2, ref dependencies);
                }
            }

            AddTypeToList(trainingProperty, ref dependencies);

            return dependencies;
        }

        private static void AddTypeToList(Type type, ref HashSet<Type> dependencies)
        {
            if (type != null)
            {
                dependencies.Add(type);
            }
        }
    }
}
