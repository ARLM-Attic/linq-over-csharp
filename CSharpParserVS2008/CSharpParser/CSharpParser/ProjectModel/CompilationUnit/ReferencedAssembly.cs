using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an assembly reference.
  /// </summary>
  // ==================================================================================
  public sealed class ReferencedAssembly : ReferencedUnit
  {
    #region Private fields 

    private readonly string _Alias;
    private readonly Assembly _Assembly;
    private static readonly string _DotNetSystemFolder;
    private static readonly Dictionary<string, string> _FullNames = 
      new Dictionary<string, string>();

    #endregion

    #region Lifecycle methods 

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Obtains the location of system assemblies.
    /// </summary>
    // --------------------------------------------------------------------------------
    static ReferencedAssembly()
    {
      Assembly systemAsm = Assembly.ReflectionOnlyLoad("mscorlib");
      _DotNetSystemFolder = Path.GetDirectoryName(systemAsm.Location);
      _FullNames.Add("System.Core", 
        "System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, processorArchitecture=MSIL");
      _FullNames.Add("System.Xml.Linq",
        "System.Xml.Linq, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, processorArchitecture=MSIL");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a reference to the specified system assembly.
    /// </summary>
    /// <param name="name">System assembly name.</param>
    // --------------------------------------------------------------------------------
    public ReferencedAssembly(string name)
      : this(name, String.Empty, String.Empty)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a reference to the specified assembly.
    /// </summary>
    /// <param name="name">Assembly name</param>
    /// <param name="path">Assembly path</param>
    // --------------------------------------------------------------------------------
    public ReferencedAssembly(string name, string path)
      : this(name, path, String.Empty)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a reference to the specified assembly.
    /// </summary>
    /// <param name="name">Assembly name</param>
    /// <param name="path">Assembly path</param>
    /// <param name="alias">Alias name</param>
    // --------------------------------------------------------------------------------
    public ReferencedAssembly(string name, string path, string alias)
      : base(name)
    {
      _Alias = alias;

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
          _Assembly = Assembly.Load(new AssemblyName(name));
          return;
        }
        catch (FileNotFoundException)
        {
          // --- This exeption is caught intentionally
        }

        path = _DotNetSystemFolder;
      }

      // --- Path is specified, so load the assembly with the specified path
      if (!name.EndsWith(".dll") && !name.EndsWith(".exe")) name += ".dll";
      _Assembly = Assembly.LoadFrom(Path.Combine(path, name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a reference from the specified assembly.
    /// </summary>
    /// <param name="assembly">.NET assembly</param>
    // --------------------------------------------------------------------------------
    public ReferencedAssembly(Assembly assembly)
      : this(assembly, String.Empty)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a reference from the specified assembly with the provided alias.
    /// </summary>
    /// <param name="assembly">.NET assembly</param>
    /// <param name="alias">Assembly alias name</param>
    // --------------------------------------------------------------------------------
    public ReferencedAssembly(Assembly assembly, string alias)
      : base(assembly.GetName().Name)
    {
      _Assembly = assembly;
      _Alias = alias;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the folder used by the .NET system assemblies.
    /// </summary>
    // --------------------------------------------------------------------------------
    public static string DotNetSystemFolder
    {
      get { return _DotNetSystemFolder; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the alias.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Alias
    {
      get { return _Alias; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the assembly used by this reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Assembly Assembly
    {
      get { return _Assembly; }
    }

    #endregion
  }
}
