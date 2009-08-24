namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a function members, that is a member that contains executable code.
  /// These are: methods, constructors, properties, indexers, events, operators, and destructors.
  /// </summary>
  // ================================================================================================
  public abstract class FunctionMemberEntity : MemberEntity, IDefinesDeclarationSpace
  {
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
      DeclarationSpace = new LocalVariableDeclarationSpace();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the declaration space of the member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public DeclarationSpace DeclarationSpace { get; private set; }
  }
}
