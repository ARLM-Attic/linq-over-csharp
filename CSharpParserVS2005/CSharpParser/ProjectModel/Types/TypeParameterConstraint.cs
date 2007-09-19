using System;
using System.Collections.Generic;
using CSharpParser.ParserFiles;
using CSharpParser.Collections;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This enumeration defines the usable parameter constraint types.
  /// </summary>
  // ==================================================================================
  public enum ConstraintClassification
  {
    Class,
    Struct,
    New,
    Type
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a constraint element on the type parameter constraint list.
  /// </summary>
  // ==================================================================================
  public sealed class ConstraintElement : LanguageElement
  {
    #region Private fields

    private readonly ConstraintClassification _Classification;
    private readonly TypeReference _Type;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a constraint representing a "class", "struct" or "new()" constraint.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    /// <param name="classification">Type of constraint.</param>
    // --------------------------------------------------------------------------------
    public ConstraintElement(Token token, CSharpSyntaxParser parser, 
      ConstraintClassification classification) : base(token, parser)
    {
      _Classification = classification;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a constraint representing a type constraint.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    /// <param name="type">Type of the constraint.</param>
    // --------------------------------------------------------------------------------
    public ConstraintElement(Token token, CSharpSyntaxParser parser, TypeReference type)
      : base(token, parser)
    {
      _Classification = ConstraintClassification.Type;
      _Type = type;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the classification of the constraint.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ConstraintClassification Classification
    {
      get { return _Classification; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type represented by this constraint
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference Type
    {
      get { return _Type; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this constraint element is a "class".
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsClass
    {
      get { return _Classification == ConstraintClassification.Class;  }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this constraint element is a "struct".
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsStruct
    {
      get { return _Classification == ConstraintClassification.Struct; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this constraint element is a type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsType
    {
      get { return _Classification == ConstraintClassification.Type; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this constraint element is a "class" or 
    /// "struct" element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsClassOrStruct
    {
      get
      {
        return _Classification == ConstraintClassification.Class
               || _Classification == ConstraintClassification.Struct;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this constraint element is a primary element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPrimary
    {
      get
      {
        if (IsClassOrStruct) return true;
        if (IsType &&
          _Type.RightMostPart.IsResolvedToType && 
          (_Type.RightMostPart.ResolvingType.IsClass ||
          _Type.RightMostPart.ResolvingType.TypeObject.Equals(typeof(Enum)))
          ) return true;
        return false;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this constraint element is a constructor element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNew
    {
      get { return _Classification == ConstraintClassification.New; }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type represents a type parameter constaint used within the context of a
  /// generic type or member definition.
  /// </summary>
  // ==================================================================================
  public sealed class TypeParameterConstraint : LanguageElement, IUsesResolutionContext
  {
    #region Private fields

    private readonly List<ConstraintElement> _Constraints =
      new List<ConstraintElement>();
    private ConstraintElement _Primary;
    private readonly List<ConstraintElement> _SecondaryConstraints = 
      new List<ConstraintElement>();
    private ConstraintElement _NewConstraint;
    private TypeParameter _OwnerParameter;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new parameter constraint declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    // --------------------------------------------------------------------------------
    public TypeParameterConstraint(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the primary constraint element
    /// </summary>
    // --------------------------------------------------------------------------------
    public ConstraintElement Primary
    {
      get { return _Primary; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if there is any primary constraint element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasPrimary
    {
      get { return _Primary != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of secondary constraint element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<ConstraintElement> SecondaryConstraints
    {
      get { return _SecondaryConstraints; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if there is any secondary constraint element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasSecondary
    {
      get { return _SecondaryConstraints.Count > 0; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the constructor constraint element
    /// </summary>
    // --------------------------------------------------------------------------------
    public ConstraintElement NewConstraint
    {
      get { return _NewConstraint; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if there is any constructor constraint element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasNew
    {
      get { return _NewConstraint != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the iterator for all constraint elements.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<ConstraintElement> Constraints
    {
      get { return _Constraints; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type parameter owning this constraint.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeParameter OwnerParameter
    {
      get { return _OwnerParameter; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the owner parameter of this constraint.
    /// </summary>
    /// <param name="owner">Owner type parameter.</param>
    // --------------------------------------------------------------------------------
    public void AssignOwnerParameter(TypeParameter owner)
    {
      _OwnerParameter = owner;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new element to this constraint.
    /// </summary>
    /// <param name="element">Element to add.</param>
    // --------------------------------------------------------------------------------
    public void AddConstraintElement(ConstraintElement element)
    {
      _Constraints.Add(element);
      _SecondaryConstraints.Add(element);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Separates constraint elements into primary,secondary and constructor elements.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void SeparateConstraintElements()
    {
      // --- Check for primary constraint
      if (_SecondaryConstraints.Count == 0) return;
      if (_SecondaryConstraints[0].IsPrimary)
      {
        _Primary = _SecondaryConstraints[0];
        _SecondaryConstraints.RemoveAt(0);
      }

      // --- Check for constructor constraint
      if (_SecondaryConstraints.Count == 0) return;
      int lastIndex = _SecondaryConstraints.Count - 1;
      if (_SecondaryConstraints[lastIndex].IsNew)
      {
        _NewConstraint = _SecondaryConstraints[lastIndex];
        _SecondaryConstraints.RemoveAt(lastIndex);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if this constraint is valid, it does not uses special classes.
    /// </summary>
    /// <remarks>
    /// Constraint cannot be System.Array, System.Delegate, System.Enum, 
    /// System.ValueType or System.Object
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void CheckClassConstraint()
    {
      if (HasPrimary && Primary.IsType && Primary.Type.RightMostPart.IsResolvedToType)
      {
        ITypeCharacteristics resolvingType = Primary.Type.RightMostPart.ResolvingType;
        object typeObject = resolvingType.TypeObject;
        if (typeObject.Equals(typeof(object)) ||
          typeObject.Equals(typeof(Array)) ||
          typeObject.Equals(typeof(Delegate)) ||
          typeObject.Equals(typeof(Enum)) ||
          typeObject.Equals(typeof(ValueType)))
        {
          Parser.Error0702(Primary.Token, Primary.Type.FullName);
          Invalidate();
        }
        else if (resolvingType.IsClass && resolvingType.IsSealed)
        {
          Parser.Error0701(Primary.Token, Primary.Type.FullName);
          Invalidate();
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if "new()" is used with "struct".
    /// </summary>
    // --------------------------------------------------------------------------------
    public void CheckNewWithStruct()
    {
      if (HasPrimary && Primary.IsStruct && HasNew)
      {
        Parser.Error0451(NewConstraint.Token);
        NewConstraint.Invalidate();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the specified secondary element confomrs with the basic rules.
    /// </summary>
    /// <param name="element">Elementto check.</param>
    // --------------------------------------------------------------------------------
    public void CheckSecondaryElement(ConstraintElement element)
    {
      // --- Class or struct cannot be secondary constraints
      if (element.IsClassOrStruct)
      {
        Parser.Error0449(element.Token);
        element.Invalidate();
      }

      // --- "new()" cannot be used with "struct"
      if (HasPrimary && Primary.IsStruct && element.IsNew)
      {
        Parser.Error0451(element.Token);
        element.Invalidate();
      }

      // --- "new()" must be the last element
      if (element.IsNew)
      {
        Parser.Error0401(element.Token);
        element.Invalidate();
      }

      // --- Check for interface types
      if (element.IsType && element.Type.RightMostPart.IsResolvedToType)
      {
        ITypeCharacteristics resolvingType = element.Type.RightMostPart.ResolvingType;
        if (resolvingType.IsClass)
        {
          // --- Classes cannot be secondary constraints
          Parser.Error0406(element.Token, element.Type.FullName);
          element.Invalidate();
        }
        else if (!resolvingType.IsInterface)
        {
          // --- Non-interface types cannot be secondary constraints
          Parser.Error0701(element.Token, element.Type.FullName);
          element.Invalidate();
        }
      }
    }

    #endregion

    #region IUsesResolutionContext implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this namespace fragment.
    /// </summary>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
      foreach (ConstraintElement constraint in _Constraints)
      {
        if (constraint.Classification == ConstraintClassification.Type)
        constraint.Type.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion
  }

  #region TypeParameterConstraintCollection

  // ==================================================================================
  /// <summary>
  /// This class represents a list of type parameter constraint that can be indexed 
  /// by the constraint parameter name.
  /// </summary>
  // ==================================================================================
  public class TypeParameterConstraintCollection : 
    RestrictedIndexedCollection<TypeParameterConstraint>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used for indexing.
    /// </summary>
    /// <param name="item">Namespace item.</param>
    /// <returns>
    /// Full name of the namespace.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(TypeParameterConstraint item)
    {
      return item.Name;
    }
  }

  #endregion
}
