using System;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of namespace and type entities.
  /// </summary>
  // ================================================================================================
  public abstract class NamespaceOrTypeEntity : SemanticEntity, INamedEntity
  {
    /// <summary>Backing field for DeclarationSpace property.</summary>
    protected DeclarationSpace _DeclarationSpace;

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
      _DeclarationSpace = new DeclarationSpace();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the namespace or type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; protected set; }

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
    public virtual string FullyQualifiedName 
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
    /// Gets the string representation of the object.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override string ToString()
    {
      if (Parent == null)
      {
        return Name;
      }

      if (this is TypeParameterEntity)
      {
        // A method's type parameter must not be prefixed with parent's ToString, 
        // because that would lead to infinite recursion 
        // Member.ToString() -> Signature.ToString -> Parameter.Type.ToString -> (loop)
        if (Parent is MethodEntity)
        {
          return Name;
        }
        else
        {
          return Parent + "'" + Name;
        }
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
  }
}
