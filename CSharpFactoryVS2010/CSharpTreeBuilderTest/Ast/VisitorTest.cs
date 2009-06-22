using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;
using SoftwareApproach.TestingExtensions;

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
      var compilationUnitNode = project.SyntaxTree.SourceFileNodes[0];

      // Set up expectations on mock
      var visitorMock = new Mock<ISyntaxNodeVisitor>(MockBehavior.Strict);
      visitorMock.Setup(v => v.Visit(compilationUnitNode));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.ExternAliasNodes[0]));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.UsingNodes[0]));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.UsingNodes[0].TypeName));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.UsingNodes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit((UsingAliasNode) compilationUnitNode.UsingNodes[1]));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.UsingNodes[1].TypeName));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.UsingNodes[1].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.GlobalAttributes[0]));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0].Arguments[0]));
      //visitorMock.Setup(v => v.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0].Arguments[0].Expression));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.NamespaceDeclarations[0]));
      visitorMock.Setup(v => v.Visit((ClassDeclarationNode) compilationUnitNode.TypeDeclarations[0]));
      visitorMock.Setup(v => v.Visit((StructDeclarationNode) compilationUnitNode.TypeDeclarations[1]));
      visitorMock.Setup(v => v.Visit((InterfaceDeclarationNode) compilationUnitNode.TypeDeclarations[2]));
      visitorMock.Setup(v => v.Visit((EnumDeclarationNode) compilationUnitNode.TypeDeclarations[3]));
      visitorMock.Setup(v => v.Visit((DelegateDeclarationNode) compilationUnitNode.TypeDeclarations[4]));
      visitorMock.Setup(v => v.Visit(((DelegateDeclarationNode)compilationUnitNode.TypeDeclarations[4]).TypeName));
      visitorMock.Setup(v => v.Visit(((DelegateDeclarationNode)compilationUnitNode.TypeDeclarations[4]).TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((DelegateDeclarationNode)compilationUnitNode.TypeDeclarations[4]).FormalParameters));

      // Act
      compilationUnitNode.AcceptVisitor(visitorMock.Object);

      // Verify that all set up expectation were met. Order is not verified, unfortunately.
      visitorMock.VerifyAll();
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
      var typeOrNamespaceNode = project.SyntaxTree.SourceFileNodes[0].UsingNodes[0].TypeName;

      // Set up expectations on mock
      var visitorMock = new Mock<ISyntaxNodeVisitor>(MockBehavior.Strict);
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode));
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[0]));                                                   // System
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[1]));                                                   // Collections
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[2]));                                                   // Generic
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[3]));                                                   // IDictionary
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0]));                                        // System.Nullable<int>**[][,]
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeTags[0]));                              // System
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeTags[1]));                              // Nullable
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeTags[1].Arguments[0]));                   // int
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeTags[1].Arguments[0].TypeTags[0]));         // int
      visitorMock.Setup(v => v.Visit((PointerModifierNode) typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeModifiers[0]));   // *
      visitorMock.Setup(v => v.Visit((PointerModifierNode) typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeModifiers[1]));   // *
      //visitorMock.Setup(v => v.Visit((ArrayModifierNode) typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeModifiers[2]));     // []
      //visitorMock.Setup(v => v.Visit((ArrayModifierNode) typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeModifiers[3]));     // [,]
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[1]));                                        // string**[][,]
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[1].TypeTags[0]));                              // string
      visitorMock.Setup(v => v.Visit((PointerModifierNode) typeOrNamespaceNode.TypeTags[3].Arguments[1].TypeModifiers[0]));   // *
      visitorMock.Setup(v => v.Visit((PointerModifierNode) typeOrNamespaceNode.TypeTags[3].Arguments[1].TypeModifiers[1]));   // *
      //visitorMock.Setup(v => v.Visit((ArrayModifierNode) typeOrNamespaceNode.TypeTags[3].Arguments[1].TypeModifiers[2]));     // []
      //visitorMock.Setup(v => v.Visit((ArrayModifierNode) typeOrNamespaceNode.TypeTags[3].Arguments[1].TypeModifiers[3]));     // [,]

      // Act
      typeOrNamespaceNode.AcceptVisitor(visitorMock.Object);

      // Verify that all set up expectation were met. Order is not verified, unfortunately.
      visitorMock.VerifyAll();
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
      var classDeclarationNode = (ClassDeclarationNode)project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0];

      // Set up expectations on mock
      var visitorMock = new Mock<ISyntaxNodeVisitor>(MockBehavior.Strict);
      visitorMock.Setup(v => v.Visit(classDeclarationNode));

      // Class attributes
      visitorMock.Setup(v => v.Visit(classDeclarationNode.AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[1]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[1].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[1].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.AttributeDecorations[0].Attributes[1].Arguments[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.AttributeDecorations[1]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[1]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.AttributeDecorations[1].Attributes[0].Arguments[2]));

      // Type params and base types
      visitorMock.Setup(v => v.Visit(classDeclarationNode.TypeParameters[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.TypeParameters[1]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.BaseTypes[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.BaseTypes[0].TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.BaseTypes[1]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.BaseTypes[1].TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.TypeParameterConstraints[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.TypeParameterConstraints[1]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.TypeParameterConstraints[1].ConstraintTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.TypeParameterConstraints[1].ConstraintTags[1]));

      // Constant declaration
      visitorMock.Setup(v => v.Visit((ConstDeclarationNode) classDeclarationNode.MemberDeclarations[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[0].AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[0].AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[0].AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[0].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[0].AttributeDecorations[0].Attributes[0].Arguments[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[0].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((ConstDeclarationNode)classDeclarationNode.MemberDeclarations[0]).ConstTags[0]));
      visitorMock.Setup(v => v.Visit(((ConstDeclarationNode)classDeclarationNode.MemberDeclarations[0]).ConstTags[1]));

      // Field declaration
      visitorMock.Setup(v => v.Visit((FieldDeclarationNode) classDeclarationNode.MemberDeclarations[1]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[1].AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[1].AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[1].AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[1].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[1].AttributeDecorations[0].Attributes[0].Arguments[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[1].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[1].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((FieldDeclarationNode)classDeclarationNode.MemberDeclarations[1]).FieldTags[0]));
      visitorMock.Setup(v => v.Visit(((FieldDeclarationNode)classDeclarationNode.MemberDeclarations[1]).FieldTags[1]));

      // Method declaration
      visitorMock.Setup(v => v.Visit((MethodDeclarationNode) classDeclarationNode.MemberDeclarations[2]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[2].AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[2].AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[2].AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[2].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[2].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[2].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[2].TypeParameters[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[2].TypeParameters[1]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[2].TypeParameterConstraints[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[2].TypeParameterConstraints[0].ConstraintTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[2].TypeParameterConstraints[1]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[2].TypeParameterConstraints[1].ConstraintTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[2]).FormalParameters));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[2]).FormalParameters.Items[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[2]).FormalParameters.Items[0].TypeName));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[2]).FormalParameters.Items[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[2]).FormalParameters.Items[1]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[2]).FormalParameters.Items[1].TypeName));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[2]).FormalParameters.Items[1].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[2]).FormalParameters.Items[2]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[2]).FormalParameters.Items[2].TypeName));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[2]).FormalParameters.Items[2].TypeName.TypeTags[0]));
      //visitorMock.Setup(v => v.Visit((ArrayModifierNode)((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[2]).FormalParameters.Items[2].TypeName.TypeModifiers[0]));
