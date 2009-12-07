using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a function members, that is a member that contains executable code.
  /// These are: methods, constructors, properties, indexers, events, operators, and destructors.
  /// </summary>
  // ================================================================================================
  public abstract class FunctionMemberEntity : NonTypeMemberEntity, IDefinesLocalVariableDeclarationSpace
  {
    #region State

    /// <summary>Backing field for DeclarationSpace property.</summary>
    protected DeclarationSpace _DeclarationSpace = new LocalVariableDeclarationSpace();

    /// <summary>Gets or sets a value indicating whether this member can be overridden.</summary>
    public bool IsVirtual { get; set; }

    /// <summary>Gets or sets a value indicating whether this member is on override of an inherited member.</summary>
    public bool IsOverride { get; set; }

    /// <summary>Gets or sets a value indicating whether this member is sealed, meaning that it cannot be overridden.</summary>
    public bool IsSealed { get; set; }

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
    // ----------------------------------------------------------------------------------------------
    protected FunctionMemberEntity(FunctionMemberEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      // DeclarationSpace should not be cloned.
      IsVirtual = template.IsVirtual;
      IsOverride = template.IsOverride;
      IsSealed = template.IsSealed;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an entity from the declaration space by name.
    /// </summary>
    /// <param name="name">The name of the declared entity.</param>
    /// <returns>The entity declared with the supplied name or null if no such declaration.</returns>
    // ----------------------------------------------------------------------------------------------
    public INamedEntity GetDeclaredEntityByName(string name)
    {
      return _DeclarationSpace.GetSingleEntity<INamedEntity>(name);
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
    }

    #endregion
  }
}
