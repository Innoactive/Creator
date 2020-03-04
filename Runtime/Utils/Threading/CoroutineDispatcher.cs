using Innoactive.Hub.Unity;

namespace Innoactive.Hub.Threading
{
    /// <summary>
    /// Auxiliary class that allows starting UnityCoroutines from a context that is not
    /// itself a MonoBehaviour.
    /// </summary>
    public class CoroutineDispatcher : UnitySingleton<CoroutineDispatcher>
    {
    }
}
