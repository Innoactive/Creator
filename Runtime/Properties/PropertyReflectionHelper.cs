using System;
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
                UniqueNameReference reference = ReflectionUtils.GetValueFromPropertyOrField(data, info) as UniqueNameReference;

                if (reference == null || string.IsNullOrEmpty(reference.UniqueName))
                {
                    return;
                }

                if (RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsName(reference.UniqueName) == false)
                {
                    return;
                }

                Type refType = ReflectionUtils
                    .GetConcreteImplementationsOf(reference.GetReferenceType())
                    .Where(typeof(LockableProperty).IsAssignableFrom)
                    .FirstOrDefault(type => Enumerable.All(type.Assembly.GetReferencedAssemblies(),
                        assemblyName => UnitTestChecker.IsUnitTesting || (assemblyName.Name != "UnityEditor" && assemblyName.Name != "nunit.framework")));

                if (refType != null)
                {
                    IEnumerable<Type> types = GetLockableDependenciesFrom(refType);
                    foreach (Type type in types)
                    {
                        LockableProperty property = GetProperty(reference, type);
                        if (property != null)
                        {
                            result.Add(new LockablePropertyData(property));
                        }
                    }
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
                    return prop as LockableProperty;
                }
            }
            Debug.LogWarningFormat("Could not find fitting {0} type in SceneObject {1}", type.Name, reference.UniqueName);
            return null;
        }

        private static IEnumerable<Type> GetLockableDependenciesFrom(Type trainingProperty)
        {
            List<Type> dependencies = new List<Type>();
            IEnumerable<Type> requiredComponents = trainingProperty.GetCustomAttributes(typeof(RequireComponent), false)
                .Cast<RequireComponent>()
                .SelectMany(rq => new []{rq.m_Type0, rq.m_Type1, rq.m_Type2});

            foreach (Type requiredComponent in requiredComponents)
            {
                if (requiredComponent != null && requiredComponent.IsSubclassOf(typeof(LockableProperty)))
                {
                    dependencies.AddRange(GetLockableDependenciesFrom(requiredComponent));
                }
            }

            dependencies.Add(trainingProperty);
            return new HashSet<Type>(dependencies);
        }
    }
}
