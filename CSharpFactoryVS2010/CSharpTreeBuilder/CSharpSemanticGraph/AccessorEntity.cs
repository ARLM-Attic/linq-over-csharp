namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an accessor. (eg. get accessor of a property)
  /// </summary>
  // ================================================================================================
  public class AccessorEntity : SemanticEntity, IHasBody
  {
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
    /// Gets the body of the accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BlockEntity Body { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this accessor is abstract (means no implementation).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsAbstract
    {
      get { return Body == null; }
    }
  }
}
