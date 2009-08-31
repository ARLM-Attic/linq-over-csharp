using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a function members, that is a member that contains executable code.
  /// These are: methods, constructors, properties, indexers, events, operators, and destructors.
  /// </summary>
  // ================================================================================================
  public abstract class FunctionMemberEntity : MemberEntity, IHasDeclarationSpace
  {
    /// <summary>Backing field for DeclarationSpace property.</summary>
    protected DeclarationSpace _DeclarationSpace;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionMemberEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the member.</param>
    /// <param name="isExplicitlyDefined">True, if the member is explicitly defined, false otherwise.</param>
    // ----------------------------------------------------------------------------------------------
    protected FunctionMemberEntity(string name, bool isExplicitlyDefined)
      : base(name, isExplicitlyDefined)
    {
      _DeclarationSpace = new LocalVariableDeclarationSpace();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity allows a declaration of an entity 
    /// with the given type and entity name.
    /// </summary>
    /// <typeparam name="TEntityType">
    /// The type of the entity to be declared. Can be any semantic entity.
    /// </typeparam>
    /// <param name="name">The name of the entity to be declared.</param>
    /// <returns>True if the entity allows the declaration, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool AllowsDeclaration<TEntityType>(string name)
      where TEntityType : SemanticEntity
    {
      return _DeclarationSpace.AllowsDeclaration<TEntityType>(name);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity allows a declaration of an entity 
    /// with the given type, entity name, and number of type parameters.
    /// </summary>
    /// <typeparam name="TEntityType">
    /// The type of the entity to be declared. Must be a generic capable type entity.
    /// </typeparam>
    /// <param name="name">The name of the entity to be declared.</param>
    /// <param name="typeParameterCount">The number of type parameters of the entity to be declared.</param>
    /// <returns>True if the entity allows the declaration, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool AllowsDeclaration<TEntityType>(string name, int typeParameterCount)
      where TEntityType : GenericCapableTypeEntity
    {
      return _DeclarationSpace.AllowsDeclaration<TEntityType>(name, typeParameterCount);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity allows a declaration of an entity 
    /// with the given type and signature.
    /// </summary>
    /// <typeparam name="TEntityType">
    /// The type of the entity to be declared. Must be an overloadable semantic entity.
    /// </typeparam>
    /// <param name="signature">The signature of the entity to be declared.</param>
    /// <returns>True if the entity allows the declaration, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool AllowsDeclaration<TEntityType>(Signature signature)
      where TEntityType : SemanticEntity, IOverloadableEntity
    {
      return _DeclarationSpace.AllowsDeclaration<TEntityType>(signature);
    }
  }
}
