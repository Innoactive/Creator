#if UNITY_EDITOR
using Innoactive.Hub.Threading;
using System.Linq;
using Innoactive.Hub.Training.Unity.Utils;
using Innoactive.Hub.Training.Utils;
using Innoactive.Hub.Training.Utils.Serialization;
using NUnit.Framework;
using UnityEngine;

namespace Innoactive.Hub.Unity.Tests.Training
{
    public abstract class RuntimeTests
    {
        protected ITrainingSerializer Serializer { get; } = new NewtonsoftJsonSerializer();

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

#endif
