namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a function member that has a body (as opposed to having accessors).
  /// </summary>
  // ================================================================================================
  public abstract class FunctionMemberWithBodyEntity : FunctionMemberEntity, IHasBody
  {
    #region State

    /// <summary>Backing field for Body property.</summary>
    private BlockStatementEntity _Body;

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionMemberWithBodyEntity"/> class.
    /// </summary>
    /// <param name="isDeclaredInSource">True if the member is explicitly declared in source code, false otherwise.</param>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="name">The name of the member.</param>
    /// <param name="isAbstract">A value indicating whether the function member is abstract.</param>
    // ----------------------------------------------------------------------------------------------
    protected FunctionMemberWithBodyEntity(
      bool isDeclaredInSource, 
      AccessibilityKind? accessibility, 
      string name,
      bool isAbstract)
      : 
      base(isDeclaredInSource, accessibility, name)
    {
      if (!isAbstract)
      {
        Body = new BlockStatementEntity();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionMemberWithBodyEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected FunctionMemberWithBodyEntity(FunctionMemberWithBodyEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      if (template.Body != null)
      {
        Body = (BlockStatementEntity)template.Body.GetGenericClone(typeParameterMap);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the body of the function member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BlockStatementEntity Body
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
