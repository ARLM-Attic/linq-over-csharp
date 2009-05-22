// ================================================================================================
// SourceFileNode.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.IO;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This type defines a source file node in the syntax tree.
  /// </summary>
  // ================================================================================================
  public sealed class SourceFileNode: NamespaceScopeNode
  {
    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SourceFileNode"/> class.
    /// </summary>
    /// <param name="fullName">The full name of the source file.</param>
    // ----------------------------------------------------------------------------------------------
    public SourceFileNode(string fullName): base(null)
    {
      Name = Path.GetFileName(fullName);
      FullName = fullName;
      GlobalAttributes = new AttributeDecorationNodeCollection();
    }

    #endregion

    #region Public Properties

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the source file.
    /// </summary>
    /// <value>The name.</value>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the full name of the source file.
    /// </summary>
    /// <value>The full name.</value>
    // ----------------------------------------------------------------------------------------------
    public string FullName { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the global attributes belonging to this source file.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AttributeDecorationNodeCollection GlobalAttributes { get; private set; }
    
    #endregion

    #region Output methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates the output items describing this source file.
    /// </summary>
    /// <param name="options">The output formatting options.</param>
    /// <returns>Colection of output items to be written.</returns>
    // ----------------------------------------------------------------------------------------------
    public OutputItemCollection CreateOutput(SyntaxTreeOutputOptions options)
    {
      var serializer = new OutputItemSerializer(options);
      serializer.Append(GetOutputSegment());
      return serializer.OutputItems;
    }

    #endregion
  }
}