// ================================================================================================
// PropertyDeclarationNode.cs
//
// Created: 2009.05.17, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a property declaration node.
  /// </summary>
  // ================================================================================================
  public class PropertyDeclarationNode: PropertyDeclarationNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public PropertyDeclarationNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves the "get" accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AccessorNode GetAccessor { get { return FindAccessor("get"); } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves the "set" accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AccessorNode SetAccessor { get { return FindAccessor("set"); } }
  }
}