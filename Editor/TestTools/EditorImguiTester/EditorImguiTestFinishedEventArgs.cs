using System;

namespace VPG.CreatorEditor.TestTools
{
    /// <summary>
    /// Event args for event which is fired when a <see cref="IEditorImguiTest"/> test finishes its execution.
    /// </summary>
    internal class EditorImguiTestFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// Result from the last <see cref="IEditorImguiTest"/>.
        /// </summary>
        public TestState Result { get; private set; }

        public EditorImguiTestFinishedEventArgs(TestState result)
        {
            Result = result;
        }
    }
}
