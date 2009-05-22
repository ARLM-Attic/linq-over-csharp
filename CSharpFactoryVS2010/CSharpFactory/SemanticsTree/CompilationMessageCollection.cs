// ================================================================================================
// CompilationMessageCollection.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Linq;
using CSharpFactory.Collections;
using CSharpTreeBuilder.Ast;

namespace CSharpFactory.Semantics
{
  // ================================================================================================
  /// <summary>
  /// This class represents a collection of comilation message nodes.
  /// </summary>
  // ================================================================================================
  public sealed class CompilationMessageCollection : ImmutableCollection<CompilationMessageNode>,
                                                     ISemanticsNodeCollection
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes all compilation messages from the collection that belong to the specified source file 
    /// node.
    /// </summary>
    /// <param name="sourceFileNode">The source file node.</param>
    // ----------------------------------------------------------------------------------------------
    public void RemoveNodes(SourceFileNode sourceFileNode)
    {
      var messagesToRemove =
        from message in this
        where message.SourceFileNode == sourceFileNode
        select message;
      foreach (var message in messagesToRemove)
        Remove(message);
    }
  }
}