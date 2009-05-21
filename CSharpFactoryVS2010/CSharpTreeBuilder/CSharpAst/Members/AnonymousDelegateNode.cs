// ================================================================================================
// AnonymousDelegateNode.cs
//
// Created: 2009.05.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents an anonymous delegate.
  /// </summary>
  // ================================================================================================
  public class AnonymousDelegateNode : PrimaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AnonymousDelegateNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public AnonymousDelegateNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parameter list.
    /// </summary>
    /// <value>The parameter list.</value>
    // ----------------------------------------------------------------------------------------------
    public FormalParameterListNode ParameterList { get; internal set; }
  }
}