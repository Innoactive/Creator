using System.Runtime.Serialization;
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Properties;
using Innoactive.Creator.Core.Utils;
using UnityEngine;

namespace Innoactive.Creator.Core.Behaviors
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
            /// <see cref="ModeParameter{T}"/> of the highlight color.
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
            public ScenePropertyReference<IHighlightProperty> ObjectToHighlight { get; set; }

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
                if (data.ObjectToHighlight.Value != null)
                {
                    data.ObjectToHighlight.Value.Highlight(data.HighlightColor);
                }
            }
        }

        private class DeactivatingProcess : InstantStageProcess<EntityData>
        {
            /// <inheritdoc />
            public override void Start(EntityData data)
            {
                if (data.ObjectToHighlight.Value != null)
                {
                    data.ObjectToHighlight.Value.Unhighlight();
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

        public HighlightObjectBehavior() : this("", Color.magenta)
        {
        }

        public HighlightObjectBehavior(string sceneObjectName, Color highlightColor, string name = "Highlight Object")
        {
            Data = new EntityData()
            {
                ObjectToHighlight = new ScenePropertyReference<IHighlightProperty>(sceneObjectName),
                HighlightColor = highlightColor,
                Name = name
            };
        }

        public HighlightObjectBehavior(IHighlightProperty target) : this(target, Color.magenta)
        {
        }

        public HighlightObjectBehavior(IHighlightProperty target, Color highlightColor, string name = "Highlight Object") : this(TrainingReferenceUtils.GetNameFrom(target), highlightColor, name)
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
