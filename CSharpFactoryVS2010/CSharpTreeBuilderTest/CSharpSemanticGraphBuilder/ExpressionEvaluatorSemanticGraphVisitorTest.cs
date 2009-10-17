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
    /// Tests the evaluation of literals.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Literal()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ExpressionEvaluatorSemanticGraphVisitor\Literal.cs");
      InvokeParser(project).ShouldBeTrue();

      var members = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A").Members.ToList();

      int i = 0;

      // object a0 = null;
      {
        var exp = ((members[i++] as FieldEntity).Initializer as ScalarInitializerEntity).Expression;
        exp.Result.GetType().ShouldEqual(typeof (NullExpressionResult));
      }
      // bool a1 = true;
      {
        var exp = ((members[i++] as FieldEntity).Initializer as ScalarInitializerEntity).Expression;
        (exp.Result as ValueExpressionResult).Type.ToString().ShouldEqual("global::System.Boolean");
      }
      // decimal a2 = 2m;
      {
        var exp = ((members[i++] as FieldEntity).Initializer as ScalarInitializerEntity).Expression;
        (exp.Result as ValueExpressionResult).Type.ToString().ShouldEqual("global::System.Decimal");
      }
      // int a3 = 3;
      {
        var exp = ((members[i++] as FieldEntity).Initializer as ScalarInitializerEntity).Expression;
        (exp.Result as ValueExpressionResult).Type.ToString().ShouldEqual("global::System.Int32");
      }
      // uint a4 = 4u;
      {
        var exp = ((members[i++] as FieldEntity).Initializer as ScalarInitializerEntity).Expression;
        (exp.Result as ValueExpressionResult).Type.ToString().ShouldEqual("global::System.UInt32");
      }
      // long a5 = 5l;
      {
        var exp = ((members[i++] as FieldEntity).Initializer as ScalarInitializerEntity).Expression;
        (exp.Result as ValueExpressionResult).Type.ToString().ShouldEqual("global::System.Int64");
      }
      // ulong a6 = 6ul;
      {
        var exp = ((members[i++] as FieldEntity).Initializer as ScalarInitializerEntity).Expression;
        (exp.Result as ValueExpressionResult).Type.ToString().ShouldEqual("global::System.UInt64");
      }
      // char a7 = '7';
      {
        var exp = ((members[i++] as FieldEntity).Initializer as ScalarInitializerEntity).Expression;
        (exp.Result as ValueExpressionResult).Type.ToString().ShouldEqual("global::System.Char");
      }
      // float a8 = 8f;
      {
        var exp = ((members[i++] as FieldEntity).Initializer as ScalarInitializerEntity).Expression;
        (exp.Result as ValueExpressionResult).Type.ToString().ShouldEqual("global::System.Single");
      }
      // double a9 = 9d;
      {
        var exp = ((members[i++] as FieldEntity).Initializer as ScalarInitializerEntity).Expression;
        (exp.Result as ValueExpressionResult).Type.ToString().ShouldEqual("global::System.Double");
      }
      // string a10 = "10";
      {
        var exp = ((members[i++] as FieldEntity).Initializer as ScalarInitializerEntity).Expression;
        (exp.Result as ValueExpressionResult).Type.ToString().ShouldEqual("global::System.String");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the evaluation of simple names.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [Ignore] // TODO: remove ignore when simple name resolution is implemented
    public void SimpleName()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ExpressionEvaluatorSemanticGraphVisitor\SimpleName.cs");
      InvokeParser(project).ShouldBeTrue();

      var members = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A").Members.ToList();

      // static int a = b;
      {
        var exp = ((members[0] as FieldEntity).Initializer as ScalarInitializerEntity).Expression;

        var simpleName = exp as SimpleNameExpressionEntity;
        simpleName.Entity.ShouldEqual(members[1] as FieldEntity);

        var variableExpressionResult = simpleName.Result as VariableExpressionResult;
        variableExpressionResult.Variable.ShouldEqual(members[1] as IVariableEntity);
        variableExpressionResult.Type.ShouldEqual((members[1] as FieldEntity).Type);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the evaluation of default value expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void DefaultValue()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ExpressionEvaluatorSemanticGraphVisitor\DefaultValue.cs");
      InvokeParser(project).ShouldBeTrue();

      var classA = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A",1);
      var members = classA.Members.ToList();

      int i = 0;

      // B a1 = default(B);
      {
        var classB = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("B");

        var exp = ((members[i++] as FieldEntity).Initializer as ScalarInitializerEntity).Expression;

        var defaultValueExpressionEntity = exp as DefaultValueExpressionEntity;
        defaultValueExpressionEntity.Type.ShouldEqual(classB);

        var valueExpressionResult = defaultValueExpressionEntity.Result as ValueExpressionResult;
        valueExpressionResult.Type.ShouldEqual(classB);
      }

      // object a2 = default(T);
      {
        var exp = ((members[i++] as FieldEntity).Initializer as ScalarInitializerEntity).Expression;

        var defaultValueExpressionEntity = exp as DefaultValueExpressionEntity;
        defaultValueExpressionEntity.Type.ShouldEqual(classA.GetOwnTypeParameterByName("T"));

        var valueExpressionResult = defaultValueExpressionEntity.Result as ValueExpressionResult;
        valueExpressionResult.Type.ShouldEqual(classA.GetOwnTypeParameterByName("T"));
      }
    }
  }
}