// ================================================================================================
// NewOperatorWithExplicitArrayNode.cs
//
// Created: 2009.05.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a "new" operator with implicit array declaration.
  /// </summary>
  // ================================================================================================
  public sealed class NewOperatorWithExplicitArrayNode : NewOperatorWithArrayNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NewOperatorWithExplicitArrayNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public NewOperatorWithExplicitArrayNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the type.
    /// </summary>
    /// <value>The name of the type.</value>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName { get; internal set; }
  }
}