// ================================================================================================
// SyntaxNodeReference.cs
//
// Created: 2009.05.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Collections.Generic;
using System.Linq;
using CSharpFactory.Collections;
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

  // ================================================================================================
  /// <summary>
  /// This class represents a collection of <see cref="SyntaxNodeReference"/> instances.
  /// </summary>
  /// <remarks>
  /// The <see cref="GetReferencesFor"/> method retrieves the references belonging to the specified
  /// source file.
  /// 
  /// </remarks>
  // ================================================================================================
  public sealed class SyntaxNodeReferenceCollection : ImmutableCollection<SyntaxNodeReference>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxNodeReferenceCollection"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SyntaxNodeReferenceCollection()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxNodeReferenceCollection"/> class.
    /// </summary>
    /// <param name="refs">The initial references.</param>
    // ----------------------------------------------------------------------------------------------
    public SyntaxNodeReferenceCollection(IEnumerable<SyntaxNodeReference> refs)
    {
      foreach(var item in refs) Add(item);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves an immutable collection of syntax node references belonging to the specified source
    /// file.
    /// </summary>
    /// <param name="sourceFile">The source file.</param>
    /// <returns>All syntax node references belonging to the source file.</returns>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<SyntaxNodeReference> GetReferencesFor(SourceFileNode sourceFile)
    {
      var result = new SyntaxNodeReferenceCollection(this.Where(
        node => node.SourceFileNode == sourceFile));
      result.MakeReadOnly();
      return result;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves an immutable collection of syntax node references with a specific type.
    /// </summary>
    /// <param name="exactMatch">
    /// The true value indicates that exact type match is used. The false value indicates that not 
    /// only the syntax nodes with the specified type but also nodes with derived types are to be
    /// retrieved.
    /// </param>
    /// <typeparam name="TNode">The type of the syntax nodes to retrieve.</typeparam>
    /// <returns>All syntax node references matching with the criteria.</returns>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<SyntaxNodeReference> GetReferencesFor<TNode>(bool exactMatch)
      where TNode: SyntaxNode
    {
      var result = new SyntaxNodeReferenceCollection(this.Where(
        node => node.SyntaxNode.GetType() == typeof(TNode) || 
          !exactMatch && node.SyntaxNode.GetType().IsSubclassOf(typeof(TNode))));
      result.MakeReadOnly();
      return result;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves an immutable collection of syntax node references with a specific type.
    /// </summary>
    /// <typeparam name="TNode">The type of the syntax nodes to retrieve.</typeparam>
    /// <returns>All syntax node references matching with the criteria.</returns>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<SyntaxNodeReference> GetReferencesFor<TNode>()
      where TNode : SyntaxNode
    {
      return GetReferencesFor<TNode>(true);
    }
  }
}