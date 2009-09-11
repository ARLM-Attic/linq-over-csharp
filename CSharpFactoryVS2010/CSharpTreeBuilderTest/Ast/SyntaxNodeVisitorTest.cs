using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;
using SoftwareApproach.TestingExtensions;
using Rhino.Mocks;

namespace CSharpTreeBuilderTest.Ast
{
  // ================================================================================================
  /// <summary>
  /// Tests the visitor traversal of the AST nodes
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class SyntaxNodeVisitorTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests that returning false from an AcceptVisitor stops the visiting.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void StoppingVisiting()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SyntaxNodeVisitor\CompilationUnitNodeVisitorTest.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var compilationUnitNode = project.SyntaxTree.CompilationUnitNodes[0];

      // Arrange
      var mocks = new MockRepository();
      var visitorMock = mocks.StrictMock<ISyntaxNodeVisitor>();
      using (mocks.Ordered())
      {
        Expect.Call(visitorMock.Visit(compilationUnitNode)).Return(false);
      }

      mocks.ReplayAll();

      // Act
      compilationUnitNode.AcceptVisitor(visitorMock);

      // Assert
      mocks.VerifyAll();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the visiting of a CompilationUnitNode
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void VisitCompilationUnitNode()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SyntaxNodeVisitor\CompilationUnitNodeVisitorTest.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var compilationUnitNode = project.SyntaxTree.CompilationUnitNodes[0];

