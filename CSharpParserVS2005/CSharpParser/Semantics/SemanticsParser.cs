using System;
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
/*
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Imports the 'extern alias' assemblies into the appropriate namespace hierarchy.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void ImportExternalReferences()
    {
      foreach (ReferencedUnit reference in _CompilationUnit.ReferencedUnits)
      {
        ReferencedAssembly asmRef = reference as ReferencedAssembly;
        if (asmRef == null) continue;
        NamespaceHierarchy hierarchy;
        if (String.IsNullOrEmpty(asmRef.Alias))
        {
          // --- This resource goes to the global hierarchy
          hierarchy = _CompilationUnit.GlobalHierarchy;
        }
        else
        {
          // --- This resource goes to a named hierarchy
          if (!_CompilationUnit.NamespaceHierarchies.TryGetValue(asmRef.Alias, out hierarchy))
          {
            // --- No hierarchy for this namespace, create one
            hierarchy = new NamespaceHierarchy(asmRef.Alias);
            _CompilationUnit.NamespaceHierarchies.Add(hierarchy.Name, hierarchy);
          }
        }
        
        // --- Import the external resource to this hierarchy
        hierarchy.ImportTypes(asmRef.Assembly, null);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Imports the referenced assemblies into the global namespace hierarchy.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void ImportReferences()
    {
      foreach (ReferencedUnit reference in _CompilationUnit.ReferencedUnits)
      {
        ReferencedAssembly asmRef = reference as ReferencedAssembly;
        if (asmRef != null)
        {
          _CompilationUnit.GlobalHierarchy.ImportTypes(asmRef.Assembly, null);
        }
      }
    }
*/
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
