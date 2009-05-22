// ================================================================================================
// SyntaxNodeReferenceCollection.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Collections.Generic;
using System.Linq;
using CSharpFactory.Collections;
using CSharpTreeBuilder.Ast;

namespace CSharpFactory.Semantics
{
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