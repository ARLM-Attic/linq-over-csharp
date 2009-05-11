// ================================================================================================
// CompilationEntity.cs
//
// Created: 2009.05.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;

namespace CSharpFactory.Semantics
{
  // ================================================================================================
  /// <summary>
  /// This class intended to be the base class of all compilation model entities.
  /// </summary>
  /// <remarks>
  /// A CompilationEntity instance can reference to zero, one or more syntax nodes. For example, an 
  /// enumerated type declaration can reference to one syntax node that starts with the "enum" token 
  /// of the type declaration and is termintated with the closing "}". A partial class declaration
  /// can have not only one but even two or more segments even is separate source files, so in this 
  /// case each segment refers to a separate syntax node.
  /// Use the <see cref="AddSyntaxNodeReference"/> method to add a reference to a syntax node. You 
  /// can call this method zero, one or more times. All calls add the passed syntax node to the 
  /// collection represented by <see cref="SyntaxNodes"/>. The very first call set the 
  /// <see cref="SyntaxNode"/> property as well.
  /// </remarks>
  // ================================================================================================
  public abstract class CompilationEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CompilationEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected CompilationEntity()
    {
      SyntaxNodes = new SyntaxNodeReferenceCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the syntax node.
    /// </summary>
    /// <value>The syntax node.</value>
    // ----------------------------------------------------------------------------------------------
    public SyntaxNodeReference SyntaxNode { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the syntax nodes.
    /// </summary>
    /// <value>The syntax nodes.</value>
    // ----------------------------------------------------------------------------------------------
    public SyntaxNodeReferenceCollection SyntaxNodes { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds the syntax node reference.
    /// </summary>
    /// <param name="nodeReference">The node reference.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddSyntaxNodeReference(SyntaxNodeReference nodeReference)
    {
      if (nodeReference == null) throw new ArgumentNullException("nodeReference");
      if (SyntaxNode == null) SyntaxNode = nodeReference;
      SyntaxNodes.Add(nodeReference);
    }
  }
}