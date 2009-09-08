// ================================================================================================
// FieldTagNode.cs
//
// Created: 2009.05.16, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a field member declaration tag node.
  /// </summary>
  // ================================================================================================
  public class FieldTagNode : MemberDeclarationNode
  {
    /// <summary>
    /// Backing field for Initializer property.
    /// </summary>
    private VariableInitializerNode _Initializer;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FieldTagNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public FieldTagNode(Token start)
      : base(start)
    {
      IdentifierToken = start;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the equal token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token EqualToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public VariableInitializerNode Initializer
    {
      get
      {
        return _Initializer;
      }

      internal set
      {
        _Initializer = value;
        if (_Initializer != null)
        {
          _Initializer.ParentNode = this;
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasInitializer
    {
      get { return Initializer != null; }
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      if (!visitor.Visit(this)) { return; }

      if (Initializer!=null)
      {
        Initializer.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}