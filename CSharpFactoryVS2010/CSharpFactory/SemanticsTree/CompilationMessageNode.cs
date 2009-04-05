// ================================================================================================
// CompilationMessageNode.cs
//
// Created: 2009.04.05, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Collections.Generic;
using System.Linq;
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;
using CSharpFactory.Syntax;

namespace CSharpFactory.Semantics
{
  // ================================================================================================
  /// <summary>
  /// This class represents a compilation message node.
  /// </summary>
  // ================================================================================================
  public class CompilationMessageNode: SemanticsNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CompilationMessageNode"/> class.
    /// </summary>
    /// <param name="sourceFileNode">The source file node.</param>
    /// <param name="code">The message code.</param>
    /// <param name="token">The message token.</param>
    /// <param name="description">The textual description.</param>
    // ----------------------------------------------------------------------------------------------
    public CompilationMessageNode(SourceFileNode sourceFileNode, string code, Token token,
      string description)
      : base(sourceFileNode)
    {
      Code = code;
      MessageToken = token;
      Description = description;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the code of the message.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Code { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the descriptive text belonging to the message.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Description { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the token belonging to the message.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token MessageToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the other optional message related tokens.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<Token> RelatedTokens { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the optional parameters belonging to the message.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<object> Parameters { get; internal set; }
  }

  // ================================================================================================
  /// <summary>
  /// This class represents a collection of comilation message nodes.
  /// </summary>
  // ================================================================================================
  public sealed class CompilationMessages : ImmutableCollection<CompilationMessageNode>,
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