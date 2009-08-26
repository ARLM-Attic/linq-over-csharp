namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a function member that has a body (as opposed to having accessors).
  /// </summary>
  // ================================================================================================
  public abstract class FunctionMemberWithBodyEntity : FunctionMemberEntity, IHasBody
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionMemberWithBodyEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the member.</param>
    /// <param name="isExplicitlyDefined">A value indicating whether the member is explicitly defined.</param>
    /// <param name="isAbstract">A value indicating whether the function member is abstract.</param>
    // ----------------------------------------------------------------------------------------------
    protected FunctionMemberWithBodyEntity(string name, bool isExplicitlyDefined, bool isAbstract)
      : base(name, isExplicitlyDefined)
    {
      if (!isAbstract)
      {
        Body = new BlockEntity();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the body of the function member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BlockEntity Body { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this function member is abstract (ie. no implementation).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsAbstract
    {
      get { return Body == null; }
    }
  }
}
