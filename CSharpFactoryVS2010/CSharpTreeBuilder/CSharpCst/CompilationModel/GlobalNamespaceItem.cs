// ================================================================================================
// GlobalNamespaceItem.cs
//
// Created: 2009.05.10, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Cst
{
  // ================================================================================================
  /// <summary>
  /// This class represents an instance of the global namespace. Any type declaration not nested in
  /// a namespace belongs to the global namespace. 
  /// </summary>
  // ================================================================================================
  public sealed class GlobalNamespaceItem : NamespaceItem
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalNamespaceItem"/> class with the name
    /// "global".
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public GlobalNamespaceItem()
      : base("global")
    {
      IsExplicit = false;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance represents the global namespace or not..
    /// </summary>
    /// <value>This type always returns true.</value>
    // ----------------------------------------------------------------------------------------------
    public override bool IsGlobal { get { return true; } }
  }
}