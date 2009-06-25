// ================================================================================================
// SourceFileNode.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.Collections.Generic;
using System.IO;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This type defines a source file node in the syntax tree.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>{ <em>UsingNamespaceNode</em> | <em>UsingAliasNode</em> } {
  ///         <em>ExternAliasNode</em> } { <em>AttributeDecorationNode</em> }<br/>
  ///         { <em>NamespaceDeclarationNode</em> | <em>TypeDeclarationNode</em> }</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  ///             { <em>ExternAliasNode</em> }: see <see cref="NamespaceScopeNode"/><br/>
  ///             { <em>UsingNamespaceNode</em> | <em>UsingAliasNode</em> }: see <see cref="NamespaceScopeNode"/><br/>
  ///             { <em>AttributeDecorationNode</em> }: <see cref="GlobalAttributes"/><br/>
  ///             { <em>NamespaceDeclarationNode</em> | <em>TypeDeclarationNode</em> }: see
  ///             <see cref="NamespaceScopeNode"/><br/>
  ///             { <em>NamespaceDeclarationNode</em> }: see <see cref="NamespaceScopeNode"/><br/>
  ///             { <em>TypeDeclarationNode</em> }: see <see cref="NamespaceScopeNode"/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public sealed class SourceFileNode : NamespaceScopeNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SourceFileNode"/> class.
    /// </summary>
    /// <param name="fullName">The full name of the source file.</param>
    // ----------------------------------------------------------------------------------------------
    public SourceFileNode(string fullName) : base(null, null)
    {
      Name = Path.GetFileName(fullName);
      FullName = fullName;
      GlobalAttributes = new AttributeDecorationNodeCollection {ParentNode = this};
      Pragmas = new PragmaNodeCollection {ParentNode = this};
      TokenizedUnit = new SymbolStream();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SourceFileNode"/> class without a filename.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SourceFileNode()
      : this(null)
    {
    }

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
    /// Gets the global attributes belonging to this source file.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AttributeDecorationNodeCollection GlobalAttributes { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the collection of pragmas belonging to this source file.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public PragmaNodeCollection Pragmas { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of top level "#region" pragmas belonging to this source file.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<RegionPragmaNode> Regions
    {
      get
      {
        foreach (var pragma in Pragmas)
        {
          var regionPragma = pragma as RegionPragmaNode;
          if (regionPragma != null && regionPragma.ParentRegion == null) 
            yield return regionPragma;
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the tokenized version of this compilation unit.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SymbolStream TokenizedUnit { get; private set; }

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
      serializer.Append(GetOutputSegment());
      return serializer.OutputItems;
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
        ExternAliasNodes,
        UsingNodes,
        GlobalAttributes,
        InScopeDeclarations
        );
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a UsingNamespaceNode to the UsingNodes collection
    /// </summary>
    /// <param name="node">a UsingNamespaceNode</param>
    // ----------------------------------------------------------------------------------------------
    public void AddChild(UsingNamespaceNode node)
    {
      node.ParentNode = this;
      UsingNodes.Add(node);
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      visitor.Visit(this);

      // Visit extern alias nodes
      foreach (var node in ExternAliasNodes)
      {
        node.AcceptVisitor(visitor);
      }

      // Visit using nodes
      foreach (var node in UsingNodes)
      {
        node.AcceptVisitor(visitor);
      }

      // Visit extern alias nodes
      foreach (var node in GlobalAttributes)
      {
        node.AcceptVisitor(visitor);
      }

      // Visit in-scope declaration nodes (namespaces and types)
      foreach (var node in InScopeDeclarations)
      {
        node.AcceptVisitor(visitor);
      }
    }

    #endregion

  }
}