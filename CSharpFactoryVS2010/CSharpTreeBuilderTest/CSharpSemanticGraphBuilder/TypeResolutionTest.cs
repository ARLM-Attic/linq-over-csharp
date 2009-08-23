using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the type resolution logic.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class TypeResolutionTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// No type arguments (§4.4.1) can be present in a namespace-name (only types can have type arguments).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Error_NoTypeArgumentsCanBePresentInANamespaceName()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\Error_NoTypeArgumentsCanBePresentInANamespaceName.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("TBD001");
      project.Warnings.Count.ShouldEqual(0);

      project.SemanticGraph.GlobalNamespace.UsingNamespaces.ToArray()[0].NamespaceReference.ResolutionState.ShouldEqual(ResolutionState.Unresolvable);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving a type to type parameter.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void TypeParameter()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\TypeParameter.cs");
      InvokeParser(project).ShouldBeTrue();

      var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
      fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      fieldEntity.Type.TargetEntity.FullyQualifiedName.ShouldEqual("A`1.T1");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of single-tag namespace names in using namespace entities.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingNamespaceNames_SingleTag()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\UsingNamespaceNames_SingleTag.cs");
      InvokeParser(project).ShouldBeTrue();

      var global = project.SemanticGraph.GlobalNamespace;
      var namespaceA = global.GetChildNamespaceByName("A");
      var namespaceB = namespaceA.GetChildNamespaceByName("B");

      // using A;
      {
        var usingNamespace = global.UsingNamespaces.ToArray()[0];
        usingNamespace.NamespaceReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        usingNamespace.NamespaceReference.TargetEntity.ShouldEqual(namespaceA);
        usingNamespace.ImportedNamespace.ShouldEqual(namespaceA);
      }
      // using B;
      {
        var usingNamespace = namespaceA.UsingNamespaces.ToArray()[0];
        usingNamespace.NamespaceReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        usingNamespace.NamespaceReference.TargetEntity.ShouldEqual(namespaceB);
        usingNamespace.ImportedNamespace.ShouldEqual(namespaceB);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0576: Namespace 'namespace' contains a definition conflicting with alias 'identifier'
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0576_UsingAliasConflictsWithNamespaceDeclaration()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0576_UsingAliasConflictsWithNamespaceDeclaration.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0576");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0576: Namespace 'namespace' contains a definition conflicting with alias 'identifier'
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0576_UsingAliasConflictsWithTypeDeclaration()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0576_UsingAliasConflictsWithTypeDeclaration.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0576");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0246: The type or namespace name 'A' could not be found (are you missing a using directive or an assembly reference?)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0246_NamespaceNameCouldNotBeFound()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0246_NamespaceNameCouldNotBeFound.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0246");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving builtin types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuiltInTypes()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\BuiltInTypes.cs");
      InvokeParser(project).ShouldBeTrue();

      int i = 0;

      // sbyte a1;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a1");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Sbyte);
        builtin.Name.ShouldEqual("sbyte");
        builtin.DistinctiveName.ShouldEqual("sbyte");
        builtin.FullyQualifiedName.ShouldEqual("sbyte");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        var alias = builtin.AliasedType as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.SByte");

        builtin.BaseTypeReferences.ShouldEqual(builtin.AliasedType.BaseTypeReferences);
        builtin.Members.ShouldEqual(builtin.AliasedType.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasedType.DeclarationSpace);
      }
      // byte a2;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a2");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Byte);
        builtin.Name.ShouldEqual("byte");
        builtin.DistinctiveName.ShouldEqual("byte");
        builtin.FullyQualifiedName.ShouldEqual("byte");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        var alias = builtin.AliasedType as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Byte");

        builtin.BaseTypeReferences.ShouldEqual(builtin.AliasedType.BaseTypeReferences);
        builtin.Members.ShouldEqual(builtin.AliasedType.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasedType.DeclarationSpace);
      }
      // short a3;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a3");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Short);
        builtin.Name.ShouldEqual("short");
        builtin.DistinctiveName.ShouldEqual("short");
        builtin.FullyQualifiedName.ShouldEqual("short");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        var alias = builtin.AliasedType as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Int16");

        builtin.BaseTypeReferences.ShouldEqual(builtin.AliasedType.BaseTypeReferences);
        builtin.Members.ShouldEqual(builtin.AliasedType.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasedType.DeclarationSpace);
      }
      // ushort a4;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a4");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Ushort);
        builtin.Name.ShouldEqual("ushort");
        builtin.DistinctiveName.ShouldEqual("ushort");
        builtin.FullyQualifiedName.ShouldEqual("ushort");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        var alias = builtin.AliasedType as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.UInt16");

        builtin.BaseTypeReferences.ShouldEqual(builtin.AliasedType.BaseTypeReferences);
        builtin.Members.ShouldEqual(builtin.AliasedType.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasedType.DeclarationSpace);
      }
      // int a5;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a5");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Int);
        builtin.Name.ShouldEqual("int");
        builtin.DistinctiveName.ShouldEqual("int");
        builtin.FullyQualifiedName.ShouldEqual("int");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        var alias = builtin.AliasedType as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Int32");

        builtin.BaseTypeReferences.ShouldEqual(builtin.AliasedType.BaseTypeReferences);
        builtin.Members.ShouldEqual(builtin.AliasedType.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasedType.DeclarationSpace);
      }
      // uint a6;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a6");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Uint);
        builtin.Name.ShouldEqual("uint");
        builtin.DistinctiveName.ShouldEqual("uint");
        builtin.FullyQualifiedName.ShouldEqual("uint");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        var alias = builtin.AliasedType as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.UInt32");

        builtin.BaseTypeReferences.ShouldEqual(builtin.AliasedType.BaseTypeReferences);
        builtin.Members.ShouldEqual(builtin.AliasedType.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasedType.DeclarationSpace);
      }
      // long a7;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a7");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Long);
        builtin.Name.ShouldEqual("long");
        builtin.DistinctiveName.ShouldEqual("long");
        builtin.FullyQualifiedName.ShouldEqual("long");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        var alias = builtin.AliasedType as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Int64");

        builtin.BaseTypeReferences.ShouldEqual(builtin.AliasedType.BaseTypeReferences);
        builtin.Members.ShouldEqual(builtin.AliasedType.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasedType.DeclarationSpace);
      }
      // ulong a8;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a8");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Ulong);
        builtin.Name.ShouldEqual("ulong");
        builtin.DistinctiveName.ShouldEqual("ulong");
        builtin.FullyQualifiedName.ShouldEqual("ulong");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        var alias = builtin.AliasedType as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.UInt64");

        builtin.BaseTypeReferences.ShouldEqual(builtin.AliasedType.BaseTypeReferences);
        builtin.Members.ShouldEqual(builtin.AliasedType.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasedType.DeclarationSpace);
      }
      // char a9;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a9");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Char);
        builtin.Name.ShouldEqual("char");
        builtin.DistinctiveName.ShouldEqual("char");
        builtin.FullyQualifiedName.ShouldEqual("char");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        var alias = builtin.AliasedType as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Char");

        builtin.BaseTypeReferences.ShouldEqual(builtin.AliasedType.BaseTypeReferences);
        builtin.Members.ShouldEqual(builtin.AliasedType.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasedType.DeclarationSpace);
      }
      // float a10;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a10");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Float);
        builtin.Name.ShouldEqual("float");
        builtin.DistinctiveName.ShouldEqual("float");
        builtin.FullyQualifiedName.ShouldEqual("float");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeTrue();
        builtin.IsIntegralType.ShouldBeFalse();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        var alias = builtin.AliasedType as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Single");

        builtin.BaseTypeReferences.ShouldEqual(builtin.AliasedType.BaseTypeReferences);
        builtin.Members.ShouldEqual(builtin.AliasedType.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasedType.DeclarationSpace);
      }
      // double a11;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a11");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Double);
        builtin.Name.ShouldEqual("double");
        builtin.DistinctiveName.ShouldEqual("double");
        builtin.FullyQualifiedName.ShouldEqual("double");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeTrue();
        builtin.IsIntegralType.ShouldBeFalse();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        var alias = builtin.AliasedType as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Double");

        builtin.BaseTypeReferences.ShouldEqual(builtin.AliasedType.BaseTypeReferences);
        builtin.Members.ShouldEqual(builtin.AliasedType.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasedType.DeclarationSpace);
      }
      // bool a12;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a12");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Bool);
        builtin.Name.ShouldEqual("bool");
        builtin.DistinctiveName.ShouldEqual("bool");
        builtin.FullyQualifiedName.ShouldEqual("bool");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeFalse();
        builtin.IsNumericType.ShouldBeFalse();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        var alias = builtin.AliasedType as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Boolean");

        builtin.BaseTypeReferences.ShouldEqual(builtin.AliasedType.BaseTypeReferences);
        builtin.Members.ShouldEqual(builtin.AliasedType.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasedType.DeclarationSpace);
      }
      // decimal a13;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a13");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Decimal);
        builtin.Name.ShouldEqual("decimal");
        builtin.DistinctiveName.ShouldEqual("decimal");
        builtin.FullyQualifiedName.ShouldEqual("decimal");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeFalse();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        var alias = builtin.AliasedType as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Decimal");

        builtin.BaseTypeReferences.ShouldEqual(builtin.AliasedType.BaseTypeReferences);
        builtin.Members.ShouldEqual(builtin.AliasedType.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasedType.DeclarationSpace);
      }
      // object a14;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a14");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Object);
        builtin.Name.ShouldEqual("object");
        builtin.DistinctiveName.ShouldEqual("object");
        builtin.FullyQualifiedName.ShouldEqual("object");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeFalse();
        builtin.IsNumericType.ShouldBeFalse();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeTrue();
        builtin.IsSimpleType.ShouldBeFalse();
        builtin.IsValueType.ShouldBeFalse();

        var alias = builtin.AliasedType as ClassEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Object");

        builtin.BaseTypeReferences.ShouldEqual(builtin.AliasedType.BaseTypeReferences);
        builtin.Members.ShouldEqual(builtin.AliasedType.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasedType.DeclarationSpace);
      }
      // string a15;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a15");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.String);
        builtin.Name.ShouldEqual("string");
        builtin.DistinctiveName.ShouldEqual("string");
        builtin.FullyQualifiedName.ShouldEqual("string");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeFalse();
        builtin.IsNumericType.ShouldBeFalse();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeTrue();
        builtin.IsSimpleType.ShouldBeFalse();
        builtin.IsValueType.ShouldBeFalse();

        var alias = builtin.AliasedType as ClassEntity;
        alias.FullyQualifiedName.ShouldEqual("System.String");

        builtin.BaseTypeReferences.ShouldEqual(builtin.AliasedType.BaseTypeReferences);
        builtin.Members.ShouldEqual(builtin.AliasedType.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasedType.DeclarationSpace);
      }

      // int? a16;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a16");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var nullable = fieldEntity.Type.TargetEntity as NullableTypeEntity;
        var builtin = nullable.UnderlyingType as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Int);

        var alias = builtin.AliasedType as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Int32");
      }

      // int*[] a17;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a17");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var array = fieldEntity.Type.TargetEntity as ArrayTypeEntity;
        var pointer = array.UnderlyingType as PointerToTypeEntity;
        var builtin = pointer.UnderlyingType as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Int);

        var alias = builtin.AliasedType as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Int32");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the resolution of built-in base types
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuiltInBaseTypes()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\BuiltInBaseTypes.cs");
      InvokeParser(project).ShouldBeTrue();

      // class A1 : object
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[0].BaseTypeReferences.ToArray()[0];
        baseTypeRef.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("object");
        var aliasedTypeRef = ((BuiltInTypeEntity)baseTypeRef.TargetEntity).AliasedTypeReference;
        aliasedTypeRef.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        aliasedTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("System.Object");
        ((BuiltInTypeEntity)baseTypeRef.TargetEntity).AliasedType.FullyQualifiedName.ShouldEqual("System.Object");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving pointer-to-type references.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PointerToType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\PointerToType.cs");
      InvokeParser(project).ShouldBeTrue();

      var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
      fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      var pointer1 = fieldEntity.Type.TargetEntity as PointerToTypeEntity;
      pointer1.BaseTypeReferences.Count().ShouldEqual(0);
      pointer1.Members.Count().ShouldEqual(0);
      pointer1.DeclarationSpace.ShouldEqual(project.SemanticGraph.GlobalNamespace.ChildTypes[1].DeclarationSpace);
      pointer1.DistinctiveName.ShouldEqual("A2**");
      pointer1.FullyQualifiedName.ShouldEqual("A2**");
      pointer1.Name.ShouldEqual("A2");
      pointer1.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace.ChildTypes[1].Parent);
      pointer1.SyntaxNodes.Count.ShouldEqual(0);
      pointer1.IsPointerType.ShouldBeTrue();
      pointer1.IsReferenceType.ShouldBeFalse();
      pointer1.IsValueType.ShouldBeFalse();
      pointer1.UnderlyingType.PointerType.ShouldEqual(pointer1);

      var pointer2 = pointer1.UnderlyingType as PointerToTypeEntity;
      pointer2.BaseTypeReferences.Count().ShouldEqual(0);
      pointer2.Members.Count().ShouldEqual(0);
      pointer2.DeclarationSpace.ShouldEqual(project.SemanticGraph.GlobalNamespace.ChildTypes[1].DeclarationSpace);
      pointer2.DistinctiveName.ShouldEqual("A2*");
      pointer2.FullyQualifiedName.ShouldEqual("A2*");
      pointer2.Name.ShouldEqual("A2");
      pointer2.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace.ChildTypes[1].Parent);
      pointer2.SyntaxNodes.Count.ShouldEqual(0);
      pointer2.UnderlyingType.PointerType.ShouldEqual(pointer2);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving pointer-to-unknown (void*) references.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PointerToUnknown()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\PointerToUnknown.cs");
      InvokeParser(project).ShouldBeTrue();

      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var pointer1 = fieldEntity.Type.TargetEntity as PointerToUnknownTypeEntity;
        pointer1.BaseTypeReferences.Count().ShouldEqual(0);
        pointer1.Members.Count().ShouldEqual(0);
        pointer1.DeclarationSpace.ShouldBeNull();
        pointer1.DistinctiveName.ShouldEqual("void*");
        pointer1.FullyQualifiedName.ShouldEqual("void*");
        pointer1.Name.ShouldEqual("void*");
        pointer1.Parent.ShouldBeNull();
        pointer1.SyntaxNodes.Count.ShouldEqual(0);
        pointer1.IsPointerType.ShouldBeTrue();
        pointer1.IsReferenceType.ShouldBeFalse();
        pointer1.IsValueType.ShouldBeFalse();
      }
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[1] as FieldEntity;
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var pointer1 = fieldEntity.Type.TargetEntity as PointerToTypeEntity;
        pointer1.Name.ShouldEqual("void*");
        pointer1.DistinctiveName.ShouldEqual("void**");
        pointer1.FullyQualifiedName.ShouldEqual("void**");
        pointer1.Parent.ShouldBeNull();
        pointer1.DeclarationSpace.ShouldBeNull();
        pointer1.SyntaxNodes.Count.ShouldEqual(0);
        var pointer2 = pointer1.UnderlyingType as PointerToUnknownTypeEntity;
        pointer2.Name.ShouldEqual("void*");
        pointer2.DistinctiveName.ShouldEqual("void*");
        pointer2.FullyQualifiedName.ShouldEqual("void*");
        pointer2.Parent.ShouldBeNull();
        pointer2.DeclarationSpace.ShouldBeNull();
        pointer2.SyntaxNodes.Count.ShouldEqual(0);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving nullable type references.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NullableType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\NullableType.cs");
      InvokeParser(project).ShouldBeTrue();

      // A2? a1;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a1");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var nullable = fieldEntity.Type.TargetEntity as NullableTypeEntity;
        nullable.Name.ShouldEqual("A2");
        nullable.DistinctiveName.ShouldEqual("A2?");
        nullable.FullyQualifiedName.ShouldEqual("A2?");
        nullable.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace);
        nullable.SyntaxNodes.Count.ShouldEqual(0);
        nullable.IsPointerType.ShouldBeFalse();
        nullable.IsReferenceType.ShouldBeFalse();
        nullable.IsValueType.ShouldBeTrue();
        var underlyingType = nullable.UnderlyingType as StructEntity;
        underlyingType.FullyQualifiedName.ShouldEqual("A2");
        underlyingType.NullableType.ShouldEqual(nullable);

        var aliasedType = nullable.AliasedType as ConstructedGenericTypeEntity;
        aliasedType.FullyQualifiedName.ShouldEqual("System.Nullable`1[A2]");
        aliasedType.UnderlyingType.ShouldEqual(project.SemanticGraph.NullableGenericTypeDefinition);

        nullable.BaseTypeReferences.ShouldEqual(nullable.AliasedType.BaseTypeReferences);
        nullable.Members.ShouldEqual(nullable.AliasedType.Members);
        nullable.DeclarationSpace.ShouldEqual(nullable.AliasedType.DeclarationSpace);
      }
      // A2?[] a2;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[1] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a2");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var array = fieldEntity.Type.TargetEntity as ArrayTypeEntity;
        var nullable = array.UnderlyingType as NullableTypeEntity;
        nullable.Name.ShouldEqual("A2");
        nullable.DistinctiveName.ShouldEqual("A2?");
        nullable.FullyQualifiedName.ShouldEqual("A2?");
        nullable.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace);
        nullable.SyntaxNodes.Count.ShouldEqual(0);
        var underlyingType = nullable.UnderlyingType as StructEntity;
        underlyingType.FullyQualifiedName.ShouldEqual("A2");
        underlyingType.NullableType.ShouldEqual(nullable);

        var aliasedType = nullable.AliasedType as ConstructedGenericTypeEntity;
        aliasedType.FullyQualifiedName.ShouldEqual("System.Nullable`1[A2]");
        aliasedType.UnderlyingType.ShouldEqual(project.SemanticGraph.NullableGenericTypeDefinition);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving array type references.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ArrayType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\ArrayType.cs");
      InvokeParser(project).ShouldBeTrue();

      // A2[][,] a1;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var array1 = fieldEntity.Type.TargetEntity as ArrayTypeEntity;
        array1.BaseType.FullyQualifiedName.ShouldEqual("System.Array");
        array1.Members.Count().ShouldEqual(0);
        array1.DeclarationSpace.ShouldEqual(project.SemanticGraph.GlobalNamespace.ChildTypes[1].DeclarationSpace);
        array1.DistinctiveName.ShouldEqual("A2[][,]");
        array1.FullyQualifiedName.ShouldEqual("A2[][,]");
        array1.Name.ShouldEqual("A2");
        array1.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace.ChildTypes[1].Parent);
        array1.Rank.ShouldEqual(2);
        array1.SyntaxNodes.Count.ShouldEqual(0);
        array1.IsPointerType.ShouldBeFalse();
        array1.IsReferenceType.ShouldBeTrue();
        array1.IsValueType.ShouldBeFalse();
        array1.UnderlyingType.GetArrayTypeByRank(2).ShouldEqual(array1);

        var array2 = array1.UnderlyingType as ArrayTypeEntity;
        array2.DeclarationSpace.ShouldEqual(project.SemanticGraph.GlobalNamespace.ChildTypes[1].DeclarationSpace);
        array2.DistinctiveName.ShouldEqual("A2[]");
        array2.FullyQualifiedName.ShouldEqual("A2[]");
        array2.Name.ShouldEqual("A2");
        array2.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace.ChildTypes[1].Parent);
        array2.Rank.ShouldEqual(1);
        array2.SyntaxNodes.Count.ShouldEqual(0);
        array2.UnderlyingType.GetArrayTypeByRank(1).ShouldEqual(array2);
      }
      // A2**[][,] a2;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[1] as FieldEntity;
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var array1 = fieldEntity.Type.TargetEntity as ArrayTypeEntity;
        array1.FullyQualifiedName.ShouldEqual("A2**[][,]");
        var array2 = array1.UnderlyingType as ArrayTypeEntity;
        array2.FullyQualifiedName.ShouldEqual("A2**[]");
        var pointer1 = array2.UnderlyingType as PointerToTypeEntity;
        pointer1.FullyQualifiedName.ShouldEqual("A2**");
        var pointer2 = pointer1.UnderlyingType as PointerToTypeEntity;
        pointer2.FullyQualifiedName.ShouldEqual("A2*");
        var structEntity = pointer2.UnderlyingType as StructEntity;
        structEntity.FullyQualifiedName.ShouldEqual("A2");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of namespace names in using namespace entities.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingNamespaceNames_MultiTag()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\UsingNamespaceNames_MultiTag.cs");
      InvokeParser(project).ShouldBeTrue();

      project.Errors.Count.ShouldEqual(0);
      project.Warnings.Count.ShouldEqual(0);

      var global = project.SemanticGraph.GlobalNamespace;
      var namespaceA = global.GetChildNamespaceByName("A");
      var namespaceB = namespaceA.GetChildNamespaceByName("B");
      var namespaceC = namespaceB.GetChildNamespaceByName("C");

      // using A.B;
      {
        var usingNamespace = global.UsingNamespaces.ToArray()[0];
        usingNamespace.NamespaceReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        usingNamespace.NamespaceReference.TargetEntity.ShouldEqual(namespaceB);
        usingNamespace.ImportedNamespace.ShouldEqual(namespaceB);
      }
      // using B.C;
      {
        var usingNamespace = namespaceA.UsingNamespaces.ToArray()[0];
        usingNamespace.NamespaceReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        usingNamespace.NamespaceReference.TargetEntity.ShouldEqual(namespaceC);
        usingNamespace.ImportedNamespace.ShouldEqual(namespaceC);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving to type declared in base class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void DeclaredInBaseClass()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\DeclaredInBaseClass.cs");
      InvokeParser(project).ShouldBeTrue();

      int i = 0;

      var classA = project.SemanticGraph.GlobalNamespace.ChildTypes[0] as ClassEntity;

      // E x1;
      {
        var field = classA.Members.ToArray()[i++] as FieldEntity;
        field.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        field.Type.TargetEntity.FullyQualifiedName.ShouldEqual("C.E");
      }
      // E<int> x2;
      {
        var field = classA.Members.ToArray()[i++] as FieldEntity;
        field.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        field.Type.TargetEntity.FullyQualifiedName.ShouldEqual("C.E`1[int]");
        ((ConstructedGenericTypeEntity)field.Type.TargetEntity).UnderlyingType.FullyQualifiedName.ShouldEqual("C.E`1");
      }
      // F.G x3;
      {
        var field = classA.Members.ToArray()[i++] as FieldEntity;
        field.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        field.Type.TargetEntity.FullyQualifiedName.ShouldEqual("D.F.G");
      }
      // F<int>.G x4;
      {
        var field = classA.Members.ToArray()[i++] as FieldEntity;
        field.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        field.Type.TargetEntity.FullyQualifiedName.ShouldEqual("D.F`1.G[int]");
      }
      // F<int>.G<int> x5;
      {
        var field = classA.Members.ToArray()[i++] as FieldEntity;
        field.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        field.Type.TargetEntity.FullyQualifiedName.ShouldEqual("D.F`1.G`1[int,int]");
        ((ConstructedGenericTypeEntity)field.Type.TargetEntity).UnderlyingType.FullyQualifiedName.ShouldEqual("D.F`1.G`1");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0118: 'A' is a 'namespace' but is used like a 'type'
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0118_NamespaceIsUsedLikeAType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0118_NamespaceIsUsedLikeAType.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0118");
      project.Warnings.Count.ShouldEqual(0);

      var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[0].BaseTypeReferences.ToArray()[0];
      baseTypeRef.ResolutionState.ShouldEqual(ResolutionState.Unresolvable);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving a constructed generic class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ConstructedGenericClass()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\ConstructedGenericClass.cs");
      InvokeParser(project).ShouldBeTrue();

      var fields = project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("N").ChildTypes[0].Members.ToArray();
      int i = 0;

      // A<int, long>.B1 b1;
      {
        var fieldEntity = fields[i++] as FieldEntity;
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.Type.TargetEntity as ConstructedGenericTypeEntity;
        typeEntity.Name.ShouldEqual("B1");
        typeEntity.DistinctiveName.ShouldEqual("B1[N.A1,N.A2]");
        typeEntity.FullyQualifiedName.ShouldEqual("N.A`2.B1[N.A1,N.A2]");
        typeEntity.BaseTypeReferences.ShouldEqual(typeEntity.UnderlyingType.BaseTypeReferences);
        typeEntity.Members.ShouldEqual(typeEntity.UnderlyingType.Members);
        typeEntity.SyntaxNodes.Count.ShouldEqual(0);
        typeEntity.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("N").ChildTypes[0]);
        typeEntity.DeclarationSpace.ShouldEqual(typeEntity.UnderlyingType.DeclarationSpace);
        typeEntity.IsPointerType.ShouldBeFalse();
        typeEntity.IsReferenceType.ShouldBeTrue();
        typeEntity.IsValueType.ShouldBeFalse();

        typeEntity.TypeArguments[0].FullyQualifiedName.ShouldEqual("N.A1");
        typeEntity.TypeArguments[1].FullyQualifiedName.ShouldEqual("N.A2");

        typeEntity.UnderlyingType.FullyQualifiedName.ShouldEqual("N.A`2.B1");

        ((ClassEntity)typeEntity.UnderlyingType).ConstructedGenericTypes.Count.ShouldEqual(1);
        ((ClassEntity)typeEntity.UnderlyingType).ConstructedGenericTypes[0].ShouldEqual(typeEntity);
      }
      // A<A1, A2>.B2<A3> b2;
      {
        var fieldEntity = fields[i++] as FieldEntity;
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.Type.TargetEntity as ConstructedGenericTypeEntity;
        typeEntity.Name.ShouldEqual("B2");
        typeEntity.DistinctiveName.ShouldEqual("B2`1[N.A1,N.A2,N.A3]");
        typeEntity.FullyQualifiedName.ShouldEqual("N.A`2.B2`1[N.A1,N.A2,N.A3]");
        typeEntity.BaseTypeReferences.ShouldEqual(typeEntity.UnderlyingType.BaseTypeReferences);
        typeEntity.Members.ShouldEqual(typeEntity.UnderlyingType.Members);
        typeEntity.SyntaxNodes.Count.ShouldEqual(0);
        typeEntity.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("N").ChildTypes[0]);
        typeEntity.DeclarationSpace.ShouldEqual(typeEntity.UnderlyingType.DeclarationSpace);
        typeEntity.IsPointerType.ShouldBeFalse();
        typeEntity.IsReferenceType.ShouldBeTrue();
        typeEntity.IsValueType.ShouldBeFalse();

        typeEntity.TypeArguments[0].FullyQualifiedName.ShouldEqual("N.A1");
        typeEntity.TypeArguments[1].FullyQualifiedName.ShouldEqual("N.A2");
        typeEntity.TypeArguments[2].FullyQualifiedName.ShouldEqual("N.A3");

        typeEntity.UnderlyingType.FullyQualifiedName.ShouldEqual("N.A`2.B2`1");

        ((ClassEntity)typeEntity.UnderlyingType).ConstructedGenericTypes.Count.ShouldEqual(1);
        ((ClassEntity)typeEntity.UnderlyingType).ConstructedGenericTypes[0].ShouldEqual(typeEntity);
      }
      //  A<A1, A2>.B3<A4> b3;
      {
        var fieldEntity = fields[i++] as FieldEntity;
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.Type.TargetEntity as ConstructedGenericTypeEntity;
        typeEntity.Name.ShouldEqual("B3");
        typeEntity.DistinctiveName.ShouldEqual("B3`1[N.A1,N.A2,N.A4]");
        typeEntity.FullyQualifiedName.ShouldEqual("N.A`2.B3`1[N.A1,N.A2,N.A4]");
        typeEntity.BaseTypeReferences.ShouldEqual(typeEntity.UnderlyingType.BaseTypeReferences);
        typeEntity.Members.ShouldEqual(typeEntity.UnderlyingType.Members);
        typeEntity.SyntaxNodes.Count.ShouldEqual(0);
        typeEntity.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("N").ChildTypes[0]);
        typeEntity.DeclarationSpace.ShouldEqual(typeEntity.UnderlyingType.DeclarationSpace);
        typeEntity.IsPointerType.ShouldBeFalse();
        typeEntity.IsReferenceType.ShouldBeTrue();
        typeEntity.IsValueType.ShouldBeFalse();

        typeEntity.TypeArguments[0].FullyQualifiedName.ShouldEqual("N.A1");
        typeEntity.TypeArguments[1].FullyQualifiedName.ShouldEqual("N.A2");
        typeEntity.TypeArguments[2].FullyQualifiedName.ShouldEqual("N.A4");

        typeEntity.UnderlyingType.FullyQualifiedName.ShouldEqual("N.A`2.B3`1");

        ((ClassEntity)typeEntity.UnderlyingType).ConstructedGenericTypes.Count.ShouldEqual(1);
        ((ClassEntity)typeEntity.UnderlyingType).ConstructedGenericTypes[0].ShouldEqual(typeEntity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving a constructed generic struct.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ConstructedGenericStruct()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\ConstructedGenericStruct.cs");
      InvokeParser(project).ShouldBeTrue();

      var fields = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray();
      int i = 0;

      // B<int> a;
      {
        var fieldEntity = fields[i++] as FieldEntity;
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.Type.TargetEntity as ConstructedGenericTypeEntity;
        typeEntity.Name.ShouldEqual("B");
        typeEntity.DistinctiveName.ShouldEqual("B`1[int]");
        typeEntity.FullyQualifiedName.ShouldEqual("B`1[int]");
        typeEntity.BaseTypeReferences.ShouldEqual(typeEntity.UnderlyingType.BaseTypeReferences);
        typeEntity.Members.ShouldEqual(typeEntity.UnderlyingType.Members);
        typeEntity.SyntaxNodes.Count.ShouldEqual(0);
        typeEntity.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace);
        typeEntity.DeclarationSpace.ShouldEqual(typeEntity.UnderlyingType.DeclarationSpace);
        typeEntity.IsPointerType.ShouldBeFalse();
        typeEntity.IsReferenceType.ShouldBeFalse();
        typeEntity.IsValueType.ShouldBeTrue();
        typeEntity.TypeArguments[0].FullyQualifiedName.ShouldEqual("int");
        typeEntity.UnderlyingType.FullyQualifiedName.ShouldEqual("B`1");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving a generic type, where the type argument is also a generic type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void GenericTypeArgument()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\GenericTypeArgument.cs");
      InvokeParser(project).ShouldBeTrue();

      var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
      fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      var typeEntity = fieldEntity.Type.TargetEntity as ConstructedGenericTypeEntity;
      typeEntity.ShouldNotBeNull();
      typeEntity.Name.ShouldEqual("A3");
      typeEntity.DistinctiveName.ShouldEqual("A3`1[A2.A3`1[A4]]");
      typeEntity.FullyQualifiedName.ShouldEqual("A2.A3`1[A2.A3`1[A4]]");
      typeEntity.UnderlyingType.FullyQualifiedName.ShouldEqual("A2.A3`1");

      var typeArgTypeEntity = typeEntity.TypeArguments[0] as ConstructedGenericTypeEntity;
      typeArgTypeEntity.FullyQualifiedName.ShouldEqual("A2.A3`1[A4]");
      typeArgTypeEntity.UnderlyingType.FullyQualifiedName.ShouldEqual("A2.A3`1");

      var typeArgTypeEntity2 = typeArgTypeEntity.TypeArguments[0] as ClassEntity;
      typeArgTypeEntity2.FullyQualifiedName.ShouldEqual("A4");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Precedence rule 1: name in local declaration space has precedence 1
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Precedence1_Local()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\Precedence1_Local.cs");
      InvokeParser(project).ShouldBeTrue();

      project.Errors.Count.ShouldEqual(0);
      project.Warnings.Count.ShouldEqual(0);

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("A").ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.Type.TargetEntity.FullyQualifiedName.ShouldEqual("A.B.C");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Precedence rule 2: name in base type's declaration space has precedence 2
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Precedence2_BaseType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\Precedence2_BaseType.cs");
      InvokeParser(project).ShouldBeTrue();

      project.Errors.Count.ShouldEqual(0);
      project.Warnings.Count.ShouldEqual(0);

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("A").ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.Type.TargetEntity.FullyQualifiedName.ShouldEqual("D.C");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Precedence rule 3a: name in one level higher parent's declaration space has precedence 3.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Precedence3a_Level1_Parent()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\Precedence3a_Level1_Parent.cs");
      InvokeParser(project).ShouldBeTrue();

      project.Errors.Count.ShouldEqual(0);
      project.Warnings.Count.ShouldEqual(0);

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("A").ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.Type.TargetEntity.FullyQualifiedName.ShouldEqual("A.C");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Precedence rule 3b: alias defined one level higher has precedence 3.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Precedence3b_Level1_UsingAlias()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\Precedence3b_Level1_UsingAlias.cs");
      InvokeParser(project).ShouldBeTrue();

      project.Errors.Count.ShouldEqual(0);
      project.Warnings.Count.ShouldEqual(0);

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("A").ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.Type.TargetEntity.FullyQualifiedName.ShouldEqual("E");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Precedence rule 4: using namespace defined one level higher has precedence 4.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Precedence4_Level1_UsingNamespace()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\Precedence4_Level1_UsingNamespace.cs");
      InvokeParser(project).ShouldBeTrue();

      project.Errors.Count.ShouldEqual(0);
      project.Warnings.Count.ShouldEqual(0);

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("A").ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.Type.TargetEntity.FullyQualifiedName.ShouldEqual("N.C");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Precedence rule 5a: name in two level higher parent's declaration space has precedence 5.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Precedence5a_Level2_Parent()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\Precedence5a_Level2_Parent.cs");
      InvokeParser(project).ShouldBeTrue();

      project.Errors.Count.ShouldEqual(0);
      project.Warnings.Count.ShouldEqual(0);

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("A").ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.Type.TargetEntity.FullyQualifiedName.ShouldEqual("C");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Precedence rule 5b: alias defined one level higher has precedence 5.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Precedence5b_Level2_UsingAlias()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\Precedence5b_Level2_UsingAlias.cs");
      InvokeParser(project).ShouldBeTrue();

      project.Errors.Count.ShouldEqual(0);
      project.Warnings.Count.ShouldEqual(0);

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("A").ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.Type.TargetEntity.FullyQualifiedName.ShouldEqual("E");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Precedence rule 6: using namespace defined two level higher has precedence 6.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Precedence6_Level2_UsingNamespace()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\Precedence6_Level2_UsingNamespace.cs");
      InvokeParser(project).ShouldBeTrue();

      project.Errors.Count.ShouldEqual(0);
      project.Warnings.Count.ShouldEqual(0);

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("A").ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.Type.TargetEntity.FullyQualifiedName.ShouldEqual("N.C");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests that namespaces are not imported with using namespace directives (only types).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NamespacesAreNotImportedWithUsing()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\NamespacesAreNotImportedWithUsing.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0246");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests that only those usings are considered that has the name to be resolved in scope.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NotInScopeForUsingNamespace()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\NotInScopeForUsingNamespace.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0246");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0104: 'C' is an ambiguous reference between 'B1.C' and 'B2.C'
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0104_AmbigousReference()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0104_AmbigousReference.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0104");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests that using namespace directive finds embedded types too.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingFindsEmbeddedTypeToo()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\UsingFindsEmbeddedTypeToo.cs");
      InvokeParser(project).ShouldBeTrue();

      project.Errors.Count.ShouldEqual(0);
      project.Warnings.Count.ShouldEqual(0);

      var field = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.Type.TargetEntity.FullyQualifiedName.ShouldEqual("B.C.D");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests that using namespace directive finds generic types too.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingFindsGenericTypeToo()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\UsingFindsGenericTypeToo.cs");
      InvokeParser(project).ShouldBeTrue();

      project.Errors.Count.ShouldEqual(0);
      project.Warnings.Count.ShouldEqual(0);

      var field = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.Type.TargetEntity.FullyQualifiedName.ShouldEqual("B.C`1.D`1[int,long]");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the resolution of non-generic base types
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NonGenericBaseTypes()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\NonGenericBaseTypes.cs");
      InvokeParser(project).ShouldBeTrue();

      // class A1 : C0
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[0].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("C0");
      }
      // class A2 : C0.C1
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[1].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("C0.C1");
      }
      // class A3 : N1.N1C0
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[2].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N1.N1C0");
      }
      // class A4 : N1.N2.N2C0
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[3].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N1.N2.N2C0");
      }
      // class A5 : N1.N2.N2C0.N2C1
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[4].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N1.N2.N2C0.N2C1");
      }
      // class N3C1 : N3C2
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("N3").ChildTypes[0].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N3.N3C2");
      }
      // class N5C1 : N4C1
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("N4").ChildNamespaces[0].ChildTypes[0].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N4.N4C1");
      }
      // class N5C2 : N6.N6C1
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("N4").ChildNamespaces[0].ChildTypes[1].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N4.N6.N6C1");
      }
      // class N5C2C1 : N5C1
      {
        var baseTypeRef = ((ClassEntity)project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("N4").ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[0].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N4.N5.N5C1");
      }
      // class N5C2C2 : N4C1
      {
        var baseTypeRef = ((ClassEntity)project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("N4").ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[1].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N4.N4C1");
      }
      // class N5C2C3 : N5C2C1
      {
        var baseTypeRef = ((ClassEntity)project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("N4").ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[2].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N4.N5.N5C2.N5C2C1");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the resolution of generic base types
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void GenericBaseTypes()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\GenericBaseTypes.cs");
      InvokeParser(project).ShouldBeTrue();

      int i = 0;

      // class A1<T1> : A3<T1, A2<T1>>
      {
        project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseType.FullyQualifiedName.ShouldEqual("A3`2[A1`1.T1,A2`1[A1`1.T1]]");
      }
      // class A2<T2> : A3<int, long>
      {
        project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseType.FullyQualifiedName.ShouldEqual("A3`2[int,long]");
      }
      // class A3<T3, T4>
      {
        project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseType.FullyQualifiedName.ShouldEqual("System.Object");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the resolution of reflected base types
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ReflectedBaseTypes()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\ReflectedBaseTypes.cs");
      InvokeParser(project).ShouldBeTrue();

      int i = 0;

      // class A1 : System.Object
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseTypeReferences.ToArray()[0];
        baseTypeRef.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("System.Object");
      }
      // class A2 : System.Collections.Generic.Dictionary<int,long>
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseTypeReferences.ToArray()[0];
        baseTypeRef.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("System.Collections.Generic.Dictionary`2[int,long]");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the resolution of implicit base types
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ImplicitBaseTypes()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\ImplicitBaseTypes.cs");
      InvokeParser(project).ShouldBeTrue();

      int i = 0;

      // class A1 // implicitly : System.Object
      {
        project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseType.FullyQualifiedName.ShouldEqual("System.Object");
      }
      // struct A2 // implicitly: System.ValueType
      {
        project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseType.FullyQualifiedName.ShouldEqual("System.ValueType");
      }
      // enum A3 // implicitly: System.Enum
      {
        project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseType.FullyQualifiedName.ShouldEqual("System.Enum");
      }
      // delegate void A4(); // implicitly: System.MulticastDelegate
      {
        project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseType.FullyQualifiedName.ShouldEqual("System.MulticastDelegate");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of base types of types imported from mscorlib.dll
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ResolveMscorlibBaseTypes()
    {
      var project = new CSharpProject(WorkingFolder);
      var factory = new MetadataImporterSemanticEntityFactory(project, project.SemanticGraph);
      factory.CreateEntitiesFromAssembly(Assembly.GetAssembly(typeof(int)).Location, "global");
      project.SemanticGraph.AcceptVisitor(new TypeResolverPass1SemanticGraphVisitor(project, project.SemanticGraph));

      project.Warnings.Count.ShouldEqual(0);
      project.Errors.Count.ShouldEqual(0);

      var collection = project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("System")
        .GetChildNamespaceByName("Collections").GetChildNamespaceByName("ObjectModel")
        .GetChildTypeByDistinctiveName("Collection`1");
      var keyedCollection = project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("System")
        .GetChildNamespaceByName("Collections").GetChildNamespaceByName("ObjectModel")
        .GetChildTypeByDistinctiveName("KeyedCollection`2");
      keyedCollection.FullyQualifiedName.ShouldEqual("System.Collections.ObjectModel.KeyedCollection`2");
      ((ConstructedGenericTypeEntity)keyedCollection.BaseType).UnderlyingType.ShouldEqual(collection);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of builtin type aliases to types defined in mscorlib.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ResolveAliasesToMscorlibTypes()
    {
      var project = new CSharpProject(WorkingFolder);
      var factory = new MetadataImporterSemanticEntityFactory(project, project.SemanticGraph);
      factory.CreateEntitiesFromAssembly(Assembly.GetAssembly(typeof(int)).Location, "global");
      project.SemanticGraph.AcceptVisitor(new TypeResolverPass1SemanticGraphVisitor(project, project.SemanticGraph));

      project.Warnings.Count.ShouldEqual(0);
      project.Errors.Count.ShouldEqual(0);

      // Builtin type aliases must be resolved
      project.SemanticGraph.BuiltInTypes.All(
        builtInType => builtInType.AliasedTypeReference.ResolutionState == ResolutionState.Resolved).ShouldBeTrue();

      // Checking only one if the builtin types whether it resolved to the right system type.
      project.SemanticGraph.GetBuiltInTypeByName("int").AliasedType.FullyQualifiedName.ShouldEqual("System.Int32");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0246: The type or namespace name 'A' could not be found (are you missing a using directive or an assembly reference?)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0246_TypeNameCouldNotBeFound()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0246_TypeNameCouldNotBeFound.cs");
      InvokeParser(project).ShouldBeFalse();

      var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[0].BaseTypeReferences.ToArray()[0];
      baseTypeRef.ResolutionState.ShouldEqual(ResolutionState.Unresolvable);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0246");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0138: A using namespace directive can only be applied to namespaces; 'A' is a type not a namespace
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0138_UsingNamespaceWithTypeName()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0138_UsingNamespaceWithTypeName.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0138");
      project.Warnings.Count.ShouldEqual(0);
    }
  }
}