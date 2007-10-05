using System.Collections.Generic;
using CSharpParser.Collections;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents a type parameter used in generic constructions.
  /// </summary>
  // ==================================================================================
  public sealed class TypeParameter : AttributedElement, ITypeCharacteristics 
  {
    #region Private fields

    private TypeParameterConstraint _Constraint;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a type parameter according to the info provided by the
    /// specified token.
    /// </summary>
    /// <param name="token">Token providing information about the element.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public TypeParameter(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the constraint related to this type parameter
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeParameterConstraint Constraint
    {
      get { return _Constraint; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this typeparameter has a constraint or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasConstraint
    {
      get { return _Constraint != null; }  
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if a type is open or not.
    /// </summary>
    /// <remarks>
    /// A type is open, if directly or indireclty references to a type parametes.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool IsOpenType
    {
      get { return true; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of dimensions of an array type.
    /// </summary>
    /// <returns>Number of array dimensions.</returns>
    // --------------------------------------------------------------------------------
    public int GetArrayRank()
    {
      return 0;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the element type of this type.
    /// </summary>
    /// <returns>
    /// Element type for a pointer, reference or array; otherwise, null.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics GetElementType()
    {
      return null;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the types directly nested into this type.
    /// </summary>
    /// <returns>
    /// Dictionary of nested types keyed by the CLR names of the nested types. Empty
    /// dictionary is retrieved if there is no nested type.
    /// </returns>
    // --------------------------------------------------------------------------------
    public Dictionary<string, ITypeCharacteristics> GetNestedTypes()
    {
      return new Dictionary<string, ITypeCharacteristics>();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the underlying type of an enum type.
    /// </summary>
    /// <remarks>
    /// Throws an exception, if the underlying type is not an enum type.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics GetUnderlyingEnumType()
    {
      return null;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsRuntimeType
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference unit where the type is defined.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ReferencedUnit DeclaringUnit
    {
      get { return Parser.CompilationUnit.ThisUnit; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is an unmanaged .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsUnmanagedType
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the base type of this type.
    /// </summary>
    /// <remarks>
    /// If there is no explicit base type for this type, a corresponding reference to
    /// System.Object should be returned.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics BaseType
    {
      get { return null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type that declares the current nested type.
    /// </summary>
    /// <remarks>
    /// If there is no declaring type, null should be returned.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics DeclaringType
    {
      get { return null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type encompasses or refers to 
    /// another type; that is, whether the current Type is an array, a pointer, or is 
    /// passed by reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasElementType
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is abstract and must be overridden.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsAbstract
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an array.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsArray
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a class; that is, not a value 
    /// type or interface.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsClass
    {
      get 
      {
        return !HasConstraint ||
               (Constraint.HasPrimary && (Constraint.Primary.IsClass ||
               Constraint.Primary.IsType && Constraint.Primary.Type.IsClass));
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type represents an enumeration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsEnum
    {
      get 
      {
        return HasConstraint && Constraint.HasPrimary &&
               Constraint.Primary.IsType && Constraint.Primary.Type.IsEnum;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current type is a generic type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGenericType
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type represents a generic type 
    /// definition, from which other generic types can be constructed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGenericTypeDefinition
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Makes an array type from the current type with the specified rank.
    /// </summary>
    /// <param name="rank">Rank of array type to be created</param>
    /// <returns>
    /// Array type created from this type.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics MakeArrayType(int rank)
    {
      return new ArrayType(this, rank);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Makes a pointer type from the current type with the specified rank.
    /// </summary>
    /// <returns>
    /// Pointer type created from this type.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics MakePointerType()
    {
      return new PointerType(this);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of type parameters.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int TypeParameterCount
    {
      get { return 0; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an interface; that is, not a 
    /// class or a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsInterface
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type object represents a type 
    /// whose definition is nested inside the definition of another type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNested
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is nested and visible only within 
    /// its own assembly.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNestedAssembly
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is nested and declared private.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNestedPrivate
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether a class is nested and declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNestedPublic
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is not declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNotPublic
    {
      get { return true; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a pointer.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPointer
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is one of the primitive types.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPrimitive
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPublic
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared sealed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsSealed
    {
      get { return true; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared static.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsStatic
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsValueType
    {
      get
      {
        return HasConstraint && Constraint.HasPrimary && (Constraint.Primary.IsStruct ||
               Constraint.Primary.IsType && Constraint.Primary.Type.IsStatic);
      }
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
    public bool IsVisible
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Namespace
    {
      get { return string.Empty; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the object carrying detailed information about this type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public object TypeObject
    {
      get { return this; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Assigns a constraint to this type parameter.
    /// </summary>
    /// <param name="constraint">Constraint of this type parameter</param>
    // --------------------------------------------------------------------------------
    public void AssignConstraint(TypeParameterConstraint constraint)
    {
      _Constraint = constraint;
      _Constraint.AssignOwnerParameter(this);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Check if this type parameter depends on any other type parameters through 
    /// constraints.
    /// </summary>
    /// <param name="context">Type parameter context to check dependency.</param>
    /// <remarks>
    /// In case of dependency, the appropriate compiler error is raised.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool CheckDependency(TypeParameterCollection context)
    {
      return CheckDependency(context, _Constraint);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Check if this type parameter depends on the specified type parameters through 
    /// constraints.
    /// </summary>
    /// <param name="context">Type parameter context to check dependency.</param>
    /// <param name="constraint">Constraint to check.</param>
    /// <remarks>
    /// In case of invalid dependency, the appropriate compiler error is raised.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool CheckDependency(TypeParameterCollection context, 
      TypeParameterConstraint constraint)
    {
      if (constraint == null) return false;
      foreach (ConstraintElement element in constraint.Constraints)
      {
        if (element.IsType && element.Type.RightMostPart.IsResolvedToTypeParameter)
        {
          TypeParameter paramToCheck;
          if (context.TryGetValue(element.Type.RightMostPart.Name, out paramToCheck))
          {
            // --- Check for circular dependency
            if (Name == paramToCheck.Name)
            {
              // --- We found circular dependency
              Parser.Error0454(element.Token, Name, constraint.Name);
              paramToCheck.Invalidate();
              return true;
            }

            if (!paramToCheck.HasConstraint) continue;

            // --- Constraint parameter cannot have the "struct" contraint
            if (paramToCheck.Constraint.HasPrimary && 
              paramToCheck.Constraint.Primary.IsStruct)
            {
              Parser.Error0456(element.Token, paramToCheck.Constraint.Name, Name);
              paramToCheck.Invalidate();
              return true;
            }

            // --- If the current parameter has the "struct" constraint, its constraint
            // --- parameter cannot have a class-type.
            if (Constraint.HasPrimary && Constraint.Primary.IsStruct &&
              paramToCheck.Constraint.HasPrimary &&
              paramToCheck.Constraint.Primary.IsType &&
              paramToCheck.Constraint.Primary.Type.RightMostPart.ResolvingType.IsClass)
            {
              Parser.Error0455(element.Token, Name, 
                paramToCheck.Constraint.Primary.Type.RightMostPart.ResolvingType.FullName,
                "System.ValueType");
              paramToCheck.Invalidate();
              return true;
            }

            // TODO: More checks should be added:
            // --- If the current parameter (S) has a class-type constraint A and its 
            // --- constraint (T) has a class-type constraint B then there must be an 
            // --- identity conversion or implicit reference conversion from A to B or an 
            // --- implicit reference conversion from B to A.

            // --- If S also depends on type parameter U and U has a class-type constraint 
            // --- A and T has a class-type constraint B then there must be an identity 
            // --- conversion or implicit reference conversion from A to B or an implicit 
            // --- reference conversion from B to A.

            // --- Any type argument used for a type parameter with a constructor 
            // --- constraint shall have a public parameterless constructor (this includes 
            // --- all value types) or be a type parameter having the value type constraint 
            // --- or constructor constraint.

            // --- Check the constraint of type parameter causing dependency.
            if (paramToCheck.IsValid && CheckDependency(context, paramToCheck.Constraint))
              return true;
          }
        }
      }
      return false;
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a collection of type parameters.
  /// </summary>
  // ==================================================================================
  public class TypeParameterCollection : RestrictedIndexedCollection<TypeParameter>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used for indexing.
    /// </summary>
    /// <param name="item">TypeParameter item.</param>
    /// <returns>
    /// Name of the type parameter.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(TypeParameter item)
    {
      return item.Name;
    }
  }
}
