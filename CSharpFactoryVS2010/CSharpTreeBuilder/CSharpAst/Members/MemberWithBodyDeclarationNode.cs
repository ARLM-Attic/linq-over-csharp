// ================================================================================================
// MemberWithBodyDeclarationNode.cs
//
// Created: 2009.04.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a type member declaration with a body.
  /// </summary>
  // ================================================================================================
  public abstract class MemberWithBodyDeclarationNode : MemberDeclarationNode
  {
    /// <summary>
    /// Backing field for Body property.
    /// </summary>
    private BlockStatementNode _Body;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberWithBodyDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected MemberWithBodyDeclarationNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the body of this member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BlockStatementNode Body
    {
      get { return _Body; }
      internal set
      {
        _Body = value;
        if (_Body != null)
        {
          _Body.ParentNode = this;
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has body.
    /// </summary>
    /// <value><c>true</c> if this instance has body; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public bool HasBody
    {
      get { return Body != null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the closing semicolon.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token ClosingSemicolon { get; internal set; }
  }
}