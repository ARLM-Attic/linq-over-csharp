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
    /// <summary>Backing field for Members property.</summary>
    private List<MemberEntity> _Members;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected TypeEntity()
    {
      BaseTypes = new List<NamespaceOrTypeEntityReference>();
      _Members = new List<MemberEntity>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of base types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public List<NamespaceOrTypeEntityReference> BaseTypes { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of members.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<MemberEntity> Members { get { return _Members; } }

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