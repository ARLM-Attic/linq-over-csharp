using System.Linq;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;

namespace CSharpTreeBuilderTest.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// Tests the copy constructor of certain SemanticEntity-descendant classes.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public sealed class CopyConstructorTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the copy constructor of ClassEntity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ClassEntity()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"CopyConstructor\ClassEntity.cs");
      InvokeParser(project).ShouldBeTrue();

      var classB = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("B",1);
      var cloneB = (ClassEntity)classB.Clone();

      cloneB.AllTypeParameters.Count().ShouldEqual(classB.AllTypeParameters.Count());
      cloneB.BaseInterfaces.Count.ShouldEqual(classB.BaseInterfaces.Count);
      cloneB.BaseType.ShouldEqual(classB.BaseType);
      cloneB.BaseTypeCount.ShouldEqual(classB.BaseTypeCount);
      cloneB.BaseTypeReferences.Count().ShouldEqual(classB.BaseTypeReferences.Count());
      cloneB.BuiltInTypeValue.ShouldEqual(classB.BuiltInTypeValue);
      cloneB.ChildTypes.Count().ShouldEqual(classB.ChildTypes.Count());
      cloneB.DeclaredAccessibility.ShouldEqual(classB.DeclaredAccessibility);
      cloneB.EffectiveAccessibility.ShouldEqual(classB.EffectiveAccessibility);
      cloneB.FullyQualifiedName.ShouldEqual(classB.FullyQualifiedName);
      cloneB.IsAbstract.ShouldEqual(classB.IsAbstract);
      cloneB.IsPartial.ShouldEqual(classB.IsPartial);
      cloneB.IsSealed.ShouldEqual(classB.IsSealed);
      cloneB.IsStatic.ShouldEqual(classB.IsStatic);
      cloneB.Members.Count().ShouldEqual(classB.Members.Count());
      cloneB.MetadataImporterFactory.ShouldEqual(classB.MetadataImporterFactory);
      cloneB.Name.ShouldEqual(classB.Name);
      cloneB.OwnTypeParameterCount.ShouldEqual(classB.OwnTypeParameterCount);
      cloneB.Parent.ShouldEqual(classB.Parent);
      cloneB.PointerType.ShouldEqual(classB.PointerType);
      cloneB.Program.ShouldEqual(classB.Program);
      cloneB.ReflectedMetadata.ShouldEqual(classB.ReflectedMetadata);
      cloneB.RootNamespace.ShouldEqual(classB.RootNamespace);
      cloneB.SemanticGraph.ShouldEqual(classB.SemanticGraph);
      cloneB.SyntaxNodes.Count.ShouldEqual(classB.SyntaxNodes.Count);
      cloneB.TypeParameterMap.ShouldEqual(classB.TypeParameterMap);
      cloneB.UnderlyingOfNullableType.ShouldEqual(classB.UnderlyingOfNullableType);

      var methodMA = classB.GetMethod("MA", 1, null);
      var cloneMA = cloneB.GetMethod("MA", 1, null);

      cloneMA.AllTypeParameters.Count().ShouldEqual(methodMA.AllTypeParameters.Count());
      cloneMA.DeclaredAccessibility.ShouldEqual(methodMA.DeclaredAccessibility);
      cloneMA.FullyQualifiedName.ShouldEqual(methodMA.FullyQualifiedName);
      cloneMA.Interface.ShouldEqual(methodMA.Interface);
      cloneMA.InterfaceReference.ShouldEqual(methodMA.InterfaceReference);
      cloneMA.IsAbstract.ShouldEqual(methodMA.IsAbstract);
      cloneMA.IsDeclaredInSource.ShouldEqual(methodMA.IsDeclaredInSource);
      cloneMA.IsExplicitlyImplemented.ShouldEqual(methodMA.IsExplicitlyImplemented);
      cloneMA.IsGeneric.ShouldEqual(methodMA.IsGeneric);
      cloneMA.IsNew.ShouldEqual(methodMA.IsNew);
      cloneMA.IsOverride.ShouldEqual(methodMA.IsOverride);
      cloneMA.IsPartial.ShouldEqual(methodMA.IsPartial);
      cloneMA.IsSealed.ShouldEqual(methodMA.IsSealed);
      cloneMA.IsStatic.ShouldEqual(methodMA.IsStatic);
      cloneMA.IsVirtual.ShouldEqual(methodMA.IsVirtual);
      cloneMA.Name.ShouldEqual(methodMA.Name);
      cloneMA.OwnTypeParameterCount.ShouldEqual(methodMA.OwnTypeParameterCount);
      cloneMA.Parameters.Count().ShouldEqual(methodMA.Parameters.Count());
      cloneMA.Parent.ShouldEqual(cloneB);
      cloneMA.Program.ShouldEqual(methodMA.Program);
      cloneMA.ReflectedMetadata.ShouldEqual(methodMA.ReflectedMetadata);
      cloneMA.ReturnType.ShouldEqual(methodMA.ReturnType);
      cloneMA.ReturnTypeReference.ShouldEqual(methodMA.ReturnTypeReference);
    }
  }
}
