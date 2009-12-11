using System;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a variable that is not a field (eg. parameter, array element).
  /// </summary>
  // ================================================================================================
  public abstract class NonFieldVariableEntity : SemanticEntity, IVariableEntity
  {
    #region State

    /// <summary>Gets the name of the variable.</summary>
    public string Name { get; private set; }

    /// <summary>Gets the type reference of the variable.</summary>
    public Resolver<TypeEntity> TypeReference { get; private set; }

    /// <summary>Gets the initializer of the variable.</summary>
    public VariableInitializer Initializer { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NonFieldVariableEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="typeReference">A reference to the type of the variable.</param>
    /// <param name="initializer">The initializer of the variable.</param>
    // ----------------------------------------------------------------------------------------------
    protected NonFieldVariableEntity(
      string name, 
      Resolver<TypeEntity> typeReference, 
      VariableInitializer initializer)
    {
      if (name == null)
      {
        throw new ArgumentNullException("name");
      }
      if (typeReference == null)
      {
        throw new ArgumentNullException("typeReference");
      }

      Name = name;
      TypeReference = typeReference;
      Initializer = initializer;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NonFieldVariableEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected NonFieldVariableEntity(NonFieldVariableEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      Name = template.Name;

      if (template.TypeReference != null)
      {
        TypeReference = (Resolver<TypeEntity>)template.TypeReference.GetGenericClone(typeParameterMap);
      }

      if (template.Initializer != null)
      {
        Initializer = (VariableInitializer)template.Initializer.GetGenericClone(typeParameterMap);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the fully qualified name of the variable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string FullyQualifiedName
    {
      get
      {
        return Name;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the variable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Type
    {
      get
      {
        return TypeReference != null ? TypeReference.Target : null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this variable is an array. 
    /// Null if the type of the variable is not yet resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool? IsArray
    {
      get
      {
        return Type == null ? null : Type.IsArrayType as bool?;
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

      if (Initializer != null)
      {
        Initializer.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
