// ================================================================================================
// ReferencedAssembly.cs
//
// Revised: 2009.03.04, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CSharpFactory.ProjectModel
{
  // ================================================================================================
  /// <summary>
  /// This type represents an assembly reference.
  /// </summary>
  // ================================================================================================
  public sealed class ReferencedAssembly : ReferencedUnit
  {
    #region Constants

    const string SystemCoreAsm = "System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, processorArchitecture=MSIL";
    const string SystemWorkflowAsm = "System.Workflow.Runtime, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL";

    #endregion

    #region Private fields

    private static readonly Dictionary<string, string> _FullNames = 
      new Dictionary<string, string>();

    #endregion

    #region Lifecycle methods 

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Obtains the location of system assemblies.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    static ReferencedAssembly()
    {
      // --- Prepare .NET system folders
      var systemAsm20 = Assembly.ReflectionOnlyLoad("mscorlib");
      DotNet20SystemFolder = Path.GetDirectoryName(systemAsm20.Location);
      var systemAsm30 = Assembly.Load(new AssemblyName(SystemWorkflowAsm));
      DotNet30SystemFolder = Path.GetDirectoryName(systemAsm30.Location);
      var systemAsm35 = Assembly.Load(new AssemblyName(SystemCoreAsm));
      DotNet35SystemFolder = Path.GetDirectoryName(systemAsm35.Location);

      _FullNames.Add("System.Xml.Linq",
        "System.Xml.Linq, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, processorArchitecture=MSIL");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a reference to the specified system assembly.
    /// </summary>
    /// <param name="name">System assembly name.</param>
    // ----------------------------------------------------------------------------------------------
    public ReferencedAssembly(string name)
      : this(name, String.Empty, String.Empty)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a reference to the specified assembly.
    /// </summary>
    /// <param name="name">Assembly name</param>
    /// <param name="path">Assembly path</param>
    // ----------------------------------------------------------------------------------------------
    public ReferencedAssembly(string name, string path)
      : this(name, path, String.Empty)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a reference to the specified assembly.
    /// </summary>
    /// <param name="name">Assembly name</param>
    /// <param name="path">Assembly path</param>
    /// <param name="alias">AliasToken name</param>
    // ----------------------------------------------------------------------------------------------
    public ReferencedAssembly(string name, string path, string alias)
      : base(name)
    {
      Alias = alias;

      // --- Try to load the assembly in several ways
      if (String.IsNullOrEmpty(path))
      {
        // --- Try to load the assembly with assemblyName
        try
        {
          if (_FullNames.ContainsKey(name))
          {
            name = _FullNames[name];
          }
          Assembly = Assembly.Load(new AssemblyName(name));
          return;
        }
        catch (FileNotFoundException)
        {
          // --- This exeption is caught intentionally
        }

        Assembly = LoadFromPath(DotNet35SystemFolder, name);
        if (Assembly != null) return;

        Assembly = LoadFromPath(DotNet30SystemFolder, name);
        if (Assembly != null) return;

        Assembly = LoadFromPath(DotNet20SystemFolder, name);
        return;
      }

      // --- Path is specified, so load the assembly with the specified path
      Assembly = LoadFromPath(path, name);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a reference from the specified assembly.
    /// </summary>
    /// <param name="assembly">.NET assembly</param>
    // ----------------------------------------------------------------------------------------------
    public ReferencedAssembly(Assembly assembly)
      : this(assembly, String.Empty)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a reference from the specified assembly with the provided alias.
    /// </summary>
    /// <param name="assembly">.NET assembly</param>
    /// <param name="alias">Assembly alias name</param>
    // ----------------------------------------------------------------------------------------------
    public ReferencedAssembly(Assembly assembly, string alias)
      : base(assembly.GetName().Name)
    {
      Assembly = assembly;
      Alias = alias;
    }

    #endregion

    #region Public properties

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the folder used by the .NET 2.0 system assemblies.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public static string DotNet20SystemFolder { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the folder used by the .NET 3.0 system assemblies.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public static string DotNet30SystemFolder { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the folder used by the .NET 2.0 system assemblies.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public static string DotNet35SystemFolder { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the folder used by the .NET 2.0 system assemblies.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public static string DotNet40SystemFolder { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the alias.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Alias { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the assembly used by this reference.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Assembly Assembly { get; private set; }

    #endregion

    #region Helper methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Loads the assembly with the specified name from the given path.
    /// </summary>
    /// <param name="path">Path to load the assembly from.</param>
    /// <param name="name">Name of the asseambly file.</param>
    /// <returns>The loaded assembly if found; otherwise, null.</returns>
    // ----------------------------------------------------------------------------------------------
    private Assembly LoadFromPath(string path, string name)
    {
      if (!name.EndsWith(".dll") && !name.EndsWith(".exe")) name += ".dll";
      try
      {
        return Assembly.LoadFrom(Path.Combine(path, name));
      }
      catch (FileNotFoundException)
      {
        return null;
      }
    }

    #endregion
  }
}
