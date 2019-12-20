// Checks if Unity has .Net 2.0 subset enabled - the Hub-SDK requires the full .Net 2.0 variant.
// This can't really be done as an Editor script, since this would require compilation to
// succeed before running, which isn't the case for .Net subset.
// If Scripting Runtime Version is .NET 4.X, Api Compatibility Level 4.X is required, since the
// .Net Standard 2.0 does not provide access to Win32.Registry.
#if NET_2_0_SUBSET
#error This Unity project is configured for .Net 2.0 subset, but the Innoactive Hub SDK requires full .Net functionality. Please go to "PlayerSettings > Other Settings" and change Api Compatibility Level to .Net 4.X.
#elif NET_STANDARD_2_0
#error This Unity project is configured for Scripting Runtime Version 4.X, but uses Api Compatibility for .Net Standard 2.0. The Innoactive Hub SDK requires Api Compatibility level 4.X. Please go to "PlayerSettings > Other Settings" and change Api Compatibility Level to .Net 4.X.
#endif


