// ================================================================================================
// TypeDeclarationItem.cs
//
// Created: 2009.05.07, by Istvan Novak (DeepDiver)
// ================================================================================================
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
}