#warning Attributes are missing from FormalParameterNode so these expectations are commented out at the moment
      //visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[2]).FormalParameters.Items[2].AttributeDecorations[0]));
      //visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[2]).FormalParameters.Items[2].AttributeDecorations[0].Attributes[0]));
      //visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[2]).FormalParameters.Items[2].AttributeDecorations[0].Attributes[0].TypeName));
      //visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[2]).FormalParameters.Items[2].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));

      // Method declaration (explicit interface implementation)
      visitorMock.Setup(v => v.Visit((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[3]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[3].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[3].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[3]).FormalParameters));

      // Property declaration
      visitorMock.Setup(v => v.Visit((PropertyDeclarationNode)classDeclarationNode.MemberDeclarations[4]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[4].AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[4].AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[4].AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[4].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[4].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[4].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((PropertyDeclarationNode)classDeclarationNode.MemberDeclarations[4]).GetAccessor));
      visitorMock.Setup(v => v.Visit(((PropertyDeclarationNode)classDeclarationNode.MemberDeclarations[4]).SetAccessor));

      // Event declaration (field-like)
#warning Field-like event declaration testing commented out, because the parser creates a FieldDeclarationNode for this
      //visitorMock.Setup(v => v.Visit((EventPropertyDeclarationNode) classDeclarationNode.MemberDeclarations[5]));
      visitorMock.Setup(v => v.Visit((FieldDeclarationNode)classDeclarationNode.MemberDeclarations[5]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[5].AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[5].AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[5].AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[5].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[5].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[5].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[5].TypeName.TypeTags[0].Arguments[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[5].TypeName.TypeTags[0].Arguments[0].TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((FieldDeclarationNode)classDeclarationNode.MemberDeclarations[5]).FieldTags[0]));

      // Event declaration (property-like)
      visitorMock.Setup(v => v.Visit((EventPropertyDeclarationNode)classDeclarationNode.MemberDeclarations[6]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[6].AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[6].AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[6].AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[6].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));
#warning EventPropertyDeclarationNode TypeName visiting checking commented out because of a bug (TypeName contains the MemberName)
      //visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[6].TypeName));
      //visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[6].TypeName.TypeTags[0]));
      //visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[6].TypeName.TypeTags[0].Arguments[0]));
      //visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[6].TypeName.TypeTags[0].Arguments[0].TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((EventPropertyDeclarationNode)classDeclarationNode.MemberDeclarations[6]).AddAccessor));
      visitorMock.Setup(v => v.Visit(((EventPropertyDeclarationNode)classDeclarationNode.MemberDeclarations[6]).RemoveAccessor));

      // Indexer declaration
      visitorMock.Setup(v => v.Visit((IndexerDeclarationNode)classDeclarationNode.MemberDeclarations[7]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[7].AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[7].AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[7].AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[7].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[7].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[7].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((IndexerDeclarationNode)classDeclarationNode.MemberDeclarations[7]).FormalParameters));
      visitorMock.Setup(v => v.Visit(((IndexerDeclarationNode)classDeclarationNode.MemberDeclarations[7]).FormalParameters.Items[0]));
      visitorMock.Setup(v => v.Visit(((IndexerDeclarationNode)classDeclarationNode.MemberDeclarations[7]).FormalParameters.Items[0].TypeName));
      visitorMock.Setup(v => v.Visit(((IndexerDeclarationNode)classDeclarationNode.MemberDeclarations[7]).FormalParameters.Items[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((IndexerDeclarationNode)classDeclarationNode.MemberDeclarations[7]).FormalParameters.Items[1]));
      visitorMock.Setup(v => v.Visit(((IndexerDeclarationNode)classDeclarationNode.MemberDeclarations[7]).FormalParameters.Items[1].TypeName));
      visitorMock.Setup(v => v.Visit(((IndexerDeclarationNode)classDeclarationNode.MemberDeclarations[7]).FormalParameters.Items[1].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((PropertyDeclarationNode)classDeclarationNode.MemberDeclarations[7]).GetAccessor));
      visitorMock.Setup(v => v.Visit(((PropertyDeclarationNode)classDeclarationNode.MemberDeclarations[7]).SetAccessor));

      // Operator declaration
      visitorMock.Setup(v => v.Visit((OperatorDeclarationNode) classDeclarationNode.MemberDeclarations[8]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[8].AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[8].AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[8].AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[8].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[8].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[8].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[8].TypeName.TypeTags[0].Arguments[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[8].TypeName.TypeTags[0].Arguments[0].TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[8].TypeName.TypeTags[0].Arguments[1]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[8].TypeName.TypeTags[0].Arguments[1].TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[8]).FormalParameters));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[8]).FormalParameters.Items[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[8]).FormalParameters.Items[0].TypeName));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[8]).FormalParameters.Items[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[8]).FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[8]).FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[0].TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[8]).FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[1]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[8]).FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[1].TypeTags[0]));

      // Conversion operator declaration
      visitorMock.Setup(v => v.Visit((CastOperatorDeclarationNode) classDeclarationNode.MemberDeclarations[9]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[9].AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[9].AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[9].AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[9].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[9].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[9].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[9]).FormalParameters));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[9]).FormalParameters.Items[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[9]).FormalParameters.Items[0].TypeName));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[9]).FormalParameters.Items[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[9]).FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[9]).FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[0].TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[9]).FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[1]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[9]).FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[1].TypeTags[0]));

      // Constructor declaration 
      visitorMock.Setup(v => v.Visit((ConstructorDeclarationNode) classDeclarationNode.MemberDeclarations[10]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[10].AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[10].AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[10].AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[10].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[10]).FormalParameters));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[10]).FormalParameters.Items[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[10]).FormalParameters.Items[0].TypeName));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[10]).FormalParameters.Items[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[10]).FormalParameters.Items[1]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[10]).FormalParameters.Items[1].TypeName));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[10]).FormalParameters.Items[1].TypeName.TypeTags[0]));

      // Constructor declaration with initializer
      visitorMock.Setup(v => v.Visit((ConstructorDeclarationNode) classDeclarationNode.MemberDeclarations[11]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)classDeclarationNode.MemberDeclarations[11]).FormalParameters));
