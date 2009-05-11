// ================================================================================================
// SyntaxNodeReference.cs
//
// Created: 2009.05.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Syntax;

namespace CSharpFactory.Semantics
{
  // ================================================================================================
  /// <summary>
  /// This class provides a reference to a syntax node that is located in the specific sourcefile in
  /// at a specific location.
  /// </summary>
  /// <remarks>
  /// An instance of this class is used to point to a specific syntax node belonging to a certain
  /// item in the entity model. The SourceFileNode property points to the file that holds the syntax
  /// node, while SyntaxNode declares the node within the syntax tree.
  /// This is a design decision according to the following reasons:
  /// 1) However, a syntax node obviously belongs to one and only one source file, the current 
  /// syntax tree does not allow to navigate from a syntax tree node to its parent syntax node and
  /// so does not make it possible to access the related source file node.
  /// 2) It is a design goal to support simple operations to remove all nodes from the syntax tree 
  /// and from the entity model that belongs to a specific source file.
  /// </remarks>
  // ================================================================================================
  public class SyntaxNodeReference
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxNodeReference"/> class.
    /// </summary>
    /// <param name="sourceFileNode">The source file node.</param>
    /// <param name="syntaxNode">The syntax node within the source file node.</param>
    // ----------------------------------------------------------------------------------------------
    public SyntaxNodeReference(SourceFileNode sourceFileNode, SyntaxNode syntaxNode)
    {
      SourceFileNode = sourceFileNode;
      SyntaxNode = syntaxNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source file node of this syntax node reference.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SourceFileNode SourceFileNode { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the syntax node reference.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SyntaxNode SyntaxNode { get; private set; }
  }
}