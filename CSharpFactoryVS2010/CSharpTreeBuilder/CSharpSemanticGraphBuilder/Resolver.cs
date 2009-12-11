using System;
using System.Collections.Generic;
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
  /// <remarks>
  /// The resolution process is performed only once, and the result is cached. 
  /// So a resolver object is not suitable in cases where repeated/on-demand resolution is required.
  /// </remarks>
  // ================================================================================================
  public abstract class Resolver<TTargetType> : IGenericCloneSupport
    where TTargetType : class
  {
    #region State

    /// <summary>Backing field for ResolutionState property.</summary>
    protected ResolutionState _ResolutionState;

    /// <summary>Backing field for Target property.</summary>
    protected TTargetType _Target;

    /// <summary>
    /// A dictionary holding all the objects constructed from this object by deep copying
    /// and then replacing type parameters with type arguments. 
    /// The key is a TypeParameterMap object describing all type parameters and the corresponding 
    /// type arguments.
    /// </summary>
    private readonly Dictionary<TypeParameterMap, IGenericCloneSupport> _GenericClones
      = new Dictionary<TypeParameterMap, IGenericCloneSupport>(new TypeParameterMapEqualityComparer());

    /// <summary>Backing field for TypeParameterMap property.</summary>
    public TypeParameterMap TypeParameterMap { get; private set; }

    /// <summary>Gets or sets the generic template of this entity.</summary>
    public IGenericCloneSupport DirectGenericTemplate { get; private set; }

    #endregion

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
    /// Initializes a new instance of the <see cref="Resolver{TTargetType}"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected Resolver(Resolver<TTargetType> template, TypeParameterMap typeParameterMap)
    {
      ResolutionState = ResolutionState.NotYetResolved;
      DirectGenericTemplate = template;
      TypeParameterMap = typeParameterMap;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new resolver.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A new resolver constructed from this resolver using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected virtual Resolver<TTargetType> ConstructNew(TypeParameterMap typeParameterMap)
    {
      throw new ApplicationException("Abstract Resolver cannot be constructed.");
    }

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
    /// Gets the target object. Null if not resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual TTargetType Target
    {
      get
      {
        return _Target;
      }
      private set
      {
        _Target = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the state of the resolution.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual ResolutionState ResolutionState
    {
      get
      {
        return _ResolutionState;
      }
      private set
      {
        _ResolutionState = value;
      }
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

    #region IGenericCloneSupport

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a semantic object cloned from this object using as a generic template.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A semantic object constructed from this object using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public IGenericCloneSupport GetGenericClone(TypeParameterMap typeParameterMap)
    {
      IGenericCloneSupport clone;

      if (_GenericClones.ContainsKey(typeParameterMap))
      {
        clone = _GenericClones[typeParameterMap];
      }
      else
      {
        clone = ConstructNew(typeParameterMap);

        _GenericClones.Add(typeParameterMap, clone);
      }

      return clone;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this object was cloned from a generic template.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsGenericClone
    {
      get
      {
        return DirectGenericTemplate != null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the first generic template in the chain of template->clone relationships,
    /// where none of the type parameters were bound. Null if this object was not cloned.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IGenericCloneSupport OriginalGenericTemplate
    {
      get
      {
        return IsGenericClone && DirectGenericTemplate.DirectGenericTemplate != null
                 ? DirectGenericTemplate.OriginalGenericTemplate
                 : DirectGenericTemplate;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of the objects cloned from this generic template object 
    /// by replacing type parameters with type arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<IGenericCloneSupport> GenericClones
    {
      get { return _GenericClones.Values; }
    }

    #endregion
  }
}
