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

        public LocalizedStringBehaviorMock(LocalizedString localizedString)
        {
            Data.LocalizedString = localizedString;
        }
    }
}
