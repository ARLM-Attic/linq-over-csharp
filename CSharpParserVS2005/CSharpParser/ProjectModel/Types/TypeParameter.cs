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
  public sealed class TypeParameter : TypeBase, 
    ITypeAbstraction 
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
    public override bool IsOpenType
    {
      get { return true; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets an enumerable representing the generic type arguments.
    /// </summary>
    /// <returns>Enumerable representing the types.</returns>
    // --------------------------------------------------------------------------------
    protected override IEnumerable<ITypeAbstraction> GetArguments()
    {
      yield break;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating, if the current type is a generic type parameter.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsGenericParameter
    {
      get { return true; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a class; that is, not a value 
    /// type or interface.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsClass
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
    public override bool IsEnum
    {
      get 
      {
        return HasConstraint && Constraint.HasPrimary &&
               Constraint.Primary.IsType && Constraint.Primary.Type.IsEnum;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an interface; that is, not a 
    /// class or a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsInterface
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type object represents a type 
    /// whose definition is nested inside the definition of another type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsNested
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
      get { return true; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsPublic
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared sealed.
    /// </summary>
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
    public override bool IsVisible
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string Namespace
    {
      get { return string.Empty; }
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
        if (element.IsType && element.Type.TailIsTypeParameter)
        {
          TypeParameter paramToCheck;
          if (context.TryGetValue(element.Type.Tail.Name, out paramToCheck))
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
              paramToCheck.Constraint.Primary.Type.Tail.TypeInstance.IsClass)
            {
              Parser.Error0455(element.Token, Name, 
                paramToCheck.Constraint.Primary.Type.Tail.TypeInstance.FullName,
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
