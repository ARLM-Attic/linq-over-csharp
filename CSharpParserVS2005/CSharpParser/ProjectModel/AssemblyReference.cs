using System.IO;
using System.Reflection;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an assembly reference.
  /// </summary>
  // ==================================================================================
  public sealed class AssemblyReference : CompilationReference
  {
    #region Private fields 

    private readonly Assembly _Assembly;
    private static readonly string _DotNetSystemFolder;

    #endregion

    #region Lifecycle methods 

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Obtains the location of system assemblies.
    /// </summary>
    // --------------------------------------------------------------------------------
    static AssemblyReference()
    {
      Assembly systemAsm = Assembly.ReflectionOnlyLoad("mscorlib");
      _DotNetSystemFolder = Path.GetDirectoryName(systemAsm.Location);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a reference to the specified system assembly.
    /// </summary>
    /// <param name="name">System assembly name.</param>
    // --------------------------------------------------------------------------------
    public AssemblyReference(string name)
      : this(name + ".dll", _DotNetSystemFolder)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a reference to the specified assembly.
    /// </summary>
    /// <param name="name">Assembly name</param>
    /// <param name="path">Assembly path</param>
    // --------------------------------------------------------------------------------
    public AssemblyReference(string name, string path)
      : base(name)
    {
      _Assembly = Assembly.LoadFrom(Path.Combine(path, name));
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

    #endregion
  }
}
