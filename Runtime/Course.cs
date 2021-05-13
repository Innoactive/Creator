using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using VPG.Creator.Core.Attributes;
using VPG.Creator.Core.Configuration.Modes;
using VPG.Creator.Core.EntityOwners;

namespace VPG.Creator.Core
{
    /// <summary>
    /// An implementation of <see cref="ICourse"/> class.
    /// It contains a complete information about the training workflow.
    /// </summary>
    [DataContract(IsReference = true)]
    public class Course : Entity<Course.EntityData>, ICourse
    {
        /// <summary>
        /// The data class for a course.
        /// </summary>
        public class EntityData : EntityCollectionData<IChapter>, ICourseData
        {
            /// <inheritdoc />
            [DataMember]
            public IList<IChapter> Chapters { get; set; }

            /// <inheritdoc />
            public IChapter FirstChapter
            {
                get { return Chapters[0]; }
            }

            /// <inheritdoc />
            public override IEnumerable<IChapter> GetChildren()
            {
                return Chapters.ToArray();
            }

            /// <inheritdoc />
            public IChapter Current { get; set; }

            /// <inheritdoc />
            [DataMember]
            [HideInTrainingInspector]
            public string Name { get; set; }

            /// <inheritdoc />
            public IMode Mode { get; set; }
        }

        /// <summary>
        /// Step that is currently being executed.
        /// </summary>
        [DataMember]
        public IStep CurrentStep { get; protected set; }

        private class ActivatingProcess : EntityIteratingProcess<IChapter>
        {
            private IEnumerator<IChapter> enumerator;

            public ActivatingProcess(IEntitySequenceDataWithMode<IChapter> data) : base(data)
            {
            }

            /// <inheritdoc />
            public override void Start()
            {
                base.Start();
                enumerator = Data.GetChildren().GetEnumerator();
            }

            /// <inheritdoc />
            protected override bool ShouldActivateCurrent()
            {
                return true;
            }

            /// <inheritdoc />
            protected override bool ShouldDeactivateCurrent()
            {
                return true;
            }

            /// <inheritdoc />
            protected override bool TryNext(out IChapter entity)
            {
                if (enumerator == null || (enumerator.MoveNext() == false))
                {
                    entity = default;
                    return false;
                }
                else
                {
                    entity = enumerator.Current;
                    return true;
                }
            }
        }

        /// <inheritdoc />
        ICourseData IDataOwner<ICourseData>.Data
        {
            get { return Data; }
        }

        /// <inheritdoc />
        public override IProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }

        /// <inheritdoc />
        public override IProcess GetDeactivatingProcess()
        {
            return new StopEntityIteratingProcess<IChapter>(Data);
        }

        protected Course() : this(null, new IChapter[0])
        {
        }

        public Course(string name, IChapter chapter) : this(name, new List<IChapter> { chapter })
        {
        }

        public Course(string name, IEnumerable<IChapter> chapters)
        {
            Data.Chapters = chapters.ToList();
            Data.Name = name;
        }
    }
}
