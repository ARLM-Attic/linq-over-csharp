// ================================================================================================
// EndRegionPragmaNode.cs
//
// Created: 2009.06.17, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents the "#endregion" pragma node.
  /// </summary>
  // ================================================================================================
  public class EndRegionPragmaNode : PragmaNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EndRegionPragmaNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="regionPragma">The "#region" pragma belonging to this "#endregion".</param>
    // ----------------------------------------------------------------------------------------------
    public EndRegionPragmaNode(Token start, RegionPragmaNode regionPragma)
      : base(start)
    {
      RegionPragma = regionPragma;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the #region pragma belonging to this "#endregion".
    /// </summary>
    /// <value>The region pragma.</value>
    // ----------------------------------------------------------------------------------------------
    public RegionPragmaNode RegionPragma { get; private set; }
  }
}