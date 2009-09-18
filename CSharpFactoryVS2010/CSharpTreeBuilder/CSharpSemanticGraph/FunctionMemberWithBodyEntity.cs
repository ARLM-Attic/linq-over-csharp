namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a function member that has a body (as opposed to having accessors).
  /// </summary>
  // ================================================================================================
  public abstract class FunctionMemberWithBodyEntity : FunctionMemberEntity, IHasBody
  {
    /// <summary>
    /// Backing field for Body property.
    /// </summary>
    private BlockEntity _Body;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionMemberWithBodyEntity"/> class.
    /// </summary>
    /// <param name="isExplicitlyDefined">A value indicating whether the member is explicitly defined.</param>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="name">The name of the member.</param>
    /// <param name="isAbstract">A value indicating whether the function member is abstract.</param>
    // ----------------------------------------------------------------------------------------------
    protected FunctionMemberWithBodyEntity(
      bool isExplicitlyDefined, 
      AccessibilityKind? accessibility, 
      string name,
      bool isAbstract)
      : 
      base(isExplicitlyDefined, accessibility, name)
    {
      if (!isAbstract)
      {
        Body = new BlockEntity();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the body of the function member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BlockEntity Body
    {
      get
      {
        return _Body;
      }

      set
      {
        _Body = value;
        if (_Body != null)
        {
          _Body.Parent = this;
        }
      }
    }

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