      // Arrange
      var mocks = new MockRepository();
      var visitorMock = mocks.StrictMock<ISyntaxNodeVisitor>();
      using (mocks.Ordered())
      {
        Expect.Call(visitorMock.Visit(compilationUnitNode)).Return(true);
        Expect.Call(visitorMock.Visit(compilationUnitNode.ExternAliasNodes[0])).Return(true);
        Expect.Call(visitorMock.Visit(compilationUnitNode.UsingNodes[0])).Return(true);
        Expect.Call(visitorMock.Visit(compilationUnitNode.UsingNodes[0].NamespaceOrTypeName)).Return(true);
        Expect.Call(visitorMock.Visit(compilationUnitNode.UsingNodes[0].NamespaceOrTypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit((UsingAliasNode)compilationUnitNode.UsingNodes[1])).Return(true);
        Expect.Call(visitorMock.Visit(compilationUnitNode.UsingNodes[1].NamespaceOrTypeName)).Return(true);
        Expect.Call(visitorMock.Visit(compilationUnitNode.UsingNodes[1].NamespaceOrTypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(compilationUnitNode.GlobalAttributes[0])).Return(true);
        Expect.Call(visitorMock.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0])).Return(true);
        Expect.Call(visitorMock.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0].TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0].Arguments[0])).Return(true);
        Expect.Call(visitorMock.Visit((LiteralNode)compilationUnitNode.GlobalAttributes[0].Attributes[0].Arguments[0].Expression)).Return(true);
        Expect.Call(visitorMock.Visit(compilationUnitNode.NamespaceDeclarations[0])).Return(true);
        Expect.Call(visitorMock.Visit(compilationUnitNode.NamespaceDeclarations[0].NamespaceDeclarations[0])).Return(true);
        Expect.Call(visitorMock.Visit((ClassDeclarationNode)compilationUnitNode.NamespaceDeclarations[0].NamespaceDeclarations[0].TypeDeclarations[0])).Return(true);
        Expect.Call(visitorMock.Visit((ClassDeclarationNode)compilationUnitNode.TypeDeclarations[0])).Return(true);
        Expect.Call(visitorMock.Visit((StructDeclarationNode)compilationUnitNode.TypeDeclarations[1])).Return(true);
        Expect.Call(visitorMock.Visit((InterfaceDeclarationNode)compilationUnitNode.TypeDeclarations[2])).Return(true);
        Expect.Call(visitorMock.Visit((EnumDeclarationNode)compilationUnitNode.TypeDeclarations[3])).Return(true);
        Expect.Call(visitorMock.Visit((DelegateDeclarationNode)compilationUnitNode.TypeDeclarations[4])).Return(true);
        Expect.Call(visitorMock.Visit(((DelegateDeclarationNode)compilationUnitNode.TypeDeclarations[4]).Type)).Return(true);
        Expect.Call(visitorMock.Visit(((DelegateDeclarationNode)compilationUnitNode.TypeDeclarations[4]).Type.TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(((DelegateDeclarationNode)compilationUnitNode.TypeDeclarations[4]).Type.TypeName.TypeTags[0])).Return(true);
      }
      mocks.ReplayAll();

      // Act
      compilationUnitNode.AcceptVisitor(visitorMock);

      // Assert
      mocks.VerifyAll();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the visiting of a TypeOrNamespaceNode
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void VisitTypeOrNamespaceNode()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SyntaxNodeVisitor\TypeOrNamespaceNodeVisitorTest.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var typeOrNamespaceNode = project.SyntaxTree.CompilationUnitNodes[0].UsingNodes[0].NamespaceOrTypeName;

      // Arrange
      var mocks = new MockRepository();
      var visitorMock = mocks.StrictMock<ISyntaxNodeVisitor>();
      using (mocks.Ordered())
      {
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode)).Return(true);
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[0])).Return(true); // System
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[1])).Return(true); // Collections
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[2])).Return(true); // Generic
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[3])).Return(true); // IDictionary
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0])).Return(true); // System.Nullable<int>**[][,]
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeName)).Return(true); // System.Nullable<int>
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeName.TypeTags[0])).Return(true); // System
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeName.TypeTags[1])).Return(true); // Nullable
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeName.TypeTags[1].Arguments[0])).Return(true); // int
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeName.TypeTags[1].Arguments[0].TypeName)).Return(true); // int
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeName.TypeTags[1].Arguments[0].TypeName.TypeTags[0])).Return(true); // int
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].RankSpecifiers[0])).Return(true);     // []
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].RankSpecifiers[1])).Return(true);     // [,]
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[1])).Return(true); // string**[][,]
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[1].TypeName)).Return(true); // string
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[1].TypeName.TypeTags[0])).Return(true); // string
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[1].RankSpecifiers[0])).Return(true);     // []
        Expect.Call(visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[1].RankSpecifiers[1])).Return(true);     // [,]
      }
      mocks.ReplayAll();

      // Act
      typeOrNamespaceNode.AcceptVisitor(visitorMock);

      // Assert
      mocks.VerifyAll();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the visiting of a ClassDeclarationNode
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void VisitClassDeclarationNode()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SyntaxNodeVisitor\ClassDeclarationNodeVisitorTest.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var classDeclarationNode = (ClassDeclarationNode)project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0];

      // Arrange
      var mocks = new MockRepository();
      var visitorMock = mocks.StrictMock<ISyntaxNodeVisitor>();
      using (mocks.Ordered())
      {
        Expect.Call(visitorMock.Visit(classDeclarationNode)).Return(true);

        // Class attributes
        Expect.Call(visitorMock.Visit(classDeclarationNode.AttributeDecorations[0])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[1])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[1].TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[1].TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[1].Arguments[0])).Return(true);
        Expect.Call(visitorMock.Visit((LiteralNode)classDeclarationNode.AttributeDecorations[0].Attributes[1].Arguments[0].Expression)).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.AttributeDecorations[1])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[0])).Return(true);
        Expect.Call(visitorMock.Visit((LiteralNode)classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[0].Expression)).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[1])).Return(true);
        Expect.Call(visitorMock.Visit((LiteralNode)classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[1].Expression)).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[2])).Return(true);
        Expect.Call(visitorMock.Visit((LiteralNode)classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[2].Expression)).Return(true);

        // Type params and base types
        Expect.Call(visitorMock.Visit(classDeclarationNode.TypeParameters[0])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.TypeParameters[0].AttributeDecorations[0])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.TypeParameters[0].AttributeDecorations[0].Attributes[0])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.TypeParameters[0].AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.TypeParameters[0].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.TypeParameters[1])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.BaseTypes[0])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.BaseTypes[0].TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.BaseTypes[0].TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.BaseTypes[1])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.BaseTypes[1].TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.BaseTypes[1].TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.TypeParameterConstraints[0])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0].Type)).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0].Type.TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0].Type.TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.TypeParameterConstraints[1])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.TypeParameterConstraints[1].ConstraintTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(classDeclarationNode.TypeParameterConstraints[1].ConstraintTags[1])).Return(true);

        // Constant declaration
        {
          var constDeclarationNode = (ConstDeclarationNode)classDeclarationNode.MemberDeclarations[0];
          Expect.Call(visitorMock.Visit(constDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(constDeclarationNode.AttributeDecorations[0])).Return(true);
          Expect.Call(visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
          Expect.Call(visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(constDeclarationNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(constDeclarationNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(constDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(constDeclarationNode.ConstTags[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)constDeclarationNode.ConstTags[0].Expression)).Return(true);
          Expect.Call(visitorMock.Visit(constDeclarationNode.ConstTags[1])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)constDeclarationNode.ConstTags[1].Expression)).Return(true);
        }
        // Field declaration
        {
          var fieldDeclarationNode = (FieldDeclarationNode)classDeclarationNode.MemberDeclarations[1];
          Expect.Call(visitorMock.Visit(fieldDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(fieldDeclarationNode.AttributeDecorations[0])).Return(true);
          Expect.Call(visitorMock.Visit(fieldDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
          Expect.Call(visitorMock.Visit(fieldDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(fieldDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(fieldDeclarationNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(fieldDeclarationNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(fieldDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(fieldDeclarationNode.FieldTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(fieldDeclarationNode.FieldTags[1])).Return(true);
          var init = fieldDeclarationNode.FieldTags[1].Initializer as ExpressionInitializerNode;
          Expect.Call(visitorMock.Visit(init)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)init.Expression)).Return(true);
        }
        // Method declaration
        {
          var methodDeclarationNode = (MethodDeclarationNode)classDeclarationNode.MemberDeclarations[2];
          Expect.Call(visitorMock.Visit(methodDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.AttributeDecorations[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.TypeParameters[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.TypeParameters[1])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.TypeParameterConstraints[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.TypeParameterConstraints[1])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.TypeParameterConstraints[1].ConstraintTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.FormalParameters[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.FormalParameters[0].Type)).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.FormalParameters[0].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.FormalParameters[1])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.FormalParameters[1].Type)).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.FormalParameters[1].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.FormalParameters[1].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.FormalParameters[2])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.FormalParameters[2].AttributeDecorations[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.FormalParameters[2].AttributeDecorations[0].Attributes[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.FormalParameters[2].AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.FormalParameters[2].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.FormalParameters[2].Type)).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.FormalParameters[2].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.FormalParameters[2].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.FormalParameters[2].Type.RankSpecifiers[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.Body)).Return(true);
          var expressionStatement = methodDeclarationNode.Body.Statements[0] as ExpressionStatementNode;
          Expect.Call(visitorMock.Visit(expressionStatement)).Return(true);
          var exp = expressionStatement.Expression as AssignmentExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)exp.LeftOperand)).Return(true);
          var defaultExp = exp.RightOperand as DefaultValueExpressionNode;
          Expect.Call(visitorMock.Visit(defaultExp)).Return(true);
          Expect.Call(visitorMock.Visit(defaultExp.Type)).Return(true);
          Expect.Call(visitorMock.Visit(defaultExp.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(defaultExp.Type.TypeName.TypeTags[0])).Return(true);
        }
        // Method declaration (explicit interface implementation)
        {
          var methodDeclarationNode2 = (MethodDeclarationNode)classDeclarationNode.MemberDeclarations[3];
          Expect.Call(visitorMock.Visit(methodDeclarationNode2)).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode2.Type)).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode2.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode2.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode2.Body)).Return(true);
        }
        // Property declaration
        {
          var propertyDeclarationNode = (PropertyDeclarationNode)classDeclarationNode.MemberDeclarations[4];
          Expect.Call(visitorMock.Visit(propertyDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(propertyDeclarationNode.AttributeDecorations[0])).Return(true);
          Expect.Call(visitorMock.Visit(propertyDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
          Expect.Call(visitorMock.Visit(propertyDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(propertyDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(propertyDeclarationNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(propertyDeclarationNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(propertyDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(propertyDeclarationNode.GetAccessor)).Return(true);
          Expect.Call(visitorMock.Visit(propertyDeclarationNode.SetAccessor)).Return(true);
        }
        // Event declaration (field-like)
        {
          var eventDeclarationNode = (FieldDeclarationNode)classDeclarationNode.MemberDeclarations[5];
          Expect.Call(visitorMock.Visit(eventDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode.AttributeDecorations[0])).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode.Type.TypeName.TypeTags[0].Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode.Type.TypeName.TypeTags[0].Arguments[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode.FieldTags[0])).Return(true);
          var init = eventDeclarationNode.FieldTags[0].Initializer as ExpressionInitializerNode;
          Expect.Call(visitorMock.Visit(init)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)init.Expression)).Return(true);
        }
        // Event declaration (property-like)
        {
          var eventDeclarationNode2 = (EventPropertyDeclarationNode)classDeclarationNode.MemberDeclarations[6];
          Expect.Call(visitorMock.Visit(eventDeclarationNode2)).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode2.AttributeDecorations[0])).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode2.AttributeDecorations[0].Attributes[0])).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode2.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode2.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode2.Type)).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode2.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode2.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode2.Type.TypeName.TypeTags[0].Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode2.Type.TypeName.TypeTags[0].Arguments[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode2.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode2.AddAccessor)).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode2.AddAccessor.Body)).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode2.RemoveAccessor)).Return(true);
          Expect.Call(visitorMock.Visit(eventDeclarationNode2.RemoveAccessor.Body)).Return(true);
        }
        // Indexer declaration
        {
          var indexerDeclarationNode = (IndexerDeclarationNode)classDeclarationNode.MemberDeclarations[7];
          Expect.Call(visitorMock.Visit(indexerDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.AttributeDecorations[0])).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.FormalParameters[0])).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.FormalParameters[0].Type)).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.FormalParameters[0].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.FormalParameters[1])).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.FormalParameters[1].Type)).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.FormalParameters[1].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.FormalParameters[1].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.GetAccessor)).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.GetAccessor.Body)).Return(true);
          Expect.Call(visitorMock.Visit((ReturnStatementNode)indexerDeclarationNode.GetAccessor.Body.Statements[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)((ReturnStatementNode)indexerDeclarationNode.GetAccessor.Body.Statements[0]).Expression)).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.SetAccessor)).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.SetAccessor.Body)).Return(true);
        }
        // Operator declaration
        {
          var operatorDeclarationNode = (OperatorDeclarationNode)classDeclarationNode.MemberDeclarations[8];
          Expect.Call(visitorMock.Visit(operatorDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.AttributeDecorations[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type.TypeName.TypeTags[0].Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type.TypeName.TypeTags[0].Arguments[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type.TypeName.TypeTags[0].Arguments[1])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type.TypeName.TypeTags[0].Arguments[1].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type.TypeName.TypeTags[0].Arguments[1].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[1])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[1].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[1].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Body)).Return(true);
          Expect.Call(visitorMock.Visit((ReturnStatementNode)operatorDeclarationNode.Body.Statements[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)((ReturnStatementNode)operatorDeclarationNode.Body.Statements[0]).Expression)).Return(true);
        }
        // Conversion operator declaration
        {
          var castOperatorDeclarationNode = (CastOperatorDeclarationNode)classDeclarationNode.MemberDeclarations[9];
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.AttributeDecorations[0])).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0])).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type)).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[1])).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[1].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[1].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.Body)).Return(true);
          Expect.Call(visitorMock.Visit((ReturnStatementNode)castOperatorDeclarationNode.Body.Statements[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)((ReturnStatementNode)castOperatorDeclarationNode.Body.Statements[0]).Expression)).Return(true);
        }
        // Constructor declaration 
        {
          var constructorDeclarationNode = (ConstructorDeclarationNode)classDeclarationNode.MemberDeclarations[10];
          Expect.Call(visitorMock.Visit(constructorDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.AttributeDecorations[0])).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.FormalParameters[0])).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.FormalParameters[0].Type)).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.FormalParameters[0].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.FormalParameters[1])).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.FormalParameters[1].Type)).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.FormalParameters[1].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.FormalParameters[1].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.Body)).Return(true);
        }
        // Constructor declaration with this initializer
        {
          var constructorDeclarationNode2 = (ConstructorDeclarationNode)classDeclarationNode.MemberDeclarations[11];
          Expect.Call(visitorMock.Visit(constructorDeclarationNode2)).Return(true);
          Expect.Call(visitorMock.Visit((ThisConstructorInitializerNode)constructorDeclarationNode2.Initializer)).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode2.Initializer.Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)constructorDeclarationNode2.Initializer.Arguments[0].Expression)).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode2.Initializer.Arguments[1])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)constructorDeclarationNode2.Initializer.Arguments[1].Expression)).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode2.Body)).Return(true);
        }
        // Constructor declaration with base initializer
        {
          var constructorDeclarationNode3 = (ConstructorDeclarationNode)classDeclarationNode.MemberDeclarations[12];
          Expect.Call(visitorMock.Visit(constructorDeclarationNode3)).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode3.FormalParameters[0])).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode3.FormalParameters[0].Type)).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode3.FormalParameters[0].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode3.FormalParameters[0].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit((BaseConstructorInitializerNode)constructorDeclarationNode3.Initializer)).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode3.Body)).Return(true);
        }
        // Destructor declaration
        {
          var destructorDeclarationNode = (DestructorDeclarationNode)classDeclarationNode.MemberDeclarations[13];
          Expect.Call(visitorMock.Visit(destructorDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(destructorDeclarationNode.AttributeDecorations[0])).Return(true);
          Expect.Call(visitorMock.Visit(destructorDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
          Expect.Call(visitorMock.Visit(destructorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(destructorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(destructorDeclarationNode.Body)).Return(true);
        }
        // Nested class declaration
        {
          var nestedClassDeclarationNode = (ClassDeclarationNode) classDeclarationNode.NestedTypes[0];
          Expect.Call(visitorMock.Visit(nestedClassDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(nestedClassDeclarationNode.AttributeDecorations[0])).Return(true);
          Expect.Call(visitorMock.Visit(nestedClassDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
          Expect.Call(visitorMock.Visit(nestedClassDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(nestedClassDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
        }
      }
      mocks.ReplayAll();

      // Act
      classDeclarationNode.AcceptVisitor(visitorMock);

      // Assert
      mocks.VerifyAll();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the visiting of a StructDeclarationNode
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void VisitStructDeclarationNode()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SyntaxNodeVisitor\StructDeclarationNodeVisitorTest.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var structDeclarationNode = (StructDeclarationNode)project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0];

      // Arrange
      var mocks = new MockRepository();
      var visitorMock = mocks.StrictMock<ISyntaxNodeVisitor>();
      using (mocks.Ordered())
      {
        Expect.Call(visitorMock.Visit(structDeclarationNode)).Return(true);

        // Struct attributes
        Expect.Call(visitorMock.Visit(structDeclarationNode.AttributeDecorations[0])).Return(true);
        Expect.Call(visitorMock.Visit(structDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
        Expect.Call(visitorMock.Visit(structDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(structDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);

        // Type params and base types
        Expect.Call(visitorMock.Visit(structDeclarationNode.TypeParameters[0])).Return(true);
        Expect.Call(visitorMock.Visit(structDeclarationNode.TypeParameters[1])).Return(true);
        Expect.Call(visitorMock.Visit(structDeclarationNode.BaseTypes[0])).Return(true);
        Expect.Call(visitorMock.Visit(structDeclarationNode.BaseTypes[0].TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(structDeclarationNode.BaseTypes[0].TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(structDeclarationNode.TypeParameterConstraints[0])).Return(true);
        Expect.Call(visitorMock.Visit(structDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(structDeclarationNode.TypeParameterConstraints[1])).Return(true);
        Expect.Call(visitorMock.Visit(structDeclarationNode.TypeParameterConstraints[1].ConstraintTags[0])).Return(true);

        // Constant declaration
        {
          var constDeclarationNode = (ConstDeclarationNode)structDeclarationNode.MemberDeclarations[0];
          Expect.Call(visitorMock.Visit(constDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(constDeclarationNode.AttributeDecorations[0])).Return(true);
          Expect.Call(visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
          Expect.Call(visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(constDeclarationNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(constDeclarationNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(constDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(constDeclarationNode.ConstTags[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)constDeclarationNode.ConstTags[0].Expression)).Return(true);
        }
        // Field declaration
        {
          var fieldDeclarationNode = (FieldDeclarationNode)structDeclarationNode.MemberDeclarations[1];
          Expect.Call(visitorMock.Visit(fieldDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(fieldDeclarationNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(fieldDeclarationNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(fieldDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(fieldDeclarationNode.FieldTags[0])).Return(true);
        }
        // Method declaration
        {
          var methodDeclarationNode = (MethodDeclarationNode)structDeclarationNode.MemberDeclarations[2];
          Expect.Call(visitorMock.Visit(methodDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(methodDeclarationNode.Body)).Return(true);
        }
        // Property declaration
        {
          var propertyDeclarationNode = (PropertyDeclarationNode)structDeclarationNode.MemberDeclarations[3];
          Expect.Call(visitorMock.Visit(propertyDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(propertyDeclarationNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(propertyDeclarationNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(propertyDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(propertyDeclarationNode.GetAccessor)).Return(true);
          Expect.Call(visitorMock.Visit(propertyDeclarationNode.GetAccessor.Body)).Return(true);
          Expect.Call(visitorMock.Visit((ReturnStatementNode)propertyDeclarationNode.GetAccessor.Body.Statements[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)((ReturnStatementNode)propertyDeclarationNode.GetAccessor.Body.Statements[0]).Expression)).Return(true);
          Expect.Call(visitorMock.Visit(propertyDeclarationNode.SetAccessor)).Return(true);
          Expect.Call(visitorMock.Visit(propertyDeclarationNode.SetAccessor.Body)).Return(true);
        }
        // Event declaration (property-like)
        {
          var eventPropertyDeclarationNode = (EventPropertyDeclarationNode)structDeclarationNode.MemberDeclarations[4];
          Expect.Call(visitorMock.Visit(eventPropertyDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(eventPropertyDeclarationNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(eventPropertyDeclarationNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(eventPropertyDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(eventPropertyDeclarationNode.Type.TypeName.TypeTags[0].Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit(eventPropertyDeclarationNode.Type.TypeName.TypeTags[0].Arguments[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(eventPropertyDeclarationNode.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(eventPropertyDeclarationNode.AddAccessor)).Return(true);
          Expect.Call(visitorMock.Visit(eventPropertyDeclarationNode.AddAccessor.Body)).Return(true);
          Expect.Call(visitorMock.Visit(eventPropertyDeclarationNode.RemoveAccessor)).Return(true);
          Expect.Call(visitorMock.Visit(eventPropertyDeclarationNode.RemoveAccessor.Body)).Return(true);
        }
        // Indexer declaration
        {
          var indexerDeclarationNode = (IndexerDeclarationNode)structDeclarationNode.MemberDeclarations[5];
          Expect.Call(visitorMock.Visit(indexerDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.FormalParameters[0])).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.FormalParameters[0].Type)).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.FormalParameters[0].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.GetAccessor)).Return(true);
          Expect.Call(visitorMock.Visit(indexerDeclarationNode.GetAccessor.Body)).Return(true);
          Expect.Call(visitorMock.Visit((ReturnStatementNode)indexerDeclarationNode.GetAccessor.Body.Statements[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)((ReturnStatementNode)indexerDeclarationNode.GetAccessor.Body.Statements[0]).Expression)).Return(true);
        }
        // Operator declaration
        {
          var operatorDeclarationNode = (OperatorDeclarationNode)structDeclarationNode.MemberDeclarations[6];
          Expect.Call(visitorMock.Visit(operatorDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type.TypeName.TypeTags[0].Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type.TypeName.TypeTags[0].Arguments[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type.TypeName.TypeTags[0].Arguments[1])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type.TypeName.TypeTags[0].Arguments[1].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Type.TypeName.TypeTags[0].Arguments[1].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[1])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[1].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[1].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(operatorDeclarationNode.Body)).Return(true);
          Expect.Call(visitorMock.Visit((ReturnStatementNode)operatorDeclarationNode.Body.Statements[0])).Return(true);
          var exp = ((ReturnStatementNode) operatorDeclarationNode.Body.Statements[0]).Expression as
            ObjectCreationExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0].Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0].Arguments[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0].Arguments[1])).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0].Arguments[1].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0].Arguments[1].TypeName.TypeTags[0])).Return(true);
        }
        // Conversion operator declaration
        {
          var castOperatorDeclarationNode = (CastOperatorDeclarationNode)structDeclarationNode.MemberDeclarations[7];
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0])).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type)).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[1])).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[1].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0].Arguments[1].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castOperatorDeclarationNode.Body)).Return(true);
          Expect.Call(visitorMock.Visit((ReturnStatementNode)castOperatorDeclarationNode.Body.Statements[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)((ReturnStatementNode)castOperatorDeclarationNode.Body.Statements[0]).Expression)).Return(true);
        }
        // Constructor declaration 
        {
          var constructorDeclarationNode = (ConstructorDeclarationNode) structDeclarationNode.MemberDeclarations[8];
          Expect.Call(visitorMock.Visit(constructorDeclarationNode)).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.FormalParameters[0])).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.FormalParameters[0].Type)).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.FormalParameters[0].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(constructorDeclarationNode.Body)).Return(true);
          var expressionStatement = constructorDeclarationNode.Body.Statements[0] as ExpressionStatementNode;
          Expect.Call(visitorMock.Visit(expressionStatement)).Return(true);
          var assignment = expressionStatement.Expression as AssignmentExpressionNode;
          Expect.Call(visitorMock.Visit(assignment)).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode) assignment.LeftOperand)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode) assignment.RightOperand)).Return(true);
        }
        // Nested class declaration
        Expect.Call(visitorMock.Visit((ClassDeclarationNode)structDeclarationNode.NestedTypes[0])).Return(true);
      }
      mocks.ReplayAll();

      // Act
      structDeclarationNode.AcceptVisitor(visitorMock);

      // Assert
      mocks.VerifyAll();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the visiting of an InterfaceDeclarationNode
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void VisitInterfaceDeclarationNode()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SyntaxNodeVisitor\InterfaceDeclarationNodeVisitorTest.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var interfaceDeclarationNode = (InterfaceDeclarationNode)project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0];

      // Arrange
      var mocks = new MockRepository();
      var visitorMock = mocks.StrictMock<ISyntaxNodeVisitor>();
      using (mocks.Ordered())
      {
        Expect.Call(visitorMock.Visit(interfaceDeclarationNode)).Return(true);

        // Interface attributes
        Expect.Call(visitorMock.Visit(interfaceDeclarationNode.AttributeDecorations[0])).Return(true);
        Expect.Call(visitorMock.Visit(interfaceDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
        Expect.Call(visitorMock.Visit(interfaceDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(interfaceDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);

        // Type params and base types
        Expect.Call(visitorMock.Visit(interfaceDeclarationNode.TypeParameters[0])).Return(true);
        Expect.Call(visitorMock.Visit(interfaceDeclarationNode.TypeParameters[1])).Return(true);
        Expect.Call(visitorMock.Visit(interfaceDeclarationNode.BaseTypes[0])).Return(true);
        Expect.Call(visitorMock.Visit(interfaceDeclarationNode.BaseTypes[0].TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(interfaceDeclarationNode.BaseTypes[0].TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(interfaceDeclarationNode.TypeParameterConstraints[0])).Return(true);
        Expect.Call(visitorMock.Visit(interfaceDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(interfaceDeclarationNode.TypeParameterConstraints[1])).Return(true);
        Expect.Call(visitorMock.Visit(interfaceDeclarationNode.TypeParameterConstraints[1].ConstraintTags[0])).Return(true);

        // Method declaration
        var methodDeclarationNode = (MethodDeclarationNode)interfaceDeclarationNode.MemberDeclarations[0];
        Expect.Call(visitorMock.Visit(methodDeclarationNode)).Return(true);
        Expect.Call(visitorMock.Visit(methodDeclarationNode.AttributeDecorations[0])).Return(true);
        Expect.Call(visitorMock.Visit(methodDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
        Expect.Call(visitorMock.Visit(methodDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(methodDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(methodDeclarationNode.Type)).Return(true);
        Expect.Call(visitorMock.Visit(methodDeclarationNode.Type.TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(methodDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);

        // Property declaration
        var propertyDeclarationNode = (PropertyDeclarationNode)interfaceDeclarationNode.MemberDeclarations[1];
        Expect.Call(visitorMock.Visit(propertyDeclarationNode)).Return(true);
        Expect.Call(visitorMock.Visit(propertyDeclarationNode.Type)).Return(true);
        Expect.Call(visitorMock.Visit(propertyDeclarationNode.Type.TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(propertyDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(propertyDeclarationNode.GetAccessor)).Return(true);
        Expect.Call(visitorMock.Visit(propertyDeclarationNode.SetAccessor)).Return(true);

        // Event declaration
        var interfaceEventDeclarationNode = (InterfaceEventDeclarationNode)interfaceDeclarationNode.MemberDeclarations[2];
        Expect.Call(visitorMock.Visit(interfaceEventDeclarationNode)).Return(true);
        Expect.Call(visitorMock.Visit(interfaceEventDeclarationNode.Type)).Return(true);
        Expect.Call(visitorMock.Visit(interfaceEventDeclarationNode.Type.TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(interfaceEventDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(interfaceEventDeclarationNode.Type.TypeName.TypeTags[0].Arguments[0])).Return(true);
        Expect.Call(visitorMock.Visit(interfaceEventDeclarationNode.Type.TypeName.TypeTags[0].Arguments[0].TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(interfaceEventDeclarationNode.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0])).Return(true);

        // Indexer declaration
        var indexerDeclarationNode = (IndexerDeclarationNode)interfaceDeclarationNode.MemberDeclarations[3];
        Expect.Call(visitorMock.Visit(indexerDeclarationNode)).Return(true);
        Expect.Call(visitorMock.Visit(indexerDeclarationNode.Type)).Return(true);
        Expect.Call(visitorMock.Visit(indexerDeclarationNode.Type.TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(indexerDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(indexerDeclarationNode.FormalParameters[0])).Return(true);
        Expect.Call(visitorMock.Visit(indexerDeclarationNode.FormalParameters[0].Type)).Return(true);
        Expect.Call(visitorMock.Visit(indexerDeclarationNode.FormalParameters[0].Type.TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(indexerDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(indexerDeclarationNode.GetAccessor)).Return(true);
      }
      mocks.ReplayAll();

      // Act
      interfaceDeclarationNode.AcceptVisitor(visitorMock);

      // Assert
      mocks.VerifyAll();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the visiting of an EnumDeclarationNode
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void VisitEnumDeclarationNode()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SyntaxNodeVisitor\EnumDeclarationNodeVisitorTest.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var enumDeclarationNode = (EnumDeclarationNode)project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0];

      // Arrange
      var mocks = new MockRepository();
      var visitorMock = mocks.StrictMock<ISyntaxNodeVisitor>();
      using (mocks.Ordered())
      {
        Expect.Call(visitorMock.Visit(enumDeclarationNode)).Return(true);

        // Enum attributes
        Expect.Call(visitorMock.Visit(enumDeclarationNode.AttributeDecorations[0])).Return(true);
        Expect.Call(visitorMock.Visit(enumDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
        Expect.Call(visitorMock.Visit(enumDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(enumDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);

        // Base type
        Expect.Call(visitorMock.Visit(enumDeclarationNode.EnumBase)).Return(true);
        Expect.Call(visitorMock.Visit(enumDeclarationNode.EnumBase.TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(enumDeclarationNode.EnumBase.TypeName.TypeTags[0])).Return(true);

        // Enum member declaration
        Expect.Call(visitorMock.Visit(enumDeclarationNode.Values[0])).Return(true);
        Expect.Call(visitorMock.Visit(enumDeclarationNode.Values[0].AttributeDecorations[0])).Return(true);
        Expect.Call(visitorMock.Visit(enumDeclarationNode.Values[0].AttributeDecorations[0].Attributes[0])).Return(true);
        Expect.Call(visitorMock.Visit(enumDeclarationNode.Values[0].AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(enumDeclarationNode.Values[0].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit((LiteralNode)enumDeclarationNode.Values[0].Expression)).Return(true);
        Expect.Call(visitorMock.Visit(enumDeclarationNode.Values[1])).Return(true);
      }
      mocks.ReplayAll();

      // Act
      enumDeclarationNode.AcceptVisitor(visitorMock);

      // Assert
      mocks.VerifyAll();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the visiting of an DelegateDeclarationNode
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void VisitDelegateDeclarationNode()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SyntaxNodeVisitor\DelegateDeclarationNodeVisitorTest.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var delegateDeclarationNode = (DelegateDeclarationNode)project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0];

      // Arrange
      var mocks = new MockRepository();
      var visitorMock = mocks.StrictMock<ISyntaxNodeVisitor>();
      using (mocks.Ordered())
      {
        Expect.Call(visitorMock.Visit(delegateDeclarationNode)).Return(true);

        // Delegate attributes
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.AttributeDecorations[0])).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.AttributeDecorations[0].Attributes[0])).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);

        // Return type, type parameters and constraints
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.Type)).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.Type.TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.Type.TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.TypeParameters[0])).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.TypeParameters[1])).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.TypeParameterConstraints[0])).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.TypeParameterConstraints[1])).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.TypeParameterConstraints[1].ConstraintTags[0])).Return(true);

        // Formal parameters
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.FormalParameters[0])).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.FormalParameters[0].AttributeDecorations[0])).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.FormalParameters[0].AttributeDecorations[0].Attributes[0])).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.FormalParameters[0].AttributeDecorations[0].Attributes[0].TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.FormalParameters[0].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.FormalParameters[0].Type)).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.FormalParameters[0].Type.TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.FormalParameters[0].Type.TypeName.TypeTags[0])).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.FormalParameters[1])).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.FormalParameters[1].Type)).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.FormalParameters[1].Type.TypeName)).Return(true);
        Expect.Call(visitorMock.Visit(delegateDeclarationNode.FormalParameters[1].Type.TypeName.TypeTags[0])).Return(true);
      }
      mocks.ReplayAll();

      // Act
      delegateDeclarationNode.AcceptVisitor(visitorMock);

      // Assert
      mocks.VerifyAll();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the visiting of StatementNode nodes
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void VisitStatementNodes()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SyntaxNodeVisitor\StatementVisitorTest.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      
      // method1 includes all kinds of statements except yield-s
      var method1Body = ((MethodDeclarationNode)project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0]).Body;
      // method2 includes yield statements
      var method2Body = ((MethodDeclarationNode)project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[1]).Body;

      // Arrange
      var mocks = new MockRepository();
      var visitorMock = mocks.StrictMock<ISyntaxNodeVisitor>();
      using (mocks.Ordered())
      {
        Expect.Call(visitorMock.Visit(method1Body)).Return(true);
        
        int i = 0;

        // empty statement
        {
          var emptyStatementNode = (EmptyStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(emptyStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(emptyStatementNode.Labels[0])).Return(true);
        }
        // local variable declaration
        {
          var variableDeclarationStatementNode = (VariableDeclarationStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(variableDeclarationStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(variableDeclarationStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit(variableDeclarationStatementNode.Declaration)).Return(true);
          Expect.Call(visitorMock.Visit(variableDeclarationStatementNode.Declaration.Type)).Return(true);
          Expect.Call(visitorMock.Visit(variableDeclarationStatementNode.Declaration.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(variableDeclarationStatementNode.Declaration.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(variableDeclarationStatementNode.Declaration.VariableTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(variableDeclarationStatementNode.Declaration.VariableTags[1])).Return(true);
          var expInit =
            variableDeclarationStatementNode.Declaration.VariableTags[1].Initializer as ExpressionInitializerNode;
          Expect.Call(visitorMock.Visit(expInit)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)expInit.Expression)).Return(true);
        }
        // local variable declaration (var)
        {
          var varNode = (VariableDeclarationStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(varNode)).Return(true);
          Expect.Call(visitorMock.Visit(varNode.Declaration)).Return(true);
          Expect.Call(visitorMock.Visit(varNode.Declaration.Type)).Return(true);
          Expect.Call(visitorMock.Visit(varNode.Declaration.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(varNode.Declaration.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(varNode.Declaration.VariableTags[0])).Return(true);
          var expInit =
            varNode.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
          Expect.Call(visitorMock.Visit(expInit)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)expInit.Expression)).Return(true);
        }
        // local constant declaration
        {
          var constStatementNode = (ConstStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(constStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(constStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit(constStatementNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(constStatementNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(constStatementNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(constStatementNode.ConstTags[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)constStatementNode.ConstTags[0].Expression)).Return(true);
          Expect.Call(visitorMock.Visit(constStatementNode.ConstTags[1])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)constStatementNode.ConstTags[1].Expression)).Return(true);
        }
        // expression statement
        {
          var expressionStatementNode = (ExpressionStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(expressionStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(expressionStatementNode.Labels[0])).Return(true);
          var exp = expressionStatementNode.Expression as PostIncrementExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)exp.Operand)).Return(true);
        }
        // if statement
        {
          var ifStatementNode = (IfStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(ifStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(ifStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)ifStatementNode.Condition)).Return(true);
          Expect.Call(visitorMock.Visit((BlockStatementNode)ifStatementNode.ThenStatement)).Return(true);
          Expect.Call(visitorMock.Visit((BlockStatementNode)ifStatementNode.ElseStatement)).Return(true);
        }
        // switch statement
        {
          var switchStatementNode = (SwitchStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(switchStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(switchStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)switchStatementNode.Expression)).Return(true);
          Expect.Call(visitorMock.Visit(switchStatementNode.SwitchSections[0])).Return(true);
          Expect.Call(visitorMock.Visit(switchStatementNode.SwitchSections[0].Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)switchStatementNode.SwitchSections[0].Labels[0].Expression)).Return(true);
          Expect.Call(visitorMock.Visit(switchStatementNode.SwitchSections[0].Labels[1])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)switchStatementNode.SwitchSections[0].Labels[1].Expression)).Return(true);
          var goto1 = switchStatementNode.SwitchSections[0].Statements[0] as GotoStatementNode;
          Expect.Call(visitorMock.Visit(goto1)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)goto1.Expression)).Return(true);
          Expect.Call(visitorMock.Visit(switchStatementNode.SwitchSections[1])).Return(true);
          Expect.Call(visitorMock.Visit(switchStatementNode.SwitchSections[1].Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)switchStatementNode.SwitchSections[1].Labels[0].Expression)).Return(true);
          Expect.Call(visitorMock.Visit((GotoStatementNode)switchStatementNode.SwitchSections[1].Statements[0])).Return(true);
          Expect.Call(visitorMock.Visit(switchStatementNode.SwitchSections[2])).Return(true);
          Expect.Call(visitorMock.Visit(switchStatementNode.SwitchSections[2].Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit((BreakStatementNode)switchStatementNode.SwitchSections[2].Statements[0])).Return(true);
        }
        // while statement
        {
          var whileStatementNode = (WhileStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(whileStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(whileStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)whileStatementNode.Condition)).Return(true);
          Expect.Call(visitorMock.Visit((BlockStatementNode)whileStatementNode.Statement)).Return(true);
        }
        // do statement
        {
          var doWhileStatementNode = (DoWhileStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(doWhileStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(doWhileStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit((BlockStatementNode)doWhileStatementNode.Statement)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)doWhileStatementNode.Condition)).Return(true);
        }
        // for-statement (with local-variable-declaration)
        {
          var forStatementNode = (ForStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(forStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(forStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit(forStatementNode.Initializer)).Return(true);
          Expect.Call(visitorMock.Visit(forStatementNode.Initializer.Type)).Return(true);
          Expect.Call(visitorMock.Visit(forStatementNode.Initializer.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(forStatementNode.Initializer.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(forStatementNode.Initializer.VariableTags[0])).Return(true);
          var expInit = forStatementNode.Initializer.VariableTags[0].Initializer as ExpressionInitializerNode;
          Expect.Call(visitorMock.Visit(expInit)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)expInit.Expression)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)forStatementNode.Condition)).Return(true);
          Expect.Call(visitorMock.Visit((PostIncrementExpressionNode)forStatementNode.Iterators[0])).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)((PostIncrementExpressionNode)forStatementNode.Iterators[0]).Operand)).Return(true);
          Expect.Call(visitorMock.Visit((ContinueStatementNode)forStatementNode.Statement)).Return(true);
        }
        // for-statement (with statement-expression-list)
        {
          var forStatementNode2 = (ForStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(forStatementNode2)).Return(true);
          Expect.Call(visitorMock.Visit(forStatementNode2.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit((PostIncrementExpressionNode)forStatementNode2.Initializers[0])).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)((PostIncrementExpressionNode)forStatementNode2.Initializers[0]).Operand)).Return(true);
          Expect.Call(visitorMock.Visit((PostIncrementExpressionNode)forStatementNode2.Initializers[1])).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)((PostIncrementExpressionNode)forStatementNode2.Initializers[1]).Operand)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)forStatementNode2.Condition)).Return(true);
          Expect.Call(visitorMock.Visit((PostIncrementExpressionNode)forStatementNode2.Iterators[0])).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)((PostIncrementExpressionNode)forStatementNode2.Iterators[0]).Operand)).Return(true);
          Expect.Call(visitorMock.Visit((PostIncrementExpressionNode)forStatementNode2.Iterators[1])).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)((PostIncrementExpressionNode)forStatementNode2.Iterators[1]).Operand)).Return(true);
          Expect.Call(visitorMock.Visit((BlockStatementNode)forStatementNode2.Statement)).Return(true);
        }
        // foreach-statement
        {
          var foreachStatementNode = (ForeachStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(foreachStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(foreachStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit(foreachStatementNode.Type)).Return(true);
          Expect.Call(visitorMock.Visit(foreachStatementNode.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(foreachStatementNode.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)foreachStatementNode.CollectionExpression)).Return(true);
          Expect.Call(visitorMock.Visit((BlockStatementNode)foreachStatementNode.Statement)).Return(true);
        }
        // try-statement
        {
          var tryStatementNode = (TryStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(tryStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(tryStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit(tryStatementNode.TryBlock)).Return(true);
          Expect.Call(visitorMock.Visit(tryStatementNode.CatchClauses[0])).Return(true);
          Expect.Call(visitorMock.Visit(tryStatementNode.CatchClauses[0].Type)).Return(true);
          Expect.Call(visitorMock.Visit(tryStatementNode.CatchClauses[0].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(tryStatementNode.CatchClauses[0].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(tryStatementNode.CatchClauses[0].Block)).Return(true);
          Expect.Call(visitorMock.Visit(tryStatementNode.CatchClauses[1])).Return(true);
          Expect.Call(visitorMock.Visit(tryStatementNode.CatchClauses[1].Block)).Return(true);
          Expect.Call(visitorMock.Visit(tryStatementNode.FinallyBlock)).Return(true);
        }
        // checked-statement
        {
          var checkedStatementNode = (CheckedStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(checkedStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(checkedStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit(checkedStatementNode.Block)).Return(true);
        }
        // unchecked-statement
        {
          var uncheckedStatementNode = (UncheckedStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(uncheckedStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(uncheckedStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit(uncheckedStatementNode.Block)).Return(true);
        }
        // lock-statement
        {
          var lockStatementNode = (LockStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(lockStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(lockStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit((ThisAccessNode)lockStatementNode.Expression)).Return(true);
          Expect.Call(visitorMock.Visit((BlockStatementNode)lockStatementNode.Statement)).Return(true);
        }
        // using-statement (with local-variable-declaration)
        {
          var usingStatementNode = (UsingStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(usingStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(usingStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit(usingStatementNode.Initializer)).Return(true);
          Expect.Call(visitorMock.Visit(usingStatementNode.Initializer.Type)).Return(true);
          Expect.Call(visitorMock.Visit(usingStatementNode.Initializer.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(usingStatementNode.Initializer.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(usingStatementNode.Initializer.VariableTags[0])).Return(true);
          var expInit = usingStatementNode.Initializer.VariableTags[0].Initializer as ExpressionInitializerNode;
          Expect.Call(visitorMock.Visit(expInit)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)expInit.Expression)).Return(true);
          Expect.Call(visitorMock.Visit((BlockStatementNode)usingStatementNode.Statement)).Return(true);
        }
        // using-statement (with expression)
        {
          var usingStatementNode = (UsingStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(usingStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(usingStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)usingStatementNode.Expression)).Return(true);
          Expect.Call(visitorMock.Visit((BlockStatementNode)usingStatementNode.Statement)).Return(true);
        }
        // return statement
        {
          var returnStatementNode = (ReturnStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(returnStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(returnStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)returnStatementNode.Expression)).Return(true);
        }
        // goto-statement
        {
          var gotoStatementNode = (GotoStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(gotoStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(gotoStatementNode.Labels[0])).Return(true);
        }
        // throw-statement
        {
          var throwStatementNode = (ThrowStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(throwStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(throwStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit((ObjectCreationExpressionNode)throwStatementNode.Expression)).Return(true);
          Expect.Call(visitorMock.Visit(((ObjectCreationExpressionNode)throwStatementNode.Expression).Type)).Return(true);
          Expect.Call(visitorMock.Visit(((ObjectCreationExpressionNode)throwStatementNode.Expression).Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(((ObjectCreationExpressionNode)throwStatementNode.Expression).Type.TypeName.TypeTags[0])).Return(true);
        }
        // unsafe-statement
        {
          var unsafeStatementNode = (UnsafeStatementNode)method1Body.Statements[i++];
          Expect.Call(visitorMock.Visit(unsafeStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(unsafeStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit(unsafeStatementNode.Block)).Return(true);

          int j = 0;

          var stringVarNode = (VariableDeclarationStatementNode)unsafeStatementNode.Block.Statements[j++];
          Expect.Call(visitorMock.Visit(stringVarNode)).Return(true);
          Expect.Call(visitorMock.Visit(stringVarNode.Declaration)).Return(true);
          Expect.Call(visitorMock.Visit(stringVarNode.Declaration.Type)).Return(true);
          Expect.Call(visitorMock.Visit(stringVarNode.Declaration.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(stringVarNode.Declaration.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(stringVarNode.Declaration.VariableTags[0])).Return(true);
          var expInit = stringVarNode.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
          Expect.Call(visitorMock.Visit(expInit)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)expInit.Expression)).Return(true);

          // fixed-statement
          {
            var fixedStatementNode = (FixedStatementNode)unsafeStatementNode.Block.Statements[j++];
            Expect.Call(visitorMock.Visit(fixedStatementNode)).Return(true);
            Expect.Call(visitorMock.Visit(fixedStatementNode.Labels[0])).Return(true);
            Expect.Call(visitorMock.Visit(fixedStatementNode.Type)).Return(true);
            Expect.Call(visitorMock.Visit(fixedStatementNode.Type.TypeName)).Return(true);
            Expect.Call(visitorMock.Visit(fixedStatementNode.Type.TypeName.TypeTags[0])).Return(true);
            Expect.Call(visitorMock.Visit(fixedStatementNode.Initializers[0])).Return(true);
            Expect.Call(visitorMock.Visit((SimpleNameNode)fixedStatementNode.Initializers[0].Expression)).Return(true);
            Expect.Call(visitorMock.Visit(fixedStatementNode.Initializers[1])).Return(true);
            Expect.Call(visitorMock.Visit((SimpleNameNode)fixedStatementNode.Initializers[1].Expression)).Return(true);
            Expect.Call(visitorMock.Visit((BlockStatementNode)fixedStatementNode.Statement)).Return(true);
          }
          // stackalloc initializer
          // char* p = stackalloc char[256];
          {
            var stackallocDecl = (VariableDeclarationStatementNode)unsafeStatementNode.Block.Statements[j++];
            Expect.Call(visitorMock.Visit(stackallocDecl)).Return(true);
            Expect.Call(visitorMock.Visit(stackallocDecl.Declaration)).Return(true);
            Expect.Call(visitorMock.Visit(stackallocDecl.Declaration.Type)).Return(true);
            Expect.Call(visitorMock.Visit(stackallocDecl.Declaration.Type.TypeName)).Return(true);
            Expect.Call(visitorMock.Visit(stackallocDecl.Declaration.Type.TypeName.TypeTags[0])).Return(true);
            Expect.Call(visitorMock.Visit(stackallocDecl.Declaration.VariableTags[0])).Return(true);
            var stackallocInit = stackallocDecl.Declaration.VariableTags[0].Initializer as StackAllocInitializerNode;
            Expect.Call(visitorMock.Visit(stackallocInit)).Return(true);
            Expect.Call(visitorMock.Visit(stackallocInit.Type)).Return(true);
            Expect.Call(visitorMock.Visit(stackallocInit.Type.TypeName)).Return(true);
            Expect.Call(visitorMock.Visit(stackallocInit.Type.TypeName.TypeTags[0])).Return(true);
            Expect.Call(visitorMock.Visit((LiteralNode)stackallocInit.Expression)).Return(true);
          }
        }

        // IteratorMethod
        Expect.Call(visitorMock.Visit(method2Body)).Return(true);

        // yield-return-statement
        {
          var yieldReturnStatementNode = (YieldReturnStatementNode) method2Body.Statements[0];
          Expect.Call(visitorMock.Visit(yieldReturnStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(yieldReturnStatementNode.Labels[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)yieldReturnStatementNode.Expression)).Return(true);
        }
        // yield-break-statement
        {
          var yieldBreakStatementNode = (YieldBreakStatementNode) method2Body.Statements[1];
          Expect.Call(visitorMock.Visit(yieldBreakStatementNode)).Return(true);
          Expect.Call(visitorMock.Visit(yieldBreakStatementNode.Labels[0])).Return(true);
        }
      }
      mocks.ReplayAll();

      // Act
      method1Body.AcceptVisitor(visitorMock);
      method2Body.AcceptVisitor(visitorMock);

      // Assert
      mocks.VerifyAll();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the visiting of ExpressionNode nodes
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void VisitExpressionNodes()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SyntaxNodeVisitor\ExpressionVisitorTest.cs");
      InvokeParser(project, true, false).ShouldBeTrue();

      // The first method includes a collection initializer with all kinds of expressions
      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varInit = ((VariableDeclarationStatementNode)method.Body.Statements[0]).Declaration.VariableTags[0].Initializer;
      var collInit = ((ObjectCreationExpressionNode) ((ExpressionInitializerNode) varInit).Expression).ObjectOrCollectionInitializer;

      // Arrange
      var mocks = new MockRepository();
      var visitorMock = mocks.StrictMock<ISyntaxNodeVisitor>();
      using (mocks.Ordered())
      {
        Expect.Call(visitorMock.Visit(collInit)).Return(true);

        int i = 0;

        // array-creation-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as ArrayCreationExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.ArraySizeSpecifier)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.ArraySizeSpecifier.Expressions[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.ArraySizeSpecifier.Expressions[1])).Return(true);
          Expect.Call(visitorMock.Visit(exp.Initializer)).Return(true);
          var arrInit = exp.Initializer.VariableInitializers[0] as ArrayInitializerNode;
          Expect.Call(visitorMock.Visit(arrInit)).Return(true);
          Expect.Call(visitorMock.Visit((ExpressionInitializerNode)arrInit.VariableInitializers[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)((ExpressionInitializerNode)arrInit.VariableInitializers[0]).Expression)).Return(true);
          Expect.Call(visitorMock.Visit((ExpressionInitializerNode)arrInit.VariableInitializers[1])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)((ExpressionInitializerNode)arrInit.VariableInitializers[1]).Expression)).Return(true);
        }
        // true-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as LiteralNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
        }
        // false-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as LiteralNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
        }
        // char-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as LiteralNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
        }
        // decimal-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as LiteralNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
        }
        // double-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as LiteralNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
        }
        // int32-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as LiteralNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
        }
        // int64-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as LiteralNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
        }
        // null-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as LiteralNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
        }
        // single-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as LiteralNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
        }
        // string-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as LiteralNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
        }
        // uint32-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as LiteralNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
        }
        // uint64-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as LiteralNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
        }
        // simple-name
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as SimpleNameNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
        }
        // parenthesized-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as ParenthesizedExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.Expression)).Return(true);
        }
        // primary-expression-member-access
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as PrimaryExpressionMemberAccessNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)exp.PrimaryExpression)).Return(true);
          Expect.Call(visitorMock.Visit(exp.MemberName)).Return(true);
        }
        // pointer-member-access (inside an invocation-expression)
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as InvocationExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          var pointerAccess = exp.PrimaryExpression as PointerMemberAccessNode;
          Expect.Call(visitorMock.Visit(pointerAccess)).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)pointerAccess.PrimaryExpression)).Return(true);
          Expect.Call(visitorMock.Visit(pointerAccess.MemberName)).Return(true);
        }
        // predefined-type-member-access
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as PredefinedTypeMemberAccessNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit(exp.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.MemberName)).Return(true);
        }
        // qualified-alias-member-access
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as QualifiedAliasMemberAccessNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit(exp.QualifiedAliasMember)).Return(true);
          Expect.Call(visitorMock.Visit(exp.MemberName)).Return(true);
        }
        // invocation-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as InvocationExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)exp.PrimaryExpression)).Return(true);
          Expect.Call(visitorMock.Visit(((SimpleNameNode)exp.PrimaryExpression).Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit(((SimpleNameNode)exp.PrimaryExpression).Arguments[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(((SimpleNameNode)exp.PrimaryExpression).Arguments[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.Arguments[0].Expression)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Arguments[1])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.Arguments[1].Expression)).Return(true);
        }
        // element-access
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as ElementAccessNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)exp.PrimaryExpression)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.Expressions[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.Expressions[1])).Return(true);
        }
        // this-access
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as ThisAccessNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
        }
        // base-member-access
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as BaseMemberAccessNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit(exp.MemberName)).Return(true);
        }
        // base-element-access
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as BaseElementAccessNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.Expressions[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.Expressions[1])).Return(true);
        }
        // post-increment-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as PostIncrementExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)exp.Operand)).Return(true);
        }
        // post-decrement-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as PostDecrementExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)exp.Operand)).Return(true);
        }
        // object-creation-expression (with collection initializer)
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as ObjectCreationExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0].Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0].Arguments[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0].Arguments[1])).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0].Arguments[1].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0].Arguments[1].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.ObjectOrCollectionInitializer)).Return(true);
          Expect.Call(visitorMock.Visit(exp.ObjectOrCollectionInitializer.ElementInitializers[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.ObjectOrCollectionInitializer.ElementInitializers[0].ExpressionList[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.ObjectOrCollectionInitializer.ElementInitializers[0].ExpressionList[1])).Return(true);
          Expect.Call(visitorMock.Visit(exp.ObjectOrCollectionInitializer.ElementInitializers[1])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.ObjectOrCollectionInitializer.ElementInitializers[1].ExpressionList[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.ObjectOrCollectionInitializer.ElementInitializers[1].ExpressionList[1])).Return(true);
        }
        // object-creation-expression (with argument-list and object-initializer
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as ObjectCreationExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.Arguments[0].Expression)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Arguments[1])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.Arguments[1].Expression)).Return(true);
          Expect.Call(visitorMock.Visit(exp.ObjectOrCollectionInitializer)).Return(true);
          Expect.Call(visitorMock.Visit(exp.ObjectOrCollectionInitializer.MemberInitializers[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.ObjectOrCollectionInitializer.MemberInitializers[0].Expression)).Return(true);
          Expect.Call(visitorMock.Visit(exp.ObjectOrCollectionInitializer.MemberInitializers[1])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.ObjectOrCollectionInitializer.MemberInitializers[1].Expression)).Return(true);
        }
        // anonymous-object-creation-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as AnonymousObjectCreationExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          var simpleNameMemberDeclarator = exp.Declarators[0] as SimpleNameMemberDeclaratorNode;
          Expect.Call(visitorMock.Visit(simpleNameMemberDeclarator)).Return(true);
          Expect.Call(visitorMock.Visit(simpleNameMemberDeclarator.SimpleName)).Return(true);
          var memberAccessMemberDeclarator = exp.Declarators[1] as MemberAccessMemberDeclaratorNode;
          Expect.Call(visitorMock.Visit(memberAccessMemberDeclarator)).Return(true);
          var memberAccess = memberAccessMemberDeclarator.MemberAccess as PrimaryExpressionMemberAccessNode;
          Expect.Call(visitorMock.Visit(memberAccess)).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)memberAccess.PrimaryExpression)).Return(true);
          Expect.Call(visitorMock.Visit(memberAccess.MemberName)).Return(true);
          var baseMemberAccessMemberDeclarator = exp.Declarators[2] as BaseMemberAccessMemberDeclaratorNode;
          Expect.Call(visitorMock.Visit(baseMemberAccessMemberDeclarator)).Return(true);
          Expect.Call(visitorMock.Visit(baseMemberAccessMemberDeclarator.BaseMemberAccess)).Return(true);
          Expect.Call(visitorMock.Visit(baseMemberAccessMemberDeclarator.BaseMemberAccess.MemberName)).Return(true);
          var identifierMemberDeclarator = exp.Declarators[3] as IdentifierMemberDeclaratorNode;
          Expect.Call(visitorMock.Visit(identifierMemberDeclarator)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)identifierMemberDeclarator.Expression)).Return(true);
        }
        // typeof-expression (with unbound typename)
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as TypeofExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0].Arguments[0])).Return(true);
        }
        //// sizeof-expression
        //sizeof (int),
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as SizeofExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0])).Return(true);
        }
        // checked-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as CheckedExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.Expression)).Return(true);
        }
        // unchecked-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as UncheckedExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.Expression)).Return(true);
        }
        // default-value-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as DefaultValueExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.Type.TypeName.TypeTags[0])).Return(true);
        }
        // anonymous-method-expression (wrapped in a cast)
        // (Func<int, int, int>) delegate(int i1, int i2) { return 1; },
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var castExp = elemInit.NonAssignmentExpression as CastExpressionNode;
          Expect.Call(visitorMock.Visit(castExp)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[1])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[1].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[1].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[2])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[2].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[2].TypeName.TypeTags[0])).Return(true);
          var exp = castExp.Operand as AnonymousMethodExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[0].Type)).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[0].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[0].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[1])).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[1].Type)).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[1].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[1].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.Body)).Return(true);
          Expect.Call(visitorMock.Visit((ReturnStatementNode)exp.Body.Statements[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)((ReturnStatementNode)exp.Body.Statements[0]).Expression)).Return(true);
        }
        // lambda-expression (with implicit signature, expression body) (wrapped in a cast + parens)
        // (Expression<Func<int, int, int>>) ((x, y) => x + y),
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var castExp = elemInit.NonAssignmentExpression as CastExpressionNode;
          Expect.Call(visitorMock.Visit(castExp)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0].Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0].Arguments[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0].Arguments[1])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0].Arguments[1].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0].Arguments[1].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0].Arguments[2])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0].Arguments[2].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0].Arguments[2].TypeName.TypeTags[0])).Return(true);
          var paren = castExp.Operand as ParenthesizedExpressionNode;
          Expect.Call(visitorMock.Visit(paren)).Return(true);
          var exp = paren.Expression as LambdaExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[0].Type)).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[1])).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[1].Type)).Return(true);
          var binExp = exp.Expression as BinaryExpressionNode;
          Expect.Call(visitorMock.Visit(binExp)).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)binExp.LeftOperand)).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)binExp.RightOperand)).Return(true);
        }
        // lambda-expression (with explicit signature, block body) (wrapped in a cast + parens)
        // (Func<int, int, int>) ((int i1, int i2) => { return 1; }),
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var castExp = elemInit.NonAssignmentExpression as CastExpressionNode;
          Expect.Call(visitorMock.Visit(castExp)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[0].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[1])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[1].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[1].TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[2])).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[2].TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0].Arguments[2].TypeName.TypeTags[0])).Return(true);
          var paren = castExp.Operand as ParenthesizedExpressionNode;
          Expect.Call(visitorMock.Visit(paren)).Return(true);
          var exp = paren.Expression as LambdaExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[0].Type)).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[0].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[0].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[1])).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[1].Type)).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[1].Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.FormalParameters[1].Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit(exp.Block)).Return(true);
          Expect.Call(visitorMock.Visit((ReturnStatementNode)exp.Block.Statements[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)((ReturnStatementNode)exp.Block.Statements[0]).Expression)).Return(true);
        }
        // unary-expression
        // -1,
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as UnaryOperatorExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.Operand)).Return(true);
        }
        // pre-increment-expression
        // ++p,
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as PreIncrementExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)exp.Operand)).Return(true);
        }
        // pre-decrement-expression
        // --p,
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as PreDecrementExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)exp.Operand)).Return(true);
        }
        // cast-expression
        // (int) 1,
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var castExp = elemInit.NonAssignmentExpression as CastExpressionNode;
          Expect.Call(visitorMock.Visit(castExp)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(castExp.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)castExp.Operand)).Return(true);
        }
        // binary-expression
        // 1 + 1,
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as BinaryExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.LeftOperand)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.RightOperand)).Return(true);
        }
        // type-testing-expression
        // 1 is int,
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as TypeTestingExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.LeftOperand)).Return(true);
          Expect.Call(visitorMock.Visit(exp.RightOperand)).Return(true);
          Expect.Call(visitorMock.Visit(exp.RightOperand.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.RightOperand.TypeName.TypeTags[0])).Return(true);
        }
        // conditional-expression
        // true ? 1 : 2,
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as ConditionalExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.Condition)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.TrueExpression)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.FalseExpression)).Return(true);
        }
        // assignment (wrapped in parens, otherwise assignment is not permitted here)
        // (p = 1),
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var paren = elemInit.NonAssignmentExpression as ParenthesizedExpressionNode;
          Expect.Call(visitorMock.Visit(paren)).Return(true);
          var exp = paren.Expression as AssignmentExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)exp.LeftOperand)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)exp.RightOperand)).Return(true);
        }
        //query-expression
        //from int i in myList
        //from int j in myList
        //let k = j
        //where true
        //join l in myList on i equals l
        //join n in myList on i equals n into o
        //orderby k ascending , j descending
        //select i
        //into m
        //  group m by 0
        {
          var elemInit = collInit.ElementInitializers[i++];
          Expect.Call(visitorMock.Visit(elemInit)).Return(true);
          var exp = elemInit.NonAssignmentExpression as QueryExpressionNode;
          Expect.Call(visitorMock.Visit(exp)).Return(true);
          Expect.Call(visitorMock.Visit(exp.FromClause)).Return(true);
          Expect.Call(visitorMock.Visit(exp.FromClause.Type)).Return(true);
          Expect.Call(visitorMock.Visit(exp.FromClause.Type.TypeName)).Return(true);
          Expect.Call(visitorMock.Visit(exp.FromClause.Type.TypeName.TypeTags[0])).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode) exp.FromClause.Expression)).Return(true);
          Expect.Call(visitorMock.Visit(exp.QueryBody)).Return(true);
          int j = 0;
          {
            var bodyClause = exp.QueryBody.BodyClauses[j++] as FromClauseNode;
            Expect.Call(visitorMock.Visit(bodyClause)).Return(true);
            Expect.Call(visitorMock.Visit(bodyClause.Type)).Return(true);
            Expect.Call(visitorMock.Visit(bodyClause.Type.TypeName)).Return(true);
            Expect.Call(visitorMock.Visit(bodyClause.Type.TypeName.TypeTags[0])).Return(true);
            Expect.Call(visitorMock.Visit((SimpleNameNode)bodyClause.Expression)).Return(true);
          }
          {
            var bodyClause = exp.QueryBody.BodyClauses[j++] as LetClauseNode;
            Expect.Call(visitorMock.Visit(bodyClause)).Return(true);
            Expect.Call(visitorMock.Visit((SimpleNameNode)bodyClause.Expression)).Return(true);
          }
          {
            var bodyClause = exp.QueryBody.BodyClauses[j++] as WhereClauseNode;
            Expect.Call(visitorMock.Visit(bodyClause)).Return(true);
            Expect.Call(visitorMock.Visit((LiteralNode)bodyClause.Expression)).Return(true);
          }
          {
            var bodyClause = exp.QueryBody.BodyClauses[j++] as JoinClauseNode;
            Expect.Call(visitorMock.Visit(bodyClause)).Return(true);
            Expect.Call(visitorMock.Visit(bodyClause.Type)).Return(true);
            Expect.Call(visitorMock.Visit((SimpleNameNode)bodyClause.InExpression)).Return(true);
            Expect.Call(visitorMock.Visit((SimpleNameNode)bodyClause.OnExpression)).Return(true);
            Expect.Call(visitorMock.Visit((SimpleNameNode)bodyClause.EqualsExpression)).Return(true);
          }
          {
            var bodyClause = exp.QueryBody.BodyClauses[j++] as JoinIntoClauseNode;
            Expect.Call(visitorMock.Visit(bodyClause)).Return(true);
            Expect.Call(visitorMock.Visit(bodyClause.Type)).Return(true);
            Expect.Call(visitorMock.Visit((SimpleNameNode)bodyClause.InExpression)).Return(true);
            Expect.Call(visitorMock.Visit((SimpleNameNode)bodyClause.OnExpression)).Return(true);
            Expect.Call(visitorMock.Visit((SimpleNameNode)bodyClause.EqualsExpression)).Return(true);
          }
          {
            var bodyClause = exp.QueryBody.BodyClauses[j++] as OrderByClauseNode;
            Expect.Call(visitorMock.Visit(bodyClause)).Return(true);
            Expect.Call(visitorMock.Visit(bodyClause.Orderings[0])).Return(true);
            Expect.Call(visitorMock.Visit((SimpleNameNode)bodyClause.Orderings[0].Expression)).Return(true);
            Expect.Call(visitorMock.Visit(bodyClause.Orderings[1])).Return(true);
            Expect.Call(visitorMock.Visit((SimpleNameNode)bodyClause.Orderings[1].Expression)).Return(true);
          }
          Expect.Call(visitorMock.Visit(exp.QueryBody.SelectClause)).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)exp.QueryBody.SelectClause.Expression)).Return(true);
          var cont = exp.QueryBody.QueryContinuation;
          Expect.Call(visitorMock.Visit(cont)).Return(true);
          Expect.Call(visitorMock.Visit(cont.QueryBody)).Return(true);
          Expect.Call(visitorMock.Visit(cont.QueryBody.GroupClause)).Return(true);
          Expect.Call(visitorMock.Visit((SimpleNameNode)cont.QueryBody.GroupClause.GroupExpression)).Return(true);
          Expect.Call(visitorMock.Visit((LiteralNode)cont.QueryBody.GroupClause.ByExpression)).Return(true);
        }

      }
      mocks.ReplayAll();

      // Act
      collInit.AcceptVisitor(visitorMock);

      // Assert
      mocks.VerifyAll();
    }

  }
}
