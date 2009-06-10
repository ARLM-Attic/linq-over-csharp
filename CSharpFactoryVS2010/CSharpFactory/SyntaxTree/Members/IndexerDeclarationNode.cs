// ================================================================================================
// IndexerDeclarationNode.cs
//
// Created: 2009.05.18, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.AstFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class defines an indexer.
  /// </summary>
  // ================================================================================================
  public class IndexerDeclarationNode: PropertyDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="IndexerDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public IndexerDeclarationNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the optional dot token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token DotToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the "this" token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token ThisToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the formal parameters of the indexer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public FormalParameterListNode FormalParameters { get; internal set; }
 }
}