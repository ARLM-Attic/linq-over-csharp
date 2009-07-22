namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a member of a type.
  /// </summary>
  // ================================================================================================
  public abstract class MemberEntity : SemanticEntity, INamedEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the member.</param>
    /// <param name="isExplicitlyDefined">True, if the member is explicitly defined, false otherwise.</param>
    // ----------------------------------------------------------------------------------------------
    protected MemberEntity(string name, bool isExplicitlyDefined)
    {
      Name = name;
      IsExplicitlyDefined = isExplicitlyDefined;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the distinctive name of the member, which is unique for all entities in a declaration space.
    /// Eg. a method's distinctive name is its signature.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual string DistinctiveName
    {
      get { return Name; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether the member is explicitly defined, or created by the parser.
    /// Eg. value types have a default constructor which is implicitly declared.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsExplicitlyDefined { get; private set; }
  }
}
