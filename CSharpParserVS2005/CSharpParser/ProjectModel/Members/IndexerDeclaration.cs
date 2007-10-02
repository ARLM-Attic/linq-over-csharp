using System.Text;
using CSharpParser.Collections;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a property member declaration.
  /// </summary>
  // ==================================================================================
  public class IndexerDeclaration : PropertyDeclaration
  {
    #region Private fields

    private readonly FormalParameterCollection _FormalParameters = new FormalParameterCollection();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new property member declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="declaringType">Type declaring this member.</param>
    // --------------------------------------------------------------------------------
    public IndexerDeclaration(Token token, TypeDeclaration declaringType)
      : base(token, declaringType)
    {
      Name = "this";
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the formal parameters belonging to this indexer.
    /// </summary>
    // --------------------------------------------------------------------------------
    public FormalParameterCollection FormalParameters
    {
      get { return _FormalParameters; }
    }


    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the explicit property name.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override TypeReference ExplicitName
    {
      set { _ExplicitName = value;}
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name of the member.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string FullName
    {
      get
      {
        return _ExplicitName == null ? "this" : _ExplicitName.FullName + ".this";
      }
    }

    #endregion

    #region Overridden methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the signature of this method.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string Signature
    {
      get
      {
        StringBuilder sb = new StringBuilder(100);
        sb.Append(FullName);
        sb.Append('(');
        bool isFirst = true;
        foreach (FormalParameter par in _FormalParameters)
        {
          if (!isFirst)
          {
            sb.Append(", ");
          }
          sb.Append(par.Type.FullName);
          isFirst = false;
        }
        sb.Append(')');
        return sb.ToString();
      }
    }

    #endregion

    #region Type resolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
      base.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      foreach (FormalParameter param in _FormalParameters)
      {
        param.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion

    #region Semantic checks

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks the semantics for the specified field declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override void CheckSemantics()
    {
      CheckGeneralMemberSemantics();

      // --- Indexers cannot be static
      if (IsStatic) StaticNotAllowed();
      else
      {
        CheckMethodModifiers();
      }
    }

    #endregion

  }

  // ==================================================================================
  /// <summary>
  /// This type defines a collection of type declarations that can be indexed by the
  /// full name of the type.
  /// </summary>
  // ==================================================================================
  public class IndexerDeclarationCollection :
    RestrictedIndexedCollection<IndexerDeclaration>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used by the indexing.
    /// </summary>
    /// <param name="item">IndexerDeclaration item.</param>
    /// <returns>
    /// Name of the indexer declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(IndexerDeclaration item)
    {
      return item.Signature;
    }
  }
}
