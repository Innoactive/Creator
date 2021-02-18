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
using Innoactive.Creator.Unity;
using UnityEngine;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// Helper class which provides methods to extract <see cref="LockablePropertyData"/> from different course entities.
    /// </summary>
    internal static class PropertyReflectionHelper
    {
        public static List<LockablePropertyData> ExtractLockablePropertiesFromStep(IStepData data)
        {

            List<LockablePropertyData> result = new List<LockablePropertyData>();

            if (data == null)
            {
                return result;
            }

            foreach (ITransition transition in data.Transitions.Data.Transitions)
            {
                foreach (ICondition condition in transition.Data.Conditions)
                {
                    result.AddRange(ExtractLockablePropertiesFromConditions(condition.Data));
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts all <see cref="LockableProperties"/> from given condition.
        /// </summary>
        /// <param name="data">Condition to be used for extraction</param>
        /// <param name="checkRequiredComponentsToo">if true the [RequiredComponents] will be checked and added too.</param>
        public static List<ISceneObjectProperty> ExtractPropertiesFromConditions<T>(IConditionData data, bool checkRequiredComponentsToo = true)
        {
            List<ISceneObjectProperty> result = new List<ISceneObjectProperty>();

            List<MemberInfo> memberInfo = GetAllPropertyReferencesFromCondition(data);
            memberInfo.ForEach(info =>
            {
                UniqueNameReference reference = ReflectionUtils.GetValueFromPropertyOrField(data, info) as UniqueNameReference;

                if (reference == null || string.IsNullOrEmpty(reference.UniqueName))
                {
                    return;
                }

                if (RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsName(reference.UniqueName) == false)
                {
                    return;
                }

                IEnumerable<Type> refs = ExtractFittingPropertyTypeFrom<ISceneObjectProperty>(reference);

                Type refType = refs.FirstOrDefault();
                if (refType != null)
                {
                    IEnumerable<Type> types = new[] {refType};
                    if (checkRequiredComponentsToo)
                    {
                        types = GetDependenciesFrom(refType);
                    }

                    foreach (Type type in types)
                    {
                        ISceneObjectProperty property = GetFittingPropertyFromReference<ISceneObjectProperty>(reference, type);
                        if (property != null)
                        {
                            result.Add(property);
                        }
                    }
                }
            });

            return result;
        }

        /// <summary>
        /// Extracts all <see cref="LockableProperties"/> from given condition.
        /// </summary>
        /// <param name="data">Condition to be used for extraction</param>
        /// <param name="checkRequiredComponentsToo">if true the [RequiredComponents] will be checked and added too.</param>
        public static List<LockablePropertyData> ExtractLockablePropertiesFromConditions(IConditionData data, bool checkRequiredComponentsToo = true)
        {
            List<LockablePropertyData> result = new List<LockablePropertyData>();

            List<MemberInfo> memberInfo = GetAllPropertyReferencesFromCondition(data);
            memberInfo.ForEach(info =>
            {
                UniqueNameReference reference = ReflectionUtils.GetValueFromPropertyOrField(data, info) as UniqueNameReference;

                if (reference == null || string.IsNullOrEmpty(reference.UniqueName))
                {
                    return;
                }

                if (RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsName(reference.UniqueName) == false)
                {
                    return;
                }

                IEnumerable<Type> refs = ExtractFittingPropertyTypeFrom<LockableProperty>(reference);

                Type refType = refs.FirstOrDefault();
                if (refType != null)
                {
                    IEnumerable<Type> types = new[] {refType};
                    if (checkRequiredComponentsToo)
                    {
                        types = GetDependenciesFrom<LockableProperty>(refType);
                    }

                    foreach (Type type in types)
                    {
                        LockableProperty property = GetFittingPropertyFromReference<LockableProperty>(reference, type);
                        if (property != null)
                        {
                            result.Add(new LockablePropertyData(property));
                        }
                    }
                }
            });

            return result;
        }

        private static IEnumerable<Type> ExtractFittingPropertyTypeFrom<T>(UniqueNameReference reference) where T : ISceneObjectProperty
        {
            IEnumerable<Type> refs = ReflectionUtils.GetConcreteImplementationsOf(reference.GetReferenceType());
            refs = refs.Where(typeof(T).IsAssignableFrom);

            if (UnitTestChecker.IsUnitTesting == false)
            {
                refs = refs.Where(type => type.Assembly.GetReferencedAssemblies().All(name => name.Name != "nunit.framework"));
                refs = refs.Where(type => type.Assembly.GetReferencedAssemblies().All(name => name.Name != "UnityEditor"));
            }

            return refs;
        }

        private static List<MemberInfo> GetAllPropertyReferencesFromCondition(IConditionData conditionData)
        {
            List<MemberInfo> memberInfo = conditionData.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(info =>
                    info.PropertyType.IsConstructedGenericType && info.PropertyType.GetGenericTypeDefinition() ==
                    typeof(ScenePropertyReference<>))
                .Cast<MemberInfo>()
                .ToList();

            memberInfo.AddRange(conditionData.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(info =>
                    info.FieldType.IsConstructedGenericType && info.FieldType.GetGenericTypeDefinition() ==
                    typeof(ScenePropertyReference<>)));

            return memberInfo;
        }

        private static T GetFittingPropertyFromReference<T>(UniqueNameReference reference, Type type) where T : class, ISceneObjectProperty
        {
            ISceneObject sceneObject = RuntimeConfigurator.Configuration.SceneObjectRegistry.GetByName(reference.UniqueName);
            foreach (ISceneObjectProperty prop in sceneObject.Properties)
            {
                if (prop.GetType() == type)
                {
                    return prop as T;
                }
            }
            Debug.LogWarningFormat("Could not find fitting {0} type in SceneObject {1}", type.Name, reference.UniqueName);
            return null;
        }

        /// <summary>
        /// Get training scene properties which the given type dependence on.
        /// </summary>
        private static IEnumerable<Type> GetDependenciesFrom(Type trainingProperty)
        {
            return GetDependenciesFrom<ISceneObjectProperty>(trainingProperty);
        }

        /// <summary>
        /// Get training scene properties which the given type dependence on, which has to be a subclass of <T>
        /// </summary>
        private static IEnumerable<Type> GetDependenciesFrom<T>(Type trainingProperty) where T : ISceneObjectProperty
        {
            List<Type> dependencies = new List<Type>();
            IEnumerable<Type> requiredComponents = trainingProperty.GetCustomAttributes(typeof(RequireComponent), false)
                .Cast<RequireComponent>()
                .SelectMany(rq => new []{rq.m_Type0, rq.m_Type1, rq.m_Type2});

            foreach (Type requiredComponent in requiredComponents)
            {
                if (requiredComponent != null && requiredComponent.IsSubclassOf(typeof(T)))
                {
                    dependencies.AddRange(GetDependenciesFrom<T>(requiredComponent));
                }
            }

            dependencies.Add(trainingProperty);
            return new HashSet<Type>(dependencies);
        }
    }
}
