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
        visitorMock.Visit(compilationUnitNode);
        visitorMock.Visit(compilationUnitNode.ExternAliasNodes[0]);
        visitorMock.Visit(compilationUnitNode.UsingNodes[0]);
        visitorMock.Visit(compilationUnitNode.UsingNodes[0].TypeName);
        visitorMock.Visit(compilationUnitNode.UsingNodes[0].TypeName.TypeTags[0]);
        visitorMock.Visit((UsingAliasNode)compilationUnitNode.UsingNodes[1]);
        visitorMock.Visit(compilationUnitNode.UsingNodes[1].TypeName);
        visitorMock.Visit(compilationUnitNode.UsingNodes[1].TypeName.TypeTags[0]);
        visitorMock.Visit(compilationUnitNode.GlobalAttributes[0]);
        visitorMock.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0]);
        visitorMock.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0].TypeName);
        visitorMock.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0].Arguments[0]);
        visitorMock.Visit((StringLiteralNode)compilationUnitNode.GlobalAttributes[0].Attributes[0].Arguments[0].Expression);
        visitorMock.Visit(compilationUnitNode.NamespaceDeclarations[0]);
        visitorMock.Visit(compilationUnitNode.NamespaceDeclarations[0].NamespaceDeclarations[0]);
        visitorMock.Visit((ClassDeclarationNode)compilationUnitNode.NamespaceDeclarations[0].NamespaceDeclarations[0].TypeDeclarations[0]);
        visitorMock.Visit((ClassDeclarationNode)compilationUnitNode.TypeDeclarations[0]);
        visitorMock.Visit((StructDeclarationNode)compilationUnitNode.TypeDeclarations[1]);
        visitorMock.Visit((InterfaceDeclarationNode)compilationUnitNode.TypeDeclarations[2]);
        visitorMock.Visit((EnumDeclarationNode)compilationUnitNode.TypeDeclarations[3]);
        visitorMock.Visit((DelegateDeclarationNode)compilationUnitNode.TypeDeclarations[4]);
        visitorMock.Visit(((DelegateDeclarationNode)compilationUnitNode.TypeDeclarations[4]).TypeName);
        visitorMock.Visit(((DelegateDeclarationNode)compilationUnitNode.TypeDeclarations[4]).TypeName.TypeTags[0]);
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
      var typeOrNamespaceNode = project.SyntaxTree.CompilationUnitNodes[0].UsingNodes[0].TypeName;

      // Arrange
      var mocks = new MockRepository();
      var visitorMock = mocks.StrictMock<ISyntaxNodeVisitor>();
      using (mocks.Ordered())
      {
        visitorMock.Visit(typeOrNamespaceNode);
        visitorMock.Visit(typeOrNamespaceNode.TypeTags[0]); // System
        visitorMock.Visit(typeOrNamespaceNode.TypeTags[1]); // Collections
        visitorMock.Visit(typeOrNamespaceNode.TypeTags[2]); // Generic
        visitorMock.Visit(typeOrNamespaceNode.TypeTags[3]); // IDictionary
        visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0]); // System.Nullable<int>**[][,]
        visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeTags[0]); // System
        visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeTags[1]); // Nullable
        visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeTags[1].Arguments[0]); // int
        visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeTags[1].Arguments[0].TypeTags[0]); // int
        visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].RankSpecifiers[0]);     // []
        visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].RankSpecifiers[1]);     // [,]
        visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[1]); // string**[][,]
        visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[1].TypeTags[0]); // string
        visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[1].RankSpecifiers[0]);     // []
        visitorMock.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[1].RankSpecifiers[1]);     // [,]
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
        visitorMock.Visit(classDeclarationNode);

        // Class attributes
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[0]);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[1]);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[1].TypeName);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[1].TypeName.TypeTags[0]);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[1].Arguments[0]);
        visitorMock.Visit((FalseLiteralNode)classDeclarationNode.AttributeDecorations[0].Attributes[1].Arguments[0].Expression);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[1]);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0]);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].TypeName);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[0]);
        visitorMock.Visit((StringLiteralNode)classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[0].Expression);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[1]);
        visitorMock.Visit((StringLiteralNode)classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[1].Expression);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[2]);
        visitorMock.Visit((StringLiteralNode)classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[2].Expression);

        // Type params and base types
        visitorMock.Visit(classDeclarationNode.TypeParameters[0]);
        visitorMock.Visit(classDeclarationNode.TypeParameters[0].AttributeDecorations[0]);
        visitorMock.Visit(classDeclarationNode.TypeParameters[0].AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(classDeclarationNode.TypeParameters[0].AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(classDeclarationNode.TypeParameters[0].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit(classDeclarationNode.TypeParameters[1]);
        visitorMock.Visit(classDeclarationNode.BaseTypes[0]);
        visitorMock.Visit(classDeclarationNode.BaseTypes[0].TypeTags[0]);
        visitorMock.Visit(classDeclarationNode.BaseTypes[1]);
        visitorMock.Visit(classDeclarationNode.BaseTypes[1].TypeTags[0]);
        visitorMock.Visit(classDeclarationNode.TypeParameterConstraints[0]);
        visitorMock.Visit(classDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0]);
        visitorMock.Visit(classDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0].TypeName);
        visitorMock.Visit(classDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0].TypeName.TypeTags[0]);
        visitorMock.Visit(classDeclarationNode.TypeParameterConstraints[1]);
        visitorMock.Visit(classDeclarationNode.TypeParameterConstraints[1].ConstraintTags[0]);
        visitorMock.Visit(classDeclarationNode.TypeParameterConstraints[1].ConstraintTags[1]);

        // Constant declaration
        {
          var constDeclarationNode = (ConstDeclarationNode)classDeclarationNode.MemberDeclarations[0];
          visitorMock.Visit(constDeclarationNode);
          visitorMock.Visit(constDeclarationNode.AttributeDecorations[0]);
          visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0]);
          visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
          visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
          visitorMock.Visit(constDeclarationNode.TypeName);
          visitorMock.Visit(constDeclarationNode.TypeName.TypeTags[0]);
          visitorMock.Visit(constDeclarationNode.ConstTags[0]);
          visitorMock.Visit((Int32LiteralNode)constDeclarationNode.ConstTags[0].Expression);
          visitorMock.Visit(constDeclarationNode.ConstTags[1]);
          visitorMock.Visit((Int32LiteralNode)constDeclarationNode.ConstTags[1].Expression);
        }
        // Field declaration
        {
          var fieldDeclarationNode = (FieldDeclarationNode)classDeclarationNode.MemberDeclarations[1];
          visitorMock.Visit(fieldDeclarationNode);
          visitorMock.Visit(fieldDeclarationNode.AttributeDecorations[0]);
          visitorMock.Visit(fieldDeclarationNode.AttributeDecorations[0].Attributes[0]);
          visitorMock.Visit(fieldDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
          visitorMock.Visit(fieldDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
          visitorMock.Visit(fieldDeclarationNode.TypeName);
          visitorMock.Visit(fieldDeclarationNode.TypeName.TypeTags[0]);
          visitorMock.Visit(fieldDeclarationNode.FieldTags[0]);
          visitorMock.Visit(fieldDeclarationNode.FieldTags[1]);
          var init = fieldDeclarationNode.FieldTags[1].Initializer as ExpressionInitializerNode;
          visitorMock.Visit(init);
          visitorMock.Visit((Int32LiteralNode)init.Expression);
        }
        // Method declaration
        {
          var methodDeclarationNode = (MethodDeclarationNode)classDeclarationNode.MemberDeclarations[2];
          visitorMock.Visit(methodDeclarationNode);
          visitorMock.Visit(methodDeclarationNode.AttributeDecorations[0]);
          visitorMock.Visit(methodDeclarationNode.AttributeDecorations[0].Attributes[0]);
          visitorMock.Visit(methodDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
          visitorMock.Visit(methodDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
          visitorMock.Visit(methodDeclarationNode.TypeName);
          visitorMock.Visit(methodDeclarationNode.TypeName.TypeTags[0]);
          visitorMock.Visit(methodDeclarationNode.TypeParameters[0]);
          visitorMock.Visit(methodDeclarationNode.TypeParameters[1]);
          visitorMock.Visit(methodDeclarationNode.TypeParameterConstraints[0]);
          visitorMock.Visit(methodDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0]);
          visitorMock.Visit(methodDeclarationNode.TypeParameterConstraints[1]);
          visitorMock.Visit(methodDeclarationNode.TypeParameterConstraints[1].ConstraintTags[0]);
          visitorMock.Visit(methodDeclarationNode.FormalParameters[0]);
          visitorMock.Visit(methodDeclarationNode.FormalParameters[0].TypeName);
          visitorMock.Visit(methodDeclarationNode.FormalParameters[0].TypeName.TypeTags[0]);
          visitorMock.Visit(methodDeclarationNode.FormalParameters[1]);
          visitorMock.Visit(methodDeclarationNode.FormalParameters[1].TypeName);
          visitorMock.Visit(methodDeclarationNode.FormalParameters[1].TypeName.TypeTags[0]);
          visitorMock.Visit(methodDeclarationNode.FormalParameters[2]);
          visitorMock.Visit(methodDeclarationNode.FormalParameters[2].AttributeDecorations[0]);
          visitorMock.Visit(methodDeclarationNode.FormalParameters[2].AttributeDecorations[0].Attributes[0]);
          visitorMock.Visit(methodDeclarationNode.FormalParameters[2].AttributeDecorations[0].Attributes[0].TypeName);
          visitorMock.Visit(methodDeclarationNode.FormalParameters[2].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
          visitorMock.Visit(methodDeclarationNode.FormalParameters[2].TypeName);
          visitorMock.Visit(methodDeclarationNode.FormalParameters[2].TypeName.TypeTags[0]);
          visitorMock.Visit(methodDeclarationNode.FormalParameters[2].TypeName.RankSpecifiers[0]);
          visitorMock.Visit(methodDeclarationNode.Body);
          var expressionStatement = methodDeclarationNode.Body.Statements[0] as ExpressionStatementNode;
          visitorMock.Visit(expressionStatement);
          var exp = expressionStatement.Expression as AssignmentExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((SimpleNameNode)exp.LeftOperand);
          var defaultExp = exp.RightOperand as DefaultValueExpressionNode;
          visitorMock.Visit(defaultExp);
          visitorMock.Visit(defaultExp.TypeName);
          visitorMock.Visit(defaultExp.TypeName.TypeTags[0]);
        }
        // Method declaration (explicit interface implementation)
        {
          var methodDeclarationNode2 = (MethodDeclarationNode)classDeclarationNode.MemberDeclarations[3];
          visitorMock.Visit(methodDeclarationNode2);
          visitorMock.Visit(methodDeclarationNode2.TypeName);
          visitorMock.Visit(methodDeclarationNode2.TypeName.TypeTags[0]);
          visitorMock.Visit(methodDeclarationNode2.Body);
        }
        // Property declaration
        {
          var propertyDeclarationNode = (PropertyDeclarationNode)classDeclarationNode.MemberDeclarations[4];
          visitorMock.Visit(propertyDeclarationNode);
          visitorMock.Visit(propertyDeclarationNode.AttributeDecorations[0]);
          visitorMock.Visit(propertyDeclarationNode.AttributeDecorations[0].Attributes[0]);
          visitorMock.Visit(propertyDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
          visitorMock.Visit(propertyDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
          visitorMock.Visit(propertyDeclarationNode.TypeName);
          visitorMock.Visit(propertyDeclarationNode.TypeName.TypeTags[0]);
          visitorMock.Visit(propertyDeclarationNode.GetAccessor);
          visitorMock.Visit(propertyDeclarationNode.SetAccessor);
        }
        // Event declaration (field-like)
        {
          var eventDeclarationNode = (FieldDeclarationNode)classDeclarationNode.MemberDeclarations[5];
          visitorMock.Visit(eventDeclarationNode);
          visitorMock.Visit(eventDeclarationNode.AttributeDecorations[0]);
          visitorMock.Visit(eventDeclarationNode.AttributeDecorations[0].Attributes[0]);
          visitorMock.Visit(eventDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
          visitorMock.Visit(eventDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
          visitorMock.Visit(eventDeclarationNode.TypeName);
          visitorMock.Visit(eventDeclarationNode.TypeName.TypeTags[0]);
          visitorMock.Visit(eventDeclarationNode.TypeName.TypeTags[0].Arguments[0]);
          visitorMock.Visit(eventDeclarationNode.TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
          visitorMock.Visit(eventDeclarationNode.FieldTags[0]);
          var init = eventDeclarationNode.FieldTags[0].Initializer as ExpressionInitializerNode;
          visitorMock.Visit(init);
          visitorMock.Visit((NullLiteralNode)init.Expression);
        }
        // Event declaration (property-like)
        {
          var eventDeclarationNode2 = (EventPropertyDeclarationNode)classDeclarationNode.MemberDeclarations[6];
          visitorMock.Visit(eventDeclarationNode2);
          visitorMock.Visit(eventDeclarationNode2.AttributeDecorations[0]);
          visitorMock.Visit(eventDeclarationNode2.AttributeDecorations[0].Attributes[0]);
          visitorMock.Visit(eventDeclarationNode2.AttributeDecorations[0].Attributes[0].TypeName);
          visitorMock.Visit(eventDeclarationNode2.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
          visitorMock.Visit(eventDeclarationNode2.TypeName);
          visitorMock.Visit(eventDeclarationNode2.TypeName.TypeTags[0]);
          visitorMock.Visit(eventDeclarationNode2.TypeName.TypeTags[0].Arguments[0]);
          visitorMock.Visit(eventDeclarationNode2.TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
          visitorMock.Visit(eventDeclarationNode2.AddAccessor);
          visitorMock.Visit(eventDeclarationNode2.AddAccessor.Body);
          visitorMock.Visit(eventDeclarationNode2.RemoveAccessor);
          visitorMock.Visit(eventDeclarationNode2.RemoveAccessor.Body);
        }
        // Indexer declaration
        {
          var indexerDeclarationNode = (IndexerDeclarationNode)classDeclarationNode.MemberDeclarations[7];
          visitorMock.Visit(indexerDeclarationNode);
          visitorMock.Visit(indexerDeclarationNode.AttributeDecorations[0]);
          visitorMock.Visit(indexerDeclarationNode.AttributeDecorations[0].Attributes[0]);
          visitorMock.Visit(indexerDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
          visitorMock.Visit(indexerDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
          visitorMock.Visit(indexerDeclarationNode.TypeName);
          visitorMock.Visit(indexerDeclarationNode.TypeName.TypeTags[0]);
          visitorMock.Visit(indexerDeclarationNode.FormalParameters[0]);
          visitorMock.Visit(indexerDeclarationNode.FormalParameters[0].TypeName);
          visitorMock.Visit(indexerDeclarationNode.FormalParameters[0].TypeName.TypeTags[0]);
          visitorMock.Visit(indexerDeclarationNode.FormalParameters[1]);
          visitorMock.Visit(indexerDeclarationNode.FormalParameters[1].TypeName);
          visitorMock.Visit(indexerDeclarationNode.FormalParameters[1].TypeName.TypeTags[0]);
          visitorMock.Visit(indexerDeclarationNode.GetAccessor);
          visitorMock.Visit(indexerDeclarationNode.GetAccessor.Body);
          visitorMock.Visit((ReturnStatementNode)indexerDeclarationNode.GetAccessor.Body.Statements[0]);
          visitorMock.Visit((Int32LiteralNode)((ReturnStatementNode)indexerDeclarationNode.GetAccessor.Body.Statements[0]).Expression);
          visitorMock.Visit(indexerDeclarationNode.SetAccessor);
          visitorMock.Visit(indexerDeclarationNode.SetAccessor.Body);
        }
        // Operator declaration
        {
          var operatorDeclarationNode = (OperatorDeclarationNode)classDeclarationNode.MemberDeclarations[8];
          visitorMock.Visit(operatorDeclarationNode);
          visitorMock.Visit(operatorDeclarationNode.AttributeDecorations[0]);
          visitorMock.Visit(operatorDeclarationNode.AttributeDecorations[0].Attributes[0]);
          visitorMock.Visit(operatorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
          visitorMock.Visit(operatorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
          visitorMock.Visit(operatorDeclarationNode.TypeName);
          visitorMock.Visit(operatorDeclarationNode.TypeName.TypeTags[0]);
          visitorMock.Visit(operatorDeclarationNode.TypeName.TypeTags[0].Arguments[0]);
          visitorMock.Visit(operatorDeclarationNode.TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
          visitorMock.Visit(operatorDeclarationNode.TypeName.TypeTags[0].Arguments[1]);
          visitorMock.Visit(operatorDeclarationNode.TypeName.TypeTags[0].Arguments[1].TypeTags[0]);
          visitorMock.Visit(operatorDeclarationNode.FormalParameters[0]);
          visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].TypeName);
          visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0]);
          visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0].Arguments[0]);
          visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
          visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0].Arguments[1]);
          visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0].Arguments[1].TypeTags[0]);
          visitorMock.Visit(operatorDeclarationNode.Body);
          visitorMock.Visit((ReturnStatementNode)operatorDeclarationNode.Body.Statements[0]);
          visitorMock.Visit((NullLiteralNode)((ReturnStatementNode)operatorDeclarationNode.Body.Statements[0]).Expression);
        }
        // Conversion operator declaration
        {
          var castOperatorDeclarationNode = (CastOperatorDeclarationNode)classDeclarationNode.MemberDeclarations[9];
          visitorMock.Visit(castOperatorDeclarationNode);
          visitorMock.Visit(castOperatorDeclarationNode.AttributeDecorations[0]);
          visitorMock.Visit(castOperatorDeclarationNode.AttributeDecorations[0].Attributes[0]);
          visitorMock.Visit(castOperatorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
          visitorMock.Visit(castOperatorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
          visitorMock.Visit(castOperatorDeclarationNode.TypeName);
          visitorMock.Visit(castOperatorDeclarationNode.TypeName.TypeTags[0]);
          visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0]);
          visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].TypeName);
          visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0]);
          visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0].Arguments[0]);
          visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
          visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0].Arguments[1]);
          visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0].Arguments[1].TypeTags[0]);
          visitorMock.Visit(castOperatorDeclarationNode.Body);
          visitorMock.Visit((ReturnStatementNode)castOperatorDeclarationNode.Body.Statements[0]);
          visitorMock.Visit((NullLiteralNode)((ReturnStatementNode)castOperatorDeclarationNode.Body.Statements[0]).Expression);
        }
        // Constructor declaration 
        {
          var constructorDeclarationNode = (ConstructorDeclarationNode)classDeclarationNode.MemberDeclarations[10];
          visitorMock.Visit(constructorDeclarationNode);
          visitorMock.Visit(constructorDeclarationNode.AttributeDecorations[0]);
          visitorMock.Visit(constructorDeclarationNode.AttributeDecorations[0].Attributes[0]);
          visitorMock.Visit(constructorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
          visitorMock.Visit(constructorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
          visitorMock.Visit(constructorDeclarationNode.FormalParameters[0]);
          visitorMock.Visit(constructorDeclarationNode.FormalParameters[0].TypeName);
          visitorMock.Visit(constructorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0]);
          visitorMock.Visit(constructorDeclarationNode.FormalParameters[1]);
          visitorMock.Visit(constructorDeclarationNode.FormalParameters[1].TypeName);
          visitorMock.Visit(constructorDeclarationNode.FormalParameters[1].TypeName.TypeTags[0]);
          visitorMock.Visit(constructorDeclarationNode.Body);
        }
        // Constructor declaration with this initializer
        {
          var constructorDeclarationNode2 = (ConstructorDeclarationNode)classDeclarationNode.MemberDeclarations[11];
          visitorMock.Visit(constructorDeclarationNode2);
          visitorMock.Visit((ThisConstructorInitializerNode)constructorDeclarationNode2.Initializer);
          visitorMock.Visit(constructorDeclarationNode2.Initializer.Arguments[0]);
          visitorMock.Visit((Int32LiteralNode)constructorDeclarationNode2.Initializer.Arguments[0].Expression);
          visitorMock.Visit(constructorDeclarationNode2.Initializer.Arguments[1]);
          visitorMock.Visit((Int32LiteralNode)constructorDeclarationNode2.Initializer.Arguments[1].Expression);
          visitorMock.Visit(constructorDeclarationNode2.Body);
        }
        // Constructor declaration with base initializer
        {
          var constructorDeclarationNode3 = (ConstructorDeclarationNode)classDeclarationNode.MemberDeclarations[12];
          visitorMock.Visit(constructorDeclarationNode3);
          visitorMock.Visit(constructorDeclarationNode3.FormalParameters[0]);
          visitorMock.Visit(constructorDeclarationNode3.FormalParameters[0].TypeName);
          visitorMock.Visit(constructorDeclarationNode3.FormalParameters[0].TypeName.TypeTags[0]);
          visitorMock.Visit((BaseConstructorInitializerNode)constructorDeclarationNode3.Initializer);
          visitorMock.Visit(constructorDeclarationNode3.Body);
        }
        // Destructor declaration
        {
          var destructorDeclarationNode = (DestructorDeclarationNode)classDeclarationNode.MemberDeclarations[13];
          visitorMock.Visit(destructorDeclarationNode);
          visitorMock.Visit(destructorDeclarationNode.AttributeDecorations[0]);
          visitorMock.Visit(destructorDeclarationNode.AttributeDecorations[0].Attributes[0]);
          visitorMock.Visit(destructorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
          visitorMock.Visit(destructorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
          visitorMock.Visit(destructorDeclarationNode.Body);
        }
        // Nested class declaration
        {
          var nestedClassDeclarationNode = (ClassDeclarationNode) classDeclarationNode.NestedTypes[0];
          visitorMock.Visit(nestedClassDeclarationNode);
          visitorMock.Visit(nestedClassDeclarationNode.AttributeDecorations[0]);
          visitorMock.Visit(nestedClassDeclarationNode.AttributeDecorations[0].Attributes[0]);
          visitorMock.Visit(nestedClassDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
          visitorMock.Visit(nestedClassDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
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
        visitorMock.Visit(structDeclarationNode);

        // Struct attributes
        visitorMock.Visit(structDeclarationNode.AttributeDecorations[0]);
        visitorMock.Visit(structDeclarationNode.AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(structDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(structDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);

        // Type params and base types
        visitorMock.Visit(structDeclarationNode.TypeParameters[0]);
        visitorMock.Visit(structDeclarationNode.TypeParameters[1]);
        visitorMock.Visit(structDeclarationNode.BaseTypes[0]);
        visitorMock.Visit(structDeclarationNode.BaseTypes[0].TypeTags[0]);
        visitorMock.Visit(structDeclarationNode.TypeParameterConstraints[0]);
        visitorMock.Visit(structDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0]);
        visitorMock.Visit(structDeclarationNode.TypeParameterConstraints[1]);
        visitorMock.Visit(structDeclarationNode.TypeParameterConstraints[1].ConstraintTags[0]);

        // Constant declaration
        {
          var constDeclarationNode = (ConstDeclarationNode)structDeclarationNode.MemberDeclarations[0];
          visitorMock.Visit(constDeclarationNode);
          visitorMock.Visit(constDeclarationNode.AttributeDecorations[0]);
          visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0]);
          visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
          visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
          visitorMock.Visit(constDeclarationNode.TypeName);
          visitorMock.Visit(constDeclarationNode.TypeName.TypeTags[0]);
          visitorMock.Visit(constDeclarationNode.ConstTags[0]);
          visitorMock.Visit((Int32LiteralNode)constDeclarationNode.ConstTags[0].Expression);
        }
        // Field declaration
        {
          var fieldDeclarationNode = (FieldDeclarationNode)structDeclarationNode.MemberDeclarations[1];
          visitorMock.Visit(fieldDeclarationNode);
          visitorMock.Visit(fieldDeclarationNode.TypeName);
          visitorMock.Visit(fieldDeclarationNode.TypeName.TypeTags[0]);
          visitorMock.Visit(fieldDeclarationNode.FieldTags[0]);
        }
        // Method declaration
        {
          var methodDeclarationNode = (MethodDeclarationNode)structDeclarationNode.MemberDeclarations[2];
          visitorMock.Visit(methodDeclarationNode);
          visitorMock.Visit(methodDeclarationNode.TypeName);
          visitorMock.Visit(methodDeclarationNode.TypeName.TypeTags[0]);
          visitorMock.Visit(methodDeclarationNode.Body);
        }
        // Property declaration
        {
          var propertyDeclarationNode = (PropertyDeclarationNode)structDeclarationNode.MemberDeclarations[3];
          visitorMock.Visit(propertyDeclarationNode);
          visitorMock.Visit(propertyDeclarationNode.TypeName);
          visitorMock.Visit(propertyDeclarationNode.TypeName.TypeTags[0]);
          visitorMock.Visit(propertyDeclarationNode.GetAccessor);
          visitorMock.Visit(propertyDeclarationNode.GetAccessor.Body);
          visitorMock.Visit((ReturnStatementNode)propertyDeclarationNode.GetAccessor.Body.Statements[0]);
          visitorMock.Visit((Int32LiteralNode)((ReturnStatementNode)propertyDeclarationNode.GetAccessor.Body.Statements[0]).Expression);
          visitorMock.Visit(propertyDeclarationNode.SetAccessor);
          visitorMock.Visit(propertyDeclarationNode.SetAccessor.Body);
        }
        // Event declaration (property-like)
        {
          var eventPropertyDeclarationNode = (EventPropertyDeclarationNode)structDeclarationNode.MemberDeclarations[4];
          visitorMock.Visit(eventPropertyDeclarationNode);
          visitorMock.Visit(eventPropertyDeclarationNode.TypeName);
          visitorMock.Visit(eventPropertyDeclarationNode.TypeName.TypeTags[0]);
          visitorMock.Visit(eventPropertyDeclarationNode.TypeName.TypeTags[0].Arguments[0]);
          visitorMock.Visit(eventPropertyDeclarationNode.TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
          visitorMock.Visit(eventPropertyDeclarationNode.AddAccessor);
          visitorMock.Visit(eventPropertyDeclarationNode.AddAccessor.Body);
          visitorMock.Visit(eventPropertyDeclarationNode.RemoveAccessor);
          visitorMock.Visit(eventPropertyDeclarationNode.RemoveAccessor.Body);
        }
        // Indexer declaration
        {
          var indexerDeclarationNode = (IndexerDeclarationNode)structDeclarationNode.MemberDeclarations[5];
          visitorMock.Visit(indexerDeclarationNode);
          visitorMock.Visit(indexerDeclarationNode.TypeName);
          visitorMock.Visit(indexerDeclarationNode.TypeName.TypeTags[0]);
          visitorMock.Visit(indexerDeclarationNode.FormalParameters[0]);
          visitorMock.Visit(indexerDeclarationNode.FormalParameters[0].TypeName);
          visitorMock.Visit(indexerDeclarationNode.FormalParameters[0].TypeName.TypeTags[0]);
          visitorMock.Visit(indexerDeclarationNode.GetAccessor);
          visitorMock.Visit(indexerDeclarationNode.GetAccessor.Body);
          visitorMock.Visit((ReturnStatementNode)indexerDeclarationNode.GetAccessor.Body.Statements[0]);
          visitorMock.Visit((Int32LiteralNode)((ReturnStatementNode)indexerDeclarationNode.GetAccessor.Body.Statements[0]).Expression);
        }
        // Operator declaration
        {
          var operatorDeclarationNode = (OperatorDeclarationNode)structDeclarationNode.MemberDeclarations[6];
          visitorMock.Visit(operatorDeclarationNode);
          visitorMock.Visit(operatorDeclarationNode.TypeName);
          visitorMock.Visit(operatorDeclarationNode.TypeName.TypeTags[0]);
          visitorMock.Visit(operatorDeclarationNode.TypeName.TypeTags[0].Arguments[0]);
          visitorMock.Visit(operatorDeclarationNode.TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
          visitorMock.Visit(operatorDeclarationNode.TypeName.TypeTags[0].Arguments[1]);
          visitorMock.Visit(operatorDeclarationNode.TypeName.TypeTags[0].Arguments[1].TypeTags[0]);
          visitorMock.Visit(operatorDeclarationNode.FormalParameters[0]);
          visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].TypeName);
          visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0]);
          visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0].Arguments[0]);
          visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
          visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0].Arguments[1]);
          visitorMock.Visit(operatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0].Arguments[1].TypeTags[0]);
          visitorMock.Visit(operatorDeclarationNode.Body);
          visitorMock.Visit((ReturnStatementNode)operatorDeclarationNode.Body.Statements[0]);
          var exp = ((ReturnStatementNode) operatorDeclarationNode.Body.Statements[0]).Expression as
            ObjectCreationExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit(exp.TypeName);
          visitorMock.Visit(exp.TypeName.TypeTags[0]);
          visitorMock.Visit(exp.TypeName.TypeTags[0].Arguments[0]);
          visitorMock.Visit(exp.TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
          visitorMock.Visit(exp.TypeName.TypeTags[0].Arguments[1]);
          visitorMock.Visit(exp.TypeName.TypeTags[0].Arguments[1].TypeTags[0]);
        }
        // Conversion operator declaration
        {
          var castOperatorDeclarationNode = (CastOperatorDeclarationNode)structDeclarationNode.MemberDeclarations[7];
          visitorMock.Visit(castOperatorDeclarationNode);
          visitorMock.Visit(castOperatorDeclarationNode.TypeName);
          visitorMock.Visit(castOperatorDeclarationNode.TypeName.TypeTags[0]);
          visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0]);
          visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].TypeName);
          visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0]);
          visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0].Arguments[0]);
          visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
          visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0].Arguments[1]);
          visitorMock.Visit(castOperatorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0].Arguments[1].TypeTags[0]);
          visitorMock.Visit(castOperatorDeclarationNode.Body);
          visitorMock.Visit((ReturnStatementNode)castOperatorDeclarationNode.Body.Statements[0]);
          visitorMock.Visit((NullLiteralNode)((ReturnStatementNode)castOperatorDeclarationNode.Body.Statements[0]).Expression);
        }
        // Constructor declaration 
        {
          var constructorDeclarationNode = (ConstructorDeclarationNode) structDeclarationNode.MemberDeclarations[8];
          visitorMock.Visit(constructorDeclarationNode);
          visitorMock.Visit(constructorDeclarationNode.FormalParameters[0]);
          visitorMock.Visit(constructorDeclarationNode.FormalParameters[0].TypeName);
          visitorMock.Visit(constructorDeclarationNode.FormalParameters[0].TypeName.TypeTags[0]);
          visitorMock.Visit(constructorDeclarationNode.Body);
          var expressionStatement = constructorDeclarationNode.Body.Statements[0] as ExpressionStatementNode;
          visitorMock.Visit(expressionStatement);
          var assignment = expressionStatement.Expression as AssignmentExpressionNode;
          visitorMock.Visit(assignment);
          visitorMock.Visit((SimpleNameNode) assignment.LeftOperand);
          visitorMock.Visit((Int32LiteralNode) assignment.RightOperand);
        }
        // Nested class declaration
        visitorMock.Visit((ClassDeclarationNode)structDeclarationNode.NestedTypes[0]);
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
        visitorMock.Visit(interfaceDeclarationNode);

        // Interface attributes
        visitorMock.Visit(interfaceDeclarationNode.AttributeDecorations[0]);
        visitorMock.Visit(interfaceDeclarationNode.AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(interfaceDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(interfaceDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);

        // Type params and base types
        visitorMock.Visit(interfaceDeclarationNode.TypeParameters[0]);
        visitorMock.Visit(interfaceDeclarationNode.TypeParameters[1]);
        visitorMock.Visit(interfaceDeclarationNode.BaseTypes[0]);
        visitorMock.Visit(interfaceDeclarationNode.BaseTypes[0].TypeTags[0]);
        visitorMock.Visit(interfaceDeclarationNode.TypeParameterConstraints[0]);
        visitorMock.Visit(interfaceDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0]);
        visitorMock.Visit(interfaceDeclarationNode.TypeParameterConstraints[1]);
        visitorMock.Visit(interfaceDeclarationNode.TypeParameterConstraints[1].ConstraintTags[0]);

        // Method declaration
        var methodDeclarationNode = (MethodDeclarationNode)interfaceDeclarationNode.MemberDeclarations[0];
        visitorMock.Visit(methodDeclarationNode);
        visitorMock.Visit(methodDeclarationNode.AttributeDecorations[0]);
        visitorMock.Visit(methodDeclarationNode.AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(methodDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(methodDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit(methodDeclarationNode.TypeName);
        visitorMock.Visit(methodDeclarationNode.TypeName.TypeTags[0]);

        // Property declaration
        var propertyDeclarationNode = (PropertyDeclarationNode)interfaceDeclarationNode.MemberDeclarations[1];
        visitorMock.Visit(propertyDeclarationNode);
        visitorMock.Visit(propertyDeclarationNode.TypeName);
        visitorMock.Visit(propertyDeclarationNode.TypeName.TypeTags[0]);
        visitorMock.Visit(propertyDeclarationNode.GetAccessor);
        visitorMock.Visit(propertyDeclarationNode.SetAccessor);

        // Event declaration
        var interfaceEventDeclarationNode = (InterfaceEventDeclarationNode)interfaceDeclarationNode.MemberDeclarations[2];
        visitorMock.Visit(interfaceEventDeclarationNode);
        visitorMock.Visit(interfaceEventDeclarationNode.TypeName);
        visitorMock.Visit(interfaceEventDeclarationNode.TypeName.TypeTags[0]);
        visitorMock.Visit(interfaceEventDeclarationNode.TypeName.TypeTags[0].Arguments[0]);
        visitorMock.Visit(interfaceEventDeclarationNode.TypeName.TypeTags[0].Arguments[0].TypeTags[0]);

        // Indexer declaration
        var indexerDeclarationNode = (IndexerDeclarationNode)interfaceDeclarationNode.MemberDeclarations[3];
        visitorMock.Visit(indexerDeclarationNode);
        visitorMock.Visit(indexerDeclarationNode.TypeName);
        visitorMock.Visit(indexerDeclarationNode.TypeName.TypeTags[0]);
        visitorMock.Visit(indexerDeclarationNode.FormalParameters[0]);
        visitorMock.Visit(indexerDeclarationNode.FormalParameters[0].TypeName);
        visitorMock.Visit(indexerDeclarationNode.FormalParameters[0].TypeName.TypeTags[0]);
        visitorMock.Visit(indexerDeclarationNode.GetAccessor);
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
        visitorMock.Visit(enumDeclarationNode);

        // Enum attributes
        visitorMock.Visit(enumDeclarationNode.AttributeDecorations[0]);
        visitorMock.Visit(enumDeclarationNode.AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(enumDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(enumDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);

        // Base type
        visitorMock.Visit(enumDeclarationNode.EnumBase);
        visitorMock.Visit(enumDeclarationNode.EnumBase.TypeTags[0]);

        // Enum member declaration
        visitorMock.Visit(enumDeclarationNode.Values[0]);
        visitorMock.Visit(enumDeclarationNode.Values[0].AttributeDecorations[0]);
        visitorMock.Visit(enumDeclarationNode.Values[0].AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(enumDeclarationNode.Values[0].AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(enumDeclarationNode.Values[0].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit((Int32LiteralNode)enumDeclarationNode.Values[0].Expression);
        visitorMock.Visit(enumDeclarationNode.Values[1]);
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
        visitorMock.Visit(delegateDeclarationNode);

        // Delegate attributes
        visitorMock.Visit(delegateDeclarationNode.AttributeDecorations[0]);
        visitorMock.Visit(delegateDeclarationNode.AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(delegateDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(delegateDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);

        // Return type, type parameters and constraints
        visitorMock.Visit(delegateDeclarationNode.TypeName);
        visitorMock.Visit(delegateDeclarationNode.TypeName.TypeTags[0]);
        visitorMock.Visit(delegateDeclarationNode.TypeParameters[0]);
        visitorMock.Visit(delegateDeclarationNode.TypeParameters[1]);
        visitorMock.Visit(delegateDeclarationNode.TypeParameterConstraints[0]);
        visitorMock.Visit(delegateDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0]);
        visitorMock.Visit(delegateDeclarationNode.TypeParameterConstraints[1]);
        visitorMock.Visit(delegateDeclarationNode.TypeParameterConstraints[1].ConstraintTags[0]);

        // Formal parameters
        visitorMock.Visit(delegateDeclarationNode.FormalParameters[0]);
        visitorMock.Visit(delegateDeclarationNode.FormalParameters[0].AttributeDecorations[0]);
        visitorMock.Visit(delegateDeclarationNode.FormalParameters[0].AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(delegateDeclarationNode.FormalParameters[0].AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(delegateDeclarationNode.FormalParameters[0].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit(delegateDeclarationNode.FormalParameters[0].TypeName);
        visitorMock.Visit(delegateDeclarationNode.FormalParameters[0].TypeName.TypeTags[0]);
        visitorMock.Visit(delegateDeclarationNode.FormalParameters[1]);
        visitorMock.Visit(delegateDeclarationNode.FormalParameters[1].TypeName);
        visitorMock.Visit(delegateDeclarationNode.FormalParameters[1].TypeName.TypeTags[0]);
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
        visitorMock.Visit(method1Body);
        
        int i = 0;

        // empty statement
        {
          var emptyStatementNode = (EmptyStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(emptyStatementNode);
          visitorMock.Visit(emptyStatementNode.Labels[0]);
        }
        // local variable declaration
        {
          var variableDeclarationStatementNode = (VariableDeclarationStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(variableDeclarationStatementNode);
          visitorMock.Visit(variableDeclarationStatementNode.Labels[0]);
          visitorMock.Visit(variableDeclarationStatementNode.Declaration);
          visitorMock.Visit(variableDeclarationStatementNode.Declaration.TypeName);
          visitorMock.Visit(variableDeclarationStatementNode.Declaration.TypeName.TypeTags[0]);
          visitorMock.Visit(variableDeclarationStatementNode.Declaration.VariableTags[0]);
          visitorMock.Visit(variableDeclarationStatementNode.Declaration.VariableTags[1]);
          var expInit =
            variableDeclarationStatementNode.Declaration.VariableTags[1].Initializer as ExpressionInitializerNode;
          visitorMock.Visit(expInit);
          visitorMock.Visit((Int32LiteralNode)expInit.Expression);
        }
        // local variable declaration (var)
        {
          var varNode = (VariableDeclarationStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(varNode);
          visitorMock.Visit(varNode.Declaration);
          visitorMock.Visit(varNode.Declaration.TypeName);
          visitorMock.Visit(varNode.Declaration.TypeName.TypeTags[0]);
          visitorMock.Visit(varNode.Declaration.VariableTags[0]);
          var expInit =
            varNode.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
          visitorMock.Visit(expInit);
          visitorMock.Visit((Int32LiteralNode)expInit.Expression);
        }
        // local constant declaration
        {
          var constStatementNode = (ConstStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(constStatementNode);
          visitorMock.Visit(constStatementNode.Labels[0]);
          visitorMock.Visit(constStatementNode.TypeName);
          visitorMock.Visit(constStatementNode.TypeName.TypeTags[0]);
          visitorMock.Visit(constStatementNode.ConstTags[0]);
          visitorMock.Visit((Int32LiteralNode)constStatementNode.ConstTags[0].Expression);
          visitorMock.Visit(constStatementNode.ConstTags[1]);
          visitorMock.Visit((Int32LiteralNode)constStatementNode.ConstTags[1].Expression);
        }
        // expression statement
        {
          var expressionStatementNode = (ExpressionStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(expressionStatementNode);
          visitorMock.Visit(expressionStatementNode.Labels[0]);
          var exp = expressionStatementNode.Expression as PostIncrementExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((SimpleNameNode)exp.Operand);
        }
        // if statement
        {
          var ifStatementNode = (IfStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(ifStatementNode);
          visitorMock.Visit(ifStatementNode.Labels[0]);
          visitorMock.Visit((TrueLiteralNode)ifStatementNode.Condition);
          visitorMock.Visit((BlockStatementNode)ifStatementNode.ThenStatement);
          visitorMock.Visit((BlockStatementNode)ifStatementNode.ElseStatement);
        }
        // switch statement
        {
          var switchStatementNode = (SwitchStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(switchStatementNode);
          visitorMock.Visit(switchStatementNode.Labels[0]);
          visitorMock.Visit((SimpleNameNode)switchStatementNode.Expression);
          visitorMock.Visit(switchStatementNode.SwitchSections[0]);
          visitorMock.Visit(switchStatementNode.SwitchSections[0].Labels[0]);
          visitorMock.Visit((Int32LiteralNode)switchStatementNode.SwitchSections[0].Labels[0].Expression);
          visitorMock.Visit(switchStatementNode.SwitchSections[0].Labels[1]);
          visitorMock.Visit((Int32LiteralNode)switchStatementNode.SwitchSections[0].Labels[1].Expression);
          var goto1 = switchStatementNode.SwitchSections[0].Statements[0] as GotoStatementNode;
          visitorMock.Visit(goto1);
          visitorMock.Visit((Int32LiteralNode)goto1.Expression);
          visitorMock.Visit(switchStatementNode.SwitchSections[1]);
          visitorMock.Visit(switchStatementNode.SwitchSections[1].Labels[0]);
          visitorMock.Visit((Int32LiteralNode)switchStatementNode.SwitchSections[1].Labels[0].Expression);
          visitorMock.Visit((GotoStatementNode)switchStatementNode.SwitchSections[1].Statements[0]);
          visitorMock.Visit(switchStatementNode.SwitchSections[2]);
          visitorMock.Visit(switchStatementNode.SwitchSections[2].Labels[0]);
          visitorMock.Visit((BreakStatementNode)switchStatementNode.SwitchSections[2].Statements[0]);
        }
        // while statement
        {
          var whileStatementNode = (WhileStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(whileStatementNode);
          visitorMock.Visit(whileStatementNode.Labels[0]);
          visitorMock.Visit((FalseLiteralNode)whileStatementNode.Condition);
          visitorMock.Visit((BlockStatementNode)whileStatementNode.Statement);
        }
        // do statement
        {
          var doWhileStatementNode = (DoWhileStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(doWhileStatementNode);
          visitorMock.Visit(doWhileStatementNode.Labels[0]);
          visitorMock.Visit((BlockStatementNode)doWhileStatementNode.Statement);
          visitorMock.Visit((FalseLiteralNode)doWhileStatementNode.Condition);
        }
        // for-statement (with local-variable-declaration)
        {
          var forStatementNode = (ForStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(forStatementNode);
          visitorMock.Visit(forStatementNode.Labels[0]);
          visitorMock.Visit(forStatementNode.Initializer);
          visitorMock.Visit(forStatementNode.Initializer.TypeName);
          visitorMock.Visit(forStatementNode.Initializer.TypeName.TypeTags[0]);
          visitorMock.Visit(forStatementNode.Initializer.VariableTags[0]);
          var expInit = forStatementNode.Initializer.VariableTags[0].Initializer as ExpressionInitializerNode;
          visitorMock.Visit(expInit);
          visitorMock.Visit((Int32LiteralNode)expInit.Expression);
          visitorMock.Visit((FalseLiteralNode)forStatementNode.Condition);
          visitorMock.Visit((PostIncrementExpressionNode)forStatementNode.Iterators[0]);
          visitorMock.Visit((SimpleNameNode)((PostIncrementExpressionNode)forStatementNode.Iterators[0]).Operand);
          visitorMock.Visit((ContinueStatementNode)forStatementNode.Statement);
        }
        // for-statement (with statement-expression-list)
        {
          var forStatementNode2 = (ForStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(forStatementNode2);
          visitorMock.Visit(forStatementNode2.Labels[0]);
          visitorMock.Visit((PostIncrementExpressionNode)forStatementNode2.Initializers[0]);
          visitorMock.Visit((SimpleNameNode)((PostIncrementExpressionNode)forStatementNode2.Initializers[0]).Operand);
          visitorMock.Visit((PostIncrementExpressionNode)forStatementNode2.Initializers[1]);
          visitorMock.Visit((SimpleNameNode)((PostIncrementExpressionNode)forStatementNode2.Initializers[1]).Operand);
          visitorMock.Visit((FalseLiteralNode)forStatementNode2.Condition);
          visitorMock.Visit((PostIncrementExpressionNode)forStatementNode2.Iterators[0]);
          visitorMock.Visit((SimpleNameNode)((PostIncrementExpressionNode)forStatementNode2.Iterators[0]).Operand);
          visitorMock.Visit((PostIncrementExpressionNode)forStatementNode2.Iterators[1]);
          visitorMock.Visit((SimpleNameNode)((PostIncrementExpressionNode)forStatementNode2.Iterators[1]).Operand);
          visitorMock.Visit((BlockStatementNode)forStatementNode2.Statement);
        }
        // foreach-statement
        {
          var foreachStatementNode = (ForeachStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(foreachStatementNode);
          visitorMock.Visit(foreachStatementNode.Labels[0]);
          visitorMock.Visit(foreachStatementNode.TypeName);
          visitorMock.Visit(foreachStatementNode.TypeName.TypeTags[0]);
          visitorMock.Visit((SimpleNameNode)foreachStatementNode.CollectionExpression);
          visitorMock.Visit((BlockStatementNode)foreachStatementNode.Statement);
        }
        // try-statement
        {
          var tryStatementNode = (TryStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(tryStatementNode);
          visitorMock.Visit(tryStatementNode.Labels[0]);
          visitorMock.Visit(tryStatementNode.TryBlock);
          visitorMock.Visit(tryStatementNode.CatchClauses[0]);
          visitorMock.Visit(tryStatementNode.CatchClauses[0].TypeName);
          visitorMock.Visit(tryStatementNode.CatchClauses[0].TypeName.TypeTags[0]);
          visitorMock.Visit(tryStatementNode.CatchClauses[0].Block);
          visitorMock.Visit(tryStatementNode.CatchClauses[1]);
          visitorMock.Visit(tryStatementNode.CatchClauses[1].Block);
          visitorMock.Visit(tryStatementNode.FinallyBlock);
        }
        // checked-statement
        {
          var checkedStatementNode = (CheckedStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(checkedStatementNode);
          visitorMock.Visit(checkedStatementNode.Labels[0]);
          visitorMock.Visit(checkedStatementNode.Block);
        }
        // unchecked-statement
        {
          var uncheckedStatementNode = (UncheckedStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(uncheckedStatementNode);
          visitorMock.Visit(uncheckedStatementNode.Labels[0]);
          visitorMock.Visit(uncheckedStatementNode.Block);
        }
        // lock-statement
        {
          var lockStatementNode = (LockStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(lockStatementNode);
          visitorMock.Visit(lockStatementNode.Labels[0]);
          visitorMock.Visit((ThisAccessNode)lockStatementNode.Expression);
          visitorMock.Visit((BlockStatementNode)lockStatementNode.Statement);
        }
        // using-statement (with local-variable-declaration)
        {
          var usingStatementNode = (UsingStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(usingStatementNode);
          visitorMock.Visit(usingStatementNode.Labels[0]);
          visitorMock.Visit(usingStatementNode.Initializer);
          visitorMock.Visit(usingStatementNode.Initializer.TypeName);
          visitorMock.Visit(usingStatementNode.Initializer.TypeName.TypeTags[0]);
          visitorMock.Visit(usingStatementNode.Initializer.VariableTags[0]);
          var expInit = usingStatementNode.Initializer.VariableTags[0].Initializer as ExpressionInitializerNode;
          visitorMock.Visit(expInit);
          visitorMock.Visit((NullLiteralNode)expInit.Expression);
          visitorMock.Visit((BlockStatementNode)usingStatementNode.Statement);
        }
        // using-statement (with expression)
        {
          var usingStatementNode = (UsingStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(usingStatementNode);
          visitorMock.Visit(usingStatementNode.Labels[0]);
          visitorMock.Visit((NullLiteralNode)usingStatementNode.Expression);
          visitorMock.Visit((BlockStatementNode)usingStatementNode.Statement);
        }
        // return statement
        {
          var returnStatementNode = (ReturnStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(returnStatementNode);
          visitorMock.Visit(returnStatementNode.Labels[0]);
          visitorMock.Visit((Int32LiteralNode)returnStatementNode.Expression);
        }
        // goto-statement
        {
          var gotoStatementNode = (GotoStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(gotoStatementNode);
          visitorMock.Visit(gotoStatementNode.Labels[0]);
        }
        // throw-statement
        {
          var throwStatementNode = (ThrowStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(throwStatementNode);
          visitorMock.Visit(throwStatementNode.Labels[0]);
          visitorMock.Visit((ObjectCreationExpressionNode)throwStatementNode.Expression);
          visitorMock.Visit(((ObjectCreationExpressionNode)throwStatementNode.Expression).TypeName);
          visitorMock.Visit(((ObjectCreationExpressionNode)throwStatementNode.Expression).TypeName.TypeTags[0]);
        }
        // unsafe-statement
        {
          var unsafeStatementNode = (UnsafeStatementNode)method1Body.Statements[i++];
          visitorMock.Visit(unsafeStatementNode);
          visitorMock.Visit(unsafeStatementNode.Labels[0]);
          visitorMock.Visit(unsafeStatementNode.Block);

          int j = 0;

          var stringVarNode = (VariableDeclarationStatementNode)unsafeStatementNode.Block.Statements[j++];
          visitorMock.Visit(stringVarNode);
          visitorMock.Visit(stringVarNode.Declaration);
          visitorMock.Visit(stringVarNode.Declaration.TypeName);
          visitorMock.Visit(stringVarNode.Declaration.TypeName.TypeTags[0]);
          visitorMock.Visit(stringVarNode.Declaration.VariableTags[0]);
          var expInit = stringVarNode.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
          visitorMock.Visit(expInit);
          visitorMock.Visit((StringLiteralNode)expInit.Expression);

          // fixed-statement
          {
            var fixedStatementNode = (FixedStatementNode)unsafeStatementNode.Block.Statements[j++];
            visitorMock.Visit(fixedStatementNode);
            visitorMock.Visit(fixedStatementNode.Labels[0]);
            visitorMock.Visit(fixedStatementNode.TypeName);
            visitorMock.Visit(fixedStatementNode.TypeName.TypeTags[0]);
            visitorMock.Visit(fixedStatementNode.Initializers[0]);
            visitorMock.Visit((SimpleNameNode)fixedStatementNode.Initializers[0].Expression);
            visitorMock.Visit(fixedStatementNode.Initializers[1]);
            visitorMock.Visit((SimpleNameNode)fixedStatementNode.Initializers[1].Expression);
            visitorMock.Visit((BlockStatementNode)fixedStatementNode.Statement);
          }
          // stackalloc initializer
          // char* p = stackalloc char[256];
          {
            var stackallocDecl = (VariableDeclarationStatementNode)unsafeStatementNode.Block.Statements[j++];
            visitorMock.Visit(stackallocDecl);
            visitorMock.Visit(stackallocDecl.Declaration);
            visitorMock.Visit(stackallocDecl.Declaration.TypeName);
            visitorMock.Visit(stackallocDecl.Declaration.TypeName.TypeTags[0]);
            visitorMock.Visit(stackallocDecl.Declaration.VariableTags[0]);
            var stackallocInit = stackallocDecl.Declaration.VariableTags[0].Initializer as StackAllocInitializerNode;
            visitorMock.Visit(stackallocInit);
            visitorMock.Visit(stackallocInit.TypeName);
            visitorMock.Visit(stackallocInit.TypeName.TypeTags[0]);
            visitorMock.Visit((Int32LiteralNode)stackallocInit.Expression);
          }
        }

        // IteratorMethod
        visitorMock.Visit(method2Body);

        // yield-return-statement
        {
          var yieldReturnStatementNode = (YieldReturnStatementNode) method2Body.Statements[0];
          visitorMock.Visit(yieldReturnStatementNode);
          visitorMock.Visit(yieldReturnStatementNode.Labels[0]);
          visitorMock.Visit((Int32LiteralNode)yieldReturnStatementNode.Expression);
        }
        // yield-break-statement
        {
          var yieldBreakStatementNode = (YieldBreakStatementNode) method2Body.Statements[1];
          visitorMock.Visit(yieldBreakStatementNode);
          visitorMock.Visit(yieldBreakStatementNode.Labels[0]);
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
        visitorMock.Visit(collInit);

        int i = 0;

        // array-creation-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as ArrayCreationExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit(exp.TypeName);
          visitorMock.Visit(exp.TypeName.TypeTags[0]);
          visitorMock.Visit(exp.ArraySizeSpecifier);
          visitorMock.Visit((Int32LiteralNode)exp.ArraySizeSpecifier.Expressions[0]);
          visitorMock.Visit((Int32LiteralNode)exp.ArraySizeSpecifier.Expressions[1]);
          visitorMock.Visit(exp.Initializer);
          var arrInit = exp.Initializer.VariableInitializers[0] as ArrayInitializerNode;
          visitorMock.Visit(arrInit);
          visitorMock.Visit((ExpressionInitializerNode)arrInit.VariableInitializers[0]);
          visitorMock.Visit((Int32LiteralNode)((ExpressionInitializerNode)arrInit.VariableInitializers[0]).Expression);
          visitorMock.Visit((ExpressionInitializerNode)arrInit.VariableInitializers[1]);
          visitorMock.Visit((Int32LiteralNode)((ExpressionInitializerNode)arrInit.VariableInitializers[1]).Expression);
        }
        // true-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as TrueLiteralNode;
          visitorMock.Visit(exp);
        }
        // false-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as FalseLiteralNode;
          visitorMock.Visit(exp);
        }
        // char-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as CharLiteralNode;
          visitorMock.Visit(exp);
        }
        // decimal-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as DecimalLiteralNode;
          visitorMock.Visit(exp);
        }
        // double-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as DoubleLiteralNode;
          visitorMock.Visit(exp);
        }
        // int32-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as Int32LiteralNode;
          visitorMock.Visit(exp);
        }
        // int64-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as Int64LiteralNode;
          visitorMock.Visit(exp);
        }
        // null-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as NullLiteralNode;
          visitorMock.Visit(exp);
        }
        // single-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as SingleLiteralNode;
          visitorMock.Visit(exp);
        }
        // string-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as StringLiteralNode;
          visitorMock.Visit(exp);
        }
        // uint32-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as UInt32LiteralNode;
          visitorMock.Visit(exp);
        }
        // uint64-literal
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as UInt64LiteralNode;
          visitorMock.Visit(exp);
        }
        // simple-name
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as SimpleNameNode;
          visitorMock.Visit(exp);
        }
        // parenthesized-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as ParenthesizedExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((Int32LiteralNode)exp.Expression);
        }
        // primary-expression-member-access
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as PrimaryExpressionMemberAccessNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((SimpleNameNode)exp.PrimaryExpression);
          visitorMock.Visit(exp.MemberName);
        }
        // pointer-member-access (inside an invocation-expression)
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as InvocationExpressionNode;
          visitorMock.Visit(exp);
          var pointerAccess = exp.PrimaryExpression as PointerMemberAccessNode;
          visitorMock.Visit(pointerAccess);
          visitorMock.Visit((SimpleNameNode)pointerAccess.PrimaryExpression);
          visitorMock.Visit(pointerAccess.MemberName);
        }
        // predefined-type-member-access
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as PredefinedTypeMemberAccessNode;
          visitorMock.Visit(exp);
          visitorMock.Visit(exp.TypeName);
          visitorMock.Visit(exp.TypeName.TypeTags[0]);
          visitorMock.Visit(exp.MemberName);
        }
        // qualified-alias-member-access
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as QualifiedAliasMemberAccessNode;
          visitorMock.Visit(exp);
          visitorMock.Visit(exp.QualifiedAliasMember);
          visitorMock.Visit(exp.MemberName);
        }
        // invocation-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as InvocationExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((SimpleNameNode)exp.PrimaryExpression);
          visitorMock.Visit(((SimpleNameNode)exp.PrimaryExpression).Arguments[0]);
          visitorMock.Visit(((SimpleNameNode)exp.PrimaryExpression).Arguments[0].TypeTags[0]);
          visitorMock.Visit(exp.Arguments[0]);
          visitorMock.Visit((Int32LiteralNode)exp.Arguments[0].Expression);
          visitorMock.Visit(exp.Arguments[1]);
          visitorMock.Visit((Int32LiteralNode)exp.Arguments[1].Expression);
        }
        // element-access
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as ElementAccessNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((SimpleNameNode)exp.PrimaryExpression);
          visitorMock.Visit((Int32LiteralNode)exp.Expressions[0]);
          visitorMock.Visit((Int32LiteralNode)exp.Expressions[1]);
        }
        // this-access
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as ThisAccessNode;
          visitorMock.Visit(exp);
        }
        // base-member-access
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as BaseMemberAccessNode;
          visitorMock.Visit(exp);
          visitorMock.Visit(exp.MemberName);
        }
        // base-element-access
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as BaseElementAccessNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((Int32LiteralNode)exp.Expressions[0]);
          visitorMock.Visit((Int32LiteralNode)exp.Expressions[1]);
        }
        // post-increment-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as PostIncrementExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((SimpleNameNode)exp.Operand);
        }
        // post-decrement-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as PostDecrementExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((SimpleNameNode)exp.Operand);
        }
        // object-creation-expression (with collection initializer)
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as ObjectCreationExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit(exp.TypeName);
          visitorMock.Visit(exp.TypeName.TypeTags[0]);
          visitorMock.Visit(exp.TypeName.TypeTags[0].Arguments[0]);
          visitorMock.Visit(exp.TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
          visitorMock.Visit(exp.TypeName.TypeTags[0].Arguments[1]);
          visitorMock.Visit(exp.TypeName.TypeTags[0].Arguments[1].TypeTags[0]);
          visitorMock.Visit(exp.ObjectOrCollectionInitializer);
          visitorMock.Visit(exp.ObjectOrCollectionInitializer.ElementInitializers[0]);
          visitorMock.Visit((Int32LiteralNode)exp.ObjectOrCollectionInitializer.ElementInitializers[0].ExpressionList[0]);
          visitorMock.Visit((Int32LiteralNode)exp.ObjectOrCollectionInitializer.ElementInitializers[0].ExpressionList[1]);
          visitorMock.Visit(exp.ObjectOrCollectionInitializer.ElementInitializers[1]);
          visitorMock.Visit((Int32LiteralNode)exp.ObjectOrCollectionInitializer.ElementInitializers[1].ExpressionList[0]);
          visitorMock.Visit((Int32LiteralNode)exp.ObjectOrCollectionInitializer.ElementInitializers[1].ExpressionList[1]);
        }
        // object-creation-expression (with argument-list and object-initializer
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as ObjectCreationExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit(exp.TypeName);
          visitorMock.Visit(exp.TypeName.TypeTags[0]);
          visitorMock.Visit(exp.Arguments[0]);
          visitorMock.Visit((Int32LiteralNode)exp.Arguments[0].Expression);
          visitorMock.Visit(exp.Arguments[1]);
          visitorMock.Visit((Int32LiteralNode)exp.Arguments[1].Expression);
          visitorMock.Visit(exp.ObjectOrCollectionInitializer);
          visitorMock.Visit(exp.ObjectOrCollectionInitializer.MemberInitializers[0]);
          visitorMock.Visit((Int32LiteralNode)exp.ObjectOrCollectionInitializer.MemberInitializers[0].Expression);
          visitorMock.Visit(exp.ObjectOrCollectionInitializer.MemberInitializers[1]);
          visitorMock.Visit((Int32LiteralNode)exp.ObjectOrCollectionInitializer.MemberInitializers[1].Expression);
        }
        // anonymous-object-creation-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as AnonymousObjectCreationExpressionNode;
          visitorMock.Visit(exp);
          var simpleNameMemberDeclarator = exp.Declarators[0] as SimpleNameMemberDeclaratorNode;
          visitorMock.Visit(simpleNameMemberDeclarator);
          visitorMock.Visit(simpleNameMemberDeclarator.SimpleName);
          var memberAccessMemberDeclarator = exp.Declarators[1] as MemberAccessMemberDeclaratorNode;
          visitorMock.Visit(memberAccessMemberDeclarator);
          var memberAccess = memberAccessMemberDeclarator.MemberAccess as PrimaryExpressionMemberAccessNode;
          visitorMock.Visit(memberAccess);
          visitorMock.Visit((SimpleNameNode)memberAccess.PrimaryExpression);
          visitorMock.Visit(memberAccess.MemberName);
          var baseMemberAccessMemberDeclarator = exp.Declarators[2] as BaseMemberAccessMemberDeclaratorNode;
          visitorMock.Visit(baseMemberAccessMemberDeclarator);
          visitorMock.Visit(baseMemberAccessMemberDeclarator.BaseMemberAccess);
          visitorMock.Visit(baseMemberAccessMemberDeclarator.BaseMemberAccess.MemberName);
          var identifierMemberDeclarator = exp.Declarators[3] as IdentifierMemberDeclaratorNode;
          visitorMock.Visit(identifierMemberDeclarator);
          visitorMock.Visit((Int32LiteralNode)identifierMemberDeclarator.Expression);
        }
        // typeof-expression (with unbound typename)
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as TypeofExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit(exp.TypeName);
          visitorMock.Visit(exp.TypeName.TypeTags[0]);
          visitorMock.Visit(exp.TypeName.TypeTags[0].Arguments[0]);
        }
        //// sizeof-expression
        //sizeof (int),
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as SizeofExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit(exp.TypeName);
          visitorMock.Visit(exp.TypeName.TypeTags[0]);
        }
        // checked-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as CheckedExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((Int32LiteralNode)exp.Expression);
        }
        // unchecked-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as UncheckedExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((Int32LiteralNode)exp.Expression);
        }
        // default-value-expression
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as DefaultValueExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit(exp.TypeName);
          visitorMock.Visit(exp.TypeName.TypeTags[0]);
        }
        // anonymous-method-expression (wrapped in a cast)
        // (Func<int, int, int>) delegate(int i1, int i2) { return 1; },
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var castExp = elemInit.NonAssignmentExpression as CastExpressionNode;
          visitorMock.Visit(castExp);
          visitorMock.Visit(castExp.TypeName);
          visitorMock.Visit(castExp.TypeName.TypeTags[0]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[0]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[1]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[1].TypeTags[0]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[2]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[2].TypeTags[0]);
          var exp = castExp.Operand as AnonymousMethodExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit(exp.FormalParameters[0]);
          visitorMock.Visit(exp.FormalParameters[0].TypeName);
          visitorMock.Visit(exp.FormalParameters[0].TypeName.TypeTags[0]);
          visitorMock.Visit(exp.FormalParameters[1]);
          visitorMock.Visit(exp.FormalParameters[1].TypeName);
          visitorMock.Visit(exp.FormalParameters[1].TypeName.TypeTags[0]);
          visitorMock.Visit(exp.Body);
          visitorMock.Visit((ReturnStatementNode)exp.Body.Statements[0]);
          visitorMock.Visit((Int32LiteralNode)((ReturnStatementNode)exp.Body.Statements[0]).Expression);
        }
        // lambda-expression (with implicit signature, expression body) (wrapped in a cast + parens)
        // (Expression<Func<int, int, int>>) ((x, y) => x + y),
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var castExp = elemInit.NonAssignmentExpression as CastExpressionNode;
          visitorMock.Visit(castExp);
          visitorMock.Visit(castExp.TypeName);
          visitorMock.Visit(castExp.TypeName.TypeTags[0]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[0]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[0].TypeTags[0].Arguments[0]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[0].TypeTags[0].Arguments[0].TypeTags[0]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[0].TypeTags[0].Arguments[1]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[0].TypeTags[0].Arguments[1].TypeTags[0]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[0].TypeTags[0].Arguments[2]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[0].TypeTags[0].Arguments[2].TypeTags[0]);
          var paren = castExp.Operand as ParenthesizedExpressionNode;
          visitorMock.Visit(paren);
          var exp = paren.Expression as LambdaExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit(exp.FormalParameters[0]);
          visitorMock.Visit(exp.FormalParameters[0].TypeName);
          visitorMock.Visit(exp.FormalParameters[1]);
          visitorMock.Visit(exp.FormalParameters[1].TypeName);
          var binExp = exp.Expression as BinaryExpressionNode;
          visitorMock.Visit(binExp);
          visitorMock.Visit((SimpleNameNode)binExp.LeftOperand);
          visitorMock.Visit((SimpleNameNode)binExp.RightOperand);
        }
        // lambda-expression (with explicit signature, block body) (wrapped in a cast + parens)
        // (Func<int, int, int>) ((int i1, int i2) => { return 1; }),
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var castExp = elemInit.NonAssignmentExpression as CastExpressionNode;
          visitorMock.Visit(castExp);
          visitorMock.Visit(castExp.TypeName);
          visitorMock.Visit(castExp.TypeName.TypeTags[0]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[0]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[1]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[1].TypeTags[0]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[2]);
          visitorMock.Visit(castExp.TypeName.TypeTags[0].Arguments[2].TypeTags[0]);
          var paren = castExp.Operand as ParenthesizedExpressionNode;
          visitorMock.Visit(paren);
          var exp = paren.Expression as LambdaExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit(exp.FormalParameters[0]);
          visitorMock.Visit(exp.FormalParameters[0].TypeName);
          visitorMock.Visit(exp.FormalParameters[0].TypeName.TypeTags[0]);
          visitorMock.Visit(exp.FormalParameters[1]);
          visitorMock.Visit(exp.FormalParameters[1].TypeName);
          visitorMock.Visit(exp.FormalParameters[1].TypeName.TypeTags[0]);
          visitorMock.Visit(exp.Block);
          visitorMock.Visit((ReturnStatementNode)exp.Block.Statements[0]);
          visitorMock.Visit((Int32LiteralNode)((ReturnStatementNode)exp.Block.Statements[0]).Expression);
        }
        // unary-expression
        // -1,
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as UnaryOperatorExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((Int32LiteralNode)exp.Operand);
        }
        // pre-increment-expression
        // ++p,
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as PreIncrementExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((SimpleNameNode)exp.Operand);
        }
        // pre-decrement-expression
        // --p,
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as PreDecrementExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((SimpleNameNode)exp.Operand);
        }
        // cast-expression
        // (int) 1,
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var castExp = elemInit.NonAssignmentExpression as CastExpressionNode;
          visitorMock.Visit(castExp);
          visitorMock.Visit(castExp.TypeName);
          visitorMock.Visit(castExp.TypeName.TypeTags[0]);
          visitorMock.Visit((Int32LiteralNode)castExp.Operand);
        }
        // binary-expression
        // 1 + 1,
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as BinaryExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((Int32LiteralNode)exp.LeftOperand);
          visitorMock.Visit((Int32LiteralNode)exp.RightOperand);
        }
        // type-testing-expression
        // 1 is int,
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as TypeTestingExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((Int32LiteralNode)exp.LeftOperand);
          visitorMock.Visit(exp.RightOperand);
          visitorMock.Visit(exp.RightOperand.TypeTags[0]);
        }
        // conditional-expression
        // true ? 1 : 2,
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as ConditionalExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((TrueLiteralNode)exp.Condition);
          visitorMock.Visit((Int32LiteralNode)exp.TrueExpression);
          visitorMock.Visit((Int32LiteralNode)exp.FalseExpression);
        }
        // assignment (wrapped in parens, otherwise assignment is not permitted here)
        // (p = 1),
        {
          var elemInit = collInit.ElementInitializers[i++];
          visitorMock.Visit(elemInit);
          var paren = elemInit.NonAssignmentExpression as ParenthesizedExpressionNode;
          visitorMock.Visit(paren);
          var exp = paren.Expression as AssignmentExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit((SimpleNameNode)exp.LeftOperand);
          visitorMock.Visit((Int32LiteralNode)exp.RightOperand);
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
          visitorMock.Visit(elemInit);
          var exp = elemInit.NonAssignmentExpression as QueryExpressionNode;
          visitorMock.Visit(exp);
          visitorMock.Visit(exp.FromClause);
          visitorMock.Visit(exp.FromClause.TypeName);
          visitorMock.Visit(exp.FromClause.TypeName.TypeTags[0]);
          visitorMock.Visit((SimpleNameNode) exp.FromClause.Expression);
          visitorMock.Visit(exp.QueryBody);
          int j = 0;
          {
            var bodyClause = exp.QueryBody.BodyClauses[j++] as FromClauseNode;
            visitorMock.Visit(bodyClause);
            visitorMock.Visit(bodyClause.TypeName);
            visitorMock.Visit(bodyClause.TypeName.TypeTags[0]);
            visitorMock.Visit((SimpleNameNode)bodyClause.Expression);
          }
          {
            var bodyClause = exp.QueryBody.BodyClauses[j++] as LetClauseNode;
            visitorMock.Visit(bodyClause);
            visitorMock.Visit((SimpleNameNode)bodyClause.Expression);
          }
          {
            var bodyClause = exp.QueryBody.BodyClauses[j++] as WhereClauseNode;
            visitorMock.Visit(bodyClause);
            visitorMock.Visit((TrueLiteralNode)bodyClause.Expression);
          }
          {
            var bodyClause = exp.QueryBody.BodyClauses[j++] as JoinClauseNode;
            visitorMock.Visit(bodyClause);
            visitorMock.Visit(bodyClause.TypeName);
            visitorMock.Visit((SimpleNameNode)bodyClause.InExpression);
            visitorMock.Visit((SimpleNameNode)bodyClause.OnExpression);
            visitorMock.Visit((SimpleNameNode)bodyClause.EqualsExpression);
          }
          {
            var bodyClause = exp.QueryBody.BodyClauses[j++] as JoinIntoClauseNode;
            visitorMock.Visit(bodyClause);
            visitorMock.Visit(bodyClause.TypeName);
            visitorMock.Visit((SimpleNameNode)bodyClause.InExpression);
            visitorMock.Visit((SimpleNameNode)bodyClause.OnExpression);
            visitorMock.Visit((SimpleNameNode)bodyClause.EqualsExpression);
          }
          {
            var bodyClause = exp.QueryBody.BodyClauses[j++] as OrderByClauseNode;
            visitorMock.Visit(bodyClause);
            visitorMock.Visit(bodyClause.Orderings[0]);
            visitorMock.Visit((SimpleNameNode)bodyClause.Orderings[0].Expression);
            visitorMock.Visit(bodyClause.Orderings[1]);
            visitorMock.Visit((SimpleNameNode)bodyClause.Orderings[1].Expression);
          }
          visitorMock.Visit(exp.QueryBody.SelectClause);
          visitorMock.Visit((SimpleNameNode)exp.QueryBody.SelectClause.Expression);
          var cont = exp.QueryBody.QueryContinuation;
          visitorMock.Visit(cont);
          visitorMock.Visit(cont.QueryBody);
          visitorMock.Visit(cont.QueryBody.GroupClause);
          visitorMock.Visit((SimpleNameNode)cont.QueryBody.GroupClause.GroupExpression);
          visitorMock.Visit((Int32LiteralNode)cont.QueryBody.GroupClause.ByExpression);
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
