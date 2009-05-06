// ================================================================================================
// LocalVariableNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax.Variables
{
  // ================================================================================================
  /// <summary>
  /// This class represents a local variable declaration
  /// </summary>
  // ================================================================================================
  public class LocalVariableNode : SyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LocalVariableNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public LocalVariableNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is an implicit type declaration.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is implicit; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool IsImplicit
    {
      get { return TypeName.TypeTags.Count == 1 && TypeName.StartToken.val == "var"; }
    }

  }
}