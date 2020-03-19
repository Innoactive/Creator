using Innoactive.Hub.Threading;
using System.Linq;
using Innoactive.Hub.Training.Unity.Utils;
using Innoactive.Hub.Training.Utils;
using Innoactive.Hub.Training.Utils.Serialization;
using Innoactive.Hub.Training.Utils.Serialization.NewtonsoftJson;
using Innoactive.Hub.Unity;
using NUnit.Framework;
using UnityEngine;

namespace Innoactive.Creator.Core.Tests.Utils
{
    public abstract class RuntimeTests
    {
        protected ICourseSerializer Serializer { get; } = new NewtonsoftJsonCourseSerializer();

        [SetUp]
        public virtual void SetUp()
        {
            SceneUtils.SetupTrainingConfiguration();
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
