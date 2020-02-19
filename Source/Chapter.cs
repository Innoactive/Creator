using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Innoactive.Hub.Training.EntityOwners;
using Innoactive.Hub.Training.Attributes;
using Innoactive.Hub.Training.Configuration;
using Innoactive.Hub.Training.Configuration.Modes;
using Innoactive.Hub.Training.Exceptions;
using Innoactive.Hub.Training.Utils;

namespace Innoactive.Hub.Training
{
    /// <summary>
    /// A chapter of a training <see cref="Course"/>.
    /// </summary>
    [DataContract(IsReference = true)]
    public class Chapter : Entity<Chapter.EntityData>, IChapter
    {
        [DataContract(IsReference = true)]
        public class EntityData : EntityCollectionData<IStep>, IChapterData
        {
            /// <inheritdoc />
            [DataMember]
            [HideInTrainingInspector]
            public string Name { get; set; }

            [DataMember]
            public IStep FirstStep { get; set; }

            [DataMember]
            public IList<IStep> Steps { get; set; }

            public override IEnumerable<IStep> GetChildren()
            {
                return Steps.ToArray();
            }

            public IMode Mode { get; set; }

            public IStep Current { get; set; }
        }

        private class ActivatingProcess : EntityIteratingProcess<EntityData, IStep>
        {
            private IEnumerator<IStep> enumerator;

            private IEnumerator<IStep> GetChildren(IChapterData data)
            {
                IStep current = data.FirstStep;

                while (current != null)
                {
                    yield return current;

                    current = current.Data.Transitions.Data.Transitions.First(transition => transition.IsCompleted).Data.TargetStep;
                }
            }

            public override void Start(EntityData data)
            {
                enumerator = GetChildren(data);
                base.Start(data);
            }

            protected override bool ShouldActivateCurrent(EntityData data)
            {
                return true;
            }

            protected override bool ShouldDeactivateCurrent(EntityData data)
            {
                return data.Current.Data.Transitions.Data.Transitions.Any(transition => transition.IsCompleted);
            }

            public override void End(EntityData data)
            {
                enumerator = null;
                base.End(data);
            }

            protected override bool TryNext(out IStep entity)
            {
                if (enumerator != null && enumerator.MoveNext())
                {
                    entity = enumerator.Current;
                    return true;
                }
                else
                {
                    entity = null;
                    return false;
                }
            }

            public override void FastForward(EntityData data)
            {
                IList<IStep> happyPath;

                if (data.Current == null)
                {
                    return;
                }

                if (data.Current.FindPathInGraph(step => step.Data.Transitions.Data.Transitions.Select(transition => transition.Data.TargetStep), null, out happyPath) == false)
                {
                    throw new InvalidStateException("The end of the chapter is not reachable from current step.");
                }

                for (int i = 0; i < happyPath.Count; i++)
                {
                    if (data.Current.LifeCycle.Stage == Stage.Inactive)
                    {
                        data.Current.LifeCycle.Activate();
                    }

                    data.Current.LifeCycle.MarkToFastForward();

                    ITransition toAutocomplete = data.Current.Data.Transitions.Data.Transitions.First(transition => transition.Data.TargetStep == happyPath[i]);
                    if (toAutocomplete.IsCompleted == false)
                    {
                        toAutocomplete.Autocomplete();
                    }

                    data.Current.LifeCycle.Deactivate();

                    data.Current = happyPath[i];
                }
            }
        }

        /// <inheritdoc />
        [DataMember]
        public ChapterMetadata ChapterMetadata { get; set; }

        private readonly IProcess<EntityData> process = new Process<EntityData>(new ActivatingProcess(), new EmptyStageProcess<EntityData>(), new StopEntityIteratingProcess<EntityData, IStep>());

        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }

        private readonly IConfigurator<EntityData> configurator = new BaseConfigurator<EntityData>().Add(new EntitySequenceConfigurator<EntityData, IStep>());

        protected override IConfigurator<EntityData> Configurator
        {
            get
            {
                return configurator;
            }
        }

        IChapterData IDataOwner<IChapterData>.Data
        {
            get
            {
                return Data;
            }
        }

        protected Chapter() : this(null, null)
        {
        }

        public Chapter(string name, IStep firstStep)
        {
            ChapterMetadata = new ChapterMetadata();

            Data = new EntityData()
            {
                Name = name,
                FirstStep = firstStep,
                Steps = new List<IStep>()
            };

            if (firstStep != null)
            {
                Data.Steps.Add(firstStep);
            }

            if (RuntimeConfigurator.Configuration.EntityStateLoggerConfig.LogChapters)
            {
                LifeCycle.StageChanged += (sender, args) =>
                {
                    RuntimeConfigurator.Configuration.EntityStateLogger.InfoFormat("<b>Chapter</b> <i>'{0}'</i> is <b>{1}</b>.\n", Data.Name, LifeCycle.Stage.ToString());
                };
            }
        }
    }
}
