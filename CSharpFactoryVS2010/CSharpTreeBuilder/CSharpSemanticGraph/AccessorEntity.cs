namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an accessor. (eg. get accessor of a property)
  /// </summary>
  // ================================================================================================
  public class AccessorEntity : SemanticEntity, IHasBody
  {
    /// <summary>
    /// Backing field for Body property.
    /// </summary>
    private BlockEntity _Body;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AccessorEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AccessorEntity(bool isAbstract)
    {
      if (!isAbstract)
      {
        Body = new BlockEntity();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the body of the accessor.
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
    /// Gets a value indicating whether this accessor is abstract (ie. no implementation).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsAbstract
    {
      get { return Body == null; }
    }
  }
}
