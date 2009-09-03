﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of all kinds of nodes in the semantic graph (namespace, type, etc.).
  /// </summary>
  // ================================================================================================
  public abstract class SemanticEntity
  {
    /// <summary>Backing field for SyntaxNodes property.</summary>
    private readonly List<ISyntaxNode> _SyntaxNodes;

    /// <summary>Backing field for Parent property.</summary>
    protected SemanticEntity _Parent;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected SemanticEntity()
    {
      _SyntaxNodes = new List<ISyntaxNode>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parent of this entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual SemanticEntity Parent
    {
      get { return _Parent; }
      set { _Parent = value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the root namespace of this entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public RootNamespaceEntity RootNamespace
    {
      get 
      {
        if (this is RootNamespaceEntity)
        {
          return this as RootNamespaceEntity;
        }

        if (Parent == null)
        {
          return null;
        }

        return Parent.RootNamespace; 
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the reflected metadata (eg. type) that this entity was created from.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public System.Reflection.MemberInfo ReflectedMetadata { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a read-only list of syntax nodes that generated this semantic entity. Can be empty.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ReadOnlyCollection<ISyntaxNode> SyntaxNodes
    {
      get { return _SyntaxNodes.AsReadOnly(); }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a syntax node to the SyntaxNodes collection.
    /// </summary>
    /// <param name="syntaxNode">A syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddSyntaxNode(ISyntaxNode syntaxNode)
    {
      _SyntaxNodes.Add(syntaxNode);
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public virtual void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      throw new ApplicationException(string.Format("SemanticEntity.AcceptVisitor called on type: {0}", GetType()));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Visits a collection where the items can change during the visiting. 
    /// Therefore first creates a copy of the collection 
    /// and checks before the visiting of a node if it still has its parent.
    /// </summary>
    /// <typeparam name="T">The type of collection items.</typeparam>
    /// <param name="collection">A collection.</param>
    /// <param name="visitor">A semantic graph visitor object.</param>
    // ----------------------------------------------------------------------------------------------
    protected static void VisitMutableCollection<T>(IEnumerable<T> collection, SemanticGraphVisitor visitor)
      where T : SemanticEntity
    {
      foreach (var item in collection.ToArray())
      {
        if (item.Parent != null)
        {
          item.AcceptVisitor(visitor);
        }
      }
    }

    #endregion
  }
}
