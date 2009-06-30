// ================================================================================================
// SyntaxNodeReferenceCollection.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.Collections;

namespace CSharpTreeBuilder.Cst
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
    /// Retrieves an immutable collection of syntax node references belonging to the specified compilation unit.
    /// </summary>
    /// <param name="compilationUnitNode">The compilation unit.</param>
    /// <returns>All syntax node references belonging to the compilation unit.</returns>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<SyntaxNodeReference> GetReferencesFor(CompilationUnitNode compilationUnitNode)
    {
      var result = new SyntaxNodeReferenceCollection(this.Where(
                                                       node => node.CompilationUnitNode == compilationUnitNode));
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
      where TNode: ISyntaxNode
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
      where TNode : ISyntaxNode
    {
      return GetReferencesFor<TNode>(true);
    }
  }
}