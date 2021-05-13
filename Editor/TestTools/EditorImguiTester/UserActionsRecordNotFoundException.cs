using System.IO;

namespace VPG.CreatorEditor.ImguiTester
{
    /// <summary>
    /// Exception that is thrown when file with recorded user actions is not found by the Editor IMGUI Tester.
    /// </summary>
    internal class UserActionsRecordNotFoundException : FileNotFoundException
    {
        public UserActionsRecordNotFoundException(string message, params object[] formatArgs) : base(string.Format(message, formatArgs))
        {
        }
    }
}
