using VPG.Core;
using VPG.Core.Behaviors;
using VPG.Core.Properties;
using VPG.Core.SceneObjects;

namespace VPG.Tests.Utils.Mocks
{
    /// <summary>
    /// Helper behavior for testing that has a reference object.
    /// </summary>
    public class ObjectReferenceBehaviorMock : Behavior<ObjectReferenceBehaviorMock.EntityData>
    {
        public class EntityData : IBehaviorData
        {
            /// <summary>
            /// Target scene object to be referenced.
            /// </summary>
            public ScenePropertyReference<ISceneObjectProperty> ReferenceObject { get; set; }

            public Metadata Metadata { get; set; }
            public string Name { get; set; }
        }

        public ObjectReferenceBehaviorMock(string sceneObjectName)
        {
            Data.ReferenceObject = new ScenePropertyReference<ISceneObjectProperty>(sceneObjectName);
        }
    }
}
