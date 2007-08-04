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
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, IResolutionRequired contextInstance)
    {
      base.ResolveTypeReferences(contextType, contextInstance);
      foreach (FormalParameter param in _FormalParameters)
      {
        param.ResolveTypeReferences(contextType, contextInstance);
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
