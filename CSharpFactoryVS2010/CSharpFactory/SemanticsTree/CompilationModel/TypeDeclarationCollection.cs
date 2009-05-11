// ================================================================================================
// TypeDeclarationCollection.cs
//
// Created: 2009.05.10, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Collections.Generic;
using System.Linq;
using CSharpFactory.Collections;

namespace CSharpFactory.Semantics
{
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