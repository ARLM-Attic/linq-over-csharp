using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;

namespace CSharpTreeBuilderTest
{
  class Orphan : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0101: The namespace 'A' already contains a definition for 'B' (namespace and class)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0101_ClassAndNamespaceSameName() 
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS0101_ClassAndNamespaceSameName.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0101");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0101: The namespace 'A' already contains a definition for 'B' (class and struct)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0101_ClassAndStructSameName()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS0101_ClassAndStructSameName.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0101");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0101: The namespace 'A' already contains a definition for 'B' (class and class - no partial)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0101_ClassAndClassSameName()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS0101_ClassAndClassSameName.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0101");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0102: The type 'A' already contains a definition for 'a1'
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0102_TypeAlreadyContainsADefinition()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS0102_TypeAlreadyContainsADefinition.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0102");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0260: Missing partial modifier on declaration (missing partial on 1st declaration)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0260_MissingPartialOnClass1()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS0260_MissingPartialOnClass1.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0260");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0260: Missing partial modifier on declaration (missing partial on 2nd declaration)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0260_MissingPartialOnClass2()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS0260_MissingPartialOnClass2.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0260");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0542: 'A': member names cannot be the same as their enclosing type
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0542_ClassOrStructAndMemberName()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS0542_ClassOrStructAndMemberName.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(6);
      foreach (var error in project.Errors)
      {
        error.Code.ShouldEqual("CS0542");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0752: A partial method cannot have out parameters
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0752_PartialMethodCannotHaveOutParameter()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS0752_PartialMethodCannotHaveOutParameter.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0752");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of partial methods
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PartialMethod()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\PartialMethod.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A1");
      {
        var method = classEntity.Members.ToList()[0] as MethodEntity;
        method.IsPartial.ShouldBeTrue();
        method.IsAbstract.ShouldBeTrue();

        method.IsGeneric.ShouldBeTrue();
        method.OwnTypeParameterCount.ShouldEqual(1);
        method.GetOwnTypeParameterByName("T1").ShouldNotBeNull();

        var parameters = method.Parameters.ToList();
        parameters.Count.ShouldEqual(1);
        parameters[0].Name.ShouldEqual("a");
        var typeref = parameters[0].TypeReference as TypeNodeBasedTypeEntityReference;
        typeref.SyntaxNode.TypeName.TypeTags[0].Identifier.ShouldEqual("T1");
      }

      {
        var method = classEntity.Members.ToList()[1] as MethodEntity;
        method.IsPartial.ShouldBeTrue();
        method.IsAbstract.ShouldBeFalse();

        method.IsGeneric.ShouldBeTrue();
        method.OwnTypeParameterCount.ShouldEqual(1);
        method.GetOwnTypeParameterByName("T2").ShouldNotBeNull();

        var parameters = method.Parameters.ToList();
        parameters.Count.ShouldEqual(1);
        parameters[0].Name.ShouldEqual("b");
        var typeref = parameters[0].TypeReference as TypeNodeBasedTypeEntityReference;
        typeref.SyntaxNode.TypeName.TypeTags[0].Identifier.ShouldEqual("T2");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Partial class: if the base type is specified multiple times then only one instance is kept.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PartialClassDuplicateBaseTypes()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\PartialClassDuplicateBaseTypes.cs");
      InvokeParser(project).ShouldBeTrue();

      var classEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0] as ClassEntity;
      var baseTypeRefs = classEntity.BaseTypeReferences.ToList();
      baseTypeRefs.Count.ShouldEqual(1);
      classEntity.BaseType.FullyQualifiedName.ShouldEqual("B");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Partial class: if a base interface is specified multiple times then only one instance is kept.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PartialClassDuplicateBaseInterfaces()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\PartialClassDuplicateBaseInterfaces.cs");
      InvokeParser(project).ShouldBeTrue();

      var classEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0] as ClassEntity;
      classEntity.BaseInterfaces.Count.ShouldEqual(3);
      classEntity.BaseInterfaces[0].FullyQualifiedName.ShouldEqual("I1");
      classEntity.BaseInterfaces[1].FullyQualifiedName.ShouldEqual("I2");
      classEntity.BaseInterfaces[2].FullyQualifiedName.ShouldEqual("I3");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0263: Partial declarations of 'A' must not specify different base classes
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0263_PartialClassConflictingBaseTypes()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0263_PartialClassConflictingBaseTypes.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0263");
      project.Warnings.Count.ShouldEqual(0);
    }
  }
}
