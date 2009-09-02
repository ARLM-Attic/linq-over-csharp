using System.Collections.Generic;
using System.Linq;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a type that can declare child types.
  /// </summary>
  // ================================================================================================
  public abstract class ChildTypeCapableTypeEntity : GenericCapableTypeEntity, IHasChildTypes
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ChildTypeCapableTypeEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    protected ChildTypeCapableTypeEntity(string name)
      : base(name)
    {
      ChildTypes = new List<TypeEntity>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of child types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public List<TypeEntity> ChildTypes { get; private set;}
 
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child type. 
    /// Also sets the child's parent property, and defines child's name in the declaration space.
    /// </summary>
    /// <param name="typeEntity">The child type entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddChildType(TypeEntity typeEntity)
    {
      ChildTypes.Add(typeEntity);
      typeEntity.Parent = this;
      _DeclarationSpace.Register(typeEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a child type by name and number of type parameters.
    /// </summary>
    /// <param name="name">The name of the type.</param>
    /// <param name="typeParameterCount">The number of type parameters.</param>
    /// <returns>The type with the given name and number of type parameters, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity GetChildType(string name, int typeParameterCount)
    {
      return _DeclarationSpace.GetSingleEntity<TypeEntity>(name, typeParameterCount);
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
      base.AcceptVisitor(visitor);

      foreach (var childTypes in ChildTypes)
      {
        childTypes.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
