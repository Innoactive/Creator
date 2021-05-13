namespace VPG.Creator.Unity
{
    /// <summary>
    /// Allows to check if we are unit testing right now.
    /// </summary>
    internal static class UnitTestChecker
    {
        /// <summary>
        /// This will be set to true by RuntimeTests, if we are unit testing.
        /// </summary>
        public static bool IsUnitTesting { get; set; } = false;
    }
}
