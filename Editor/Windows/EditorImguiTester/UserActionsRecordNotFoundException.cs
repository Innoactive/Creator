using System.IO;

namespace Innoactive.Hub.Unity.Tests.Training.Editor.EditorImguiTester
{
    /// <summary>
    /// Exception that is thrown when file with recorded user actions is not found by the Editor IMGUI Tester.
    /// </summary>
    public class UserActionsRecordNotFoundException : FileNotFoundException
    {
        public UserActionsRecordNotFoundException(string message, params object[] formatArgs) : base(string.Format(message, formatArgs))
        {
        }
    }
}
