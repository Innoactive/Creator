using System.Runtime.Serialization;
using Innoactive.Hub.Training.Attributes;
using Innoactive.Hub.Training.Configuration;
using Innoactive.Hub.Training.Configuration.Modes;
using Innoactive.Hub.Training.SceneObjects;
using Innoactive.Hub.Training.SceneObjects.Properties;
using Innoactive.Hub.Training.Utils;
using Innoactive.Hub.Unity;
using Newtonsoft.Json;
using UnityEngine;

namespace Innoactive.Hub.Training.Behaviors
{
    /// <summary>
    /// Behavior that highlights the target <see cref="ISceneObject"/> with the specified color until the behavior is being deactivated.
    /// </summary>
    [DataContract(IsReference = true)]
    public class HighlightObjectBehavior : Behavior<HighlightObjectBehavior.EntityData>
    {
        [DisplayName("Highlight Object")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            /// <summary>
            /// <see cref="ModeParameter"/> of the highlight color.
            /// Training modes can change the highlight color.
            /// </summary>
            public ModeParameter<Color> CustomHighlightColor { get; set; }

            /// <summary>
            /// Highlight color set in the Step Inspector.
            /// </summary>
            [DataMember]
            [DisplayName("Highlight color")]
            public Color HighlightColor
            {
                get
                {
                    return CustomHighlightColor.Value;
                }

                set
                {
                    CustomHighlightColor = new ModeParameter<Color>("HighlightColor", value);
                }
            }

            /// <summary>
            /// Target scene object to be highlighted.
            /// </summary>
            [DataMember]
            [DisplayName("Object to highlight")]
            public SceneObjectReference Target { get; set; }

            /// <summary>
            /// <see cref="Innoactive.Hub.Training.SceneObjects.Properties.HighlightProperty"/> of the object to be highlighted.
            /// </summary>
            public HighlightProperty HighlightProperty { get; set; }

            /// <summary>
            /// Metadata used for undo and redo feature.
            /// </summary>
            public Metadata Metadata { get; set; }

            /// <inheritdoc />
            public string Name { get; set; }
        }

        private class ActivatingProcess : InstantStageProcess<EntityData>
        {
            /// <inheritdoc />
            public override void Start(EntityData data)
            {
                data.HighlightProperty = data.Target.Value.GameObject.GetComponent<HighlightProperty>(true);
                data.HighlightProperty.Highlight(data.HighlightColor);
            }
        }

        private class DeactivatingProcess : InstantStageProcess<EntityData>
        {
            /// <inheritdoc />
            public override void Start(EntityData data)
            {
                if (data.HighlightProperty != null)
                {
                    data.HighlightProperty.Unhighlight();
                }
            }
        }

        private class EntityConfigurator : IConfigurator<EntityData>
        {
            /// <inheritdoc />
            public void Configure(EntityData data, IMode mode, Stage stage)
            {
                data.CustomHighlightColor.Configure(mode);
            }
        }

        [JsonConstructor]
        public HighlightObjectBehavior() : this("", Color.magenta)
        {
        }

        public HighlightObjectBehavior(string sceneObjectName, Color highlightColor, string name = "Highlight Object")
        {
            Data = new EntityData()
            {
                Target = new SceneObjectReference(sceneObjectName),
                HighlightColor = highlightColor,
                Name = name
            };
        }

        public HighlightObjectBehavior(ISceneObject target) : this(target, Color.magenta)
        {
        }

        public HighlightObjectBehavior(ISceneObject target, Color highlightColor, string name = "Highlight Object") : this(TrainingReferenceUtils.GetNameFrom(target), highlightColor, name)
        {
        }

        private readonly IProcess<EntityData> process = new Process<EntityData>(new ActivatingProcess(), new EmptyStageProcess<EntityData>(), new DeactivatingProcess());

        /// <inheritdoc />
        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }

        private readonly IConfigurator<EntityData> configurator = new BaseConfigurator<EntityData>().Add(new EntityConfigurator());

        /// <inheritdoc />
        protected override IConfigurator<EntityData> Configurator
        {
            get
            {
                return configurator;
            }
        }
    }
}
