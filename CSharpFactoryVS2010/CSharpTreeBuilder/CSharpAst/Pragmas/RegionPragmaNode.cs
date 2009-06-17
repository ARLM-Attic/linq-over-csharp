// ================================================================================================
// RegionPragmaNode.cs
//
// Created: 2009.06.17, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents the "#region" pragma node.
  /// </summary>
  // ================================================================================================
  public class RegionPragmaNode : PragmaNode
  {
    // --- Backing fields
    private EndRegionPragmaNode _EndRegion;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="RegionPragmaNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="parentRegion">The parent region of this "#region" pragma.</param>
    // ----------------------------------------------------------------------------------------------
    public RegionPragmaNode(Token start, RegionPragmaNode parentRegion)
      : base(start)
    {
      ParentRegion = parentRegion;
      if (parentRegion != null) parentRegion.NestedRegions.Add(this);
      NestedRegions = new RegionPragmaNodeCollection {ParentNode = this};
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parent "#region" pragma.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public RegionPragmaNode ParentRegion { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the regions nested into this "#region" pragma.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public RegionPragmaNodeCollection NestedRegions { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the related "#endregion" pragma.
    /// </summary>
    /// <value>The end region.</value>
    // ----------------------------------------------------------------------------------------------
    public EndRegionPragmaNode EndRegion
    {
      get { return _EndRegion; }
      set
      {
        _EndRegion = value;
        Terminate(_EndRegion.TerminatingToken);
      }
    }
  }
}