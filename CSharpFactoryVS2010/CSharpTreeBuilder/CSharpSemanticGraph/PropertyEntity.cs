using System;
using System.Collections.Generic;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a property member.
  /// </summary>
  // ================================================================================================
  public sealed class PropertyEntity : FunctionMemberWithAccessorsEntity
  {
    #region State

    /// <summary>Backing field for GetAccessor property.</summary>
    private AccessorEntity _GetAccessor;

    /// <summary>Backing field for SetAccessor property.</summary>
    private AccessorEntity _SetAccessor;

    /// <summary>Backing field for SetAccessor property.</summary>
    private FieldEntity _AutoImplementedField;

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyEntity"/> class.
    /// </summary>
    /// <param name="isDeclaredInSource">True if the member is explicitly declared in source code, false otherwise.</param>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="isStatic">A value indicating whether this property is static.</param>
    /// <param name="typeReference">A reference to the type of the member.</param>
    /// <param name="interfaceReference">
    /// A reference to in interface, if the member is explicitly implemented interface member.
    /// Null otherwise.
    /// </param>
    /// <param name="name">The name of the member.</param>
    /// <param name="isAutoImplemented">A value indicating whether this property is auto-implemented.</param>
    // ----------------------------------------------------------------------------------------------
    public PropertyEntity(
      bool isDeclaredInSource,
      AccessibilityKind? accessibility,
      bool isStatic,
      Resolver<TypeEntity> typeReference,
      Resolver<TypeEntity> interfaceReference,
      string name, 
      bool isAutoImplemented)
      : 
      base(isDeclaredInSource, accessibility, typeReference, interfaceReference, name)
    {
      _IsStatic = isStatic;
      
      // If the property is auto-implemented then create the backing field.
      if (isAutoImplemented)
      {
        // csc.exe uses '<{PropertyName}>k_BackingField' but we don't follow it, 
        // because the uniqueness of the field name gets tricky with explicitly implemented interface members.
        // We just use a good old GUID, because it's unique, and its name has no significance at all.
        var fieldName = System.Guid.NewGuid().ToString();
        AutoImplementedField = new FieldEntity(false, AccessibilityKind.Private, isStatic, false, typeReference, fieldName, null);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private PropertyEntity(PropertyEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      if (template.GetAccessor != null)
      {
        GetAccessor = (AccessorEntity)template.GetAccessor.GetGenericClone(typeParameterMap);
      }

      if (template.SetAccessor != null)
      {
        SetAccessor = (AccessorEntity)template.SetAccessor.GetGenericClone(typeParameterMap);
      }

      if (template.AutoImplementedField != null)
      {
        AutoImplementedField = (FieldEntity)template.AutoImplementedField.GetGenericClone(typeParameterMap);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new constructed entity.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A new semantic entity constructed from this entity using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected override SemanticEntity ConstructNew(TypeParameterMap typeParameterMap)
    {
      return new PropertyEntity(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child entity.
    /// </summary>
    /// <param name="entity">A child entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void AddChild(ISemanticEntity entity)
    {
      if (entity is AccessorEntity)
      {
        AddAccessor(entity as AccessorEntity);
      }
      else
      {
        base.AddChild(entity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds an accessor.
    /// </summary>
    /// <param name="accessor">An accessor entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddAccessor(AccessorEntity accessor)
    {
      if (GetAccessor == null)
      {
        GetAccessor = accessor;
      }
      else if (SetAccessor == null)
      {
        SetAccessor = accessor;
      }
      else
      {
        throw new ApplicationException("Can't add accessor. Both accessors are already set.");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the auto-implemented field. Returns null if the property is not auto-implemented.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public FieldEntity AutoImplementedField
    {
      get 
      { 
        return _AutoImplementedField; 
      }

      private set
      {
        _AutoImplementedField = value;

        if (_AutoImplementedField != null)
        {
          _AutoImplementedField.Parent = this;
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indictaing whether this property is auto-implemented.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsAutoImplemented
    {
      get { return AutoImplementedField != null; }
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the get accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AccessorEntity GetAccessor
    {
      get { return _GetAccessor; }
      set
      {
        if (value != null)
        {
          value.Parent = this;
        }
        _GetAccessor = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the set accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AccessorEntity SetAccessor
    {
      get { return _SetAccessor; }
      set
      {
        if (value != null)
        {
          value.Parent = this;
        }
        _SetAccessor = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the body of the function member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override IEnumerable<AccessorEntity> Accessors
    {
      get
      {
        var tempList = new List<AccessorEntity>();

        if (GetAccessor != null)
        {
          tempList.Add(GetAccessor);
        }

        if (SetAccessor != null)
        {
          tempList.Add(SetAccessor);
        }
        
        return tempList;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this member is invocable.
    /// </summary>
    /// <remarks>A member is invocable if it's a method or event, 
    /// or if it is a constant, field or property of a delegate type.</remarks>
    // ----------------------------------------------------------------------------------------------
    public override bool IsInvocable
    {
      get
      {
        return (Type != null) && Type is DelegateEntity;
      }
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      visitor.Visit(this);
      base.AcceptVisitor(visitor);

      if (AutoImplementedField != null)
      {
        AutoImplementedField.AcceptVisitor(visitor);  
      }

      if (GetAccessor != null)
      {
        GetAccessor.AcceptVisitor(visitor);
      }

      if (SetAccessor != null)
      {
        SetAccessor.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
