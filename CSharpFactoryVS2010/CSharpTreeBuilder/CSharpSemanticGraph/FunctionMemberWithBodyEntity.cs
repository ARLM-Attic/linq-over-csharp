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
    /// <param name="isExplicitlyDefined">True, if the member is explicitly defined, false otherwise.</param>
    // ----------------------------------------------------------------------------------------------
    protected FunctionMemberWithBodyEntity(string name, bool isExplicitlyDefined)
      : base(name, isExplicitlyDefined)
    {
      Body = new BlockEntity();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the body of the function member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BlockEntity Body { get; private set; }
  }
}
