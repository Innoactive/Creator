using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Innoactive.Hub.Training.EntityOwners;
using Innoactive.Hub.Training.Attributes;
using Innoactive.Hub.Training.Configuration.Modes;
using Newtonsoft.Json;

namespace Innoactive.Hub.Training
{
    /// <summary>
    /// An implementation of <see cref="ICourse"/> class.
    /// It contains a complete information about the training workflow.
    /// </summary>
    [DataContract(IsReference = true)]
    public class Course : Entity<Course.EntityData>, ICourse
    {
        public class EntityData : EntityCollectionData<IChapter>, ICourseData
        {
            [DataMember]
            public IList<IChapter> Chapters { get; set; }

            public IChapter FirstChapter
            {
                get
                {
                    return Chapters[0];
                }
            }

            public override IEnumerable<IChapter> GetChildren()
            {
                return Chapters.ToArray();
            }

            public IChapter Current { get; set; }

            [DataMember]
            [HideInTrainingInspector]
            public string Name { get; set; }

            public IMode Mode { get; set; }
        }

        /// <summary>
        /// Step that is currently being executed.
        /// </summary>
        [DataMember]
        public IStep CurrentStep { get; protected set; }

        private class ActivatingProcess : EntityIteratingProcess<EntityData, IChapter>
        {
            private IEnumerator<IChapter> enumerator;

            public override void Start(EntityData data)
            {
                base.Start(data);
                enumerator = data.GetChildren().GetEnumerator();
            }

            protected override bool ShouldActivateCurrent(EntityData data)
            {
                return true;
            }

            protected override bool ShouldDeactivateCurrent(EntityData data)
            {
                return true;
            }

            protected override bool TryNext(out IChapter entity)
            {
                if (enumerator == null || (enumerator.MoveNext() == false))
                {
                    entity = default(IChapter);
                    return false;
                }
                else
                {
                    entity = enumerator.Current;
                    return true;
                }
            }
        }

        ICourseData IDataOwner<ICourseData>.Data
        {
            get
            {
                return Data;
            }
        }

        private readonly IProcess<EntityData> process = new Process<EntityData>(new ActivatingProcess(), new EmptyStageProcess<EntityData>(), new StopEntityIteratingProcess<EntityData, IChapter>());

        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }

        [JsonConstructor]
        protected Course() : this(null, new IChapter[0])
        {
        }

        public Course(string name, IChapter chapter) : this(name, new List<IChapter> { chapter })
        {
        }

        public Course(string name, IEnumerable<IChapter> chapters)
        {
            Data = new EntityData()
            {
                Chapters = chapters.ToList(),
                Name = name
            };
        }
    }
}
