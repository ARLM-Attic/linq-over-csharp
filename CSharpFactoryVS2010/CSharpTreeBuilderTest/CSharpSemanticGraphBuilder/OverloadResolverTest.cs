using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the overload resolution logic.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public sealed class OverloadResolverTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the IsApplicable function.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void IsApplicable()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"OverloadResolver\IsApplicable.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var method_M1 = class_A.GetMember<MethodEntity>("M1");
      var method_M2 = class_A.GetMember<MethodEntity>("M2");
      var method_M3 = class_A.GetMember<MethodEntity>("M3");
      var method_M4 = class_A.GetMember<MethodEntity>("M4");

      var method_T = class_A.GetMember<MethodEntity>("T");
      var statements = method_T.Body.Statements.ToList();
      var invocation_M1 = (statements[3] as ExpressionStatementEntity).Expression as InvocationExpressionEntity;
      var invocation_M2 = (statements[4] as ExpressionStatementEntity).Expression as InvocationExpressionEntity;
      var invocation_M3 = (statements[5] as ExpressionStatementEntity).Expression as InvocationExpressionEntity;
      var invocation_M4a = (statements[6] as ExpressionStatementEntity).Expression as InvocationExpressionEntity;
      var invocation_M4b = (statements[7] as ExpressionStatementEntity).Expression as InvocationExpressionEntity;
      var invocation_M4c = (statements[8] as ExpressionStatementEntity).Expression as InvocationExpressionEntity;
      var invocation_M4d = (statements[9] as ExpressionStatementEntity).Expression as InvocationExpressionEntity;

      method_M1.IsApplicable(invocation_M1.Arguments).ShouldBeTrue();
      method_M1.IsApplicable(invocation_M2.Arguments).ShouldBeFalse();

      method_M2.IsApplicable(invocation_M1.Arguments).ShouldBeTrue();
      method_M2.IsApplicable(invocation_M2.Arguments).ShouldBeTrue();

      method_M3.IsApplicable(invocation_M1.Arguments).ShouldBeFalse();
      method_M3.IsApplicable(invocation_M3.Arguments).ShouldBeTrue();

      method_M4.IsApplicable(invocation_M3.Arguments).ShouldBeFalse();
      method_M4.IsApplicable(invocation_M4a.Arguments).ShouldBeTrue();
      method_M4.IsApplicable(invocation_M4b.Arguments).ShouldBeTrue();
      method_M4.IsApplicable(invocation_M4c.Arguments).ShouldBeFalse();
      method_M4.IsApplicable(invocation_M4d.Arguments).ShouldBeFalse();
    }
  }
}