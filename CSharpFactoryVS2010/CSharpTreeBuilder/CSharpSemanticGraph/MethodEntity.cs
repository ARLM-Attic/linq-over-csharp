using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a method.
  /// </summary>
  // ================================================================================================
  public sealed class MethodEntity : FunctionMemberWithBodyEntity, IOverloadableEntity, ICanBePartial, ICanHaveTypeParameters
  {
    /// <summary>Backing field for Parameters property.</summary>
    private readonly List<ParameterEntity> _Parameters;

    /// <summary>Backing field for AllTypeParameters property to disallow direct adding or removing.</summary>
    private readonly List<TypeParameterEntity> _AllTypeParameters;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the member.</param>
    /// <param name="isExplicitlyDefined">A value indicating whether the member is explicitly defined.</param>
    /// <param name="isAbstract">A value indicating whether the function member is abstract.</param>
    /// <param name="isPartial">A value indicating whether this method is partial.</param>
    /// <param name="isStatic">A value indicating whether this method is static.</param>
    /// <param name="returnTypeReference">Reference to the return type.</param>
    // ----------------------------------------------------------------------------------------------
    public MethodEntity(string name, bool isExplicitlyDefined, bool isAbstract, bool isPartial, bool isStatic, 
      SemanticEntityReference<TypeEntity> returnTypeReference)
      : base(name, isExplicitlyDefined, isAbstract)
    {
      _Parameters = new List<ParameterEntity>();
      _AllTypeParameters = new List<TypeParameterEntity>();
      IsPartial = isPartial;
      IsStatic = isStatic;
      ReturnTypeReference = returnTypeReference;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this method was declared as partial.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsPartial { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the method is static.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsStatic { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference to the return type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntityReference<TypeEntity> ReturnTypeReference { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the return type. Null if not yet resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity ReturnType
    {
      get
      {
        return ReturnTypeReference == null ? null : ReturnTypeReference.TargetEntity;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<ParameterEntity> Parameters
    {
      get { return _Parameters; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a parameter.
    /// </summary>
    /// <param name="parameterEntity">A parameter entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddParameter(ParameterEntity parameterEntity)
    {
      if (parameterEntity != null)
      {
        parameterEntity.Parent = this;
        _Parameters.Add(parameterEntity);
        _DeclarationSpace.Register(parameterEntity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the signature of the member.
    /// </summary>
    /// <remarks>
    /// The signature of a method consists of the name of the method, the number of type parameters 
    /// and the type and kind (value, reference, or output) of each of its formal parameters, 
    /// considered in the order left to right.
    /// The signature of a method does not include the return type.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public Signature Signature
    {
      get { return new Signature(Name, OwnTypeParameterCount, Parameters); }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a read-only list of all type parameters of this method (parent's + own).
    /// Empty list for non-generic methods.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ReadOnlyCollection<TypeParameterEntity> AllTypeParameters
    {
      get { return _AllTypeParameters.AsReadOnly(); }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a read-only collection of the own type parameters of this method (excluding parents' params). 
    /// Empty list for non-generic methods.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ReadOnlyCollection<TypeParameterEntity> OwnTypeParameters
    {
      get
      {
        return (Parent is GenericCapableTypeEntity
                  ? _AllTypeParameters.Skip((Parent as GenericCapableTypeEntity).AllTypeParameters.Count)
                  : _AllTypeParameters)
          .ToList().AsReadOnly();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of own type parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int OwnTypeParameterCount
    {
      get
      {
        return OwnTypeParameters.Count;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a generic method. 
    /// </summary>
    /// <remarks>It's a generic method if it has any type parameters.</remarks>
    // ----------------------------------------------------------------------------------------------
    public bool IsGeneric
    {
      get { return AllTypeParameters.Count() > 0; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a type parameter to this method.
    /// </summary>
    /// <param name="typeParameterEntity">The type parameter entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddTypeParameter(TypeParameterEntity typeParameterEntity)
    {
      _AllTypeParameters.Add(typeParameterEntity);

      // If the type parameter is inherited, then the Parent property is already set, and we leave it as is.
      if (typeParameterEntity.Parent == null)
      {
        typeParameterEntity.Parent = this;
        _DeclarationSpace.Register(typeParameterEntity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an own type parameter by name.
    /// </summary>
    /// <param name="name">The name of the type parameter to be found.</param>
    /// <returns>A type parameter entity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterEntity GetOwnTypeParameterByName(string name)
    {
      return _DeclarationSpace.FindEntityByName<TypeParameterEntity>(name);
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
    }

    #endregion
  }
}
