using System;
using System.Collections.Generic;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a local constant declaration statement entity.
  /// </summary>
  // ================================================================================================
  public sealed class LocalConstantDeclarationStatementEntity : StatementEntity, IHasLocalConstants
  {
    #region State

    /// <summary>Backing field for LocalConstants property.</summary>
    private readonly List<LocalConstantEntity> _LocalConstants = new List<LocalConstantEntity>();

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LocalConstantDeclarationStatementEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public LocalConstantDeclarationStatementEntity()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LocalConstantDeclarationStatementEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private LocalConstantDeclarationStatementEntity(LocalConstantDeclarationStatementEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      foreach(var constant in template.LocalConstants)
      {
        _LocalConstants.Add((LocalConstantEntity)constant.GetGenericClone(typeParameterMap));
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new constructed entity.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A new semantic entity constructed from this entity using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected override SemanticEntity ConstructNew(TypeParameterMap typeParameterMap)
    {
      return new LocalConstantDeclarationStatementEntity(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child entity.
    /// </summary>
    /// <param name="entity">A child entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void AddChild(ISemanticEntity entity)
    {
      if (entity is LocalConstantEntity)
      {
        AddLocalConstant(entity as LocalConstantEntity);
      }
      else
      {
        base.AddChild(entity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of the constants declared in this statement.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<LocalConstantEntity> LocalConstants
    {
      get
      {
        return _LocalConstants;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a constant to the declaration.
    /// </summary>
    /// <param name="constantEntity">A local constant entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddLocalConstant(LocalConstantEntity constantEntity)
    {
      if (constantEntity != null)
      {
        _LocalConstants.Add(constantEntity);
        constantEntity.Parent = this;

        // Find enclosing block
        var enclosingBlock = constantEntity.GetEnclosing<IDefinesLocalVariableDeclarationSpace>();
        if (enclosingBlock == null)
        {
          throw new ApplicationException(
            string.Format("No enclosing block found for constant '{0}'.", constantEntity));
        }

        enclosingBlock.AddDeclaration(constantEntity);
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
      visitor.Visit(this);
      base.AcceptVisitor(visitor);

      foreach (var constant in LocalConstants)
      {
        constant.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
