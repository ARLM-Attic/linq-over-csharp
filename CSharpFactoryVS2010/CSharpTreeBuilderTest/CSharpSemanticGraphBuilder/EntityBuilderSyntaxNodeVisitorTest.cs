using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.Ast;
using System;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the entity builder AST visitor
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class EntityBuilderSyntaxNodeVisitorTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of namespace entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Namespace()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\Namespace.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace;
        (namespaceEntity is RootNamespaceEntity).ShouldBeTrue();
        namespaceEntity.Name.ShouldEqual("global");
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");
        namespaceEntity.Parent.ShouldBeNull();
        namespaceEntity.IsDeclaredInSource.ShouldBeFalse();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(0);
        namespaceEntity.SemanticGraph.ShouldEqual(project.SemanticGraph);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(2);
        namespaceEntity.ChildNamespaces[0].ToString().ShouldEqual("global::A");
        namespaceEntity.ChildNamespaces[1].ToString().ShouldEqual("global::C");
      }
      // namespace global::A
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceEntity.Name.ShouldEqual("A");
        namespaceEntity.FullyQualifiedName.ShouldEqual("A");
        namespaceEntity.Parent.ToString().ShouldEqual("global");
        namespaceEntity.IsDeclaredInSource.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(2);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0]);
        namespaceEntity.SyntaxNodes[1].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[2]);
        namespaceEntity.SemanticGraph.ShouldEqual(project.SemanticGraph);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].ToString().ShouldEqual("global::A.B");
      }
      // namespace global::A.B
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildNamespaces[0];
        namespaceEntity.Name.ShouldEqual("B");
        namespaceEntity.FullyQualifiedName.ShouldEqual("A.B");
        namespaceEntity.Parent.ToString().ShouldEqual("global::A");
        namespaceEntity.IsDeclaredInSource.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(2);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].NamespaceDeclarations[0]);
        namespaceEntity.SyntaxNodes[1].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[2]);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(0);
      }
      // namespace global::C
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[1];
        namespaceEntity.Name.ShouldEqual("C");
        namespaceEntity.FullyQualifiedName.ShouldEqual("C");
        namespaceEntity.Parent.ToString().ShouldEqual("global");
        namespaceEntity.IsDeclaredInSource.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(1);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[1]);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].ToString().ShouldEqual("global::C.D");
      }
      // namespace global::C.D
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0];
        namespaceEntity.Name.ShouldEqual("D");
        namespaceEntity.FullyQualifiedName.ShouldEqual("C.D");
        namespaceEntity.Parent.ToString().ShouldEqual("global::C");
        namespaceEntity.IsDeclaredInSource.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(1);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[1]);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].ToString().ShouldEqual("global::C.D.E");
      }
      // namespace global::C.D.E
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildNamespaces[0];
        namespaceEntity.Name.ShouldEqual("E");
        namespaceEntity.FullyQualifiedName.ShouldEqual("C.D.E");
        namespaceEntity.Parent.ToString().ShouldEqual("global::C.D");
        namespaceEntity.IsDeclaredInSource.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(1);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[1].NamespaceDeclarations[0]);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].ToString().ShouldEqual("global::C.D.E.F");
      }
      // namespace global::C.D.E.F
      {
        var namespaceEntity =
          project.SemanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildNamespaces[0].ChildNamespaces[0];
        namespaceEntity.Name.ShouldEqual("F");
        namespaceEntity.FullyQualifiedName.ShouldEqual("C.D.E.F");
        namespaceEntity.Parent.ToString().ShouldEqual("global::C.D.E");
        namespaceEntity.IsDeclaredInSource.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(1);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[1].NamespaceDeclarations[0]);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(0);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The Program property of a namespace entity cannot be determined and throws an exception.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Error_ProgramOfNamespaceCannotBeDetermined()
    {
      var project = new CSharpProject(WorkingFolder);
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      var program = project.SemanticGraph.GlobalNamespace.Program;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of class entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Class()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\Class.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      var namespaceEntity = project.SemanticGraph.GlobalNamespace;
      namespaceEntity.FullyQualifiedName.ShouldEqual("global");

      var rootTypes = namespaceEntity.ChildTypes.ToList();
      rootTypes.Count.ShouldEqual(7);

      var classA = rootTypes[0] as ClassEntity;

      // class A
      classA.FullyQualifiedName.ShouldEqual("A");
      classA.Name.ShouldEqual("A");
      classA.Parent.ToString().ShouldEqual("global");
      classA.SyntaxNodes.Count.ShouldEqual(1);
      classA.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0]);

      var classATypes = classA.ChildTypes.ToList();
      classATypes.Count.ShouldEqual(7);
      classATypes[0].ToString().ShouldEqual("global::A+B");

      classA.IsGeneric.ShouldBeFalse();
      classA.IsPointerType.ShouldBeFalse();
      classA.IsReferenceType.ShouldBeTrue();
      classA.IsValueType.ShouldBeFalse();

      // base class is not yet resolved, so it's null
      classA.BaseType.ShouldBeNull();

      classA.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Assembly);
      (classA as IMemberEntity).IsNew.ShouldBeFalse();
      classA.IsStatic.ShouldBeFalse();
      classA.IsAbstract.ShouldBeFalse();
      classA.IsSealed.ShouldBeFalse();

      classA.Program.SourceProject.ShouldEqual(project);

      var childTypeCount = 0;

      // class B
      {
        var classEntity = classATypes[childTypeCount++] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("A.B");
        classEntity.Name.ShouldEqual("B");
        classEntity.Parent.ToString().ShouldEqual("global::A");
        classEntity.SyntaxNodes.Count.ShouldEqual(1);
        classEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].NestedDeclarations[0]);

        classEntity.ChildTypes.Count().ShouldEqual(0);

        classEntity.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Private);
      }
      // public class B2
      {
        var classEntity = classATypes[childTypeCount++] as ClassEntity;
        classEntity.Name.ShouldEqual("B2");
        classEntity.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Public);
      }
      // internal class B3
      {
        var classEntity = classATypes[childTypeCount++] as ClassEntity;
        classEntity.Name.ShouldEqual("B3");
        classEntity.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Assembly);
      }
      // protected class B4
      {
        var classEntity = classATypes[childTypeCount++] as ClassEntity;
        classEntity.Name.ShouldEqual("B4");
        classEntity.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Family);
      }
      // protected internal class B5
      {
        var classEntity = classATypes[childTypeCount++] as ClassEntity;
        classEntity.Name.ShouldEqual("B5");
        classEntity.EffectiveAccessibility.ShouldEqual(AccessibilityKind.FamilyOrAssembly);
      }
      // private class B6
      {
        var classEntity = classATypes[childTypeCount++] as ClassEntity;
        classEntity.Name.ShouldEqual("B6");
        classEntity.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Private);
      }
      // new class B7
      {
        var classEntity = classATypes[childTypeCount++] as ClassEntity;
        classEntity.Name.ShouldEqual("B7");
        (classEntity as IMemberEntity).IsNew.ShouldBeTrue();
      }


      // namespace C
      {
        var namespaceC = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceC.FullyQualifiedName.ShouldEqual("C");

        namespaceC.ChildTypes.Count().ShouldEqual(1);
      }
      // namespace C2
      {
        var namespaceC2 = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildNamespaces[0];
        namespaceC2.FullyQualifiedName.ShouldEqual("C.C2");

        namespaceC2.ChildTypes.Count().ShouldEqual(0);
      }
      // class D
      {
        var classD = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes.ToList()[0] as ClassEntity;
        classD.FullyQualifiedName.ShouldEqual("C.D");
        classD.Name.ShouldEqual("D");
        classD.Parent.ToString().ShouldEqual("global::C");
        classD.SyntaxNodes.Count.ShouldEqual(1);
        classD.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0]);

        classD.ChildTypes.Count().ShouldEqual(0);

        classD.BaseTypeReferences.Count().ShouldEqual(1);
        ((TypeNodeBasedTypeEntityReference)classD.BaseTypeReferences.ToArray()[0]).SyntaxNode.TypeName.TypeTags[0].Identifier.ShouldEqual("A");
      }

      var classCounter = 1;

      // public class A2
      {
        var classEntity = rootTypes[classCounter++] as ClassEntity;
        classEntity.Name.ShouldEqual("A2");
        classEntity.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Public);
      }
      // internal class A3
      {
        var classEntity = rootTypes[classCounter++] as ClassEntity;
        classEntity.Name.ShouldEqual("A3");
        classEntity.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Assembly);
      }
      // static class A4
      {
        var classEntity = rootTypes[classCounter++] as ClassEntity;
        classEntity.Name.ShouldEqual("A4");
        classEntity.IsStatic.ShouldBeTrue();
      }
      // abstract class A5
      {
        var classEntity = rootTypes[classCounter++] as ClassEntity;
        classEntity.Name.ShouldEqual("A5");
        classEntity.IsAbstract.ShouldBeTrue();
      }
      // sealed class A6
      {
        var classEntity = rootTypes[classCounter++] as ClassEntity;
        classEntity.Name.ShouldEqual("A6");
        classEntity.IsSealed.ShouldBeTrue();
      }

    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of enum entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Enum()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\Enum.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");
        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].ToString().ShouldEqual("global::A");
      }
      // enum B
      {
        var enumEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes.ToList()[0] as EnumEntity;
        enumEntity.Name.ShouldEqual("B");
        enumEntity.FullyQualifiedName.ShouldEqual("A.B");
        enumEntity.ToString().ShouldEqual("global::A.B");
        enumEntity.Parent.ToString().ShouldEqual("global::A");
        enumEntity.SyntaxNodes.Count.ShouldEqual(1);
        enumEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0]);

        enumEntity.IsPointerType.ShouldBeFalse();
        enumEntity.IsReferenceType.ShouldBeFalse();
        enumEntity.IsValueType.ShouldBeTrue();

        enumEntity.Program.SourceProject.ShouldEqual(project);
        
        enumEntity.BaseTypeReferences.ToList().Count.ShouldEqual(0);
        enumEntity.UnderlyingTypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        ((TypeNodeBasedTypeEntityReference)enumEntity.UnderlyingTypeReference).SyntaxNode.ShouldEqual(
          ((EnumDeclarationNode)project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0]).EnumBase);
      }

      // enum C
      {
        var enumEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes.ToList()[1] as EnumEntity;
        enumEntity.Name.ShouldEqual("C");

        enumEntity.UnderlyingTypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        ((ReflectedTypeBasedTypeEntityReference)enumEntity.UnderlyingTypeReference).Metadata.ShouldEqual(typeof(int));
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of struct entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Struct()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\Struct.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");
      }
      // namespace A
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceEntity.FullyQualifiedName.ShouldEqual("A");
        namespaceEntity.ChildTypes.Count().ShouldEqual(1);
        namespaceEntity.ChildTypes.ToList()[0].ToString().ShouldEqual("global::A.B");
      }
      // struct B
      {
        var structEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes.ToList()[0] as StructEntity;
        structEntity.Name.ShouldEqual("B");
        structEntity.FullyQualifiedName.ShouldEqual("A.B");
        structEntity.ToString().ShouldEqual("global::A.B");
        structEntity.Parent.ToString().ShouldEqual("global::A");
        structEntity.SyntaxNodes.Count.ShouldEqual(1);
        structEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0]);

        structEntity.IsGeneric.ShouldBeFalse();
        structEntity.IsPointerType.ShouldBeFalse();
        structEntity.IsReferenceType.ShouldBeFalse();
        structEntity.IsValueType.ShouldBeTrue();

        structEntity.Program.SourceProject.ShouldEqual(project);

        var baseTypes = structEntity.BaseTypeReferences.ToArray();
        baseTypes.Length.ShouldEqual(1);
        baseTypes[0].ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        ((TypeNodeBasedTypeEntityReference)baseTypes[0]).SyntaxNode.ShouldEqual(
          project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0].BaseTypes[0]);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of interface entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Interface()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\Interface.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");
      }
      // namespace A
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceEntity.FullyQualifiedName.ShouldEqual("A");
        namespaceEntity.ChildTypes.Count().ShouldEqual(1);
        namespaceEntity.ChildTypes.ToList()[0].ToString().ShouldEqual("global::A.B");
      }
      // interface B
      {
        var interfaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes.ToList()[0] as InterfaceEntity;
        interfaceEntity.Name.ShouldEqual("B");
        interfaceEntity.FullyQualifiedName.ShouldEqual("A.B");
        interfaceEntity.ToString().ShouldEqual("global::A.B");
        interfaceEntity.Parent.ToString().ShouldEqual("global::A");
        interfaceEntity.SyntaxNodes.Count.ShouldEqual(1);
        interfaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0]);

        interfaceEntity.IsGeneric.ShouldBeFalse();
        interfaceEntity.IsPointerType.ShouldBeFalse();
        interfaceEntity.IsReferenceType.ShouldBeTrue();
        interfaceEntity.IsValueType.ShouldBeFalse();

        interfaceEntity.Program.SourceProject.ShouldEqual(project);
        
        var baseTypes = interfaceEntity.BaseTypeReferences.ToArray();
        baseTypes.Length.ShouldEqual(1);
        baseTypes[0].ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        ((TypeNodeBasedTypeEntityReference)baseTypes[0]).SyntaxNode.ShouldEqual(
          project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0].BaseTypes[0]);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of delegate entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Delegate()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\Delegate.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");
        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].ToString().ShouldEqual("global::A");
      }
      // namespace A
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceEntity.FullyQualifiedName.ShouldEqual("A");
        namespaceEntity.ChildTypes.Count().ShouldEqual(1);
        namespaceEntity.ChildTypes.ToList()[0].ToString().ShouldEqual("global::A.B");
      }
      // delegate B
      {
        var delegateEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes.ToList()[0] as DelegateEntity;
        delegateEntity.Name.ShouldEqual("B");
        delegateEntity.FullyQualifiedName.ShouldEqual("A.B");
        delegateEntity.ToString().ShouldEqual("global::A.B");
        delegateEntity.Parent.ToString().ShouldEqual("global::A");
        delegateEntity.SyntaxNodes.Count.ShouldEqual(1);
        delegateEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0]);

        delegateEntity.IsPointerType.ShouldBeFalse();
        delegateEntity.IsReferenceType.ShouldBeTrue();
        delegateEntity.IsValueType.ShouldBeFalse();

        delegateEntity.Program.SourceProject.ShouldEqual(project);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of generic class entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void GenericClass()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\GenericClass.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace as NamespaceEntity;
        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].ToString().ShouldEqual("global::N");
      }
      // namespace N
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].ToString().ShouldEqual("global::N.A");
        namespaceEntity.ChildTypes.Count().ShouldEqual(1);
        namespaceEntity.ChildTypes.ToList()[0].ToString().ShouldEqual("global::N.A`2");
      }
      // class A<T1, T2>
      {
        var classEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes.ToList()[0] as ClassEntity;
        classEntity.Name.ShouldEqual("A");
        classEntity.FullyQualifiedName.ShouldEqual("N.A");
        classEntity.ToString().ShouldEqual("global::N.A`2");

        var childTypes = classEntity.ChildTypes.ToList();
        childTypes.Count.ShouldEqual(3);
        childTypes[0].ToString().ShouldEqual("global::N.A`2+B1");
        childTypes[1].ToString().ShouldEqual("global::N.A`2+B2`1");
        childTypes[2].ToString().ShouldEqual("global::N.A`2+B3`1");

        classEntity.OwnTypeParameters.Count.ShouldEqual(2);
        classEntity.OwnTypeParameters[0].ToString().ShouldEqual("global::N.A`2'T1");
        classEntity.OwnTypeParameters[1].ToString().ShouldEqual("global::N.A`2'T2");

        classEntity.IsGeneric.ShouldBeTrue();
        classEntity.OwnTypeParameters.ToArray().Count().ShouldEqual(2);
        var typeParams = classEntity.AllTypeParameters.ToArray();
        typeParams.Length.ShouldEqual(2);

        // type param T1
        {
          var typeParam = typeParams[0];
          typeParam.Name.ShouldEqual("T1");
          typeParam.FullyQualifiedName.ShouldEqual("N.A.T1");
          typeParam.ToString().ShouldEqual("global::N.A`2'T1");
          typeParam.IsPointerType.ShouldBeFalse();
          typeParam.IsReferenceType.ShouldBeFalse();
          typeParam.IsValueType.ShouldBeFalse();
          typeParam.BaseTypeReferences.Count().ShouldEqual(0);
          typeParam.Members.Count().ShouldEqual(0);
          typeParam.Parent.ShouldEqual(classEntity);
          typeParam.SyntaxNodes.Count.ShouldEqual(1);
          typeParam.SyntaxNodes[0].ShouldEqual(
            project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0].TypeParameters[0]);
        }
        // type param T2
        {
          var typeParam = typeParams[1];
          typeParam.FullyQualifiedName.ShouldEqual("N.A.T2");
          typeParam.Parent.ShouldEqual(classEntity);
        }
      }

      var parentClass = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes.ToList()[0] as ClassEntity;
      int child = 0;
      // class B1
      {
        var classEntity = parentClass.ChildTypes.ToList()[child++] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("N.A.B1");
        var ownTypeParams = classEntity.OwnTypeParameters.ToArray();
        ownTypeParams.Length.ShouldEqual(0);
        var typeParams = classEntity.AllTypeParameters.ToArray();
        typeParams.Length.ShouldEqual(2);
        typeParams[0].ToString().ShouldEqual("global::N.A`2'T1");
        typeParams[1].ToString().ShouldEqual("global::N.A`2'T2");
      }
      // class B2<T1>
      {
        var classEntity = parentClass.ChildTypes.ToList()[child++] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("N.A.B2");
        var ownTypeParams = classEntity.OwnTypeParameters.ToArray();
        ownTypeParams.Length.ShouldEqual(1);
        ownTypeParams[0].ToString().ShouldEqual("global::N.A`2+B2`1'T1");
        var typeParams = classEntity.AllTypeParameters.ToArray();
        typeParams.Length.ShouldEqual(3);
        typeParams[0].ToString().ShouldEqual("global::N.A`2'T1");
        typeParams[1].ToString().ShouldEqual("global::N.A`2'T2");
        typeParams[2].ToString().ShouldEqual("global::N.A`2+B2`1'T1");
      }
      // class B3<T3>
      {
        var classEntity = parentClass.ChildTypes.ToList()[child++] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("N.A.B3");
        var ownTypeParams = classEntity.OwnTypeParameters.ToArray();
        ownTypeParams.Length.ShouldEqual(1);
        ownTypeParams[0].ToString().ShouldEqual("global::N.A`2+B3`1'T3");
        var typeParams = classEntity.AllTypeParameters.ToArray();
        typeParams.Length.ShouldEqual(3);
        typeParams[0].ToString().ShouldEqual("global::N.A`2'T1");
        typeParams[1].ToString().ShouldEqual("global::N.A`2'T2");
        typeParams[2].ToString().ShouldEqual("global::N.A`2+B3`1'T3");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of field member entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Field()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\Field.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      // class A
      {
        var classEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0];
        classEntity.FullyQualifiedName.ShouldEqual("A");

        var memberArray = classEntity.Members.ToArray();
        memberArray.Length.ShouldEqual(9);
        memberArray[0].Name.ShouldEqual("a1");
        memberArray[1].Name.ShouldEqual("a2");
        memberArray[2].Name.ShouldEqual("a3");
        memberArray[3].Name.ShouldEqual("a4");
        memberArray[4].Name.ShouldEqual("a5");
        memberArray[5].Name.ShouldEqual("a6");
        memberArray[6].Name.ShouldEqual("a7");
        memberArray[7].Name.ShouldEqual("a8");
        memberArray[8].Name.ShouldEqual("a9");
      }
      
      // A a1, a2;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0].Members.ToArray()[0] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a1");
        fieldEntity.IsDeclaredInSource.ShouldBeTrue();
        fieldEntity.Parent.ToString().ShouldEqual("global::A");
        fieldEntity.SyntaxNodes.Count.ShouldEqual(1);
        var fieldNode = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as FieldDeclarationNode;
        fieldEntity.SyntaxNodes[0].ShouldEqual(fieldNode.FieldTags[0]);
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);

        fieldEntity.IsArray.ShouldBeNull();
        fieldEntity.IsNew.ShouldBeFalse();
        fieldEntity.IsStatic.ShouldBeFalse();
        fieldEntity.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Private);
        fieldEntity.IsInvocable.ShouldBeFalse();
      }
      // A a1, a2;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0].Members.ToArray()[1] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a2");
        fieldEntity.Parent.ToString().ShouldEqual("global::A");
        fieldEntity.SyntaxNodes.Count.ShouldEqual(1);
        var fieldNode = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as FieldDeclarationNode;
        fieldEntity.SyntaxNodes[0].ShouldEqual(fieldNode.FieldTags[1]);
      }
      // static A a3;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0].Members.ToArray()[2] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a3");
        fieldEntity.IsStatic.ShouldBeTrue();
      }
      // new A a4;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0].Members.ToArray()[3] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a4");
        fieldEntity.IsNew.ShouldBeTrue();
      }
      // private A a5;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0].Members.ToArray()[4] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a5");
        fieldEntity.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Private);
      }
      // protected A a6;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0].Members.ToArray()[5] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a6");
        fieldEntity.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Family);
      }
      // internal int a7;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0].Members.ToArray()[6] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a7");
        fieldEntity.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Assembly);
      }
      // protected internal A a8;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0].Members.ToArray()[7] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a8");
        fieldEntity.EffectiveAccessibility.ShouldEqual(AccessibilityKind.FamilyOrAssembly);
      }
      // public int a9;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0].Members.ToArray()[8] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a9");
        fieldEntity.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Public);
      }

      // struct S
      {
        var structEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[1] as StructEntity;
        structEntity.FullyQualifiedName.ShouldEqual("S");

        var memberArray = structEntity.Members.ToArray();
        memberArray.Length.ShouldEqual(1);
        memberArray[0].Name.ShouldEqual("s1");
      }
      // A s1;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[1].Members.ToArray()[0] as FieldEntity;
        fieldEntity.Name.ShouldEqual("s1");
        fieldEntity.IsDeclaredInSource.ShouldBeTrue();
        fieldEntity.IsStatic.ShouldBeFalse();
        fieldEntity.Parent.ToString().ShouldEqual("global::S");
        fieldEntity.SyntaxNodes.Count.ShouldEqual(1);
        var fieldNode = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[1].MemberDeclarations[0] as FieldDeclarationNode;
        fieldEntity.SyntaxNodes[0].ShouldEqual(fieldNode.FieldTags[0]);
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0107: More than one protection modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0107_TooManyProtectionModifier()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS0107_TooManyProtectionModifier.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0107");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of constant member entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ConstantMember()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\ConstantMember.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      // class A
      {
        var classEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0];
        classEntity.FullyQualifiedName.ShouldEqual("A");

        var memberArray = classEntity.Members.ToArray();
        memberArray.Length.ShouldEqual(3);
        memberArray[0].Name.ShouldEqual("a1");
        memberArray[1].Name.ShouldEqual("a2");
        memberArray[2].Name.ShouldEqual("b");
      }

      var constNode = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as ConstDeclarationNode;

      // const int a1 = 1, a2 = 2;
      {
        var constEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0].Members.ToArray()[0] as ConstantMemberEntity;
        constEntity.Name.ShouldEqual("a1");
        constEntity.IsDeclaredInSource.ShouldBeTrue();
        constEntity.IsStatic.ShouldBeTrue();
        constEntity.IsNew.ShouldBeFalse();
        constEntity.IsInvocable.ShouldBeFalse();

        constEntity.Parent.ToString().ShouldEqual("global::A");
        constEntity.SyntaxNodes.Count.ShouldEqual(1);
        constEntity.SyntaxNodes[0].ShouldEqual(constNode.ConstTags[0]);
        constEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        constEntity.InitializerExpression.ShouldNotBeNull();
      }

      {
        var constEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0].Members.ToArray()[1] as ConstantMemberEntity;
        constEntity.Name.ShouldEqual("a2");
        constEntity.Parent.ToString().ShouldEqual("global::A");
        constEntity.SyntaxNodes.Count.ShouldEqual(1);
        constEntity.SyntaxNodes[0].ShouldEqual(constNode.ConstTags[1]);
      }

      // new const int b = 4;
      {
        var constEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0].Members.ToArray()[2] as ConstantMemberEntity;
        constEntity.Name.ShouldEqual("b");
        constEntity.IsNew.ShouldBeTrue();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of using namespace entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingNamespace()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\UsingNamespace.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      var compilationUnitNode = project.SyntaxTree.CompilationUnitNodes[0];
      {
        // global root namespace
        var namespaceEntity = project.SemanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");

        // using A.B;
        var usingNamespaces = namespaceEntity.UsingNamespaces.ToList();
        usingNamespaces.Count.ShouldEqual(1);
        var usingNamespace = usingNamespaces[0];
        usingNamespace.ImportedNamespace.ShouldBeNull();
        usingNamespace.LexicalScope.CompilationUnit.ShouldEqual(compilationUnitNode);
        usingNamespace.LexicalScope.FromSourcePoint.Position.ShouldEqual(compilationUnitNode.StartPosition);
        usingNamespace.LexicalScope.ToSourcePoint.Position.ShouldEqual(compilationUnitNode.EndPosition);
        usingNamespace.NamespaceReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        usingNamespace.NamespaceReference.SyntaxNode.ShouldEqual(compilationUnitNode.UsingNodes[0].NamespaceOrTypeName);
        usingNamespace.Parent.ShouldEqual(namespaceEntity);
        usingNamespace.ReflectedMetadata.ShouldBeNull();
        usingNamespace.SyntaxNodes.Count.ShouldEqual(1);
        usingNamespace.SyntaxNodes[0].ShouldEqual(compilationUnitNode.UsingNodes[0]);
        usingNamespace.Program.SourceProject.ShouldEqual(project);
        
        compilationUnitNode.UsingNodes[0].SemanticEntities.Count.ShouldEqual(1);
        compilationUnitNode.UsingNodes[0].SemanticEntities[0].ShouldEqual(usingNamespace);

        namespaceEntity.GetUsingNamespacesBySourcePoint(new SourcePoint(null, 0)).Count().ShouldEqual(0);
        namespaceEntity.GetUsingNamespacesBySourcePoint(new SourcePoint(compilationUnitNode, 4)).Count().ShouldEqual(1);
      }
      {
        // namespace A
        var namespaceA = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceA.FullyQualifiedName.ShouldEqual("A");
        namespaceA.UsingNamespaces.Count().ShouldEqual(0);

        // namespace B
        var namespaceB = namespaceA.ChildNamespaces[0];
        namespaceB.FullyQualifiedName.ShouldEqual("A.B");
        namespaceB.UsingNamespaces.Count().ShouldEqual(1);

        // using B;
        var usingNamespace = namespaceB.UsingNamespaces.ToList()[0];
        usingNamespace.ImportedNamespace.ShouldBeNull();
        usingNamespace.LexicalScope.CompilationUnit.ShouldEqual(compilationUnitNode);
        usingNamespace.LexicalScope.FromSourcePoint.Position.ShouldEqual(compilationUnitNode.NamespaceDeclarations[0].StartPosition);
        usingNamespace.LexicalScope.ToSourcePoint.Position.ShouldEqual(compilationUnitNode.NamespaceDeclarations[0].EndPosition);
        usingNamespace.NamespaceReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        usingNamespace.NamespaceReference.SyntaxNode.ShouldEqual(compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].NamespaceOrTypeName);
        usingNamespace.Parent.ShouldEqual(namespaceB);
        usingNamespace.ReflectedMetadata.ShouldBeNull();
        usingNamespace.SyntaxNodes.Count.ShouldEqual(1);
        usingNamespace.SyntaxNodes[0].ShouldEqual(compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0]);

        compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].SemanticEntities.Count.ShouldEqual(1);
        compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].SemanticEntities[0].ShouldEqual(usingNamespace);

        namespaceB.GetUsingNamespacesBySourcePoint(new SourcePoint(null, 0)).Count().ShouldEqual(0);
        namespaceB.GetUsingNamespacesBySourcePoint(new SourcePoint(compilationUnitNode,
            compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].StartPosition)).Count().ShouldEqual(1);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0105: The using directive for '{0}' appeared previously in this namespace
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0105_UsingNamespaceDuplicate()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS0105_UsingNamespaceDuplicate.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(0);
      project.Warnings.Count.ShouldEqual(1);
      project.Warnings[0].Code.ShouldEqual("CS0105");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of using alias entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingAlias()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\UsingAlias.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      var compilationUnitNode = project.SyntaxTree.CompilationUnitNodes[0];
      {
        // global root namespace
        var namespaceEntity = project.SemanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");

        // using S = System;
        var usingAliases = namespaceEntity.UsingAliases.ToList();
        usingAliases.Count.ShouldEqual(1);
        var usingAlias = usingAliases[0];
        usingAlias.Alias.ShouldEqual("S");
        usingAlias.AliasedNamespace.ShouldBeNull();
        usingAlias.AliasedType.ShouldBeNull();
        usingAlias.LexicalScope.CompilationUnit.ShouldEqual(compilationUnitNode);
        usingAlias.LexicalScope.FromSourcePoint.Position.ShouldEqual(compilationUnitNode.StartPosition);
        usingAlias.LexicalScope.ToSourcePoint.Position.ShouldEqual(compilationUnitNode.EndPosition);
        usingAlias.NamespaceOrTypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        usingAlias.NamespaceOrTypeReference.SyntaxNode.ShouldEqual(compilationUnitNode.UsingNodes[0].NamespaceOrTypeName);
        usingAlias.Parent.ShouldEqual(namespaceEntity);
        usingAlias.ReflectedMetadata.ShouldBeNull();
        usingAlias.SyntaxNodes.Count.ShouldEqual(1);
        usingAlias.SyntaxNodes[0].ShouldEqual(compilationUnitNode.UsingNodes[0]);
        usingAlias.Program.SourceProject.ShouldEqual(project);

        compilationUnitNode.UsingNodes[0].SemanticEntities.Count.ShouldEqual(1);
        compilationUnitNode.UsingNodes[0].SemanticEntities[0].ShouldEqual(usingAlias);

        namespaceEntity.GetUsingAliasByNameAndSourcePoint("S", new SourcePoint(null, 0)).ShouldBeNull();
        namespaceEntity.GetUsingAliasByNameAndSourcePoint("X", new SourcePoint(compilationUnitNode, 4)).ShouldBeNull();
        namespaceEntity.GetUsingAliasByNameAndSourcePoint("S", new SourcePoint(compilationUnitNode, 4)).ShouldEqual(usingAlias);
      }
      {
        // namespace A
        var namespaceA = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceA.FullyQualifiedName.ShouldEqual("A");
        namespaceA.UsingAliases.Count().ShouldEqual(0);

        // namespace B
        var namespaceB = namespaceA.ChildNamespaces[0];
        namespaceB.FullyQualifiedName.ShouldEqual("A.B");
        namespaceB.UsingAliases.Count().ShouldEqual(1);

        // using B;
        var usingAlias = namespaceB.UsingAliases.ToList()[0];
        usingAlias.Alias.ShouldEqual("E");
        usingAlias.AliasedNamespace.ShouldBeNull();
        usingAlias.AliasedType.ShouldBeNull();
        usingAlias.LexicalScope.CompilationUnit.ShouldEqual(compilationUnitNode);
        usingAlias.LexicalScope.FromSourcePoint.Position.ShouldEqual(compilationUnitNode.NamespaceDeclarations[0].StartPosition);
        usingAlias.LexicalScope.ToSourcePoint.Position.ShouldEqual(compilationUnitNode.NamespaceDeclarations[0].EndPosition);
        usingAlias.NamespaceOrTypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        usingAlias.NamespaceOrTypeReference.SyntaxNode.ShouldEqual(compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].NamespaceOrTypeName);
        usingAlias.Parent.ShouldEqual(namespaceB);
        usingAlias.ReflectedMetadata.ShouldBeNull();
        usingAlias.SyntaxNodes.Count.ShouldEqual(1);
        usingAlias.SyntaxNodes[0].ShouldEqual(compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0]);

        compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].SemanticEntities.Count.ShouldEqual(1);
        compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].SemanticEntities[0].ShouldEqual(usingAlias);

        namespaceB.GetUsingAliasByNameAndSourcePoint("E", new SourcePoint(null, 0)).ShouldBeNull();
        namespaceB.GetUsingAliasByNameAndSourcePoint("X", new SourcePoint(compilationUnitNode,
          compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].StartPosition)).ShouldBeNull();
        namespaceB.GetUsingAliasByNameAndSourcePoint("E", new SourcePoint(compilationUnitNode,
          compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].StartPosition)).ShouldEqual(usingAlias);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1537: The using alias 'alias' appeared previously in this namespace
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS1537_UsingAliasDuplicate()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS1537_UsingAliasDuplicate.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Warnings.Count.ShouldEqual(0);
      project.Errors[0].Code.ShouldEqual("CS1537");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of extern alias entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ExternAlias()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\ExternAlias.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      var compilationUnitNode = project.SyntaxTree.CompilationUnitNodes[0];
      {
        // global root namespace
        var namespaceEntity = project.SemanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");

        // extern alias A;
        var externAliases = namespaceEntity.ExternAliases.ToList();
        externAliases.Count.ShouldEqual(1);
        var externAlias = externAliases[0];
        externAlias.Alias.ShouldEqual("A");
        externAlias.AliasedRootNamespace.ShouldBeNull();
        externAlias.LexicalScope.CompilationUnit.ShouldEqual(compilationUnitNode);
        externAlias.LexicalScope.FromSourcePoint.Position.ShouldEqual(compilationUnitNode.StartPosition);
        externAlias.LexicalScope.ToSourcePoint.Position.ShouldEqual(compilationUnitNode.EndPosition);
        externAlias.RootNamespaceReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        externAlias.RootNamespaceReference.SyntaxNode.ShouldEqual(compilationUnitNode.ExternAliasNodes[0]);
        externAlias.Parent.ShouldEqual(namespaceEntity);
        externAlias.ReflectedMetadata.ShouldBeNull();
        externAlias.SyntaxNodes.Count.ShouldEqual(1);
        externAlias.SyntaxNodes[0].ShouldEqual(compilationUnitNode.ExternAliasNodes[0]);
        externAlias.Program.SourceProject.ShouldEqual(project);

        compilationUnitNode.ExternAliasNodes[0].SemanticEntities.Count.ShouldEqual(1);
        compilationUnitNode.ExternAliasNodes[0].SemanticEntities[0].ShouldEqual(externAlias);

        namespaceEntity.GetExternAliasByNameAndSourcePoint("A", new SourcePoint(null, 0)).ShouldBeNull();
        namespaceEntity.GetExternAliasByNameAndSourcePoint("X", new SourcePoint(compilationUnitNode, 4)).ShouldBeNull();
        namespaceEntity.GetExternAliasByNameAndSourcePoint("A", new SourcePoint(compilationUnitNode, 4)).ShouldEqual(
          externAlias);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1537: The using alias 'alias' appeared previously in this namespace (for extern alias)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS1537_ExternAliasDuplicateName()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS1537_ExternAliasDuplicateName.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Warnings.Count.ShouldEqual(0);
      project.Errors[0].Code.ShouldEqual("CS1537");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1537: conflicting extern and using alias names
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS1537_ExternAndUsingAliasDuplicateName()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS1537_ExternAndUsingAliasDuplicateName.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Warnings.Count.ShouldEqual(0);
      project.Errors[0].Code.ShouldEqual("CS1537");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of enum members
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void EnumMember()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\EnumMember.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      var enumDeclarationNode = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0] as EnumDeclarationNode;

      var enumEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0] as EnumEntity;
      var members = enumEntity.Members.ToList();
      members.Count.ShouldEqual(2);

      int i = 0;
      {
        var enumMember = enumEntity.Members.ToList()[i] as EnumMemberEntity;
        enumMember.Name.ShouldEqual(enumDeclarationNode.Values[i].Identifier);
        enumMember.IsDeclaredInSource.ShouldBeTrue();
        enumMember.IsStatic.ShouldBeTrue();
        enumMember.IsNew.ShouldBeFalse();
        enumMember.IsInvocable.ShouldBeFalse();
        enumMember.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Public);
        enumMember.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0]);
        enumMember.ReflectedMetadata.ShouldBeNull();
        enumMember.SyntaxNodes[0].ShouldEqual(enumDeclarationNode.Values[i]);
        enumMember.TypeReference.ShouldEqual(((EnumEntity)project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0]).UnderlyingTypeReference);
      }
    }


    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of property member entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Property()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\Property.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      {
        // class A
        var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");

        // class A declaration
        var classNode = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0] as ClassDeclarationNode;
        var propertyNode = classNode.MemberDeclarations[0] as PropertyDeclarationNode;

        // Check the number of properties and auto-created backing fields
        var properties = classEntity.Members.Where(x => x is PropertyEntity).Cast<PropertyEntity>().ToList();
        properties.Count.ShouldEqual(6);

        var propertyCounter = 0;

        // int B
        {
          var property = properties[propertyCounter++];
          property.Name.ShouldEqual("B");
          property.IsAutoImplemented.ShouldBeFalse();
          property.AutoImplementedField.ShouldBeNull();
          property.IsDeclaredInSource.ShouldBeTrue();
          property.Parent.ShouldEqual(classEntity);
          property.SyntaxNodes[0].ShouldEqual(propertyNode);
          property.TypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
          property.DeclaredAccessibility.ShouldEqual(AccessibilityKind.Family);

          property.IsStatic.ShouldBeFalse();
          property.IsNew.ShouldBeFalse();
          property.IsOverride.ShouldBeFalse();
          property.IsVirtual.ShouldBeFalse();
          property.IsInvocable.ShouldBeFalse();

          property.GetAccessor.ShouldNotBeNull();
          property.GetAccessor.SyntaxNodes[0].ShouldEqual(propertyNode.GetAccessor);
          property.GetAccessor.Parent.ShouldEqual(property);
          property.GetAccessor.DeclaredAccessibility.ShouldBeNull();
          property.GetAccessor.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Family);

          property.SetAccessor.ShouldNotBeNull();
          property.SetAccessor.SyntaxNodes[0].ShouldEqual(propertyNode.SetAccessor);
          property.SetAccessor.Parent.ShouldEqual(property);
          property.SetAccessor.DeclaredAccessibility.ShouldEqual(AccessibilityKind.Private);
          property.SetAccessor.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Private);

          var accessors = property.Accessors.ToList();
          accessors.Count.ShouldEqual(2);
          accessors[0].ShouldEqual(property.GetAccessor);
          accessors[1].ShouldEqual(property.SetAccessor);
        }
        // protected int C { get; private set; }
        {
          var property = properties[propertyCounter++];
          property.Name.ShouldEqual("C");
          property.IsAutoImplemented.ShouldBeTrue();
          property.AutoImplementedField.IsDeclaredInSource.ShouldBeFalse();
          property.AutoImplementedField.IsStatic.ShouldBeFalse();
          // The name of the auto-implemented field is a guid, so we just check that it's not null.
          property.AutoImplementedField.Name.ShouldNotBeNull();
          property.AutoImplementedField.Parent.ShouldEqual(property);
          property.AutoImplementedField.SyntaxNodes.Count.ShouldEqual(0);
          property.AutoImplementedField.TypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        }
        // static int D { get; set; }
        {
          var property = properties[propertyCounter++];
          property.Name.ShouldEqual("D");
          property.IsStatic.ShouldBeTrue();
          property.AutoImplementedField.IsStatic.ShouldBeTrue();
        }
        // public virtual int E { get; set; }
        {
          var property = properties[propertyCounter++];
          property.Name.ShouldEqual("E");
          property.IsVirtual.ShouldBeTrue();
        }
        // public override int F { get; set; }
        {
          var property = properties[propertyCounter++];
          property.Name.ShouldEqual("F");
          property.IsOverride.ShouldBeTrue();
        }
        // public new int G { get; set; }
        {
          var property = properties[propertyCounter++];
          property.Name.ShouldEqual("G");
          property.IsNew.ShouldBeTrue();
        }
      }

      {
        // interface I
        var interfaceEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<InterfaceEntity>("I");

        // Check the number of properties and auto-created backing fields
        var properties = interfaceEntity.Members.Where(x => x is PropertyEntity).Cast<PropertyEntity>().ToList();
        properties.Count.ShouldEqual(1);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of method member entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Method()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\Method.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      {
        // class C
        var classEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0];
        classEntity.FullyQualifiedName.ShouldEqual("C");

        // class C declaration
        var classNode = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0] as ClassDeclarationNode;

        int i = 0;

        //  void A() { }
        {
          var method = classEntity.Members.ToList()[i] as MethodEntity;
          method.Name.ShouldEqual("A");
          
          method.IsAbstract.ShouldBeFalse();
          method.IsDeclaredInSource.ShouldBeTrue();
          method.IsGeneric.ShouldBeFalse();
          method.IsPartial.ShouldBeFalse();
          method.IsStatic.ShouldBeFalse();
          method.IsNew.ShouldBeFalse();
          method.IsOverride.ShouldBeFalse();
          method.IsVirtual.ShouldBeFalse();
          method.IsInvocable.ShouldBeTrue();

          method.Parent.ShouldEqual(classEntity);
          method.SyntaxNodes[0].ShouldEqual(classNode.MemberDeclarations[i]);
          method.ReturnTypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
          method.ReturnType.ShouldBeNull();
          method.AllTypeParameters.Count.ShouldEqual(0);
          method.OwnTypeParameterCount.ShouldEqual(0);
          method.Parameters.Count().ShouldEqual(0);
          method.Signature.ToString().ShouldEqual("A()");

          method.Program.SourceProject.ShouldEqual(project);
        }

        i++;

        //  void A<T1, T2>(long a, ref string b, out float c) 
        {
          var method = classEntity.Members.ToList()[i] as MethodEntity;
          method.Name.ShouldEqual("A");
          method.IsAbstract.ShouldBeFalse();
          method.IsDeclaredInSource.ShouldBeTrue();
          method.IsGeneric.ShouldBeTrue();
          method.IsPartial.ShouldBeFalse();
          method.IsStatic.ShouldBeFalse();
          method.Parent.ShouldEqual(classEntity);
          method.SyntaxNodes[0].ShouldEqual(classNode.MemberDeclarations[i]);
          method.ReturnTypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
          method.ReturnType.ShouldBeNull();
          method.AllTypeParameters.Count.ShouldEqual(2);
          method.OwnTypeParameterCount.ShouldEqual(2);
          method.Parameters.Count().ShouldEqual(3);
          method.Signature.ToString().ShouldEqual("A`2(?, ref ?, out ?)");

          var parameters = method.Parameters.ToList();
          var methodDeclaration = classNode.MemberDeclarations[i] as MethodDeclarationNode;
          int j = 0;

          parameters[j].Name.ShouldEqual("a");
          parameters[j].Kind.ShouldEqual(ParameterKind.Value);
          parameters[j].Parent.ShouldEqual(method);
          parameters[j].SyntaxNodes.Count.ShouldEqual(1);
          parameters[j].SyntaxNodes[0].ShouldEqual(methodDeclaration.FormalParameters[j]);
          parameters[j].TypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
          parameters[j].Type.ShouldBeNull();

          j++;

          parameters[j].Name.ShouldEqual("b");
          parameters[j].Kind.ShouldEqual(ParameterKind.Reference);
          parameters[j].Parent.ShouldEqual(method);
          parameters[j].SyntaxNodes.Count.ShouldEqual(1);
          parameters[j].SyntaxNodes[0].ShouldEqual(methodDeclaration.FormalParameters[j]);
          parameters[j].TypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
          parameters[j].Type.ShouldBeNull();

          j++;

          parameters[j].Name.ShouldEqual("c");
          parameters[j].Kind.ShouldEqual(ParameterKind.Output);
          parameters[j].Parent.ShouldEqual(method);
          parameters[j].SyntaxNodes.Count.ShouldEqual(1);
          parameters[j].SyntaxNodes[0].ShouldEqual(methodDeclaration.FormalParameters[j]);
          parameters[j].TypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
          parameters[j].Type.ShouldBeNull();
        }

        i++;

        // static void A2() { }
        {
          var method = classEntity.Members.ToList()[i] as MethodEntity;
          method.Name.ShouldEqual("A2");
          method.IsStatic.ShouldBeTrue();
        }

        i++;

        // public abstract void A3();
        {
          var method = classEntity.Members.ToList()[i] as MethodEntity;
          method.Name.ShouldEqual("A3");
          method.IsAbstract.ShouldBeTrue();
        }

        i++;
        // public virtual void A4() {}
        {
          var method = classEntity.Members.ToList()[i] as MethodEntity;
          method.Name.ShouldEqual("A4");
          method.IsVirtual.ShouldBeTrue();
        }
        
        i++;
        // public override void A5() {}
        {
          var method = classEntity.Members.ToList()[i] as MethodEntity;
          method.Name.ShouldEqual("A5");
          method.IsOverride.ShouldBeTrue();
        }

        i++;
        // public new void A6() {}
        {
          var method = classEntity.Members.ToList()[i] as MethodEntity;
          method.Name.ShouldEqual("A6");
          method.IsNew.ShouldBeTrue();
        }
      }

      {
        // class C2<T>
        var classEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[1];
        classEntity.FullyQualifiedName.ShouldEqual("C2");

        // class C2<T> declaration
        var classNode = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[1] as ClassDeclarationNode;

        int i = 0;

        //  void B<T1, T2>() {}
        {
          var method = classEntity.Members.ToList()[i] as MethodEntity;
          method.Name.ShouldEqual("B");
          method.IsAbstract.ShouldBeFalse();
          method.IsDeclaredInSource.ShouldBeTrue();
          method.IsGeneric.ShouldBeTrue();
          method.IsPartial.ShouldBeFalse();
          method.IsStatic.ShouldBeFalse();
          method.Parent.ShouldEqual(classEntity);
          method.SyntaxNodes[0].ShouldEqual(classNode.MemberDeclarations[i]);
          method.ReturnTypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
          method.ReturnType.ShouldBeNull();
          method.AllTypeParameters.Count.ShouldEqual(3);
          method.OwnTypeParameterCount.ShouldEqual(2);
          method.Parameters.Count().ShouldEqual(0);
          method.Signature.ToString().ShouldEqual("B`2()");

          var typeParameters = method.AllTypeParameters.ToList();
          typeParameters[0].ToString().ShouldEqual("global::C2`1'T");
          typeParameters[1].ToString().ShouldEqual("T1");
          typeParameters[2].ToString().ShouldEqual("T2");
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of literal entities.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Literal()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\Literal.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      // class A
      var classEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0];
      var members = classEntity.Members.ToList();

      int i = 0;

      // object a0 = null;
      {
        var initializer = (members[i++] as FieldEntity).Initializer as ScalarInitializerEntity;
        var expression = initializer.Expression;
        (expression is NullLiteralExpressionEntity).ShouldBeTrue();
        expression.Parent.ShouldEqual(initializer);
      }
      // bool a1 = true;
      {
        var initializer = (members[i++] as FieldEntity).Initializer as ScalarInitializerEntity;
        var expression = initializer.Expression;
        var literal = expression as TypedLiteralExpressionEntity;
        literal.Value.ShouldEqual(true);
        (literal.TypeReference as ReflectedTypeBasedTypeEntityReference).Metadata.ShouldEqual(typeof(bool));
        expression.Parent.ShouldEqual(initializer);
      }
      // decimal a2 = 2m;
      {
        var initializer = (members[i++] as FieldEntity).Initializer as ScalarInitializerEntity;
        var expression = initializer.Expression;
        var literal = expression as TypedLiteralExpressionEntity;
        literal.Value.ShouldEqual(2m);
        (literal.TypeReference as ReflectedTypeBasedTypeEntityReference).Metadata.ShouldEqual(typeof(decimal));
        expression.Parent.ShouldEqual(initializer);
      }
      // int a3 = 3;
      {
        var initializer = (members[i++] as FieldEntity).Initializer as ScalarInitializerEntity;
        var expression = initializer.Expression;
        var literal = expression as TypedLiteralExpressionEntity;
        literal.Value.ShouldEqual(3);
        (literal.TypeReference as ReflectedTypeBasedTypeEntityReference).Metadata.ShouldEqual(typeof(int));
        expression.Parent.ShouldEqual(initializer);
      }
      // uint a4 = 4u;
      {
        var initializer = (members[i++] as FieldEntity).Initializer as ScalarInitializerEntity;
        var expression = initializer.Expression;
        var literal = expression as TypedLiteralExpressionEntity;
        literal.Value.ShouldEqual(4u);
        (literal.TypeReference as ReflectedTypeBasedTypeEntityReference).Metadata.ShouldEqual(typeof(uint));
        expression.Parent.ShouldEqual(initializer);
      }
      // long a5 = 5l;
      {
        var initializer = (members[i++] as FieldEntity).Initializer as ScalarInitializerEntity;
        var expression = initializer.Expression;
        var literal = expression as TypedLiteralExpressionEntity;
        literal.Value.ShouldEqual(5L);
        (literal.TypeReference as ReflectedTypeBasedTypeEntityReference).Metadata.ShouldEqual(typeof(long));
        expression.Parent.ShouldEqual(initializer);
      }
      // ulong a6 = 6ul;
      {
        var initializer = (members[i++] as FieldEntity).Initializer as ScalarInitializerEntity;
        var expression = initializer.Expression;
        var literal = expression as TypedLiteralExpressionEntity;
        literal.Value.ShouldEqual(6ul);
        (literal.TypeReference as ReflectedTypeBasedTypeEntityReference).Metadata.ShouldEqual(typeof(ulong));
        expression.Parent.ShouldEqual(initializer);
      }
      // char a7 = '7';
      {
        var initializer = (members[i++] as FieldEntity).Initializer as ScalarInitializerEntity;
        var expression = initializer.Expression;
        var literal = expression as TypedLiteralExpressionEntity;
        literal.Value.ShouldEqual('7');
        (literal.TypeReference as ReflectedTypeBasedTypeEntityReference).Metadata.ShouldEqual(typeof(char));
        expression.Parent.ShouldEqual(initializer);
      }
      // float a8 = 8f;
      {
        var initializer = (members[i++] as FieldEntity).Initializer as ScalarInitializerEntity;
        var expression = initializer.Expression;
        var literal = expression as TypedLiteralExpressionEntity;
        literal.Value.ShouldEqual(8f);
        (literal.TypeReference as ReflectedTypeBasedTypeEntityReference).Metadata.ShouldEqual(typeof(float));
        expression.Parent.ShouldEqual(initializer);
      }
      // double a9 = 9d;
      {
        var initializer = (members[i++] as FieldEntity).Initializer as ScalarInitializerEntity;
        var expression = initializer.Expression;
        var literal = expression as TypedLiteralExpressionEntity;
        literal.Value.ShouldEqual(9d);
        (literal.TypeReference as ReflectedTypeBasedTypeEntityReference).Metadata.ShouldEqual(typeof(double));
        expression.Parent.ShouldEqual(initializer);
      }
      // string a10 = "10";
      {
        var initializer = (members[i++] as FieldEntity).Initializer as ScalarInitializerEntity;
        var expression = initializer.Expression;
        var literal = expression as TypedLiteralExpressionEntity;
        literal.Value.ShouldEqual("10");
        (literal.TypeReference as ReflectedTypeBasedTypeEntityReference).Metadata.ShouldEqual(typeof(string));
        expression.Parent.ShouldEqual(initializer);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of a simple name entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void SimpleName()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\SimpleName.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      // class A
      var classEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0];
      var members = classEntity.Members.ToList();

      int i = 0;

      // static int a = b;
      {
        var initializer = (members[i++] as FieldEntity).Initializer as ScalarInitializerEntity;
        var expression = initializer.Expression;
        var simpleName = expression as SimpleNameExpressionEntity;
        simpleName.EntityReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        expression.Parent.ShouldEqual(initializer);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of a default value entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void DefaultValue()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\DefaultValue.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      // class A
      var classEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0];
      var members = classEntity.Members.ToList();

      int i = 0;

      // A a = default(A);
      {
        var initializer = (members[i++] as FieldEntity).Initializer as ScalarInitializerEntity;
        var expression = initializer.Expression;
        var defaultValueExpressionEntity = expression as DefaultValueExpressionEntity;
        var typeReference = defaultValueExpressionEntity.TypeReference as TypeNodeBasedTypeEntityReference;
        typeReference.SyntaxNode.ToString().ShouldEqual("A");
        defaultValueExpressionEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        expression.Parent.ShouldEqual(initializer);
        expression.Program.SourceProject.ShouldEqual(project);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of type parameters and constraints.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void TypeParameter()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\TypeParameter.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      // class A
      var classEntity = project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[0] as ClassEntity;

      int i = 0;

      // where T1 : B, T2, I1, I2, T4, new()
      {
        var typeParameter = classEntity.OwnTypeParameters[i++];
        typeParameter.HasDefaultConstructorConstraint.ShouldBeTrue();
        typeParameter.HasReferenceTypeConstraint.ShouldBeFalse();
        typeParameter.HasNonNullableValueTypeConstraint.ShouldBeFalse();
        typeParameter.TypeReferenceConstraints.Count().ShouldEqual(5);
        typeParameter.ClassTypeConstraint.ShouldBeNull();
        typeParameter.ClassTypeConstraints.Count().ShouldEqual(0);
        typeParameter.InterfaceTypeConstraints.Count().ShouldEqual(0);
        typeParameter.TypeParameterConstraints.Count().ShouldEqual(0);
      }
      // where T2 : class
      {
        var typeParameter = classEntity.OwnTypeParameters[i++];
        typeParameter.HasDefaultConstructorConstraint.ShouldBeFalse();
        typeParameter.HasReferenceTypeConstraint.ShouldBeTrue();
        typeParameter.HasNonNullableValueTypeConstraint.ShouldBeFalse();
        typeParameter.TypeReferenceConstraints.Count().ShouldEqual(0);
        typeParameter.ClassTypeConstraint.ShouldBeNull();
        typeParameter.ClassTypeConstraints.Count().ShouldEqual(0);
        typeParameter.InterfaceTypeConstraints.Count().ShouldEqual(0);
        typeParameter.TypeParameterConstraints.Count().ShouldEqual(0);
      }
      // where T3 : struct
      {
        var typeParameter = classEntity.OwnTypeParameters[i++];
        typeParameter.HasDefaultConstructorConstraint.ShouldBeTrue();
        typeParameter.HasReferenceTypeConstraint.ShouldBeFalse();
        typeParameter.HasNonNullableValueTypeConstraint.ShouldBeTrue();
        typeParameter.TypeReferenceConstraints.Count().ShouldEqual(0);
        typeParameter.ClassTypeConstraint.ShouldBeNull();
        typeParameter.ClassTypeConstraints.Count().ShouldEqual(0);
        typeParameter.InterfaceTypeConstraints.Count().ShouldEqual(0);
        typeParameter.TypeParameterConstraints.Count().ShouldEqual(0);
      }
    }

  }
}