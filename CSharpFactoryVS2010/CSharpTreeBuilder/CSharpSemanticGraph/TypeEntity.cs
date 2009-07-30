using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a type entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public abstract class TypeEntity : NamespaceOrTypeEntity
  {
    /// <summary>Backing field for BaseTypes property.</summary>
    protected List<SemanticEntityReference<TypeEntity>> _BaseTypes;

    /// <summary>Backing field for Members property.</summary>
    protected List<MemberEntity> _Members;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected TypeEntity()
    {
      _BaseTypes = new List<SemanticEntityReference<TypeEntity>>();
      _Members = new List<MemberEntity>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a reference type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsReferenceType
    {
      get { return false; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a value type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsValueType
    {
      get { return false; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a pointer type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsPointerType 
    {
      get { return false; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of base types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual IEnumerable<SemanticEntityReference<TypeEntity>> BaseTypes
    {
      get { return _BaseTypes; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of members.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual IEnumerable<MemberEntity> Members
    {
      get { return _Members; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a base type.
    /// </summary>
    /// <param name="typeEntityReference">A type entity reference.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddBaseType(SemanticEntityReference<TypeEntity> typeEntityReference)
    {
      _BaseTypes.Add(typeEntityReference);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a member. 
    /// Also sets the member's parent property, and defines member's name in the declaration space.
    /// </summary>
    /// <param name="memberEntity">The member entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddMember(MemberEntity memberEntity)
    {
      _Members.Add(memberEntity);
      memberEntity.Parent = this;
      DeclarationSpace.Define(memberEntity);
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

      foreach (var member in Members)
      {
        member.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}