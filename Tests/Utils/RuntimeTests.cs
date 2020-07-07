using System.Linq;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Serialization;
using Innoactive.Creator.Core.Utils;
using Innoactive.Creator.Unity;
using Innoactive.Creator.Core.Serialization.NewtonsoftJson;
using Innoactive.CreatorEditor;
using NUnit.Framework;
using UnityEngine;

namespace Innoactive.Creator.Tests.Utils
{
    public abstract class RuntimeTests
    {
        protected ICourseSerializer Serializer { get; } = new NewtonsoftJsonCourseSerializer();

        [SetUp]
        public virtual void SetUp()
        {
            UnitTestChecker.IsUnitTesting = true;
            new RuntimeConfigurationSetup().Setup();
        }

        [TearDown]
        public virtual void TearDown()
        {
            foreach (GameObject gameObject in SceneUtils.GetActiveAndInactiveGameObjects())
            {
                if (gameObject.name == "Code-based tests runner")
                {
                    continue;
                }

                if (gameObject.GetComponents(typeof(Component)).Any(component => component.GetType().IsSubclassOfGenericDefinition(typeof(UnitySingleton<>))))
                {
                    continue;
                }

                Object.DestroyImmediate(gameObject, false);
            }

            CoroutineDispatcher.Instance.StopAllCoroutines();
        }
    }
}
