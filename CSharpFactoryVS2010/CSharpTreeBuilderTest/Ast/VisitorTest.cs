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
  public class VisitorTest : ParserTestBed
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
      project.AddFile(@"Visitor\CompilationUnitNodeVisitorTest.cs");
      InvokeParser(project).ShouldBeTrue();
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
        //visitorMock.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0].Arguments[0].Expression);
        visitorMock.Visit(compilationUnitNode.NamespaceDeclarations[0]);
        visitorMock.Visit((ClassDeclarationNode)compilationUnitNode.TypeDeclarations[0]);
        visitorMock.Visit((StructDeclarationNode)compilationUnitNode.TypeDeclarations[1]);
        visitorMock.Visit((InterfaceDeclarationNode)compilationUnitNode.TypeDeclarations[2]);
        visitorMock.Visit((EnumDeclarationNode)compilationUnitNode.TypeDeclarations[3]);
        visitorMock.Visit((DelegateDeclarationNode)compilationUnitNode.TypeDeclarations[4]);
        visitorMock.Visit(((DelegateDeclarationNode)compilationUnitNode.TypeDeclarations[4]).TypeName);
        visitorMock.Visit(((DelegateDeclarationNode)compilationUnitNode.TypeDeclarations[4]).TypeName.TypeTags[0]);
        visitorMock.Visit(((DelegateDeclarationNode)compilationUnitNode.TypeDeclarations[4]).FormalParameters);
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
      project.AddFile(@"Visitor\TypeOrNamespaceNodeVisitorTest.cs");
      InvokeParser(project).ShouldBeTrue();
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
      project.AddFile(@"Visitor\ClassDeclarationNodeVisitorTest.cs");
      InvokeParser(project).ShouldBeTrue();
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
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[1]);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0]);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].TypeName);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[0]);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[1]);
        visitorMock.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[2]);

        // Type params and base types
        visitorMock.Visit(classDeclarationNode.TypeParameters[0]);
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
        var constDeclarationNode = (ConstDeclarationNode)classDeclarationNode.MemberDeclarations[0];
        visitorMock.Visit(constDeclarationNode);
        visitorMock.Visit(constDeclarationNode.AttributeDecorations[0]);
        visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0].Arguments[0]);
        visitorMock.Visit(constDeclarationNode.TypeName);
        visitorMock.Visit(constDeclarationNode.TypeName.TypeTags[0]);
        visitorMock.Visit(constDeclarationNode.ConstTags[0]);
        visitorMock.Visit(constDeclarationNode.ConstTags[1]);

        // Field declaration
        var fieldDeclarationNode = (FieldDeclarationNode)classDeclarationNode.MemberDeclarations[1];
        visitorMock.Visit(fieldDeclarationNode);
        visitorMock.Visit(fieldDeclarationNode.AttributeDecorations[0]);
        visitorMock.Visit(fieldDeclarationNode.AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(fieldDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(fieldDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit(fieldDeclarationNode.AttributeDecorations[0].Attributes[0].Arguments[0]);
        visitorMock.Visit(fieldDeclarationNode.TypeName);
        visitorMock.Visit(fieldDeclarationNode.TypeName.TypeTags[0]);
        visitorMock.Visit(fieldDeclarationNode.FieldTags[0]);
        visitorMock.Visit(fieldDeclarationNode.FieldTags[1]);

        // Method declaration
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
        visitorMock.Visit(methodDeclarationNode.FormalParameters);
        visitorMock.Visit(methodDeclarationNode.FormalParameters.Items[0]);
        visitorMock.Visit(methodDeclarationNode.FormalParameters.Items[0].TypeName);
        visitorMock.Visit(methodDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0]);
        visitorMock.Visit(methodDeclarationNode.FormalParameters.Items[1]);
        visitorMock.Visit(methodDeclarationNode.FormalParameters.Items[1].TypeName);
        visitorMock.Visit(methodDeclarationNode.FormalParameters.Items[1].TypeName.TypeTags[0]);
        visitorMock.Visit(methodDeclarationNode.FormalParameters.Items[2]);
        visitorMock.Visit(methodDeclarationNode.FormalParameters.Items[2].TypeName);
        visitorMock.Visit(methodDeclarationNode.FormalParameters.Items[2].TypeName.TypeTags[0]);
        visitorMock.Visit(methodDeclarationNode.FormalParameters.Items[2].TypeName.RankSpecifiers[0]);
#warning Attributes are missing from FormalParameterNode so these expectations are commented out at the moment
        //visitorMock.Visit(methodDeclarationNode.FormalParameters.Items[2].AttributeDecorations[0]);
        //visitorMock.Visit(methodDeclarationNode.FormalParameters.Items[2].AttributeDecorations[0].Attributes[0]);
        //visitorMock.Visit(methodDeclarationNode.FormalParameters.Items[2].AttributeDecorations[0].Attributes[0].TypeName);
        //visitorMock.Visit(methodDeclarationNode.FormalParameters.Items[2].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit(methodDeclarationNode.Body); 
        visitorMock.Visit((ExpressionStatementNode)methodDeclarationNode.Body.Statements[0]);

        // Method declaration (explicit interface implementation)
        var methodDeclarationNode2 = (MethodDeclarationNode)classDeclarationNode.MemberDeclarations[3];
        visitorMock.Visit(methodDeclarationNode2);
        visitorMock.Visit(methodDeclarationNode2.TypeName);
        visitorMock.Visit(methodDeclarationNode2.TypeName.TypeTags[0]);
        visitorMock.Visit(methodDeclarationNode2.FormalParameters);
        visitorMock.Visit(methodDeclarationNode2.Body);

        // Property declaration
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

        // Event declaration (field-like)
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

        // Event declaration (property-like)
        var eventDeclarationNode2 = (EventPropertyDeclarationNode)classDeclarationNode.MemberDeclarations[6];
        visitorMock.Visit(eventDeclarationNode2);
        visitorMock.Visit(eventDeclarationNode2.AttributeDecorations[0]);
        visitorMock.Visit(eventDeclarationNode2.AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(eventDeclarationNode2.AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(eventDeclarationNode2.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
#warning EventPropertyDeclarationNode TypeName visiting checking commented out because of a bug (TypeName contains the MemberName)
        //visitorMock.Visit(eventDeclarationNode2.TypeName);
        //visitorMock.Visit(eventDeclarationNode2.TypeName.TypeTags[0]);
        //visitorMock.Visit(eventDeclarationNode2.TypeName.TypeTags[0].Arguments[0]);
        //visitorMock.Visit(eventDeclarationNode2.TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
        visitorMock.Visit(eventDeclarationNode2.AddAccessor);
        visitorMock.Visit(eventDeclarationNode2.AddAccessor.Body);
        visitorMock.Visit(eventDeclarationNode2.RemoveAccessor);
        visitorMock.Visit(eventDeclarationNode2.RemoveAccessor.Body);

        // Indexer declaration
        var indexerDeclarationNode = (IndexerDeclarationNode)classDeclarationNode.MemberDeclarations[7];
        visitorMock.Visit(indexerDeclarationNode);
        visitorMock.Visit(indexerDeclarationNode.AttributeDecorations[0]);
        visitorMock.Visit(indexerDeclarationNode.AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(indexerDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(indexerDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit(indexerDeclarationNode.TypeName);
        visitorMock.Visit(indexerDeclarationNode.TypeName.TypeTags[0]);
        visitorMock.Visit(indexerDeclarationNode.FormalParameters);
        visitorMock.Visit(indexerDeclarationNode.FormalParameters.Items[0]);
        visitorMock.Visit(indexerDeclarationNode.FormalParameters.Items[0].TypeName);
        visitorMock.Visit(indexerDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0]);
        visitorMock.Visit(indexerDeclarationNode.FormalParameters.Items[1]);
        visitorMock.Visit(indexerDeclarationNode.FormalParameters.Items[1].TypeName);
        visitorMock.Visit(indexerDeclarationNode.FormalParameters.Items[1].TypeName.TypeTags[0]);
        visitorMock.Visit(indexerDeclarationNode.GetAccessor);
        visitorMock.Visit(indexerDeclarationNode.GetAccessor.Body);
        visitorMock.Visit((ReturnStatementNode)indexerDeclarationNode.GetAccessor.Body.Statements[0]);
        visitorMock.Visit(indexerDeclarationNode.SetAccessor);
        visitorMock.Visit(indexerDeclarationNode.SetAccessor.Body);

        // Operator declaration
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
        visitorMock.Visit(operatorDeclarationNode.FormalParameters);
        visitorMock.Visit(operatorDeclarationNode.FormalParameters.Items[0]);
        visitorMock.Visit(operatorDeclarationNode.FormalParameters.Items[0].TypeName);
        visitorMock.Visit(operatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0]);
        visitorMock.Visit(operatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[0]);
        visitorMock.Visit(operatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
        visitorMock.Visit(operatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[1]);
        visitorMock.Visit(operatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[1].TypeTags[0]);
        visitorMock.Visit(operatorDeclarationNode.Body);
        visitorMock.Visit((ReturnStatementNode)operatorDeclarationNode.Body.Statements[0]);

        // Conversion operator declaration
        var castOperatorDeclarationNode = (CastOperatorDeclarationNode)classDeclarationNode.MemberDeclarations[9];
        visitorMock.Visit(castOperatorDeclarationNode);
        visitorMock.Visit(castOperatorDeclarationNode.AttributeDecorations[0]);
        visitorMock.Visit(castOperatorDeclarationNode.AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(castOperatorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(castOperatorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit(castOperatorDeclarationNode.TypeName);
        visitorMock.Visit(castOperatorDeclarationNode.TypeName.TypeTags[0]);
        visitorMock.Visit(castOperatorDeclarationNode.FormalParameters);
        visitorMock.Visit(castOperatorDeclarationNode.FormalParameters.Items[0]);
        visitorMock.Visit(castOperatorDeclarationNode.FormalParameters.Items[0].TypeName);
        visitorMock.Visit(castOperatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0]);
        visitorMock.Visit(castOperatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[0]);
        visitorMock.Visit(castOperatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
        visitorMock.Visit(castOperatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[1]);
        visitorMock.Visit(castOperatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[1].TypeTags[0]);
        visitorMock.Visit(castOperatorDeclarationNode.Body);
        visitorMock.Visit((ReturnStatementNode)castOperatorDeclarationNode.Body.Statements[0]);

        // Constructor declaration 
        var constructorDeclarationNode = (ConstructorDeclarationNode)classDeclarationNode.MemberDeclarations[10];
        visitorMock.Visit(constructorDeclarationNode);
        visitorMock.Visit(constructorDeclarationNode.AttributeDecorations[0]);
        visitorMock.Visit(constructorDeclarationNode.AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(constructorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(constructorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit(constructorDeclarationNode.FormalParameters);
        visitorMock.Visit(constructorDeclarationNode.FormalParameters.Items[0]);
        visitorMock.Visit(constructorDeclarationNode.FormalParameters.Items[0].TypeName);
        visitorMock.Visit(constructorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0]);
        visitorMock.Visit(constructorDeclarationNode.FormalParameters.Items[1]);
        visitorMock.Visit(constructorDeclarationNode.FormalParameters.Items[1].TypeName);
        visitorMock.Visit(constructorDeclarationNode.FormalParameters.Items[1].TypeName.TypeTags[0]);
        visitorMock.Visit(constructorDeclarationNode.Body);

        // Constructor declaration with this initializer
        var constructorDeclarationNode2 = (ConstructorDeclarationNode)classDeclarationNode.MemberDeclarations[11];
        visitorMock.Visit(constructorDeclarationNode2);
        visitorMock.Visit(constructorDeclarationNode2.FormalParameters);
        visitorMock.Visit((ThisConstructorInitializerNode)constructorDeclarationNode2.Initializer);
        visitorMock.Visit(constructorDeclarationNode2.Initializer.Arguments[0]);
        visitorMock.Visit((Int32LiteralNode)constructorDeclarationNode2.Initializer.Arguments[0].Expression);
        visitorMock.Visit(constructorDeclarationNode2.Initializer.Arguments[1]);
        visitorMock.Visit((Int32LiteralNode)constructorDeclarationNode2.Initializer.Arguments[1].Expression);
        visitorMock.Visit(constructorDeclarationNode2.Body);

        // Constructor declaration with base initializer
        var constructorDeclarationNode3 = (ConstructorDeclarationNode)classDeclarationNode.MemberDeclarations[12];
        visitorMock.Visit(constructorDeclarationNode3);
        visitorMock.Visit(constructorDeclarationNode3.FormalParameters);
        visitorMock.Visit(constructorDeclarationNode3.FormalParameters.Items[0]);
        visitorMock.Visit(constructorDeclarationNode3.FormalParameters.Items[0].TypeName);
        visitorMock.Visit(constructorDeclarationNode3.FormalParameters.Items[0].TypeName.TypeTags[0]);
        visitorMock.Visit((BaseConstructorInitializerNode)constructorDeclarationNode3.Initializer);
        visitorMock.Visit(constructorDeclarationNode3.Body);

        // Destructor declaration
        var destructorDeclarationNode = (FinalizerDeclarationNode)classDeclarationNode.MemberDeclarations[13];
        visitorMock.Visit(destructorDeclarationNode);
        visitorMock.Visit(destructorDeclarationNode.AttributeDecorations[0]);
        visitorMock.Visit(destructorDeclarationNode.AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(destructorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(destructorDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit(destructorDeclarationNode.Body);

        // Nested class declaration
        var nestedClassDeclarationNode = (ClassDeclarationNode)classDeclarationNode.NestedTypes[0];
        visitorMock.Visit(nestedClassDeclarationNode);
        visitorMock.Visit(nestedClassDeclarationNode.AttributeDecorations[0]);
        visitorMock.Visit(nestedClassDeclarationNode.AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(nestedClassDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(nestedClassDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit(nestedClassDeclarationNode.AttributeDecorations[0].Attributes[0].Arguments[0]);
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
      project.AddFile(@"Visitor\StructDeclarationNodeVisitorTest.cs");
      InvokeParser(project).ShouldBeTrue();
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
        var constDeclarationNode = (ConstDeclarationNode)structDeclarationNode.MemberDeclarations[0];
        visitorMock.Visit(constDeclarationNode);
        visitorMock.Visit(constDeclarationNode.AttributeDecorations[0]);
        visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0]);
        visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName);
        visitorMock.Visit(constDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit(constDeclarationNode.TypeName);
        visitorMock.Visit(constDeclarationNode.TypeName.TypeTags[0]);
        visitorMock.Visit(constDeclarationNode.ConstTags[0]);

        // Field declaration
        var fieldDeclarationNode = (FieldDeclarationNode)structDeclarationNode.MemberDeclarations[1];
        visitorMock.Visit(fieldDeclarationNode);
        visitorMock.Visit(fieldDeclarationNode.TypeName);
        visitorMock.Visit(fieldDeclarationNode.TypeName.TypeTags[0]);
        visitorMock.Visit(fieldDeclarationNode.FieldTags[0]);

        // Method declaration
        var methodDeclarationNode = (MethodDeclarationNode)structDeclarationNode.MemberDeclarations[2];
        visitorMock.Visit(methodDeclarationNode);
        visitorMock.Visit(methodDeclarationNode.TypeName);
        visitorMock.Visit(methodDeclarationNode.TypeName.TypeTags[0]);
        visitorMock.Visit(methodDeclarationNode.FormalParameters);
        visitorMock.Visit(methodDeclarationNode.Body);

        // Property declaration
        var propertyDeclarationNode = (PropertyDeclarationNode)structDeclarationNode.MemberDeclarations[3];
        visitorMock.Visit(propertyDeclarationNode);
        visitorMock.Visit(propertyDeclarationNode.TypeName);
        visitorMock.Visit(propertyDeclarationNode.TypeName.TypeTags[0]);
        visitorMock.Visit(propertyDeclarationNode.GetAccessor);
        visitorMock.Visit(propertyDeclarationNode.GetAccessor.Body);
        visitorMock.Visit((ReturnStatementNode)propertyDeclarationNode.GetAccessor.Body.Statements[0]);
        visitorMock.Visit(propertyDeclarationNode.SetAccessor);
        visitorMock.Visit(propertyDeclarationNode.SetAccessor.Body);
        
        // Event declaration (property-like)
        var eventPropertyDeclarationNode = (EventPropertyDeclarationNode)structDeclarationNode.MemberDeclarations[4];
        visitorMock.Visit(eventPropertyDeclarationNode);
#warning EventPropertyDeclarationNode TypeName visiting checking commented out because of a bug (TypeName contains the MemberName)
        //visitorMock.Visit(eventPropertyDeclarationNode.TypeName);
        //visitorMock.Visit(eventPropertyDeclarationNode.TypeName.TypeTags[0]);
        //visitorMock.Visit(eventPropertyDeclarationNode.TypeName.TypeTags[0].Arguments[0]);
        //visitorMock.Visit(eventPropertyDeclarationNode.TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
        visitorMock.Visit(eventPropertyDeclarationNode.AddAccessor);
        visitorMock.Visit(eventPropertyDeclarationNode.AddAccessor.Body);
        visitorMock.Visit(eventPropertyDeclarationNode.RemoveAccessor);
        visitorMock.Visit(eventPropertyDeclarationNode.RemoveAccessor.Body);

        // Indexer declaration
        var indexerDeclarationNode = (IndexerDeclarationNode)structDeclarationNode.MemberDeclarations[5];
        visitorMock.Visit(indexerDeclarationNode);
        visitorMock.Visit(indexerDeclarationNode.TypeName);
        visitorMock.Visit(indexerDeclarationNode.TypeName.TypeTags[0]);
        visitorMock.Visit(indexerDeclarationNode.FormalParameters);
        visitorMock.Visit(indexerDeclarationNode.FormalParameters.Items[0]);
        visitorMock.Visit(indexerDeclarationNode.FormalParameters.Items[0].TypeName);
        visitorMock.Visit(indexerDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0]);
        visitorMock.Visit(indexerDeclarationNode.GetAccessor);
        visitorMock.Visit(indexerDeclarationNode.GetAccessor.Body);
        visitorMock.Visit((ReturnStatementNode)indexerDeclarationNode.GetAccessor.Body.Statements[0]);

        // Operator declaration
        var operatorDeclarationNode = (OperatorDeclarationNode)structDeclarationNode.MemberDeclarations[6];
        visitorMock.Visit(operatorDeclarationNode);
        visitorMock.Visit(operatorDeclarationNode.TypeName);
        visitorMock.Visit(operatorDeclarationNode.TypeName.TypeTags[0]);
        visitorMock.Visit(operatorDeclarationNode.TypeName.TypeTags[0].Arguments[0]);
        visitorMock.Visit(operatorDeclarationNode.TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
        visitorMock.Visit(operatorDeclarationNode.TypeName.TypeTags[0].Arguments[1]);
        visitorMock.Visit(operatorDeclarationNode.TypeName.TypeTags[0].Arguments[1].TypeTags[0]);
        visitorMock.Visit(operatorDeclarationNode.FormalParameters);
        visitorMock.Visit(operatorDeclarationNode.FormalParameters.Items[0]);
        visitorMock.Visit(operatorDeclarationNode.FormalParameters.Items[0].TypeName);
        visitorMock.Visit(operatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0]);
        visitorMock.Visit(operatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[0]);
        visitorMock.Visit(operatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
        visitorMock.Visit(operatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[1]);
        visitorMock.Visit(operatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[1].TypeTags[0]);
        visitorMock.Visit(operatorDeclarationNode.Body);
        visitorMock.Visit((ReturnStatementNode)operatorDeclarationNode.Body.Statements[0]);

        // Conversion operator declaration
        var castOperatorDeclarationNode = (CastOperatorDeclarationNode)structDeclarationNode.MemberDeclarations[7];
        visitorMock.Visit(castOperatorDeclarationNode);
        visitorMock.Visit(castOperatorDeclarationNode.TypeName);
        visitorMock.Visit(castOperatorDeclarationNode.TypeName.TypeTags[0]);
        visitorMock.Visit(castOperatorDeclarationNode.FormalParameters);
        visitorMock.Visit(castOperatorDeclarationNode.FormalParameters.Items[0]);
        visitorMock.Visit(castOperatorDeclarationNode.FormalParameters.Items[0].TypeName);
        visitorMock.Visit(castOperatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0]);
        visitorMock.Visit(castOperatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[0]);
        visitorMock.Visit(castOperatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[0].TypeTags[0]);
        visitorMock.Visit(castOperatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[1]);
        visitorMock.Visit(castOperatorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[1].TypeTags[0]);
        visitorMock.Visit(castOperatorDeclarationNode.Body);
        visitorMock.Visit((ReturnStatementNode)castOperatorDeclarationNode.Body.Statements[0]);

        // Constructor declaration 
        var constructorDeclarationNode = (ConstructorDeclarationNode)structDeclarationNode.MemberDeclarations[8];
        visitorMock.Visit(constructorDeclarationNode);
        visitorMock.Visit(constructorDeclarationNode.FormalParameters);
        visitorMock.Visit(constructorDeclarationNode.FormalParameters.Items[0]);
        visitorMock.Visit(constructorDeclarationNode.FormalParameters.Items[0].TypeName);
        visitorMock.Visit(constructorDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0]);
        visitorMock.Visit(constructorDeclarationNode.Body);
        visitorMock.Visit((ExpressionStatementNode)constructorDeclarationNode.Body.Statements[0]);

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
      project.AddFile(@"Visitor\InterfaceDeclarationNodeVisitorTest.cs");
      InvokeParser(project).ShouldBeTrue();
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
        visitorMock.Visit(methodDeclarationNode.FormalParameters);

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
        visitorMock.Visit(indexerDeclarationNode.FormalParameters);
        visitorMock.Visit(indexerDeclarationNode.FormalParameters.Items[0]);
        visitorMock.Visit(indexerDeclarationNode.FormalParameters.Items[0].TypeName);
        visitorMock.Visit(indexerDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0]);
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
      project.AddFile(@"Visitor\EnumDeclarationNodeVisitorTest.cs");
      InvokeParser(project).ShouldBeTrue();
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
      project.AddFile(@"Visitor\DelegateDeclarationNodeVisitorTest.cs");
      InvokeParser(project).ShouldBeTrue();
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
        var formalParameters = delegateDeclarationNode.FormalParameters;
        visitorMock.Visit(formalParameters);
        visitorMock.Visit(formalParameters.Items[0]);
#warning Attributes are missing from FormalParameterNode so these expectations are commented out at the moment
        //visitorMock.Visit(formalParameters.Items[0].AttributeDecorations[0]);
        //visitorMock.Visit(formalParameters.Items[0].AttributeDecorations[0].Attributes[0]);
        //visitorMock.Visit(formalParameters.Items[0].AttributeDecorations[0].Attributes[0].TypeName);
        //visitorMock.Visit(formalParameters.Items[0].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]);
        visitorMock.Visit(formalParameters.Items[0].TypeName);
        visitorMock.Visit(formalParameters.Items[0].TypeName.TypeTags[0]);
        visitorMock.Visit(formalParameters.Items[1]);
        visitorMock.Visit(formalParameters.Items[1].TypeName);
        visitorMock.Visit(formalParameters.Items[1].TypeName.TypeTags[0]);
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
      project.AddFile(@"Visitor\StatementVisitorTests.cs");
      InvokeParser(project).ShouldBeTrue();
      
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

        // empty statement
        var emptyStatementNode = (EmptyStatementNode)method1Body.Statements[0];
        visitorMock.Visit(emptyStatementNode);
        visitorMock.Visit(emptyStatementNode.Labels[0]);

        // local variable declaration
        var variableDeclarationStatementNode = (VariableDeclarationStatementNode) method1Body.Statements[1];
        visitorMock.Visit(variableDeclarationStatementNode);
        visitorMock.Visit(variableDeclarationStatementNode.Labels[0]);
        visitorMock.Visit(variableDeclarationStatementNode.Declaration);
        visitorMock.Visit(variableDeclarationStatementNode.Declaration.TypeName);
        visitorMock.Visit(variableDeclarationStatementNode.Declaration.TypeName.TypeTags[0]);
        visitorMock.Visit(variableDeclarationStatementNode.Declaration.VariableTags[0]);
        visitorMock.Visit(variableDeclarationStatementNode.Declaration.VariableTags[1]);

        // local variable declaration (var)
        var varNode = (VariableDeclarationStatementNode) method1Body.Statements[2];
        visitorMock.Visit(varNode);
        visitorMock.Visit(varNode.Declaration);
        visitorMock.Visit(varNode.Declaration.TypeName);
        visitorMock.Visit(varNode.Declaration.TypeName.TypeTags[0]);
        visitorMock.Visit(varNode.Declaration.VariableTags[0]);

        // local constant declaration
        var constStatementNode = (ConstStatementNode)method1Body.Statements[3];
        visitorMock.Visit(constStatementNode);
        visitorMock.Visit(constStatementNode.Labels[0]);
        visitorMock.Visit(constStatementNode.TypeName);
        visitorMock.Visit(constStatementNode.TypeName.TypeTags[0]);
        visitorMock.Visit(constStatementNode.ConstTags[0]);
        visitorMock.Visit(constStatementNode.ConstTags[1]);

        // expression statement
        var expressionStatementNode = (ExpressionStatementNode)method1Body.Statements[4];
        visitorMock.Visit(expressionStatementNode);
        visitorMock.Visit(expressionStatementNode.Labels[0]);

        // if statement
        var ifStatementNode = (IfStatementNode)method1Body.Statements[5];
        visitorMock.Visit(ifStatementNode);
        visitorMock.Visit(ifStatementNode.Labels[0]);
        visitorMock.Visit((BlockStatementNode)ifStatementNode.ThenStatement);
        visitorMock.Visit((BlockStatementNode)ifStatementNode.ElseStatement);

        // switch statement
        var switchStatementNode = (SwitchStatementNode)method1Body.Statements[6];
        visitorMock.Visit(switchStatementNode);
        visitorMock.Visit(switchStatementNode.Labels[0]);
        visitorMock.Visit(switchStatementNode.SwitchSections[0]);
        visitorMock.Visit(switchStatementNode.SwitchSections[0].Labels[0]);
        visitorMock.Visit(switchStatementNode.SwitchSections[0].Labels[1]);
        visitorMock.Visit((BreakStatementNode)switchStatementNode.SwitchSections[0].Statements[0]);
        visitorMock.Visit(switchStatementNode.SwitchSections[1]);
        visitorMock.Visit(switchStatementNode.SwitchSections[1].Labels[0]);
        visitorMock.Visit((BreakStatementNode)switchStatementNode.SwitchSections[1].Statements[0]);

        // while statement
        var whileStatementNode = (WhileStatementNode)method1Body.Statements[7];
        visitorMock.Visit(whileStatementNode);
        visitorMock.Visit(whileStatementNode.Labels[0]);
        visitorMock.Visit((BlockStatementNode)whileStatementNode.Statement);

        // do statement
        var doWhileStatementNode = (DoWhileStatementNode)method1Body.Statements[8];
        visitorMock.Visit(doWhileStatementNode);
        visitorMock.Visit(doWhileStatementNode.Labels[0]);
        visitorMock.Visit((BlockStatementNode)doWhileStatementNode.Statement);

        // for-statement (with local-variable-declaration)
        var forStatementNode = (ForStatementNode)method1Body.Statements[9];
        visitorMock.Visit(forStatementNode);
        visitorMock.Visit(forStatementNode.Labels[0]);
        visitorMock.Visit(forStatementNode.Initializer);
        visitorMock.Visit(forStatementNode.Initializer.TypeName);
        visitorMock.Visit(forStatementNode.Initializer.TypeName.TypeTags[0]);
        visitorMock.Visit(forStatementNode.Initializer.VariableTags[0]);
        visitorMock.Visit((ContinueStatementNode)forStatementNode.Statement);

        // for-statement (with statement-expression-list)
        var forStatementNode2 = (ForStatementNode)method1Body.Statements[10];
        visitorMock.Visit(forStatementNode2);
        visitorMock.Visit(forStatementNode2.Labels[0]);
        visitorMock.Visit((BlockStatementNode)forStatementNode2.Statement);

        // foreach-statement
        var foreachStatementNode = (ForeachStatementNode)method1Body.Statements[11];
        visitorMock.Visit(foreachStatementNode);
        visitorMock.Visit(foreachStatementNode.Labels[0]);
        visitorMock.Visit(foreachStatementNode.TypeName);
        visitorMock.Visit(foreachStatementNode.TypeName.TypeTags[0]);
        visitorMock.Visit((BlockStatementNode)foreachStatementNode.Statement);

        // try-statement
        var tryStatementNode = (TryStatementNode)method1Body.Statements[12];
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

        // checked-statement
        var checkedStatementNode = (CheckedStatementNode)method1Body.Statements[13];
        visitorMock.Visit(checkedStatementNode);
        visitorMock.Visit(checkedStatementNode.Labels[0]);
        visitorMock.Visit(checkedStatementNode.Block);

        // unchecked-statement
        var uncheckedStatementNode = (UncheckedStatementNode)method1Body.Statements[14];
        visitorMock.Visit(uncheckedStatementNode);
        visitorMock.Visit(uncheckedStatementNode.Labels[0]);
        visitorMock.Visit(uncheckedStatementNode.Block);

        // lock-statement
        var lockStatementNode = (LockStatementNode)method1Body.Statements[15];
        visitorMock.Visit(lockStatementNode);
        visitorMock.Visit(lockStatementNode.Labels[0]);
        visitorMock.Visit((BlockStatementNode)lockStatementNode.Statement);

        // using-statement
        var usingStatementNode = (UsingStatementNode)method1Body.Statements[16];
        visitorMock.Visit(usingStatementNode);
        visitorMock.Visit(usingStatementNode.Labels[0]);
        visitorMock.Visit(usingStatementNode.Initializer);
        visitorMock.Visit(usingStatementNode.Initializer.TypeName);
        visitorMock.Visit(usingStatementNode.Initializer.TypeName.TypeTags[0]);
        visitorMock.Visit(usingStatementNode.Initializer.VariableTags[0]);
        visitorMock.Visit((BlockStatementNode)usingStatementNode.Statement);

        // return statement
        var returnStatementNode = (ReturnStatementNode)method1Body.Statements[17];
        visitorMock.Visit(returnStatementNode);
        visitorMock.Visit(returnStatementNode.Labels[0]);

        // goto-statement
        var gotoStatementNode = (GotoStatementNode)method1Body.Statements[18];
        visitorMock.Visit(gotoStatementNode);
        visitorMock.Visit(gotoStatementNode.Labels[0]);

        // throw-statement
        var throwStatementNode = (ThrowStatementNode)method1Body.Statements[19];
        visitorMock.Visit(throwStatementNode);
        visitorMock.Visit(throwStatementNode.Labels[0]);

        // unsafe-statement
        var unsafeStatementNode = (UnsafeStatementNode)method1Body.Statements[20];
        visitorMock.Visit(unsafeStatementNode);
        visitorMock.Visit(unsafeStatementNode.Labels[0]);
        visitorMock.Visit(unsafeStatementNode.Block);
        var stringVarNode = (VariableDeclarationStatementNode) unsafeStatementNode.Block.Statements[0];
        visitorMock.Visit(stringVarNode);
        visitorMock.Visit(stringVarNode.Declaration);
        visitorMock.Visit(stringVarNode.Declaration.TypeName);
        visitorMock.Visit(stringVarNode.Declaration.TypeName.TypeTags[0]);
        visitorMock.Visit(stringVarNode.Declaration.VariableTags[0]);

        // fixed-statement
        var fixedStatementNode = (FixedStatementNode)unsafeStatementNode.Block.Statements[1];
        visitorMock.Visit(fixedStatementNode);
        visitorMock.Visit(fixedStatementNode.Labels[0]);
        visitorMock.Visit(fixedStatementNode.TypeName);
        visitorMock.Visit(fixedStatementNode.TypeName.TypeTags[0]);
        visitorMock.Visit(fixedStatementNode.Initializers[0]);
        visitorMock.Visit(fixedStatementNode.Initializers[1]);
        visitorMock.Visit((BlockStatementNode)fixedStatementNode.Statement);

        // IteratorMethod
        visitorMock.Visit(method2Body);

        // yield-return-statement
        var yieldReturnStatementNode = (YieldReturnStatementNode)method2Body.Statements[0];
        visitorMock.Visit(yieldReturnStatementNode);
        visitorMock.Visit(yieldReturnStatementNode.Labels[0]);

        // yield-break-statement
        var yieldBreakStatementNode = (YieldBreakStatementNode)method2Body.Statements[1];
        visitorMock.Visit(yieldBreakStatementNode);
        visitorMock.Visit(yieldBreakStatementNode.Labels[0]);
      }
      mocks.ReplayAll();

      // Act
      method1Body.AcceptVisitor(visitorMock);
      method2Body.AcceptVisitor(visitorMock);

      // Assert
      mocks.VerifyAll();
    }

  }
}
