// ================================================================================================
// AttributeNode.cs
//
// Created: 2009.03.29, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This node represents an attribute with its arguments.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   AttributeNode: 
  ///     TypeOrNamespaceNode 
  ///       [ "(" [ AttributeArgumentNode ] { AttributeArgumentNode }  ")" ]
  /// </remarks>
  // ================================================================================================
  public class AttributeNode : SyntaxNode<AttributeDecorationNode>
  {
    // --- Backing fields
    private TypeOrNamespaceNode _TypeName;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeNode"/> class.
    /// </summary>
    /// <param name="identifier">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public AttributeNode(Token identifier)
      : base(identifier)
    {
      Arguments = new AttributeArgumentNodeCollection {ParentNode = this};
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name of the attribute.
    /// </summary>
    /// <value>The namespace.</value>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName
    {
      get { return _TypeName; }
      internal set
      {
        _TypeName = value;
        if (_TypeName!= null) _TypeName.ParentNode = this;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this attribute defines arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool DefinesArguments
    {
      get { return Arguments != null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the arguments belonging to this attribute.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AttributeArgumentNodeCollection Arguments { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has arguments.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has arguments; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasArguments
    {
      get { return DefinesArguments && Arguments != null && Arguments.Count > 0; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the output segment representing this syntax node.
    /// </summary>
    /// <returns>
    /// The OutputSegment instance describing this syntax node, or null; if the node has no output.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public override OutputSegment GetOutputSegment()
    {
      return new OutputSegment(
        SeparatorToken == null
          ? new OutputSegment(SeparatorToken)
          : (SeparatorToken.Value == ":"
               ? new OutputSegment(SpaceBeforeSegment.BeforeColonInAttributes(SeparatorToken),
                                   SpaceAfterSegment.AfterColonInAttributes())
               : new OutputSegment(SpaceBeforeSegment.BeforeComma(SeparatorToken),
                                   SpaceAfterSegment.AfterComma())),
        TypeName,
        Arguments
        );
    }
  }
}