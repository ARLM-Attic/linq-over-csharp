using System;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a member of a type.
  /// </summary>
  // ================================================================================================
  public abstract class MemberEntity : SemanticEntity, INamedEntity
  {
    /// <summary>
    /// Backing field for IsStatic property.
    /// </summary>
    /// <remarks>
    /// Not using auto-implemented property to avoid "virtual member call in constructor" warnings
    /// in descendant classes that want to initiliaze this property in the constructor.
    /// </remarks>
    protected bool _IsStatic;

    /// <summary>
    /// Backing field for Accessibility property.
    /// </summary>
    protected AccessibilityKind? _Accessibility;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberEntity"/> class.
    /// </summary>
    /// <param name="isExplicitlyDefined">True, if the member is explicitly defined, false otherwise.</param>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="name">The name of the member.</param>
    // ----------------------------------------------------------------------------------------------
    protected MemberEntity(bool isExplicitlyDefined, AccessibilityKind? accessibility, string name)
    {
      IsExplicitlyDefined = isExplicitlyDefined;
      _Accessibility = accessibility;
      Name = name;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether the member is explicitly defined, or created by the parser.
    /// Eg. value types have a default constructor which is implicitly declared.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsExplicitlyDefined { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the accessibility of the member.
    /// </summary>
    /// <remarks>If the accessibility was not set then a default is returned 
    /// which depends on the type of the containing type.</remarks>
    // ----------------------------------------------------------------------------------------------
    public virtual AccessibilityKind? Accessibility
    {
      get
      {
        if (_Accessibility != null)
        {
          return _Accessibility;
        }

        // If no declared accessibility then the default has to be returned,
        // which is based on the type of the containing type.
        if (Parent != null && Parent is TypeEntity)
        {
          var parentType = Parent as TypeEntity;

          if (parentType.IsClassType || parentType.IsStructType)
          {
            return AccessibilityKind.Private;
          }
          
          if (parentType.IsInterfaceType || parentType.IsEnumType)
          {
            return AccessibilityKind.Public;
          }
        }

        return null;
      }

      set
      {
        _Accessibility = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether this member belongs to the class itself
    /// (as opposed to an instance object).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsStatic
    {
      get { return _IsStatic; }
      set { _IsStatic = value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether this member can be overridden.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsVirtual { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether this member is on override of an inherited member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsOverride { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether this member intentionally hides an inherited member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsNew { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of the object.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override string ToString()
    {
      var result = (this is IOverloadableEntity) ? (this as IOverloadableEntity).Signature.ToString() : Name;

      return Parent == null ? result : Parent.ToString() + "_" + result;
    }
  }
}
