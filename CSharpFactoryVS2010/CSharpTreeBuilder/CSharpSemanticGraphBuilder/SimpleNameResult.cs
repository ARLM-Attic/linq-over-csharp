using System;
using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents the result of a simple name resolution. 
  /// The result is either a method group or a single semantic entity.
  /// </summary>
  // ================================================================================================
  public sealed class SimpleNameResult
  {
    #region State

    /// <summary>
    /// The single entity result of the simple name resolution.
    /// </summary>
    public ISemanticEntity SingleEntity { get; private set; }

    /// <summary>
    /// The method group result of the simple name resolution.
    /// </summary>
    public MethodGroup MethodGroup { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNameResult"/> class
    /// with a collection of semantic entities.
    /// </summary>
    /// <param name="entities">A collection of semantic entities.</param>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameResult(IEnumerable<ISemanticEntity> entities)
    {
      var entityList = entities.ToList();

      // If the set consists of a single entity that is not a method, then this entity is the result of the lookup.
      if (entityList.Count == 1 && !(entityList[0] is MethodEntity))
      {
        SingleEntity = entityList[0];
      }
      // Otherwise, if the set contains only methods, then this group of methods is the result of the lookup.
      else if (entityList.Count > 0 && entityList.All(member => member is MethodEntity))
      {
        MethodGroup = new MethodGroup(entities.Cast<MethodEntity>());
      }

      // Otherwise, the result is ambiguous
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNameResult"/> class
    /// with a single entity.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameResult(ISemanticEntity entity)
      : this(new[] { entity })
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNameResult"/> class
    /// with a method group.
    /// </summary>
    /// <param name="methodGroup">A method group.</param>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameResult(MethodGroup methodGroup)
      : this(methodGroup.Methods.Cast<ISemanticEntity>())
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this result is empty.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsEmpty
    {
      get { return SingleEntity == null && MethodGroup == null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this result is a single entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsSingleEntity
    {
      get { return SingleEntity != null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this result is a method group.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsMethodGroup
    {
      get { return MethodGroup != null; }
    }
  }
}
