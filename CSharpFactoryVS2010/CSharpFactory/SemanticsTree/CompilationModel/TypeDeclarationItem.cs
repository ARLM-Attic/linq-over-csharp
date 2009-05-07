// ================================================================================================
// TypeDeclarationItem.cs
//
// Created: 2009.05.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Collections.Generic;
using System.Linq;
using CSharpFactory.Collections;

namespace CSharpFactory.Semantics
{
  // ================================================================================================
  /// <summary>
  /// This class represents a type declaration
  /// </summary>
  // ================================================================================================
  public class TypeDeclarationItem: NamedCompilationEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeDeclarationItem"/> class with the 
    /// specified name.
    /// </summary>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeDeclarationItem(string name)
      : base(name)
    {
      NestedTypes = new TypeDeclarationCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the namespace declaring this type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceItem DeclaringNamespace { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the typed declaring this type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeDeclarationItem DeclaringType { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type declaration is a nested type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsNestedType { get { return DeclaringType != null; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of type declarations nested into this type declaration.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeDeclarationCollection NestedTypes { get; private set; }
  }

  // ================================================================================================
  /// <summary>
  /// This class represents a collection of <see cref="TypeDeclarationItem"/> instances.
  /// </summary>
  /// <remarks>
  /// In most cases type declaration instances are unique (by their name) in a collection instance.
  /// However, in case of C# compilation errors (multiple type declaration) this collection may 
  /// contain separate type instances with the same name.
  /// </remarks>
  // ================================================================================================
  public sealed class TypeDeclarationCollection : ImmutableCollection<TypeDeclarationItem>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of items with the specified name.
    /// </summary>
    /// <param name="name">The name of itmes to search for.</param>
    /// <returns></returns>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TypeDeclarationItem> GetWithName(string name)
    {
      return this.Where(decl => decl.Name == name);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Checks if this collection contains a type declaration with the specified name.
    /// </summary>
    /// <param name="name">The name.</param>
    // ----------------------------------------------------------------------------------------------
    public bool Contains(string name)
    {
      return GetWithName(name).Count() > 0;
    }
  }
}