// ================================================================================================
// SourceFileNode.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.IO;
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This type defines a source file node in the syntax tree.
  /// </summary>
  // ================================================================================================
  public sealed class SourceFileNode
  {
    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SourceFileNode"/> class.
    /// </summary>
    /// <param name="fullName">The full name of the source file.</param>
    // ----------------------------------------------------------------------------------------------
    public SourceFileNode(string fullName)
    {
      Name = Path.GetFileName(fullName);
      FullName = fullName;
      UsingNodes = new ImmutableCollection<UsingNode>();
      UsingWithAliasNodes = new ImmutableCollection<UsingWithAliasNode>();
    }

    #endregion

    #region Public Properties

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the source file.
    /// </summary>
    /// <value>The name.</value>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the full name of the source file.
    /// </summary>
    /// <value>The full name.</value>
    // ----------------------------------------------------------------------------------------------
    public string FullName { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the using clauses belonging to the source file (only without using alias)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<UsingNode> UsingNodes { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the using clauses belonging to the source file (only with using alias)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<UsingWithAliasNode> UsingWithAliasNodes { get; private set; }

    #endregion

    #region Public operations

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Create a using node and add it to the source file node.
    /// </summary>
    /// <param name="start">The start.</param>
    /// <param name="namespaceNode">The namespace node.</param>
    /// <returns>The newly created using node.</returns>
    // ----------------------------------------------------------------------------------------------
    public UsingNode AddUsing(Token start, TypeOrNamespaceNode namespaceNode)
    {
      var node = new UsingNode(start, namespaceNode);
      UsingNodes.Add(node);
      return node;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Create a using node and add it to the source file node.
    /// </summary>
    /// <param name="start">The start.</param>
    /// <param name="alias">AliasToken of the using clause</param>
    /// <param name="equalToken">The equal token.</param>
    /// <param name="typeName">Name of the type.</param>
    /// <returns>The newly created using node.</returns>
    // ----------------------------------------------------------------------------------------------
    public UsingNode AddUsingWithAlias(Token start, Token alias, Token equalToken,
      TypeOrNamespaceNode typeName)
    {
      var node = new UsingWithAliasNode(start, alias, equalToken, typeName);
      UsingWithAliasNodes.Add(node);
      return node;
    }

    #endregion

    #region Output methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates the output items describing this source file.
    /// </summary>
    /// <param name="options">The output formatting options.</param>
    /// <returns>Colection of output items to be written.</returns>
    // ----------------------------------------------------------------------------------------------
    public OutputItemCollection CreateOutput(SyntaxTreeOutputOptions options)
    {
      var serializer = new OutputItemSerializer(options);
      var segment = new OutputSegment("// --- ", Name);
      serializer.Append(segment);
      return serializer.OutputItems;
    }

    #endregion
  }

  // ================================================================================================
  /// <summary>
  /// This type defines a collection of source file nodes in the syntax tree.
  /// </summary>
  // ================================================================================================
  public sealed class SourceFileNodeCollection : ImmutableIndexedCollection<SourceFileNode>
  {
    #region Overrides of ImmutableIndexedCollection<SourceFileNode>

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the key of the specified item.
    /// </summary>
    /// <param name="item">Item used to determine the key.</param>
    // ----------------------------------------------------------------------------------------------
    protected override string GetKeyOfItem(SourceFileNode item)
    {
      return item.FullName;
    }

    #endregion
  }
}