#warning Constructor-initializer visiting is not yet implemented.
      //visitorMock.Setup(v => v.Visit(((ConstructorDeclarationNode)classDeclarationNode.MemberDeclarations[11]).Initializer));

      // Destructor declaration
      visitorMock.Setup(v => v.Visit((FinalizerDeclarationNode) classDeclarationNode.MemberDeclarations[12]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[12].AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[12].AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[12].AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[12].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));

      // Nested class declaration
      visitorMock.Setup(v => v.Visit((ClassDeclarationNode)classDeclarationNode.NestedTypes[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.NestedTypes[0].AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.NestedTypes[0].AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.NestedTypes[0].AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.NestedTypes[0].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(classDeclarationNode.NestedTypes[0].AttributeDecorations[0].Attributes[0].Arguments[0]));

      // Act
      classDeclarationNode.AcceptVisitor(visitorMock.Object);

      // Verify that all set up expectation were met. Order is not verified, unfortunately.
      visitorMock.VerifyAll();
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
      var structDeclarationNode = (StructDeclarationNode)project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0];

      // Set up expectations on mock
      var visitorMock = new Mock<ISyntaxNodeVisitor>(MockBehavior.Strict);
      visitorMock.Setup(v => v.Visit(structDeclarationNode));

      // Struct attributes
      visitorMock.Setup(v => v.Visit(structDeclarationNode.AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));

      // Type params and base types
      visitorMock.Setup(v => v.Visit(structDeclarationNode.TypeParameters[0]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.TypeParameters[1]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.BaseTypes[0]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.BaseTypes[0].TypeTags[0]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.TypeParameterConstraints[0]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.TypeParameterConstraints[1]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.TypeParameterConstraints[1].ConstraintTags[0]));

      // Constant declaration
      visitorMock.Setup(v => v.Visit((ConstDeclarationNode)structDeclarationNode.MemberDeclarations[0]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[0].AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[0].AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[0].AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[0].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[0].TypeName));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((ConstDeclarationNode)structDeclarationNode.MemberDeclarations[0]).ConstTags[0]));

      // Field declaration
      visitorMock.Setup(v => v.Visit((FieldDeclarationNode)structDeclarationNode.MemberDeclarations[1]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[1].TypeName));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[1].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((FieldDeclarationNode)structDeclarationNode.MemberDeclarations[1]).FieldTags[0]));

      // Method declaration
      visitorMock.Setup(v => v.Visit((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[2]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[2].TypeName));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[2].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[2]).FormalParameters));

      // Property declaration
      visitorMock.Setup(v => v.Visit((PropertyDeclarationNode)structDeclarationNode.MemberDeclarations[3]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[3].TypeName));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[3].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((PropertyDeclarationNode)structDeclarationNode.MemberDeclarations[3]).GetAccessor));
      visitorMock.Setup(v => v.Visit(((PropertyDeclarationNode)structDeclarationNode.MemberDeclarations[3]).SetAccessor));

      // Event declaration (property-like)
      visitorMock.Setup(v => v.Visit((EventPropertyDeclarationNode)structDeclarationNode.MemberDeclarations[4]));
