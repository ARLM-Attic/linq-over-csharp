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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates namespace hierarchies according to the provided external alias 
    /// resolutions.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void CreateNamespaceHierarchies()
    {
      foreach (ExternAliasResolution exAlias in _CompilationUnit.ExternAliasResolutions)
      {
        // --- Create namespace hierarchis for external references having an instance name.
        if (!String.IsNullOrEmpty(exAlias.Alias) &&
            !_CompilationUnit.NamespaceHierarchies.ContainsKey(exAlias.Alias))
        {
          _CompilationUnit.NamespaceHierarchies.Add(exAlias.Alias,
                                                    new NamespaceHierarchy(exAlias.Alias));
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all using directives in the compilation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void ResolveUsingDirectives()
    {
      foreach (SourceFile file in _CompilationUnit.Files)
      {
        ResolveUsingDirectives(file);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all using directives in the specified source file.
    /// </summary>
    /// <param name="file">Source file.</param>
    // --------------------------------------------------------------------------------
    public void ResolveUsingDirectives(SourceFile file)
    {
      // --- Resolve usings in this file
      foreach (UsingClause usingClause in file.Usings)
      {
        // --- Resolve the clause
        if (usingClause.HasAlias)
        {
          // --- Resolve using aliases
        }
        else
        {
          NamespaceHierarchy hierarchy;
          // --- Resolve using directive
          // --- Step 1: Select the appropriate namespace hierarchy
          TypeReference nextNamePart = usingClause.ReferencedName;
          if (usingClause.ReferencedName.IsGlobal)
          {
            if (usingClause.ReferencedName.Name == "global")
            {
              // --- Use the global namespace hierarchy
              hierarchy = _CompilationUnit.GlobalHierarchy;
            }
            else
            {
              // --- Check, if we have an 'exter alias' for the root namespace
              if (!_CompilationUnit.NamespaceHierarchies.
                TryGetValue(usingClause.ReferencedName.Name, out hierarchy))
              {
                // --- No such namespace hierarchy have been found.
                _CompilationUnit.Parser.Error0432(usingClause.Token, 
                  usingClause.ReferencedName.Name);
                // --- Go to the next using clause
                continue;
              }
            }

            // --- Go to the next part of the name after the global declaration
            nextNamePart = nextNamePart.SubType;
          }

          // --- Step 2: Now we have the namespace hierarchy, let import the types
          // --- from the specified namespace
        }
      }
      foreach (NamespaceFragment nsFragment in file.Namespaces)
      {
        ResolveUsingDirectives(nsFragment);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all using directives in the specified namespace
    /// </summary>
    /// <param name="nsFragment">Namespace</param>
    // --------------------------------------------------------------------------------
    public void ResolveUsingDirectives(NamespaceFragment nsFragment)
    {
      // --- Resolve usings in this namespace
      foreach (UsingClause usingClaues in nsFragment.Usings)
      {
        // --- Resolve the clause
      }

      // ---Resolve usings in nested namespaces
      foreach (NamespaceFragment fragment in nsFragment.NestedNamespaces)
      {
        ResolveUsingDirectives(fragment);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Imports the 'extern alias' assemblies into the appropriate namespace hierarchy.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void ImportExternalReferences()
    {
      foreach (ExternAliasResolution externAlias in _CompilationUnit.ExternAliasResolutions)
      {
        NamespaceHierarchy hierarchy;
        if (String.IsNullOrEmpty(externAlias.Alias))
        {
          // --- This resource goes to the global hierarchy
          hierarchy = _CompilationUnit.GlobalHierarchy;
        }
        else
        {
          // --- This resource goes to a named hierarchy
          if (!_CompilationUnit.NamespaceHierarchies.TryGetValue(externAlias.Alias, out hierarchy))
          {
            // --- No hierarchy for this namespace, create one
            hierarchy = new NamespaceHierarchy(externAlias.Alias);
            _CompilationUnit.NamespaceHierarchies.Add(hierarchy.Name, hierarchy);
          }
        }
        
        // --- Import the external resource to this hierarchy
        hierarchy.ImportTypes(externAlias.Assembly, null);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Imports the referenced assemblies into the global namespace hierarchy.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void ImportReferences()
    {
      foreach (ReferencedUnit reference in _CompilationUnit.References)
      {
        ReferencedAssembly asmRef = reference as ReferencedAssembly;
        if (asmRef != null)
        {
          _CompilationUnit.GlobalHierarchy.ImportTypes(asmRef.Assembly, null);
        }
      }
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
