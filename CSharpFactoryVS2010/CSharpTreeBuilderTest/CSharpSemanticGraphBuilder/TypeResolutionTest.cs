using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.ProjectContent;
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
      fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("global::A`1'T1");
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
      var namespaceA = global.GetChildNamespace("A");
      var namespaceB = namespaceA.GetChildNamespace("B");

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
    /// Error CS0576: Namespace 'namespace' contains a definition conflicting with alias 'identifier' (using alias)
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
    /// Error CS0576: Namespace 'namespace' contains a definition conflicting with alias 'identifier' (using alias)
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
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.SByte");
      }
      // byte a2;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a2");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Byte");
      }
      // short a3;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a3");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Int16");
      }
      // ushort a4;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a4");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.UInt16");
      }
      // int a5;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a5");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Int32");
      }
      // uint a6;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a6");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.UInt32");
      }
      // long a7;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a7");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Int64");
      }
      // ulong a8;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a8");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.UInt64");
      }
      // char a9;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a9");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Char");
      }
      // float a10;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a10");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Single");
      }
      // double a11;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a11");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Double");
      }
      // bool a12;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a12");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Boolean");
      }
      // decimal a13;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a13");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Decimal");
      }
      // object a14;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a14");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as ClassEntity;
        typeEntity.ToString().ShouldEqual("global::System.Object");
      }
      // string a15;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a15");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as ClassEntity;
        typeEntity.ToString().ShouldEqual("global::System.String");
      }

      // int? a16;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a16");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var nullable = fieldEntity.TypeReference.TargetEntity as ConstructedGenericTypeEntity;
        nullable.ToString().ShouldEqual("global::System.Nullable`1[global::System.Int32]");
      }

      // int*[] a17;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a17");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var array = fieldEntity.TypeReference.TargetEntity as ArrayTypeEntity;
        var pointer = array.UnderlyingType as PointerToTypeEntity;
        var typeEntity = pointer.UnderlyingType as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Int32");
      }

      // delegate void D();
      {
        var delegateEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[1] as DelegateEntity;
        delegateEntity.Name.ShouldEqual("D");
        delegateEntity.ReturnTypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = delegateEntity.ReturnType as StructEntity;
        typeEntity.FullyQualifiedName.ShouldEqual("System.Void");
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
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("System.Object");
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

      var underlyingType = project.SemanticGraph.GlobalNamespace.ChildTypes[1] as StructEntity;

      var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
      fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      var pointer1 = fieldEntity.TypeReference.TargetEntity as PointerToTypeEntity;
      pointer1.BaseTypeReferences.Count().ShouldEqual(0);
      pointer1.Members.Count().ShouldEqual(0);
      pointer1.Name.ShouldEqual("A2");
      pointer1.FullyQualifiedName.ShouldEqual("A2");
      pointer1.ToString().ShouldEqual("global::A2**");
      pointer1.Parent.ShouldEqual(underlyingType.Parent);
      pointer1.SyntaxNodes.Count.ShouldEqual(0);
      pointer1.IsPointerType.ShouldBeTrue();
      pointer1.IsReferenceType.ShouldBeFalse();
      pointer1.IsValueType.ShouldBeFalse();

      var pointer2 = pointer1.UnderlyingType as PointerToTypeEntity;
      pointer2.BaseTypeReferences.Count().ShouldEqual(0);
      pointer2.Members.Count().ShouldEqual(0);
      pointer2.Name.ShouldEqual("A2");
      pointer2.FullyQualifiedName.ShouldEqual("A2");
      pointer2.ToString().ShouldEqual("global::A2*");
      pointer2.Parent.ShouldEqual(underlyingType.Parent);
      pointer2.SyntaxNodes.Count.ShouldEqual(0);
      pointer2.UnderlyingType.PointerType.ShouldEqual(pointer2);

      underlyingType.PointerType.ShouldEqual(pointer2);
      pointer2.PointerType.ShouldEqual(pointer1);
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
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var pointer1 = fieldEntity.TypeReference.TargetEntity as PointerToTypeEntity;
        pointer1.BaseTypeReferences.Count().ShouldEqual(0);
        pointer1.Members.Count().ShouldEqual(0);
        pointer1.FullyQualifiedName.ShouldEqual("System.Void");
        pointer1.ToString().ShouldEqual("global::System.Void*");
        pointer1.Name.ShouldEqual("Void");
        pointer1.Parent.ToString().ShouldEqual("global::System");
        pointer1.SyntaxNodes.Count.ShouldEqual(0);
        pointer1.IsPointerType.ShouldBeTrue();
        pointer1.IsReferenceType.ShouldBeFalse();
        pointer1.IsValueType.ShouldBeFalse();
        var underlyingType = pointer1.UnderlyingType as StructEntity;
        underlyingType.ToString().ShouldEqual("global::System.Void");
        underlyingType.PointerType.ShouldEqual(pointer1);
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
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var nullable = fieldEntity.TypeReference.TargetEntity as ConstructedGenericTypeEntity;
        nullable.Name.ShouldEqual("Nullable");
        nullable.FullyQualifiedName.ShouldEqual("System.Nullable");
        nullable.ToString().ShouldEqual("global::System.Nullable`1[global::A2]");
        nullable.Parent.ToString().ShouldEqual("global::System");
        nullable.SyntaxNodes.Count.ShouldEqual(0);
        nullable.IsPointerType.ShouldBeFalse();
        nullable.IsReferenceType.ShouldBeFalse();
        nullable.IsValueType.ShouldBeTrue();
        var underlyingType = nullable.UnderlyingType as GenericCapableTypeEntity;
        underlyingType.FullyQualifiedName.ShouldEqual("System.Nullable");
        underlyingType.ToString().ShouldEqual("global::System.Nullable`1");

        underlyingType.ConstructedGenericTypes.Count.ShouldEqual(1);
        underlyingType.ConstructedGenericTypes[0].ShouldEqual(nullable);
      }
      // A2?[] a2;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[1] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a2");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var array = fieldEntity.TypeReference.TargetEntity as ArrayTypeEntity;
        var nullable = array.UnderlyingType as ConstructedGenericTypeEntity;
        nullable.UnderlyingType.ShouldEqual(project.SemanticGraph.NullableGenericTypeDefinition);
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
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var array1 = fieldEntity.TypeReference.TargetEntity as ArrayTypeEntity;
        array1.BaseType.FullyQualifiedName.ShouldEqual("System.Array");
        array1.Members.Count().ShouldEqual(0);
        array1.Name.ShouldEqual("A2");
        array1.FullyQualifiedName.ShouldEqual("A2");
        array1.ToString().ShouldEqual("global::A2[][,]");
        array1.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace.ChildTypes[1].Parent);
        array1.Rank.ShouldEqual(2);
        array1.SyntaxNodes.Count.ShouldEqual(0);
        array1.IsPointerType.ShouldBeFalse();
        array1.IsReferenceType.ShouldBeTrue();
        array1.IsValueType.ShouldBeFalse();
        array1.UnderlyingType.GetArrayTypeByRank(2).ShouldEqual(array1);

        var array2 = array1.UnderlyingType as ArrayTypeEntity;
        array2.Name.ShouldEqual("A2");
        array2.FullyQualifiedName.ShouldEqual("A2");
        array2.ToString().ShouldEqual("global::A2[]");
        array2.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace.ChildTypes[1].Parent);
        array2.Rank.ShouldEqual(1);
        array2.SyntaxNodes.Count.ShouldEqual(0);
        array2.UnderlyingType.GetArrayTypeByRank(1).ShouldEqual(array2);
      }
      // A2**[][,] a2;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[1] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var array1 = fieldEntity.TypeReference.TargetEntity as ArrayTypeEntity;
        array1.ToString().ShouldEqual("global::A2**[][,]");
        var array2 = array1.UnderlyingType as ArrayTypeEntity;
        array2.ToString().ShouldEqual("global::A2**[]");
        var pointer1 = array2.UnderlyingType as PointerToTypeEntity;
        pointer1.ToString().ShouldEqual("global::A2**");
        var pointer2 = pointer1.UnderlyingType as PointerToTypeEntity;
        pointer2.ToString().ShouldEqual("global::A2*");
        var structEntity = pointer2.UnderlyingType as StructEntity;
        structEntity.ToString().ShouldEqual("global::A2");
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
      var namespaceA = global.GetChildNamespace("A");
      var namespaceB = namespaceA.GetChildNamespace("B");
      var namespaceC = namespaceB.GetChildNamespace("C");

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
        field.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        field.TypeReference.TargetEntity.ToString().ShouldEqual("global::C+E");
      }
      // E<int> x2;
      {
        var field = classA.Members.ToArray()[i++] as FieldEntity;
        field.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        field.TypeReference.TargetEntity.ToString().ShouldEqual("global::C+E`1[global::System.Int32]");
        ((ConstructedGenericTypeEntity)field.TypeReference.TargetEntity).UnderlyingType.ToString().ShouldEqual("global::C+E`1");
      }
      // F.G x3;
      {
        var field = classA.Members.ToArray()[i++] as FieldEntity;
        field.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        field.TypeReference.TargetEntity.ToString().ShouldEqual("global::D+F+G");
      }
      // F<int>.G x4;
      {
        var field = classA.Members.ToArray()[i++] as FieldEntity;
        field.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        field.TypeReference.TargetEntity.ToString().ShouldEqual("global::D+F`1+G[global::System.Int32]");
      }
      // F<int>.G<int> x5;
      {
        var field = classA.Members.ToArray()[i++] as FieldEntity;
        field.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        field.TypeReference.TargetEntity.ToString().ShouldEqual("global::D+F`1+G`1[global::System.Int32,global::System.Int32]");
        ((ConstructedGenericTypeEntity)field.TypeReference.TargetEntity).UnderlyingType.ToString().ShouldEqual("global::D+F`1+G`1");
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

      var fields = project.SemanticGraph.GlobalNamespace.GetChildNamespace("N").ChildTypes[0].Members.ToArray();
      int i = 0;

      // A<int, long>.B1 b1;
      {
        var fieldEntity = fields[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as ConstructedGenericTypeEntity;
        typeEntity.Name.ShouldEqual("B1");
        typeEntity.FullyQualifiedName.ShouldEqual("N.A.B1");
        typeEntity.ToString().ShouldEqual("global::N.A`2+B1[global::N.A1,global::N.A2]");
        typeEntity.BaseTypeReferences.ShouldEqual(typeEntity.UnderlyingType.BaseTypeReferences);
        typeEntity.Members.ShouldEqual(typeEntity.UnderlyingType.Members);
        typeEntity.SyntaxNodes.Count.ShouldEqual(0);
        typeEntity.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace.GetChildNamespace("N").ChildTypes[0]);
        typeEntity.IsPointerType.ShouldBeFalse();
        typeEntity.IsReferenceType.ShouldBeTrue();
        typeEntity.IsValueType.ShouldBeFalse();

        typeEntity.TypeArguments[0].ToString().ShouldEqual("global::N.A1");
        typeEntity.TypeArguments[1].ToString().ShouldEqual("global::N.A2");

        typeEntity.UnderlyingType.ToString().ShouldEqual("global::N.A`2+B1");

        ((ClassEntity)typeEntity.UnderlyingType).ConstructedGenericTypes.Count.ShouldEqual(1);
        ((ClassEntity)typeEntity.UnderlyingType).ConstructedGenericTypes[0].ShouldEqual(typeEntity);
      }
      // A<A1, A2>.B2<A3> b2;
      {
        var fieldEntity = fields[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as ConstructedGenericTypeEntity;
        typeEntity.Name.ShouldEqual("B2");
        typeEntity.FullyQualifiedName.ShouldEqual("N.A.B2");
        typeEntity.ToString().ShouldEqual("global::N.A`2+B2`1[global::N.A1,global::N.A2,global::N.A3]");
        typeEntity.BaseTypeReferences.ShouldEqual(typeEntity.UnderlyingType.BaseTypeReferences);
        typeEntity.Members.ShouldEqual(typeEntity.UnderlyingType.Members);
        typeEntity.SyntaxNodes.Count.ShouldEqual(0);
        typeEntity.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace.GetChildNamespace("N").ChildTypes[0]);
        typeEntity.IsPointerType.ShouldBeFalse();
        typeEntity.IsReferenceType.ShouldBeTrue();
        typeEntity.IsValueType.ShouldBeFalse();

        typeEntity.TypeArguments[0].ToString().ShouldEqual("global::N.A1");
        typeEntity.TypeArguments[1].ToString().ShouldEqual("global::N.A2");
        typeEntity.TypeArguments[2].ToString().ShouldEqual("global::N.A3");

        typeEntity.UnderlyingType.ToString().ShouldEqual("global::N.A`2+B2`1");

        ((ClassEntity)typeEntity.UnderlyingType).ConstructedGenericTypes.Count.ShouldEqual(1);
        ((ClassEntity)typeEntity.UnderlyingType).ConstructedGenericTypes[0].ShouldEqual(typeEntity);
      }
      //  A<A1, A2>.B3<A4> b3;
      {
        var fieldEntity = fields[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as ConstructedGenericTypeEntity;
        typeEntity.Name.ShouldEqual("B3");
        typeEntity.FullyQualifiedName.ShouldEqual("N.A.B3");
        typeEntity.ToString().ShouldEqual("global::N.A`2+B3`1[global::N.A1,global::N.A2,global::N.A4]");
        typeEntity.BaseTypeReferences.ShouldEqual(typeEntity.UnderlyingType.BaseTypeReferences);
        typeEntity.Members.ShouldEqual(typeEntity.UnderlyingType.Members);
        typeEntity.SyntaxNodes.Count.ShouldEqual(0);
        typeEntity.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace.GetChildNamespace("N").ChildTypes[0]);
        typeEntity.IsPointerType.ShouldBeFalse();
        typeEntity.IsReferenceType.ShouldBeTrue();
        typeEntity.IsValueType.ShouldBeFalse();

        typeEntity.TypeArguments[0].ToString().ShouldEqual("global::N.A1");
        typeEntity.TypeArguments[1].ToString().ShouldEqual("global::N.A2");
        typeEntity.TypeArguments[2].ToString().ShouldEqual("global::N.A4");

        typeEntity.UnderlyingType.ToString().ShouldEqual("global::N.A`2+B3`1");

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
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as ConstructedGenericTypeEntity;
        typeEntity.Name.ShouldEqual("B");
        typeEntity.FullyQualifiedName.ShouldEqual("B");
        typeEntity.ToString().ShouldEqual("global::B`1[global::System.Int32]");
        typeEntity.BaseTypeReferences.ShouldEqual(typeEntity.UnderlyingType.BaseTypeReferences);
        typeEntity.Members.ShouldEqual(typeEntity.UnderlyingType.Members);
        typeEntity.SyntaxNodes.Count.ShouldEqual(0);
        typeEntity.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace);
        typeEntity.IsPointerType.ShouldBeFalse();
        typeEntity.IsReferenceType.ShouldBeFalse();
        typeEntity.IsValueType.ShouldBeTrue();
        typeEntity.TypeArguments[0].ToString().ShouldEqual("global::System.Int32");
        typeEntity.UnderlyingType.ToString().ShouldEqual("global::B`1");
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
      fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      var typeEntity = fieldEntity.TypeReference.TargetEntity as ConstructedGenericTypeEntity;
      typeEntity.ShouldNotBeNull();
      typeEntity.Name.ShouldEqual("A3");
      typeEntity.FullyQualifiedName.ShouldEqual("A2.A3");
      typeEntity.ToString().ShouldEqual("global::A2+A3`1[global::A2+A3`1[global::A4]]");
      typeEntity.UnderlyingType.ToString().ShouldEqual("global::A2+A3`1");

      var typeArgTypeEntity = typeEntity.TypeArguments[0] as ConstructedGenericTypeEntity;
      typeArgTypeEntity.ToString().ShouldEqual("global::A2+A3`1[global::A4]");
      typeArgTypeEntity.UnderlyingType.ToString().ShouldEqual("global::A2+A3`1");

      var typeArgTypeEntity2 = typeArgTypeEntity.TypeArguments[0] as ClassEntity;
      typeArgTypeEntity2.ToString().ShouldEqual("global::A4");
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

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.TypeReference.TargetEntity.ToString().ShouldEqual("global::A.B+C");
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

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.TypeReference.TargetEntity.ToString().ShouldEqual("global::D+C");
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

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.TypeReference.TargetEntity.ToString().ShouldEqual("global::A.C");
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

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.TypeReference.TargetEntity.ToString().ShouldEqual("global::E");
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

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.TypeReference.TargetEntity.ToString().ShouldEqual("global::N.C");
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

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.TypeReference.TargetEntity.ToString().ShouldEqual("global::C");
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

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.TypeReference.TargetEntity.ToString().ShouldEqual("global::E");
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

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").ChildTypes[0].Members.ToList()[0] as FieldEntity;
      field.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.TypeReference.TargetEntity.ToString().ShouldEqual("global::N.C");
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
      field.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.TypeReference.TargetEntity.ToString().ShouldEqual("global::B.C+D");
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
      field.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      field.TypeReference.TargetEntity.ToString().ShouldEqual("global::B.C`1+D`1[global::System.Int32,global::System.Int64]");
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
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::C0");
      }
      // class A2 : C0.C1
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[1].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::C0+C1");
      }
      // class A3 : N1.N1C0
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[2].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::N1.N1C0");
      }
      // class A4 : N1.N2.N2C0
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[3].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::N1.N2.N2C0");
      }
      // class A5 : N1.N2.N2C0.N2C1
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[4].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::N1.N2.N2C0+N2C1");
      }
      // class N3C1 : N3C2
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.GetChildNamespace("N3").ChildTypes[0].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::N3.N3C2");
      }
      // class N5C1 : N4C1
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.GetChildNamespace("N4").ChildNamespaces[0].ChildTypes[0].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::N4.N4C1");
      }
      // class N5C2 : N6.N6C1
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.GetChildNamespace("N4").ChildNamespaces[0].ChildTypes[1].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::N4.N6.N6C1");
      }
      // class N5C2C1 : N5C1
      {
        var baseTypeRef = ((ClassEntity)project.SemanticGraph.GlobalNamespace.GetChildNamespace("N4").ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[0].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::N4.N5.N5C1");
      }
      // class N5C2C2 : N4C1
      {
        var baseTypeRef = ((ClassEntity)project.SemanticGraph.GlobalNamespace.GetChildNamespace("N4").ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[1].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::N4.N4C1");
      }
      // class N5C2C3 : N5C2C1
      {
        var baseTypeRef = ((ClassEntity)project.SemanticGraph.GlobalNamespace.GetChildNamespace("N4").ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[2].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::N4.N5.N5C2+N5C2C1");
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
        project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseType.ToString().ShouldEqual("global::A3`2[global::A1`1'T1,global::A2`1[global::A1`1'T1]]");
      }
      // class A2<T2> : A3<int, long>
      {
        project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseType.ToString().ShouldEqual("global::A3`2[global::System.Int32,global::System.Int64]");
      }
      // class A3<T3, T4>
      {
        project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseType.ToString().ShouldEqual("global::System.Object");
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
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::System.Object");
      }
      // class A2 : System.Collections.Generic.Dictionary<int,long>
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseTypeReferences.ToArray()[0];
        baseTypeRef.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::System.Collections.Generic.Dictionary`2[global::System.Int32,global::System.Int64]");
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

      var collection = project.SemanticGraph.GlobalNamespace.GetChildNamespace("System")
        .GetChildNamespace("Collections").GetChildNamespace("ObjectModel")
        .GetSingleChildType<TypeEntity>("Collection", 1);
      var keyedCollection = project.SemanticGraph.GlobalNamespace.GetChildNamespace("System")
        .GetChildNamespace("Collections").GetChildNamespace("ObjectModel")
        .GetSingleChildType<TypeEntity>("KeyedCollection", 2);
      keyedCollection.ToString().ShouldEqual("global::System.Collections.ObjectModel.KeyedCollection`2");
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

      // Checking only one if the builtin types whether it resolved to the right system type.
      project.SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Int).FullyQualifiedName.ShouldEqual("System.Int32");
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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS1681: You cannot redefine the global extern alias
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS1681_ExternAliasCannotBeGlobal()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS1681_ExternAliasCannotBeGlobal.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS1681");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0430: The extern alias 'MyExternAlias' was not specified in a /reference option
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0430_ExternAliasNotResolved()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\ExternAlias.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0430");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Extern alias resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ExternAlias()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\ExternAlias.cs");
      project.AddAliasedAssemblyReference("MyExternAlias", TestAssemblyPathAndFilename);
      InvokeParser(project).ShouldBeTrue();

      project.SemanticGraph.RootNamespaces.ToList().Count.ShouldEqual(2);

      var namespaceRef = project.SemanticGraph.GlobalNamespace.ExternAliases.ToArray()[0].RootNamespaceReference;
      namespaceRef.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      namespaceRef.TargetEntity.ShouldEqual(project.SemanticGraph.GetRootNamespaceByName("MyExternAlias"));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0576: Namespace 'namespace' contains a definition conflicting with alias 'identifier' (extern alias)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0576_ExternAliasConflictsWithNamespaceDeclaration()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0576_ExternAliasConflictsWithNamespaceDeclaration.cs");
      project.AddAliasedAssemblyReference("MyExternAlias", TestAssemblyPathAndFilename);
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0576");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0576: Namespace 'namespace' contains a definition conflicting with alias 'identifier' (extern alias)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0576_ExternAliasConflictsWithTypeDeclaration()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0576_ExternAliasConflictsWithTypeDeclaration.cs");
      project.AddAliasedAssemblyReference("MyExternAlias", TestAssemblyPathAndFilename);
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0576");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The same using namespace directive specified in the same namespace but in different source regions
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void SameUsingSameNamespaceDifferentRegions()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\SameUsingSameNamespaceDifferentRegions.cs");
      InvokeParser(project).ShouldBeTrue();

      project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").UsingNamespaces.ToList().Count.ShouldEqual(2);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The same using alias directive specified in the same namespace but in different source regions
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void SameUsingAliasSameNamespaceDifferentRegions()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\SameUsingAliasSameNamespaceDifferentRegions.cs");
      InvokeParser(project).ShouldBeTrue();

      project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").UsingAliases.ToList().Count.ShouldEqual(2);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The same extern alias directive specified in the same namespace but in different source regions
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void SameExternAliasSameNamespaceDifferentRegions()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\SameExternAliasSameNamespaceDifferentRegions.cs");
      project.AddAliasedAssemblyReference("MyExternAlias", TestAssemblyPathAndFilename);
      InvokeParser(project).ShouldBeTrue();

      project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").ExternAliases.ToList().Count.ShouldEqual(2);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0246: global qualified type not found
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0246_GlobalQualifiedTypeNotFound()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0246_GlobalQualifiedTypeNotFound.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0246");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Namespace and type references with global qualifier.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void GlobalQualifiedNames()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\GlobalQualifiedNames.cs");
      InvokeParser(project).ShouldBeTrue();

      int i = 0;

      // global::A x1;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("global::A");
      }
      // global::B<int> x2;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("global::B`1[global::System.Int32]");
      }
      // global::C.D x3;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("global::C.D");
      }
      // global::C.E<int> x4;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("global::C.E`1[global::System.Int32]");
      }
      // global::C.E<int>.F<long> x5;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("global::C.E`1+F`1[global::System.Int32,global::System.Int64]");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0431: Cannot use alias 'C' with '::' since the alias references a type. Use '.' instead.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0431_QualifierRefersToType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0431_QualifierRefersToType.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0431");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Namespace and type references with extern alias qualifier.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ExternAliasQualifiedNames()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\ExternAliasQualifiedNames.cs");
      project.AddAliasedAssemblyReference("MyExternAlias", TestAssemblyPathAndFilename);
      InvokeParser(project).ShouldBeTrue();

      int i = 0;

      var classEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0];

      // MyExternAlias::Class0 x0;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ShouldEqual(
          project.SemanticGraph.GetRootNamespaceByName("MyExternAlias").GetSingleChildType<ClassEntity>("Class0"));
      }
      // MyExternAlias::A.B.Class1 x1;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ShouldEqual(
          project.SemanticGraph.GetRootNamespaceByName("MyExternAlias").GetChildNamespace("A")
          .GetChildNamespace("B").GetSingleChildType<ClassEntity>("Class1"));
      }
      // MyExternAlias::A.B.Generic1<int,long> x2;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("MyExternAlias::A.B.Generic1`2[global::System.Int32,global::System.Int64]");
      }

      i = 0;

      classEntity = project.SemanticGraph.GlobalNamespace.GetChildNamespace("B").GetChildNamespace("C")
        .GetSingleChildType<ClassEntity>("A");

      // MyExternAlias::Class0 x0;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ShouldEqual(
          project.SemanticGraph.GetRootNamespaceByName("MyExternAlias").GetSingleChildType<ClassEntity>("Class0"));
      }
      // MyExternAlias::A.B.Class1 x1;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ShouldEqual(
          project.SemanticGraph.GetRootNamespaceByName("MyExternAlias").GetChildNamespace("A")
          .GetChildNamespace("B").GetSingleChildType<ClassEntity>("Class1"));
      }
      // MyExternAlias::A.B.Generic1<int,long> x2;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("MyExternAlias::A.B.Generic1`2[global::System.Int32,global::System.Int64]");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Namespace and type references with using alias qualifier.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingAliasQualifiedNames()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\UsingAliasQualifiedNames.cs");
      InvokeParser(project).ShouldBeTrue();

      int i = 0;

      var classEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0];

      // X::Y1 x0;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("global::Y.Y1");
      }
      // X::V.W.Y2 x1;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("global::Y.V.W.Y2");
      }
      // X::Y3<int,long> x2;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("global::Y.Y3`2[global::System.Int32,global::System.Int64]");
      }

      i = 0;

      classEntity = project.SemanticGraph.GlobalNamespace.GetChildNamespace("B").GetChildNamespace("C").ChildTypes[0];

      // X::Y1 x0;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("global::Y.Y1");
      }
      // X::V.W.Y2 x1;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("global::Y.V.W.Y2");
      }
      // X::Y3<int,long> x2;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("global::Y.Y3`2[global::System.Int32,global::System.Int64]");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Enum underlying type resolution.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void EnumUnderlyingType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\EnumUnderlyingType.cs");
      InvokeParser(project).ShouldBeTrue();

      {
        var enumEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0] as EnumEntity;
        enumEntity.UnderlyingTypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        enumEntity.UnderlyingTypeReference.TargetEntity.FullyQualifiedName.ShouldEqual("System.Int32");
        (enumEntity.Members.ToList()[0] as EnumMemberEntity).TypeReference.ShouldEqual(enumEntity.UnderlyingTypeReference);
      }
      {
        var enumEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[1] as EnumEntity;
        enumEntity.UnderlyingTypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        enumEntity.UnderlyingTypeReference.TargetEntity.FullyQualifiedName.ShouldEqual("System.Int64");
        (enumEntity.Members.ToList()[0] as EnumMemberEntity).TypeReference.ShouldEqual(enumEntity.UnderlyingTypeReference);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Delegate return type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void DelegateType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\DelegateType.cs");
      InvokeParser(project).ShouldBeTrue();

      {
        var delegateEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0] as DelegateEntity;
        delegateEntity.ReturnTypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        delegateEntity.ReturnType.FullyQualifiedName.ShouldEqual("System.Int32");
      }
      {
        var delegateEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[1] as DelegateEntity;
        delegateEntity.ReturnTypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        delegateEntity.ReturnType.FullyQualifiedName.ShouldEqual("System.Void");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS1008: Type byte, sbyte, short, ushort, int, uint, long, or ulong expect
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS1008_EnumBaseNonIntegral()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS1008_EnumBaseNonIntegral.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(2);
      project.Errors[0].Code.ShouldEqual("CS1008");
      project.Errors[1].Code.ShouldEqual("CS1008");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Property type resolution.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Property()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\Property.cs");
      InvokeParser(project).ShouldBeTrue();

      var classEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0] as ClassEntity;

      int i = 0;

      // int B { get; set; }
      {
        var propertyEntity = classEntity.Members.ToList()[i] as PropertyEntity;

        // Check property type resolution
        propertyEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        propertyEntity.Type.FullyQualifiedName.ShouldEqual("System.Int32");
        propertyEntity.AutoImplementedField.Type.FullyQualifiedName.ShouldEqual("System.Int32");

        // Check explicitly implemented interface resolution
        propertyEntity.InterfaceReference.ShouldBeNull();
        propertyEntity.Interface.ShouldBeNull();

        // Check that the property can be retrieved by name.
        classEntity.GetMember<PropertyEntity>(propertyEntity.Name).ShouldEqual(propertyEntity);
      }

      // Have to skip a member, the auto-implemented property's backing field.
      i++;
      i++;

      // int I.B { get; set; }
      {
        var propertyEntity = classEntity.Members.ToList()[i] as PropertyEntity;

        // Check property type resolution
        propertyEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        propertyEntity.Type.FullyQualifiedName.ShouldEqual("System.Int32");
        propertyEntity.AutoImplementedField.Type.FullyQualifiedName.ShouldEqual("System.Int32");

        // Check explicitly implemented interface resolution
        propertyEntity.InterfaceReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        propertyEntity.Interface.ToString().ShouldEqual("global::C.I`1[global::System.Int32]");

        // Check that the property can be retrieved by name.
        classEntity.GetMember<PropertyEntity>(propertyEntity.Name, propertyEntity.Interface).ShouldEqual(propertyEntity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Method type resolution.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Method()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\Method.cs");
      InvokeParser(project).ShouldBeTrue();

      var classEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0] as ClassEntity;
      var members = classEntity.Members.ToList();

      int i = 0;

      // void B<T2, T3>(A<int, long> a, T1 t1, T2 t2, T3 t3) { }
      {
        var methodEntity = members[i] as MethodEntity;

        // Check return type resolution
        methodEntity.ReturnTypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        methodEntity.ReturnType.ToString().ShouldEqual("T3");

        // Check explicitly implemented interface resolution
        methodEntity.InterfaceReference.ShouldBeNull();
        methodEntity.Interface.ShouldBeNull();

        // Check parameter type resolution
        var parameters = methodEntity.Parameters.ToList();
        parameters[0].TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        (parameters[0].Type as ConstructedGenericTypeEntity).UnderlyingType.ShouldEqual(classEntity);
        parameters[1].TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        parameters[1].Type.ShouldEqual(classEntity.GetOwnTypeParameterByName("T1"));
        parameters[2].TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        parameters[2].Type.ShouldEqual(methodEntity.GetOwnTypeParameterByName("T2"));
        parameters[3].TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        parameters[3].Type.ShouldEqual(methodEntity.GetOwnTypeParameterByName("T3"));

        classEntity.GetMethod(methodEntity.Signature).ShouldEqual(methodEntity);
      }

      i++;

      // void C() {}
      {
        var methodEntity = members[i] as MethodEntity;

        // Check return type resolution
        methodEntity.ReturnTypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        methodEntity.ReturnType.ToString().ShouldEqual("global::System.Void");

        // Check explicitly implemented interface resolution
        methodEntity.InterfaceReference.ShouldBeNull();
        methodEntity.Interface.ShouldBeNull();

        // Check parameter type resolution
        methodEntity.Parameters.ToList().Count.ShouldEqual(0);

        classEntity.GetMethod(methodEntity.Signature).ShouldEqual(methodEntity);
      }

      i++;

      // void I<T1, T2>.B<T2, T3>(A<int, long> a, T1 t1, T2 t2, T3 t3) { }
      {
        var methodEntity = members[i] as MethodEntity;

        // Check return type resolution
        methodEntity.ReturnTypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        methodEntity.ReturnType.ToString().ShouldEqual("T3");

        // Check explicitly implemented interface resolution
        methodEntity.InterfaceReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        methodEntity.Interface.ToString().ShouldEqual("global::I`2[global::A`2'T1,global::A`2'T2]");

        // Check parameter type resolution
        var parameters = methodEntity.Parameters.ToList();
        parameters[0].TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        (parameters[0].Type as ConstructedGenericTypeEntity).UnderlyingType.ShouldEqual(classEntity);
        parameters[1].TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        parameters[1].Type.ShouldEqual(classEntity.GetOwnTypeParameterByName("T1"));
        parameters[2].TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        parameters[2].Type.ShouldEqual(methodEntity.GetOwnTypeParameterByName("T2"));
        parameters[3].TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        parameters[3].Type.ShouldEqual(methodEntity.GetOwnTypeParameterByName("T3"));

        // Check that explicitly implemented interface members are not registered into the declaration space,
        // but can be retrieved by specifying the interface too.
        classEntity.GetMethod(methodEntity.Signature).ShouldBeNull();
        classEntity.GetMethod(methodEntity.Signature, methodEntity.Interface).ShouldEqual(members[2]);
      }

      i++;

      // void global::I<T1, T2>.C() { }
      {
        var methodEntity = members[i] as MethodEntity;

        // Check return type resolution
        methodEntity.ReturnTypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        methodEntity.ReturnType.ToString().ShouldEqual("global::System.Void");

        // Check explicitly implemented interface resolution
        methodEntity.InterfaceReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        methodEntity.Interface.ToString().ShouldEqual("global::I`2[global::A`2'T1,global::A`2'T2]");

        // Check parameter type resolution
        methodEntity.Parameters.ToList().Count.ShouldEqual(0);

        // Without specifying the interface, the method's signature can be confused with void C().
        classEntity.GetMethod(methodEntity.Signature).ShouldEqual(members[1]);
        classEntity.GetMethod(methodEntity.Signature, methodEntity.Interface).ShouldEqual(members[3]);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// This test case was introduced because of a problem in the name resolution process.
    /// During the resolution process type argument AST nodes are separated from the AST tree,
    /// and their parent becomes null. Unfortunately, determining the compilation unit is using the
    /// parent property, and using namespaces can be used in the resolution only if the
    /// compilation unit of the AST node is known.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [Ignore]
    public void TypeArgumentCanBeResolvedWithUsingNamespace()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\TypeArgumentCanBeResolvedWithUsingNamespace.cs");
      InvokeParser(project).ShouldBeTrue();
    }
  }
}