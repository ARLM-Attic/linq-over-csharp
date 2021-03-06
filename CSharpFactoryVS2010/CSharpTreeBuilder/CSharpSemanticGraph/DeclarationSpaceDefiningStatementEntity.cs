﻿using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of statements that can include local variable declarations.
  /// </summary>
  // ================================================================================================
  public abstract class DeclarationSpaceDefiningStatementEntity : StatementEntity, IDefinesLocalVariableDeclarationSpace
  {
    #region State

    /// <summary>The statement's local variable declaration space.</summary>
    private readonly LocalVariableDeclarationSpace _DeclarationSpace = new LocalVariableDeclarationSpace();

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DeclarationSpaceDefiningStatementEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected DeclarationSpaceDefiningStatementEntity()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DeclarationSpaceDefiningStatementEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected DeclarationSpaceDefiningStatementEntity(DeclarationSpaceDefiningStatementEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      // Declaration spaces are intentionally not cloned.
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an entity from the declaration space by name.
    /// </summary>
    /// <param name="name">The name of the declared entity.</param>
    /// <returns>The entity declared with the supplied name or null if no such declaration.</returns>
    // ----------------------------------------------------------------------------------------------
    public INamedEntity GetDeclaredEntityByName(string name)
    {
      return _DeclarationSpace.GetSingleEntity<INamedEntity>(name);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds the declaration of an entity.
    /// </summary>
    /// <param name="namedEntity">A named entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddDeclaration(INamedEntity namedEntity)
    {
      _DeclarationSpace.Register(namedEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes the declaration of an entity.
    /// </summary>
    /// <param name="namedEntity">A named entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void RemoveDeclaration(INamedEntity namedEntity)
    {
      _DeclarationSpace.Unregister(namedEntity);
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
      visitor.Visit(this);
      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}
