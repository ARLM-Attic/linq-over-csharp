using CSharpParser.ProjectModel;
using CSharpParser.Semantics;

namespace CSharpParser.Semantics
{
  public sealed class SemanticsParser
  {
    private CompilationUnit _CompilationUnit;

    public SemanticsParser(CompilationUnit project)
    {
      _CompilationUnit = project;
    }

    public CompilationUnit CompilationUnit
    {
      get { return _CompilationUnit; }
    }

    public void CheckSemantics()
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in the related compliation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void ResolveTypeReferences()
    {
      foreach (SourceFile source in _CompilationUnit.Files)
      {
        CompilationUnit.CurrentLocation = source;
        source.ResolveTypeReferences(ResolutionContext.SourceFile, source);
      }
    }
  }
}
