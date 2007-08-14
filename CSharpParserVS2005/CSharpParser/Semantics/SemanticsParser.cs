using CSharpParser.ProjectModel;
using CSharpParser.Semantics;

namespace CSharpParser.Semantics
{
  public sealed class SemanticsParser
  {
    private readonly CompilationUnit _CompilationUnit;

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
    /// Sets resolver information for all types and namespaces declared in the
    /// source code.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void SetResolvers()
    {
      foreach (SourceFile source in _CompilationUnit.Files)
      {
        // --- Set namespace resolvers
        foreach (NamespaceFragment fragment in source.Namespaces)
        {
          fragment.SetResolver();
        }

        // --- Set type resolvers
        foreach (TypeDeclaration type in source.TypeDeclarations)
        {
          type.SetResolver();
        }
      }
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
