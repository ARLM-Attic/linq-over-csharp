// ================================================================================================
// FieldTagNode.cs
//
// Created: 2009.05.16, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a field member declaration tag node.
  /// </summary>
  // ================================================================================================
  public class FieldTagNode : MemberDeclarationNode
  {
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
    public VariableInitializerNode Initializer { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasInitializer { get { return Initializer != null; } }
  }
}