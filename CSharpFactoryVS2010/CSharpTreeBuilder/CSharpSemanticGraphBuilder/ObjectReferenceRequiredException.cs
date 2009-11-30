namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This exceptions is thrown when an instance member is referenced without an object instance.
  /// </summary>
  // ================================================================================================
  public sealed class ObjectReferenceRequiredException : ResolverException
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectReferenceRequiredException"/> class.
    /// </summary>
    /// <param name="memberName">The fully qualifed name of the member.</param>
    // ----------------------------------------------------------------------------------------------
    public ObjectReferenceRequiredException(string memberName)
    {
      MemberName = memberName;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the fully qualifed name of the member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string MemberName { get; private set; }
  }
}
