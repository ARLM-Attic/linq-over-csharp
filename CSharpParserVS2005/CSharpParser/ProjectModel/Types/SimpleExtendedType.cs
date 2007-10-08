using System;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a simple extended types that have their own element types
  /// like pointers and arrays.
  /// </summary>
  // ==================================================================================
  public abstract class SimpleExtendedType : ExtendedType
  {
    #region Private fields 

    private readonly ITypeAbstraction _ElementType;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a constructed type from the specified element type.
    /// </summary>
    /// <param name="elementType">Element type instance.</param>
    // --------------------------------------------------------------------------------
    protected SimpleExtendedType(ITypeAbstraction elementType)
    {
      if (elementType == null) throw new ArgumentNullException("elementType");
      _ElementType = elementType;
    }

    #endregion

    #region Overridden properties and methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if a type is open or not.
    /// </summary>
    /// <remarks>
    /// A type is open, if directly or indireclty references to a type parametes.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override bool IsOpenType
    {
      get { return  _ElementType.IsOpenType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsRuntimeType
    {
      get { return _ElementType.IsRuntimeType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference unit where the type is defined.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override ReferencedUnit DeclaringUnit
    {
      get { return _ElementType.DeclaringUnit; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type encompasses or refers to 
    /// another type; that is, whether the current Type is an array, a pointer, or is 
    /// passed by reference.
    /// </summary>
    /// <remarks>
    /// Constructed types always have element type.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override bool HasElementType
    {
      get { return _ElementType != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is abstract and must be overridden.
    /// </summary>
    /// <remarks>
    /// Constructed types are never abstract.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override bool IsAbstract
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a class; that is, not a value 
    /// type or interface.
    /// </summary>
    /// <remarks>
    /// Constructed types are never classes.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override bool IsClass
    {
      get { return true; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current type is a generic type.
    /// </summary>
    /// <remarks>
    /// Constructed types are never generic.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override bool IsGenericType
    {
      get { return _ElementType.IsGenericType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type represents a generic type 
    /// definition, from which other generic types can be constructed.
    /// </summary>
    /// <remarks>
    /// Constructed types are never generic definitions.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override bool IsGenericTypeDefinition
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of type parameters.
    /// </summary>
    /// <remarks>
    /// Constructed types never have type parameters.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override int TypeParameterCount
    {
      get { return 0; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an interface; that is, not a 
    /// class or a value type.
    /// </summary>
    /// <remarks>
    /// Constructed types are never interfaces.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override bool IsInterface
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is not declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsNotPublic
    {
      get { return _ElementType.IsNotPublic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsPublic
    {
      get { return _ElementType.IsPublic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared sealed.
    /// </summary>
    /// <remarks>
    /// Constructed types are always sealed.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override bool IsSealed
    {
      get { return true; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared static.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsStatic
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsValueType
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type can be accessed by code outside the 
    /// assembly.
    /// </summary>
    /// <value>
    /// true if the current Type is a public type or a public nested type such that 
    /// all the enclosing types are public; otherwise, false.
    /// </value>
    /// <remarks>
    /// Use this property to determine whether a type is part of the public 
    /// interface of a component assembly.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override bool IsVisible
    {
      get { return _ElementType.IsVisible; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the simple name of the current member.
    /// </summary>
    /// <remarks>The simple name does not contain any adornements.</remarks>
    // --------------------------------------------------------------------------------
    public override string SimpleName
    {
      get { return _ElementType.SimpleName; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string Namespace
    {
      get { return _ElementType.Namespace; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the element type of this type.
    /// </summary>
    /// <returns>
    /// Element type for a pointer, reference or array; otherwise, null.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override ITypeAbstraction GetElementType()
    {
      return _ElementType;
    }

    #endregion
  }
}