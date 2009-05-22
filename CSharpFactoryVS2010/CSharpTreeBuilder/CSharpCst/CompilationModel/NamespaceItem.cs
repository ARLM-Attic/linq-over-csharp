// ================================================================================================
// NamespaceItem.cs
//
// Created: 2009.05.07, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Cst
{
  // ================================================================================================
  /// <summary>
  /// This class represents a namespace and all nested namespace and type declarations.
  /// </summary>
  /// <remarks>
  /// Compound namespace definition are represented as namespace hierarchies. For example, the 
  /// "System.Collections.Generic" namespace is represented by three instances of this class in a 
  /// parent-child-grandchild relationship with <see cref="NamespaceItem"/> instances like "System",
  /// "Collections" and "Generic".
  /// The <see cref="IsExplicit"/> property shows whether the namespace has been explicitly defined
  /// during the compilation or only implicitly. For example, if the source code defines the 
  /// "MyCompany.MyTypes" namespace only but does not define explicitly the "MyCompany" namespace,
  /// "MyTypes" is explicit, "MyCompany" is implicit.
  /// </remarks>
  // ================================================================================================
  public class NamespaceItem: NamedCompilationEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceItem"/> class with the spacified name.
    /// </summary>
    /// <param name="name">The name of this namespace instance.</param>
    // ----------------------------------------------------------------------------------------------
    public NamespaceItem(string name): base (name)
    {
      NestedNamespaces = new NamespaceCollection();
      NestedTypes = new TypeDeclarationCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance represents the global namespace or not..
    /// </summary>
    /// <value>This type always returns false;</value>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsGlobal { get { return false; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether this namespace instance is explicitly declared.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is explicit; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool IsExplicit { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parent namespace of this namespace item.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceItem ParentNamespace { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of nested namespaces declared within this namespace.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceCollection NestedNamespaces { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of type declarations nested into this namespace item.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeDeclarationCollection NestedTypes { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name of this namespace.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override string FullName
    {
      get
      {
        return ParentNamespace == null 
          ? Name 
          : string.Format("{0}.{1}", ParentNamespace.FullName, Name);
      }
    }
  }
}