using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a function members, that is a member that contains executable code.
  /// These are: methods, constructors, properties, indexers, events, operators, and destructors.
  /// </summary>
  // ================================================================================================
  public abstract class FunctionMemberEntity : NonTypeMemberEntity
  {
    #region State

    /// <summary>Backing field for DeclarationSpace property.</summary>
    protected DeclarationSpace _DeclarationSpace = new LocalVariableDeclarationSpace();

    /// <summary>Gets or sets a value indicating whether this member can be overridden.</summary>
    public virtual bool IsVirtual { get; set; }

    /// <summary>Gets or sets a value indicating whether this member is on override of an inherited member.</summary>
    public virtual bool IsOverride { get; set; }

    /// <summary>Gets or sets a value indicating whether this member is sealed, meaning that it cannot be overridden.</summary>
    public virtual bool IsSealed { get; set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionMemberEntity"/> class.
    /// </summary>
    /// <param name="isDeclaredInSource">True if the member is explicitly declared in source code, false otherwise.</param>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="name">The name of the member.</param>
    // ----------------------------------------------------------------------------------------------
    protected FunctionMemberEntity(bool isDeclaredInSource, AccessibilityKind? accessibility, string name)
      : base(isDeclaredInSource, accessibility, name)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionMemberEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    /// <param name="resolveTypeParameters">True to resolve type parameters immediately, false to defer it.</param>
    // ----------------------------------------------------------------------------------------------
    protected FunctionMemberEntity(FunctionMemberEntity template, TypeParameterMap typeParameterMap, bool resolveTypeParameters)
      : base(template, typeParameterMap, resolveTypeParameters)
    {
      // DeclarationSpace should not be cloned.
      IsVirtual = template.IsVirtual;
      IsOverride = template.IsOverride;
      IsSealed = template.IsSealed;
    }
  }
}
