using System.Collections.Generic;
using System.Linq;
using Innoactive.CreatorEditor;
using UnityEditor;

/// <summary>
/// Checks for assemblies specified and adds/removes the symbol according to there existence.
/// </summary>
[InitializeOnLoad]
internal class AssemblySymbolChecker
{
    static AssemblySymbolChecker()
    {
        CheckForClass("Innoactive.Creator.Core", "Innoactive.Creator.Core.Behaviors.BehaviorSequence", "BASIC_CONDITION_BEHAVIORS");
        CheckForAssembly("Innoactive.Creator.BasicInteraction", "BASIC_INTERACTION");
        CheckForAssembly("Innoactive.Creator.BasicUI", "BASIC_UI");
    }

    /// <summary>
    /// Tries to find the given assembly name, and add/removes the symbol according to the existence of it.
    /// </summary>
    /// <param name="assemblyName">The assembly name looked for, just the name, no full name.</param>
    /// <param name="symbol">The symbol added/removed.</param>
    public static void CheckForAssembly(string assemblyName, string symbol)
    {
        if (EditorReflectionUtils.AssemblyExists(assemblyName))
        {
            AddSymbol(symbol);
        }
        else
        {
            RemoveSymbol(symbol);
        }
    }

    /// <summary>
    /// Tries to find the given assembly name, and add/removes the symbol according to the existence of it.
    /// </summary>
    /// <param name="assemblyName">The assembly name looked for, just the name, no full name.</param>
    /// <param name="className">The class name looked for.</param>
    /// <param name="symbol">The symbol added/removed.</param>
    public static void CheckForClass(string assemblyName, string className, string symbol)
    {
        if (EditorReflectionUtils.ClassExists(assemblyName, className))
        {
            AddSymbol(symbol);
        }
        else
        {
            RemoveSymbol(symbol);
        }
    }

    private static void AddSymbol(string symbol)
    {
        BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
        BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
        List<string> symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';').ToList();

        if (symbols.Contains(symbol) == false)
        {
            symbols.Add(symbol);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", symbols.ToArray()));
        }
    }

    private static void RemoveSymbol(string symbol)
    {
        BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
        BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
        List<string> symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';').ToList();

        if (symbols.Contains(symbol))
        {
            symbols.Remove(symbol);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", symbols.ToArray()));
        }
    }
}
