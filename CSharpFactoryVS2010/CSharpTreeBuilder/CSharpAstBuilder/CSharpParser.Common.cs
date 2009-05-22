// ================================================================================================
// CSharpAstBuilder.Common.cs
//
// Created: 2009.05.22, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// Commom parts of the AST Builder
  /// </summary>
  // ================================================================================================
  public partial class CSharpParser
  {
    /// <summary>
    /// Builds the ast for source file.
    /// </summary>
    /// <param name="sourceFile">The source file.</param>
    /// <returns></returns>
    public static SourceFileNode BuildAstForSourceFile(SourceFileBase sourceFile)
    {
      // TODO: Implement this method
      return new SourceFileNode(sourceFile.FullName);
    }
  }
}