using System;
using System.Collections.Generic;
using CSharpParser.ProjectModel;
using CSharpParser.Semantics;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This type represents an array type constructed from a declaration in the 
  /// source code.
  /// </summary>
  // ==================================================================================
  public class GenericType : ExtendedType
  {
    #region Private fields

    private readonly ITypeAbstraction _ConstructingType;
    private readonly List<ITypeAbstraction> _TypeParameters;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates constructed generic type from the specified construction type with the
    /// given type parameters.
    /// </summary>
    /// <param name="constructingType">Type the generic type is constructed from.</param>
    /// <param name="typeParameters">Type parameters of this generic type.</param>
    // --------------------------------------------------------------------------------
    public GenericType(ITypeAbstraction constructingType,
      IEnumerable<ITypeAbstraction> typeParameters)
    {
      _ConstructingType = constructingType;
      _TypeParameters = new List<ITypeAbstraction>(typeParameters);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates constructed generic type from the specified construction type with the
    /// given type parameter.
    /// </summary>
    /// <param name="constructingType">Type the generic type is constructed from.</param>
    /// <param name="parameter">Single type parameter.</param>
    // --------------------------------------------------------------------------------
    public GenericType(ITypeAbstraction constructingType, ITypeAbstraction
      parameter)
    {
      if (!parameter.IsValueType)
      {
        throw new InvalidOperationException("Valuetype expected.");
      }
      _ConstructingType = constructingType;
      _TypeParameters = new List<ITypeAbstraction>();
      _TypeParameters.Add(parameter);
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the factory type of this generic type
    /// </summary>
    // --------------------------------------------------------------------------------
    public ITypeAbstraction ConstructingType
    {
      get { return _ConstructingType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of type parameters
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<ITypeAbstraction> TypeParameters
    {
      get { return _TypeParameters; }
    }

    #endregion

    #region Overridden methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an array.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsArray
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a pointer.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsPointer
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is an unmanaged .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsUnmanagedType
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the current member.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string Name
    {
      get
      {
        if (_TypeParameters.Count == 0) return _ConstructingType.SimpleName;
        return string.Format("{0}`{1}", _ConstructingType.SimpleName, 
          _TypeParameters.Count);
      }
    }

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
      get
      {
        foreach (ITypeAbstraction param in _TypeParameters)
          if (param.IsOpenType) return true;
        return false;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsRuntimeType
    {
      get { return _ConstructingType.IsRuntimeType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference unit where the type is defined.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override ReferencedUnit DeclaringUnit
    {
      get { return _ConstructingType.DeclaringUnit; }
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
      get { return false; }
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
      get { return _ConstructingType.IsAbstract; }
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
      get { return _ConstructingType.IsClass; }
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
      get { return true; }
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
      get { return _TypeParameters.Count; }
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
      get { return _ConstructingType.IsInterface; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is not declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsNotPublic
    {
      get { return _ConstructingType.IsNotPublic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsPublic
    {
      get { return _ConstructingType.IsPublic; }
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
      get { return _ConstructingType.IsSealed; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared static.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsStatic
    {
      get { return _ConstructingType.IsStatic; }
    }

    /// <summary>
    /// Gets a value indicating whether the Type is a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsValueType
    {
      get { return _ConstructingType.IsValueType; }
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
      get { return _ConstructingType.IsVisible; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the simple name of the current member.
    /// </summary>
    /// <remarks>The simple name does not contain any adornements.</remarks>
    // --------------------------------------------------------------------------------
    public override string SimpleName
    {
      get { return _ConstructingType.SimpleName; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string Namespace
    {
      get { return _ConstructingType.Namespace; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parametrized name of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string ParametrizedName
    {
      get { return TypeBase.GetParametrizedName(this); }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a list representing the generic arguments of a type.
    /// </summary>
    /// <returns>
    /// Arguments of a generic typeor generic type declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override List<ITypeAbstraction> GetGenericArguments()
    {
      return _TypeParameters;
    }

    #endregion
  }
}