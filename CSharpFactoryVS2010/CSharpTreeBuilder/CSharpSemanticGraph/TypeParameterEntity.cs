using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a type parameter (aka generic parameter) entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class TypeParameterEntity : TypeEntity
  {
    /// <summary>
    /// Backing field for TypeReferenceConstraints property.
    /// </summary>
    private readonly List<SemanticEntityReference<TypeEntity>> _TypeReferenceConstraints;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParameterEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the type parameter.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterEntity(string name)
      : base(null, name)
    {
      _TypeReferenceConstraints = new List<SemanticEntityReference<TypeEntity>>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value specifying whether this type parameter has a default constructor constraint 
    /// (ie "new()").
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasDefaultConstructorConstraint { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value specifying whether this type parameter has a "class" constraint.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasReferenceTypeConstraint { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value specifying whether this type parameter has a "struct" constraint.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasNonNullableValueTypeConstraint { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only list of constraints that are specified with type name.
    /// </summary>
    /// <remarks>
    /// Before type name resolution it is not possible to distinguish 
    /// class types, interface types and type parameters.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<SemanticEntityReference<TypeEntity>> TypeReferenceConstraints
    {
      get { return _TypeReferenceConstraints; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a constraint that is given with a type reference.
    /// </summary>
    /// <param name="typeReference">A type reference.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddTypeReferenceConstraint(SemanticEntityReference<TypeEntity> typeReference)
    {
      if (typeReference != null)
      {
        _TypeReferenceConstraints.Add(typeReference);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only list of constraints that are resolved to a class type.
    /// </summary>
    /// <remarks>
    /// In a semantically correct program there can be at most 1 class type among the constraints,
    /// but the data model must be able to represent erroneous constraints too.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TypeEntity> ClassTypeConstraints
    {
      get
      {
        return from typeReference in _TypeReferenceConstraints
               where typeReference.ResolutionState == ResolutionState.Resolved
                     && typeReference.TargetEntity.IsClassType
               select typeReference.TargetEntity;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the class type constraint.
    /// </summary>
    /// <remarks>
    /// If none or more than one constraint is a class type constraint the returns null.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity ClassTypeConstraint
    {
      get
      {
        var classTypeConstraints = (from typeReference in _TypeReferenceConstraints
                                    where typeReference.ResolutionState == ResolutionState.Resolved
                                          && typeReference.TargetEntity.IsClassType
                                    select typeReference.TargetEntity).ToList();

        return classTypeConstraints.Count == 1 ? classTypeConstraints[0] : null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only list of constraints that are resolved to a type parameter.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TypeParameterEntity> TypeParameterConstraints
    {
      get
      {
        return from typeReference in _TypeReferenceConstraints
               where typeReference.ResolutionState == ResolutionState.Resolved
                     && typeReference.TargetEntity is TypeParameterEntity
               select typeReference.TargetEntity as TypeParameterEntity;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only list of constraints that are resolved to an interface type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TypeEntity> InterfaceTypeConstraints
    {
      get
      {
        return from typeReference in _TypeReferenceConstraints
               where typeReference.ResolutionState == ResolutionState.Resolved
                     && typeReference.TargetEntity.IsInterfaceType
               select typeReference.TargetEntity;
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
    }

    #endregion
  }
}
