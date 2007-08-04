using CSharpParser.Collections;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a cast operator declaration.
  /// </summary>
  // ==================================================================================
  public sealed class CastOperatorDeclaration : MethodDeclaration
  {
    #region Private fields

    private bool _IsExplicit;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new cast operator declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="declaringType">Type declaring this member.</param>
    // --------------------------------------------------------------------------------
    public CastOperatorDeclaration(Token token, TypeDeclaration declaringType)
      : base(token, declaringType)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if this is an explicit operator or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsExplicit
    {
      get { return _IsExplicit; }
      set { _IsExplicit = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this is an implicit operator or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsImplicit
    {
      get { return !_IsExplicit; }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type defines a collection of cast operator declarations that can be indexed 
  /// by the signature of the method.
  /// </summary>
  // ==================================================================================
  public class CastOperatorDeclarationCollection : 
    RestrictedIndexedCollection<CastOperatorDeclaration>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used by the indexing.
    /// </summary>
    /// <param name="item">CastOperatorDeclaration item.</param>
    /// <returns>
    /// Signature of the cast operator declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(CastOperatorDeclaration item)
    {
      return item.Signature;
    }
  }
}
