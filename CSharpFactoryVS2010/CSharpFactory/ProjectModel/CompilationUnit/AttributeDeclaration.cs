using System;
using System.Collections.Generic;
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;
using CSharpFactory.Semantics;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents an attribute declaration
  /// </summary>
  // ==================================================================================
  public sealed class AttributeDeclaration : LanguageElement, IUsesResolutionContext
  {
    #region Private fields

    private string _Scope;
    private TypeReference _TypeReference;
    private readonly List<AttributeArgument> _Arguments = new List<AttributeArgument>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new attribute declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="type">Attribute type</param>
    // --------------------------------------------------------------------------------
    public AttributeDeclaration(Token token, CSharpSyntaxParser parser, TypeReference type): 
      base (token, parser)
    {
      _TypeReference = type;
    }

    #endregion

    #region Public Properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the scope of the attribute
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Scope
    {
      get { return _Scope; }
      set { _Scope = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type referenced in the attribute
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference TypeReference
    {
      get { return _TypeReference; }
      set
      {
        _TypeReference = value;
        Name = value.Name;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of arguments belonging to this attribute.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<AttributeArgument> Arguments
    {
      get { return _Arguments; }
    }

    #endregion

    #region IUsesResolutionContext implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this type reference
    /// </summary>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
      if (_TypeReference != null) 
      {
        ResolveAttributeName(_TypeReference, contextType, declarationScope, parameterScope);
      }
      foreach (AttributeArgument arg in _Arguments)
      {
        arg.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the type reference of an attribute name
    /// </summary>
    /// <param name="type">Type to resolve.</param>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    private void ResolveAttributeName(TypeReference type, ResolutionContext contextType,
      ITypeDeclarationScope declarationScope,
      ITypeParameterScope parameterScope)
    {
      NamespaceOrTypeResolver resolver = new NamespaceOrTypeResolver(Parser);
      NamespaceOrTypeResolverInfo info;

      // --- If the rightmost name is verbatim (starting with @), we resolve the name
      // --- as a type or namespace name, omitting the @ from the name.
      if (type.TailName.StartsWith("@"))
      {
        // --- Remove the '@' before name resolution
        type.Tail.Name = type.TailName.Substring(1);
        try
        {
          info = resolver.Resolve(type, contextType, declarationScope, parameterScope);
          if (!info.IsResolved) return;
          if (IsAttributeClass(info))
          {
            Validate();
          }
          else
          {
            Parser.Error0616(type.Token, type.FullName);
            Invalidate();
          }
        }
        finally
        {
          // --- Anyway, put back the '@'
          type.Tail.Name = "@" + type.TailName;
        }
      }
      else
      {
        // --- We try to resolve the name with and without the 'Attribute' suffix.
        // --- Type resolution successful if one and only one alternative succeeds.

        // --- Suppress the compiler errors while resolving the attribute name
        Parser.SuppressErrors = true;
        bool normalNameResolved;
        bool suffixedNameResolved;
        try
        {
          // --- First let us try the original name
          info = resolver.Resolve(type, contextType, declarationScope, parameterScope);
          normalNameResolved = info.IsResolved && IsAttributeClass(info);

          // --- Let's try the 'Attribute' suffix
          string oldName = type.TailName;
          type.Tail.Name += "Attribute";
          try
          {
            info = resolver.Resolve(type, contextType, declarationScope, parameterScope);
            suffixedNameResolved = info.IsResolved && IsAttributeClass(info);
          }
          finally
          {
            // --- Anyway, we restore the name
            type.Tail.Name = oldName;
          }
        }
        finally
        {
          // Anyway, we allow error messages again
          Parser.SuppressErrors = false;
        }

        // --- Now check the result
        if (normalNameResolved && suffixedNameResolved)
        {
          // --- Ambigous resolution
          Parser.Error1614(type.Token, type.FullName);
          type.Invalidate();
        }
        else if (!normalNameResolved && !suffixedNameResolved)
        {
          // --- No name found;
          Parser.Error0246(type.Token, type.FullName);
          type.Invalidate();
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the result of name resolution is an System.Attribute derived class.
    /// </summary>
    /// <param name="info">Resolution information.</param>
    /// <returns>
    /// True, if the type is resolved to a System.Attribute derived type; 
    /// otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    private bool IsAttributeClass(NamespaceOrTypeResolverInfo info)
    {
      if (!info.IsResolved) return false;
      if (info.Target != ResolutionTarget.Type) return false;

      // --- At this point we have a resolved type. Go through the inheraitance chain to
      // --- Check if this is an Attribute derived class.
      ITypeAbstraction type = info.CurrentPart.TypeInstance;
      while (type.BaseType != null)
      {
        if (TypeBase.IsSame(type.BaseType, typeof(Attribute))) return true;
        type = type.BaseType;
      }
      return false;
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a collection of attribute declarations.
  /// </summary>
  // ==================================================================================
  public class AttributeCollection : ImmutableCollection<AttributeDeclaration>
  {
  }
}
