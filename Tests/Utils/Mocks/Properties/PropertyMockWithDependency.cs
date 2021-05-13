using VPG.Creator.Core.Properties;
using UnityEngine;

namespace VPG.Creator.Tests.Utils.Mocks
{
    /// <summary>
    /// Property requiring a <see cref="PropertyMock"/>.
    /// </summary>
    [RequireComponent(typeof(PropertyMock))]
    public class PropertyMockWithDependency : TrainingSceneObjectProperty
    {

    }
}
