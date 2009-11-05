using CSharpTreeBuilder.Common;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This exception is raised when a static member was expected but an instance member was detected.
  /// </summary>
  // ================================================================================================
  public sealed class StaticMemberExpectedException : NamespaceOrTypeNameResolverException
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="StaticMemberExpectedException"/> class.
    /// </summary>
    /// <param name="memberEntity">An instance member.</param>
    // ----------------------------------------------------------------------------------------------
    public StaticMemberExpectedException(IMemberEntity memberEntity)
    {
      MemberEntity = memberEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the instance member that was expected to be static.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IMemberEntity MemberEntity { get; private set; }

  }
}
