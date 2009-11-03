using System.Collections.Generic;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// A method group is a set of overloaded methods resulting from a member lookup.
  /// </summary>
  // ================================================================================================
  public sealed class MethodGroup
  {
    /// <summary>
    /// Backing field for storing methods.
    /// </summary>
    private readonly HashSet<MethodEntity> _Methods = new HashSet<MethodEntity>();

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodGroup"/> class.
    /// </summary>
    /// <param name="methods">A collection of method entities that form the method group.</param>
    // ----------------------------------------------------------------------------------------------
    public MethodGroup(IEnumerable<MethodEntity> methods)
    {
      foreach(var method in methods)
      {
        _Methods.Add(method);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of methods in the group.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<MethodEntity> Methods
    {
      get
      {
        return _Methods;
      }
    }
  }
}
