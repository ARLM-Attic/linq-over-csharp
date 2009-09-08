﻿using System;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a using namespace directive in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class UsingNamespaceEntity : UsingEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UsingNamespaceEntity"/> class.
    /// </summary>
    /// <param name="lexicalScope">The region of program text where the using entity has effect.</param>
    /// <param name="namespaceOrTypeName">The AST node that is name of the imported namespace.</param>
    // ----------------------------------------------------------------------------------------------
    public UsingNamespaceEntity(SourceRegion lexicalScope, NamespaceOrTypeNameNode namespaceOrTypeName)
      : base(lexicalScope)
    {
      if (namespaceOrTypeName == null)
      {
        throw new ArgumentNullException("namespaceOrTypeName");
      }

      NamespaceReference = new NamespaceOrTypeNameNodeBasedNamespaceEntityReference(namespaceOrTypeName);
      NamespaceName = namespaceOrTypeName.TypeTags.ToString();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the imported namespace (eg. "A.B").
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string NamespaceName { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference to the imported namespace entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeNameNodeBasedNamespaceEntityReference NamespaceReference { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the imported namespace entity, or null if not yet resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceEntity ImportedNamespace
    {
      get
      {
        return NamespaceReference.TargetEntity;
      }
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      if (!visitor.Visit(this)) { return; }
    }

    #endregion

  }
}
