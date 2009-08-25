using System.Collections.Generic;

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
      DeclarationSpace.Define(typeEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a child type by name. 
    /// </summary>
    /// <param name="distinctiveName">A distinctive name.</param>
    /// <returns>The type with the given name, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity GetChildTypeByDistinctiveName(string distinctiveName)
    {
      if (DeclarationSpace.IsNameDefined(distinctiveName))
      {
        var nameTableEntry = DeclarationSpace[distinctiveName];
        if (nameTableEntry.State == NameTableEntryState.Definite && nameTableEntry.Entity is TypeEntity)
        {
          return nameTableEntry.Entity as TypeEntity;
        }
      }
      return null;
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
