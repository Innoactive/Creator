using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.EntityOwners;
using Innoactive.Creator.Core.Exceptions;
using Innoactive.Creator.Core.Utils;
using Innoactive.Creator.Core.Utils.Logging;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// A chapter of a training <see cref="Course"/>.
    /// </summary>
    [DataContract(IsReference = true)]
    public class Chapter : Entity<Chapter.EntityData>, IChapter
    {
        /// <summary>
        /// The chapter's data class.
        /// </summary>
        [DataContract(IsReference = true)]
        public class EntityData : EntityCollectionData<IStep>, IChapterData
        {
            /// <inheritdoc />
            [DataMember]
            [HideInTrainingInspector]
            public string Name { get; set; }

            /// <summary>
            /// The first step of the chapter.
            /// </summary>
            [DataMember]
            public IStep FirstStep { get; set; }

            /// <summary>
            /// All steps of the chapter.
            /// </summary>
            [DataMember]
            public IList<IStep> Steps { get; set; }

            /// <inheritdoc />
            public override IEnumerable<IStep> GetChildren()
            {
                return Steps.ToArray();
            }

            /// <inheritdoc />
            public IMode Mode { get; set; }

            /// <inheritdoc />
            public IStep Current { get; set; }
        }

        private class ActivatingProcess : EntityIteratingProcess<IStep>
        {
            private readonly IStep firstStep;

            public ActivatingProcess(IChapterData data) : base(data)
            {
                firstStep = data.FirstStep;
            }

            private IEnumerator<IStep> enumerator;

            private IEnumerator<IStep> GetChildren()
            {
                IStep current = firstStep;

                while (current != null)
                {
                    yield return current;

                    current = current.Data.Transitions.Data.Transitions.First(transition => transition.IsCompleted).Data.TargetStep;
                }
            }

            /// <inheritdoc />
            public override void Start()
            {
                enumerator = GetChildren();
                base.Start();
            }

            /// <inheritdoc />
            protected override bool ShouldActivateCurrent()
            {
                return true;
            }

            /// <inheritdoc />
            protected override bool ShouldDeactivateCurrent()
            {
                return Data.Current.Data.Transitions.Data.Transitions.Any(transition => transition.IsCompleted);
            }

            /// <inheritdoc />
            public override void End()
            {
                enumerator = null;
                base.End();
            }

            /// <inheritdoc />
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

            /// <inheritdoc />
            public override void FastForward()
            {
                if (Data.Current == null)
                {
                    return;
                }

                if (Data.Current.FindPathInGraph(step => step.Data.Transitions.Data.Transitions.Select(transition => transition.Data.TargetStep), null, out IList<IStep> pathToChapterEnd) == false)
                {
                    throw new InvalidStateException("The end of the chapter is not reachable from the current step.");
                }

                foreach (IStep step in pathToChapterEnd)
                {
                    if (Data.Current.LifeCycle.Stage == Stage.Inactive)
                    {
                        Data.Current.LifeCycle.Activate();
                    }

                    Data.Current.LifeCycle.MarkToFastForward();

                    ITransition toAutocomplete = Data.Current.Data.Transitions.Data.Transitions.First(transition => transition.Data.TargetStep == step);
                    if (toAutocomplete.IsCompleted == false)
                    {
                        toAutocomplete.Autocomplete();
                    }

                    Data.Current.LifeCycle.Deactivate();

                    Data.Current = step;
                }
            }
        }

        /// <inheritdoc />
        [DataMember]
        public ChapterMetadata ChapterMetadata { get; set; }

        /// <inheritdoc />
        public override IProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }

        /// <inheritdoc />
        public override IProcess GetDeactivatingProcess()
        {
            return new StopEntityIteratingProcess<IStep>(Data);
        }

        /// <inheritdoc />
        protected override IConfigurator GetConfigurator()
        {
            return new SequenceConfigurator<IStep>(Data);
        }

        /// <inheritdoc />
        IChapterData IDataOwner<IChapterData>.Data
        {
            get { return Data; }
        }

        protected Chapter() : this(null, null)
        {
        }

        public Chapter(string name, IStep firstStep)
        {
            ChapterMetadata = new ChapterMetadata();

            Data.Name = name;
            Data.FirstStep = firstStep;
            Data.Steps = new List<IStep>();

            if (firstStep != null)
            {
                Data.Steps.Add(firstStep);
            }

            if (LifeCycleLoggingConfig.Instance.LogChapters)
            {
                LifeCycle.StageChanged += (sender, args) =>
                {
                    Debug.LogFormat("<b>Chapter</b> <i>'{0}'</i> is <b>{1}</b>.\n", Data.Name, LifeCycle.Stage.ToString());
                };
            }
        }
    }
}
