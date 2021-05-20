using System.Linq;
using VPG.Core.Serialization;
using VPG.Core.Utils;
using VPG.Unity;
using VPG.Core.Serialization.NewtonsoftJson;
using VPG.Editor;
using NUnit.Framework;
using UnityEngine;

namespace VPG.Tests.Utils
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
            UnitTestChecker.IsUnitTesting = false;
        }
    }
}
