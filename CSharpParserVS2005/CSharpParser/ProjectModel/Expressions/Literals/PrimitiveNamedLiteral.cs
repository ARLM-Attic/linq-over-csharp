using System;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents a primitive method literal.
  /// </summary>
  // ==================================================================================
  public sealed class PrimitiveNamedLiteral : BaseNamedLiteral
  {
    #region Private fields

    private TypeReference _Type;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new primitive method literal with the specified type.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="type">Type of the literal</param>
    // --------------------------------------------------------------------------------
    public PrimitiveNamedLiteral(Token token, CSharpSyntaxParser parser, Type type)
      : base(token, parser)
    {
      _Type = new TypeReference(token, parser, type);
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of the primitive method.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference Type
    {
      get { return _Type; }
      set { _Type = value; }
    }

    #endregion
  }
}