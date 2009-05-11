// ================================================================================================
// CompilationModel.cs
//
// Created: 2009.05.07, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpFactory.Semantics
{
  // ================================================================================================
  /// <summary>
  /// This class represents the entity model for a compilation unit.
  /// </summary>
  // ================================================================================================
  public class CompilationModel: NamedCompilationEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CompilationModel"/> class with the 
    /// specified name.
    /// </summary>
    /// <param name="name">The name of the compilation model.</param>
    // ----------------------------------------------------------------------------------------------
    public CompilationModel(string name): base(name)
    {
      GlobalNamespace = new GlobalNamespaceItem();
      Namespaces = new NamespaceCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the global namespace of this comilation unit.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public GlobalNamespaceItem GlobalNamespace { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the root namespaces belonging to the compilation unit.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceCollection Namespaces { get; private set; }
  }
}