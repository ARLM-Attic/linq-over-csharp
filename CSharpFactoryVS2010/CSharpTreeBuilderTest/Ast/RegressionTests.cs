// ================================================================================================
// RegressionTests.cs
//
// Created: 2009.06.04, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpTreeBuilderTest
{
  [TestClass]
  public class RegressionTests: ParserTestBed
  {
    // --- There is a "feature" in CoCo that compiled the following CoCo non-terminal faulty_
    // Unary<out ExpressionNode exprNode>   
    // (. exprNode = null; .)
    // =
    // (. UnaryOperatorNode unaryOp = null; .)
    // (
    //   IF (unaryHead[la.kind] || IsTypeCast())
    //   ( 
    //     ...
    //   )
    // )
    // --- The IF condition is not generated and it causes problems.
    // --- The fixed version is:
    // Unary<out ExpressionNode exprNode>   
    // (. 
    //    exprNode = null;  
    //    UnaryOperatorNode unaryOp = null;
    // .)
    // =
    // (
    //   IF (unaryHead[la.kind] || IsTypeCast())
    //   ( 
    //     ...
    //   )
    // )
    // --- This version works fine.
    [TestMethod]
    public void ExpressionBug1()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Regression\ExpressionBug1.cs");
      Assert.IsTrue(InvokeParser(project));
    }
  }
}