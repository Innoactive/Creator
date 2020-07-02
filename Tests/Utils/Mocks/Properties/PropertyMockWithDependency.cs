using Innoactive.Creator.Core.Properties;
using UnityEngine;

namespace Innoactive.Creator.Tests.Utils.Mocks
{
    /// <summary>
    /// Property requiring a <see cref="PropertyMock"/>.
    /// </summary>
    [RequireComponent(typeof(PropertyMock))]
    public class PropertyMockWithDependency : TrainingSceneObjectProperty
    {

    }
}
