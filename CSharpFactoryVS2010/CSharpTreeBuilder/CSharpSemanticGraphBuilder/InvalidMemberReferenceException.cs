namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This exceptions is thrown when member access resolution encounters an invalid member reference.
  /// </summary>
  // ================================================================================================
  public sealed class InvalidMemberReferenceException : ResolverException
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidMemberReferenceException"/> class.
    /// </summary>
    /// <param name="memberName">The fully qualifed name of the member.</param>
    // ----------------------------------------------------------------------------------------------
    public InvalidMemberReferenceException(string memberName)
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
