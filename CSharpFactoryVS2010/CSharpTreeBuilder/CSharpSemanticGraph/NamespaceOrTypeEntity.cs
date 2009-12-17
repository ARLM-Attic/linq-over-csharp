using System;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of namespace and type entities.
  /// </summary>
  // ================================================================================================
  public abstract class NamespaceOrTypeEntity : SemanticEntity, INamedEntity, IDefinesDeclarationSpace
  {
    #region State

    /// <summary>The declaration space of this entity.</summary>
    protected DeclarationSpace _DeclarationSpace = new DeclarationSpace();

    /// <summary>Backing field for Name property.</summary>
    private string _Name;
    
    #endregion 

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    protected NamespaceOrTypeEntity(string name)
    {
      if (name == null)
      {
        throw new ArgumentNullException("name");
      }

      Name = name;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected NamespaceOrTypeEntity(NamespaceOrTypeEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      // Declaration space should not be copied or the new type will refer to the template's members.

      Name = template.Name;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of this entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name
    {
      get
      {
        return _Name;
      }

      protected set
      {
        if (_Name != null)
        {
          UnregisterInParentDeclarationSpace();
        }

        _Name = value;

        if (_Name != null)
        {
          RegisterInParentDeclarationSpace();
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the fully qualified name of the namespace or type, 
    /// that uniquely identifies the namespace or type amongst all others.
    /// </summary>
    /// <remarks>
    /// The fully qualified name of N is the complete hierarchical path of identifiers that lead to N,
    /// starting from the root namespace.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public string FullyQualifiedName 
    { 
      get
      {
        if (Parent is NamespaceOrTypeEntity)
        {
          return Parent is RootNamespaceEntity
            ? Name
            : string.Format("{0}.{1}", ((NamespaceOrTypeEntity)Parent).FullyQualifiedName, Name);
        }
        return Name;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds the declaration of an entity.
    /// </summary>
    /// <param name="namedEntity">A named entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddDeclaration(INamedEntity namedEntity)
    {
      _DeclarationSpace.Register(namedEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes the declaration of an entity.
    /// </summary>
    /// <param name="namedEntity">A named entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void RemoveDeclaration(INamedEntity namedEntity)
    {
      _DeclarationSpace.Unregister(namedEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of the object.
    /// </summary>
    /// <returns>The string representation of the object</returns>
    // ----------------------------------------------------------------------------------------------
    public override string ToString()
    {
      if (Parent == null)
      {
        return Name;
      }

      if (Parent is RootNamespaceEntity)
      {
        return Parent + "::" + Name;
      }

      if (Parent is NamespaceEntity)
      {
        return Parent + "." + Name; 
      }

      if (Parent is TypeEntity)
      {
        return Parent + "+" + Name;
      }

      throw new ApplicationException("Unhandled case in ToString.");
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
    }

    #endregion
  }
}
