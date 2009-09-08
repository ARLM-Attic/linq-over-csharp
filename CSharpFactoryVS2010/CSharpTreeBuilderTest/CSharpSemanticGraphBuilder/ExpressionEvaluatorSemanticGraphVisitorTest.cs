using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the ExpressionEvaluatorSemanticGraphVisitor class
  /// </summary>
  // ================================================================================================
  [TestClass]
  public sealed class ExpressionEvaluatorSemanticGraphVisitorTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the evaluation of null literal.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NullLiteral()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ExpressionEvaluatorSemanticGraphVisitor\NullLiteral.cs");
      InvokeParser(project).ShouldBeTrue();

      var field = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A").GetMember<FieldEntity>("a");
      field.Initializer.IsExpression.ShouldBeTrue();
      field.Initializer.Expression.ShouldNotBeNull();
      field.Initializer.Expression.Result.GetType().ShouldEqual(typeof (NullExpressionResult));
    }
  }
}