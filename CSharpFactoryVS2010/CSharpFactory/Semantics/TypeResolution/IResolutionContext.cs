using CSharpFactory.ProjectModel;

namespace CSharpFactory.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This interface defines the behaviour of a resolution context.
  /// </summary>
  // ==================================================================================
  public interface IResolutionContext
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source file enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    SourceFile EnclosingSourceFile { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    NamespaceFragment EnclosingNamespace { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type declaration enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    TypeDeclaration EnclosingType { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the method declaration enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    MethodDeclaration EnclosingMethod { get; }
  }
}