#warning EventPropertyDeclarationNode TypeName visiting checking commented out because of a bug (TypeName contains the MemberName)
      //visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[4].TypeName));
      //visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[4].TypeName.TypeTags[0]));
      //visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[4].TypeName.TypeTags[0].Arguments[0]));
      //visitorMock.Setup(v => v.Visit(classDeclarationNode.MemberDeclarations[4].TypeName.TypeTags[0].Arguments[0].TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((EventPropertyDeclarationNode)structDeclarationNode.MemberDeclarations[4]).AddAccessor));
      visitorMock.Setup(v => v.Visit(((EventPropertyDeclarationNode)structDeclarationNode.MemberDeclarations[4]).RemoveAccessor));

      // Indexer declaration
      visitorMock.Setup(v => v.Visit((IndexerDeclarationNode)structDeclarationNode.MemberDeclarations[5]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[5].TypeName));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[5].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((IndexerDeclarationNode)structDeclarationNode.MemberDeclarations[5]).FormalParameters));
      visitorMock.Setup(v => v.Visit(((IndexerDeclarationNode)structDeclarationNode.MemberDeclarations[5]).FormalParameters.Items[0]));
      visitorMock.Setup(v => v.Visit(((IndexerDeclarationNode)structDeclarationNode.MemberDeclarations[5]).FormalParameters.Items[0].TypeName));
      visitorMock.Setup(v => v.Visit(((IndexerDeclarationNode)structDeclarationNode.MemberDeclarations[5]).FormalParameters.Items[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((PropertyDeclarationNode)structDeclarationNode.MemberDeclarations[5]).GetAccessor));

      // Operator declaration
      visitorMock.Setup(v => v.Visit((OperatorDeclarationNode)structDeclarationNode.MemberDeclarations[6]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[6].TypeName));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[6].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[6].TypeName.TypeTags[0].Arguments[0]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[6].TypeName.TypeTags[0].Arguments[0].TypeTags[0]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[6].TypeName.TypeTags[0].Arguments[1]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[6].TypeName.TypeTags[0].Arguments[1].TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[6]).FormalParameters));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[6]).FormalParameters.Items[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[6]).FormalParameters.Items[0].TypeName));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[6]).FormalParameters.Items[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[6]).FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[6]).FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[0].TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[6]).FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[1]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[6]).FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[1].TypeTags[0]));

      // Conversion operator declaration
      visitorMock.Setup(v => v.Visit((CastOperatorDeclarationNode)structDeclarationNode.MemberDeclarations[7]));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[7].TypeName));
      visitorMock.Setup(v => v.Visit(structDeclarationNode.MemberDeclarations[7].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[7]).FormalParameters));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[7]).FormalParameters.Items[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[7]).FormalParameters.Items[0].TypeName));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[7]).FormalParameters.Items[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[7]).FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[7]).FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[0].TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[7]).FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[1]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[7]).FormalParameters.Items[0].TypeName.TypeTags[0].Arguments[1].TypeTags[0]));

      // Constructor declaration 
      visitorMock.Setup(v => v.Visit((ConstructorDeclarationNode)structDeclarationNode.MemberDeclarations[8]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[8]).FormalParameters));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[8]).FormalParameters.Items[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[8]).FormalParameters.Items[0].TypeName));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)structDeclarationNode.MemberDeclarations[8]).FormalParameters.Items[0].TypeName.TypeTags[0]));

      // Nested class declaration
      visitorMock.Setup(v => v.Visit((ClassDeclarationNode)structDeclarationNode.NestedTypes[0]));

      // Act
      structDeclarationNode.AcceptVisitor(visitorMock.Object);

      // Verify that all set up expectation were met. Order is not verified, unfortunately.
      visitorMock.VerifyAll();
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
      var interfaceDeclarationNode = (InterfaceDeclarationNode)project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0];

      // Set up expectations on mock
      var visitorMock = new Mock<ISyntaxNodeVisitor>(MockBehavior.Strict);
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode));

      // Interface attributes
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));

      // Type params and base types
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.TypeParameters[0]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.TypeParameters[1]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.BaseTypes[0]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.BaseTypes[0].TypeTags[0]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.TypeParameterConstraints[0]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.TypeParameterConstraints[1]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.TypeParameterConstraints[1].ConstraintTags[0]));

      // Method declaration
      visitorMock.Setup(v => v.Visit((MethodDeclarationNode)interfaceDeclarationNode.MemberDeclarations[0]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.MemberDeclarations[0].AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.MemberDeclarations[0].AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.MemberDeclarations[0].AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.MemberDeclarations[0].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.MemberDeclarations[0].TypeName));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.MemberDeclarations[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((MethodDeclarationNode)interfaceDeclarationNode.MemberDeclarations[0]).FormalParameters));

      // Property declaration
      visitorMock.Setup(v => v.Visit((PropertyDeclarationNode)interfaceDeclarationNode.MemberDeclarations[1]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.MemberDeclarations[1].TypeName));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.MemberDeclarations[1].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((PropertyDeclarationNode)interfaceDeclarationNode.MemberDeclarations[1]).GetAccessor));
      visitorMock.Setup(v => v.Visit(((PropertyDeclarationNode)interfaceDeclarationNode.MemberDeclarations[1]).SetAccessor));

      // Event declaration
      visitorMock.Setup(v => v.Visit((InterfaceEventDeclarationNode)interfaceDeclarationNode.MemberDeclarations[2]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.MemberDeclarations[2].TypeName));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.MemberDeclarations[2].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.MemberDeclarations[2].TypeName.TypeTags[0].Arguments[0]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.MemberDeclarations[2].TypeName.TypeTags[0].Arguments[0].TypeTags[0]));

      // Indexer declaration
      visitorMock.Setup(v => v.Visit((IndexerDeclarationNode)interfaceDeclarationNode.MemberDeclarations[3]));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.MemberDeclarations[3].TypeName));
      visitorMock.Setup(v => v.Visit(interfaceDeclarationNode.MemberDeclarations[3].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((IndexerDeclarationNode)interfaceDeclarationNode.MemberDeclarations[3]).FormalParameters));
      visitorMock.Setup(v => v.Visit(((IndexerDeclarationNode)interfaceDeclarationNode.MemberDeclarations[3]).FormalParameters.Items[0]));
      visitorMock.Setup(v => v.Visit(((IndexerDeclarationNode)interfaceDeclarationNode.MemberDeclarations[3]).FormalParameters.Items[0].TypeName));
      visitorMock.Setup(v => v.Visit(((IndexerDeclarationNode)interfaceDeclarationNode.MemberDeclarations[3]).FormalParameters.Items[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(((PropertyDeclarationNode)interfaceDeclarationNode.MemberDeclarations[3]).GetAccessor));

      // Act
      interfaceDeclarationNode.AcceptVisitor(visitorMock.Object);

      // Verify that all set up expectation were met. Order is not verified, unfortunately.
      visitorMock.VerifyAll();
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
      var enumDeclarationNode = (EnumDeclarationNode)project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0];

      // Set up expectations on mock
      var visitorMock = new Mock<ISyntaxNodeVisitor>(MockBehavior.Strict);
      visitorMock.Setup(v => v.Visit(enumDeclarationNode));

      // Enum attributes
      visitorMock.Setup(v => v.Visit(enumDeclarationNode.AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(enumDeclarationNode.AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(enumDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(enumDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));

      // Base type
      visitorMock.Setup(v => v.Visit(enumDeclarationNode.EnumBase));
      visitorMock.Setup(v => v.Visit(enumDeclarationNode.EnumBase.TypeTags[0]));

      // Enum member declaration
      visitorMock.Setup(v => v.Visit(enumDeclarationNode.Values[0]));
      visitorMock.Setup(v => v.Visit(enumDeclarationNode.Values[0].AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(enumDeclarationNode.Values[0].AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(enumDeclarationNode.Values[0].AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(enumDeclarationNode.Values[0].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(enumDeclarationNode.Values[1]));

      // Act
      enumDeclarationNode.AcceptVisitor(visitorMock.Object);

      // Verify that all set up expectation were met. Order is not verified, unfortunately.
      visitorMock.VerifyAll();
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
      var delegateDeclarationNode = (DelegateDeclarationNode)project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0];

      // Set up expectations on mock
      var visitorMock = new Mock<ISyntaxNodeVisitor>(MockBehavior.Strict);
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode));

      // Delegate attributes
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.AttributeDecorations[0]));
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.AttributeDecorations[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));

      // Return type, type parameters and constraints
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.TypeName));
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.TypeParameters[0]));
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.TypeParameters[1]));
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.TypeParameterConstraints[0]));
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.TypeParameterConstraints[0].ConstraintTags[0]));
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.TypeParameterConstraints[1]));
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.TypeParameterConstraints[1].ConstraintTags[0]));

      // Formal parameters
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.FormalParameters));
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.FormalParameters.Items[0]));
#warning Attributes are missing from FormalParameterNode so these expectations are commented out at the moment
      //visitorMock.Setup(v => v.Visit(delegateDeclarationNode.FormalParameters.Items[0].AttributeDecorations[0]));
      //visitorMock.Setup(v => v.Visit(delegateDeclarationNode.FormalParameters.Items[0].AttributeDecorations[0].Attributes[0]));
      //visitorMock.Setup(v => v.Visit(delegateDeclarationNode.FormalParameters.Items[0].AttributeDecorations[0].Attributes[0].TypeName));
      //visitorMock.Setup(v => v.Visit(delegateDeclarationNode.FormalParameters.Items[0].AttributeDecorations[0].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.FormalParameters.Items[0].TypeName));
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.FormalParameters.Items[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.FormalParameters.Items[1]));
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.FormalParameters.Items[1].TypeName));
      visitorMock.Setup(v => v.Visit(delegateDeclarationNode.FormalParameters.Items[1].TypeName.TypeTags[0]));

      // Act
      delegateDeclarationNode.AcceptVisitor(visitorMock.Object);

      // Verify that all set up expectation were met. Order is not verified, unfortunately.
      visitorMock.VerifyAll();
    }
  }
}
