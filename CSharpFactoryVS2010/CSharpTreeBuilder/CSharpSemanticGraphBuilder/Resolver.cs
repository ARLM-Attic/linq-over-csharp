using System;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of different kinds of resolvers. 
  /// A resolver translates a source object of some representation (syntax node, reflected metadata,
  /// etc.) to a target object.
  /// </summary>
  /// <typeparam name="TTargetType">The type of the target object. Any class.</typeparam>
  // ================================================================================================
  public abstract class Resolver<TTargetType> where TTargetType : class
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="Resolver{TTargetType}"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected Resolver()
    {
      ResolutionState = ResolutionState.NotYetResolved;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the target object. Null if not resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TTargetType Target { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the state of the resolution.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ResolutionState ResolutionState { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Attempts to resolve the source object and sets ResolutionState and Target accordingly.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="errorHandler">An object for error reporting.</param>
    // ----------------------------------------------------------------------------------------------
    public TTargetType Resolve(ISemanticEntity context, ICompilationErrorHandler errorHandler)
    {
      TTargetType resolvedEntity = null;

      // If the entity is not yet resolved then attempt the resolution.
      if (ResolutionState == ResolutionState.NotYetResolved)
      {
        try
        {
          // Apply the resolution logic.
          resolvedEntity = GetResolvedEntity(context, errorHandler);
        }
        catch (ResolverException e)
        {
          TranslateExceptionToError(e, errorHandler);
        }

        // Set the resolution state
        if (resolvedEntity != null)
        {
          SetResolved(resolvedEntity);
        }
        else
        {
          SetUnresolvable();
        }
      }

      return Target;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Implements the resolution logic.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="errorHandler">An object for error reporting.</param>
    /// <returns>The resolved object, or null if could not resolve.</returns>
    /// <remarks>This is the place where different resolvers implement their specific logic.</remarks>
    // ----------------------------------------------------------------------------------------------
    protected abstract TTargetType GetResolvedEntity(ISemanticEntity context, ICompilationErrorHandler errorHandler);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Translate an error reported as an exception to an item that can be added to the error reporting object.
    /// </summary>
    /// <param name="e">An exception representing a resolution error.</param>
    /// <param name="errorHandler">The object for error reporting.</param>
    // ----------------------------------------------------------------------------------------------
    protected virtual void TranslateExceptionToError(ResolverException e, ICompilationErrorHandler errorHandler)
    {
      throw new ApplicationException("Could not translate ResolverException.", e); 
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the reference to resolved state, and sets the target object.
    /// </summary>
    /// <param name="target">The resulting object of the resolution.</param>
    // ----------------------------------------------------------------------------------------------
    protected void SetResolved(TTargetType target)
    {
      Target = target;
      ResolutionState = ResolutionState.Resolved;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the reference to unresolvable state.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected void SetUnresolvable()
    {
      Target = null;
      ResolutionState = ResolutionState.Unresolvable;
    }

  }
}
