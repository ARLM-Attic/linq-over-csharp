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
    public AccessorEntity()
    {
      Body = new BlockEntity();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the body of the accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BlockEntity Body { get; private set; }
  }
}
