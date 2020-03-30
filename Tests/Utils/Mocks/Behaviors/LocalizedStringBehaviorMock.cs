using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Internationalization;

namespace Innoactive.Creator.Tests.Utils.Mocks
{
    /// <summary>
    /// Helper behavior for testing that has a <seealso cref="LocalizedString"/>.
    /// </summary>
    public class LocalizedStringBehaviorMock : Behavior<LocalizedStringBehaviorMock.EntityData>
    {
        public class EntityData : IBehaviorData
        {
            /// <summary>
            /// <seealso cref="LocalizedString"/> which can be set.
            /// </summary>
            public LocalizedString LocalizedString { get; set; }

            public Metadata Metadata { get; set; }

            public string Name { get; set; }
        }

        protected override IProcess<EntityData> Process
        {
            get
            {
                return new Process<EntityData>(new EmptyStageProcess<EntityData>(), new EmptyStageProcess<EntityData>(), new EmptyStageProcess<EntityData>());
            }
        }

        public LocalizedStringBehaviorMock(LocalizedString localizedString)
        {
            Data = new EntityData()
            {
                LocalizedString = localizedString
            };
        }
    }
}
