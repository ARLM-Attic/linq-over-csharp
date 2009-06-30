// ================================================================================================
// SyntaxTreeWriter.cs
//
// Created: 2009.03.21, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class defines the behavior of a type that is able to output a syntax tree.
  /// </summary>
  // ================================================================================================
  public abstract class SyntaxTreeWriter
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxTreeWriter"/> class.
    /// </summary>
    /// <param name="tree">The syntax tree this writer uses.</param>
    /// <param name="provider">The project provider instance.</param>
    /// <param name="options">The options used for output.</param>
    // ----------------------------------------------------------------------------------------------
    protected SyntaxTreeWriter(ICSharpSyntaxTree tree, ProjectProviderBase provider,
                               SyntaxTreeOutputOptions options)
    {
      if (tree == null) throw new ArgumentNullException("tree");
      SyntaxTree = tree;
      if (provider == null) throw new ArgumentNullException("provider");
      ProjectProvider = provider;
      OutputOptions = options ?? SyntaxTreeOutputOptions.Default;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxTreeWriter"/> class.
    /// </summary>
    /// <param name="tree">The syntax tree this writer uses.</param>
    /// <param name="provider">The project provider instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected SyntaxTreeWriter(ICSharpSyntaxTree tree, ProjectProviderBase provider) :
      this(tree, provider, null)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the syntax tree this writer uses.
    /// </summary>
    /// <value>The syntax tree.</value>
    // ----------------------------------------------------------------------------------------------
    public ICSharpSyntaxTree SyntaxTree { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the project provider.
    /// </summary>
    /// <value>The project provider.</value>
    // ----------------------------------------------------------------------------------------------
    public ProjectProviderBase ProjectProvider { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the output options.
    /// </summary>
    /// <value>The output options.</value>
    // ----------------------------------------------------------------------------------------------
    public SyntaxTreeOutputOptions OutputOptions { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Writes out the whole syntax tree.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual void WriteTree()
    {
      foreach (CompilationUnitNode compilationUnitNode in SyntaxTree.CompilationUnitNodes)
      {
        OutputItemCollection outputForSource = CreateOutput(compilationUnitNode);
        WriteOutput(compilationUnitNode, outputForSource);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a list of output items according to the specified compilation unit.
    /// </summary>
    /// <param name="compilationUnitNode">The compilation unit.</param>
    /// <returns></returns>
    // ----------------------------------------------------------------------------------------------
    protected virtual OutputItemCollection CreateOutput(CompilationUnitNode compilationUnitNode)
    {
      return compilationUnitNode.CreateOutput(OutputOptions);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Writes the list of items to the output.
    /// </summary>
    /// <param name="compilationUnitNode">The compilation unit.</param>
    /// <param name="items">The items to be written to the output.</param>
    // ----------------------------------------------------------------------------------------------
    protected abstract void WriteOutput(CompilationUnitNode compilationUnitNode, OutputItemCollection items);
  }
}