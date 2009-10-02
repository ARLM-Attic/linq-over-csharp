using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using System;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a field of a type.
  /// </summary>
  // ================================================================================================
  public sealed class FieldEntity : NonTypeMemberEntity, IVariableEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FieldEntity"/> class.
    /// </summary>
    /// <param name="isDeclaredInSource">True if the member is explicitly declared in source code, false otherwise.</param>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="isStatic">True, if the field is static, false otherwise.</param>
    /// <param name="type">The type of the field (a type entity reference).</param>
    /// <param name="name">The name of the member.</param>
    /// <param name="initializer">The initializer of the field.</param>
    // ----------------------------------------------------------------------------------------------
    public FieldEntity(
      bool isDeclaredInSource,
      AccessibilityKind? accessibility,
      bool isStatic,
      SemanticEntityReference<TypeEntity> type,
      string name, 
      VariableInitializer initializer)
      : 
      base(isDeclaredInSource, accessibility, name)
    {
      _IsStatic = isStatic;
      TypeReference = type;
      Initializer = initializer;

      if (Initializer != null)
      {
        Initializer.Parent = this;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the field.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntityReference<TypeEntity> TypeReference { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the field.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Type
    {
      get { return TypeReference == null ? null : TypeReference.TargetEntity; }
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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the initializer of the variable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public VariableInitializer Initializer { get; private set; }
    
    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      if (!visitor.Visit(this)) { return; }

      if (Initializer!=null)
      {
        Initializer.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
