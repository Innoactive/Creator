// Checks if the project has the .NET4.X Api compatibility active.
// Has to stay in runtime to properly throw the error.
#if NET_STANDARD_2_0
#error This Unity project is not configured for .Net 4.X, but the Creator SDK requires .NET 4.X support. Please go to "PlayerSettings > Other Settings" and change Api Compatibility Level.
#endif

#if NET_2_0
#error This Unity project is not configured for .Net 4.X, but the Creator SDK requires .NET 4.X support. Please go to "PlayerSettings > Other Settings" and change Api Compatibility Level.
#endif

#if NET_2_0_SUBSET
#error This Unity project is not configured for .Net 4.X, but the Creator SDK requires .NET 4.X support. Please go to "PlayerSettings > Other Settings" and change Api Compatibility Level.
#endif
