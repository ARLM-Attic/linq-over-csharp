using System.Linq;
using System.Reflection;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;

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

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A", 1);
      var fieldEntity = classEntity.Members.ToArray()[0] as FieldEntity;
      fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("global::A`1.T1");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving types in type parameter constraints.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [Ignore]
    // TODO: reintroduce when TypeParameterEntity.BaseInterfaces is implemented
    public void TypeParameterConstraints()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\TypeParameterConstraints.cs");
      InvokeParser(project).ShouldBeTrue();

      var global = project.SemanticGraph.GlobalNamespace;
      var classA = global.GetSingleChildType<ClassEntity>("A", 4);
 
      // where T1 : B, T2, I1, I2, T4, new()
      {
        var typeParameter = classA.GetOwnTypeParameterByName("T1");
        typeParameter.ClassTypeConstraint.ToString().ShouldEqual("global::B");
        typeParameter.ClassTypeConstraints.Count().ShouldEqual(1);

        var interfaceTypes = typeParameter.InterfaceTypeConstraints.ToList();
        interfaceTypes.Count().ShouldEqual(2);
        interfaceTypes[0].ToString().ShouldEqual("global::I1");
        interfaceTypes[1].ToString().ShouldEqual("global::I2`1[global::System.Int32]");

        var typeParams = typeParameter.TypeParameterConstraints.ToList();
        typeParams.Count().ShouldEqual(2);
        typeParams[0].ShouldEqual(classA.GetOwnTypeParameterByName("T2"));
        typeParams[1].ShouldEqual(classA.GetOwnTypeParameterByName("T4"));

        typeParameter.BaseClass.ToString().ShouldEqual("global::B");

        typeParameter.BaseInterfaces.Count.ShouldEqual(2);
        var interfaces = typeParameter.BaseInterfaces.OrderBy(x => x.ToString()).ToList();
        interfaces[0].ToString().ShouldEqual("global::I1");
        interfaces[1].ToString().ShouldEqual("global::I2`1[global::System.Int32]");
      }
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

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");

      int i = 0;

      // sbyte a1;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a1");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.SByte");
        typeEntity.BuiltInTypeValue = BuiltInType.Sbyte;
        typeEntity.IsSimpleType.ShouldBeTrue();
        typeEntity.IsNumericType.ShouldBeTrue();
        typeEntity.IsIntegralType.ShouldBeTrue();
        typeEntity.IsFloatingPointType.ShouldBeFalse();
        typeEntity.IsNullableType.ShouldBeFalse();
      }
      // byte a2;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a2");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Byte");
        typeEntity.IsSimpleType.ShouldBeTrue();
        typeEntity.IsNumericType.ShouldBeTrue();
        typeEntity.IsIntegralType.ShouldBeTrue();
        typeEntity.IsFloatingPointType.ShouldBeFalse();
        typeEntity.IsNullableType.ShouldBeFalse();
      }
      // short a3;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a3");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Int16");
        typeEntity.IsSimpleType.ShouldBeTrue();
        typeEntity.IsNumericType.ShouldBeTrue();
        typeEntity.IsIntegralType.ShouldBeTrue();
        typeEntity.IsFloatingPointType.ShouldBeFalse();
        typeEntity.IsNullableType.ShouldBeFalse();
      }
      // ushort a4;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a4");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.UInt16");
        typeEntity.IsSimpleType.ShouldBeTrue();
        typeEntity.IsNumericType.ShouldBeTrue();
        typeEntity.IsIntegralType.ShouldBeTrue();
        typeEntity.IsFloatingPointType.ShouldBeFalse();
        typeEntity.IsNullableType.ShouldBeFalse();
      }
      // int a5;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a5");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Int32");
        typeEntity.IsSimpleType.ShouldBeTrue();
        typeEntity.IsNumericType.ShouldBeTrue();
        typeEntity.IsIntegralType.ShouldBeTrue();
        typeEntity.IsFloatingPointType.ShouldBeFalse();
        typeEntity.IsNullableType.ShouldBeFalse();
      }
      // uint a6;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a6");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.UInt32");
        typeEntity.IsSimpleType.ShouldBeTrue();
        typeEntity.IsNumericType.ShouldBeTrue();
        typeEntity.IsIntegralType.ShouldBeTrue();
        typeEntity.IsFloatingPointType.ShouldBeFalse();
        typeEntity.IsNullableType.ShouldBeFalse();
      }
      // long a7;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a7");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Int64");
        typeEntity.IsSimpleType.ShouldBeTrue();
        typeEntity.IsNumericType.ShouldBeTrue();
        typeEntity.IsIntegralType.ShouldBeTrue();
        typeEntity.IsFloatingPointType.ShouldBeFalse();
        typeEntity.IsNullableType.ShouldBeFalse();
      }
      // ulong a8;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a8");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.UInt64");
        typeEntity.IsSimpleType.ShouldBeTrue();
        typeEntity.IsNumericType.ShouldBeTrue();
        typeEntity.IsIntegralType.ShouldBeTrue();
        typeEntity.IsFloatingPointType.ShouldBeFalse();
        typeEntity.IsNullableType.ShouldBeFalse();
      }
      // char a9;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a9");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Char");
        typeEntity.IsSimpleType.ShouldBeTrue();
        typeEntity.IsNumericType.ShouldBeTrue();
        typeEntity.IsIntegralType.ShouldBeTrue();
        typeEntity.IsFloatingPointType.ShouldBeFalse();
        typeEntity.IsNullableType.ShouldBeFalse();
      }
      // float a10;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a10");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Single");
        typeEntity.IsSimpleType.ShouldBeTrue();
        typeEntity.IsNumericType.ShouldBeTrue();
        typeEntity.IsIntegralType.ShouldBeFalse();
        typeEntity.IsFloatingPointType.ShouldBeTrue();
        typeEntity.IsNullableType.ShouldBeFalse();
      }
      // double a11;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a11");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Double");
        typeEntity.IsSimpleType.ShouldBeTrue();
        typeEntity.IsNumericType.ShouldBeTrue();
        typeEntity.IsIntegralType.ShouldBeFalse();
        typeEntity.IsFloatingPointType.ShouldBeTrue();
        typeEntity.IsNullableType.ShouldBeFalse();
      }
      // bool a12;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a12");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Boolean");
        typeEntity.IsSimpleType.ShouldBeTrue();
        typeEntity.IsNumericType.ShouldBeFalse();
        typeEntity.IsIntegralType.ShouldBeFalse();
        typeEntity.IsFloatingPointType.ShouldBeFalse();
        typeEntity.IsNullableType.ShouldBeFalse();
      }
      // decimal a13;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a13");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Decimal");
        typeEntity.IsSimpleType.ShouldBeTrue();
        typeEntity.IsNumericType.ShouldBeTrue();
        typeEntity.IsIntegralType.ShouldBeFalse();
        typeEntity.IsFloatingPointType.ShouldBeFalse();
        typeEntity.IsNullableType.ShouldBeFalse();
      }
      // object a14;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a14");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as ClassEntity;
        typeEntity.ToString().ShouldEqual("global::System.Object");
        typeEntity.IsSimpleType.ShouldBeFalse();
        typeEntity.IsNumericType.ShouldBeFalse();
        typeEntity.IsIntegralType.ShouldBeFalse();
        typeEntity.IsFloatingPointType.ShouldBeFalse();
        typeEntity.IsNullableType.ShouldBeFalse();
      }
      // string a15;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a15");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as ClassEntity;
        typeEntity.ToString().ShouldEqual("global::System.String");
        typeEntity.IsSimpleType.ShouldBeFalse();
        typeEntity.IsNumericType.ShouldBeFalse();
        typeEntity.IsIntegralType.ShouldBeFalse();
        typeEntity.IsFloatingPointType.ShouldBeFalse();
        typeEntity.IsNullableType.ShouldBeFalse();
      }

      // int? a16;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a16");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var nullable = fieldEntity.TypeReference.TargetEntity as StructEntity;
        nullable.ToString().ShouldEqual("global::System.Nullable`1[global::System.Int32]");
        nullable.IsSimpleType.ShouldBeFalse();
        nullable.IsNumericType.ShouldBeFalse();
        nullable.IsIntegralType.ShouldBeFalse();
        nullable.IsFloatingPointType.ShouldBeFalse();
        nullable.IsNullableType.ShouldBeTrue();
      }

      // int*[] a17;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a17");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var array = fieldEntity.TypeReference.TargetEntity as ArrayTypeEntity;
        var pointer = array.UnderlyingType as PointerToTypeEntity;
        var typeEntity = pointer.UnderlyingType as StructEntity;
        typeEntity.ToString().ShouldEqual("global::System.Int32");
      }

      // delegate void D();
      {
        var delegateEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<DelegateEntity>("D");
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
        var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A1");
        var baseTypeRef = classEntity.BaseTypeReferences.ToArray()[0];
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

      var underlyingType = project.SemanticGraph.GlobalNamespace.GetSingleChildType<StructEntity>("A2");

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A1");
      var fieldEntity = classEntity.Members.ToArray()[0] as FieldEntity;
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
        var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A1");
        var fieldEntity = classEntity.Members.ToArray()[0] as FieldEntity;
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

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A1");
      var structA2 = project.SemanticGraph.GlobalNamespace.GetSingleChildType<StructEntity>("A2");

      // A2? a1;
      {
        var fieldEntity = classEntity.Members.ToArray()[0] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a1");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var nullable = fieldEntity.TypeReference.TargetEntity;
        nullable.Name.ShouldEqual("Nullable");
        nullable.FullyQualifiedName.ShouldEqual("System.Nullable");
        nullable.ToString().ShouldEqual("global::System.Nullable`1[global::A2]");
        nullable.Parent.ToString().ShouldEqual("global::System");
        nullable.SyntaxNodes.Count.ShouldEqual(0);
        nullable.IsPointerType.ShouldBeFalse();
        nullable.IsReferenceType.ShouldBeFalse();
        nullable.IsValueType.ShouldBeTrue();

        nullable.IsNullableType.ShouldBeTrue();
        nullable.UnderlyingOfNullableType.ToString().ShouldEqual("global::A2");

        var underlyingType = nullable.TemplateEntity as StructEntity;
        underlyingType.FullyQualifiedName.ShouldEqual("System.Nullable");
        underlyingType.ToString().ShouldEqual("global::System.Nullable`1");

        underlyingType.GetConstructedEntity(
          new TypeParameterMap(underlyingType.TypeParameterMap, new TypeEntity[] {structA2} )
          ).ShouldEqual(nullable);
      }
      // A2?[] a2;
      {
        var fieldEntity = classEntity.Members.ToArray()[1] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a2");
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var array = fieldEntity.TypeReference.TargetEntity as ArrayTypeEntity;
        array.IsNullableType.ShouldBeFalse();
        array.UnderlyingOfNullableType.ShouldBeNull();
        var nullable = array.UnderlyingType;
        nullable.TemplateEntity.ShouldEqual(project.SemanticGraph.NullableGenericTypeDefinition);
        nullable.ShouldEqual(classEntity.GetMember<FieldEntity>("a1").Type);
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

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A1");

      // A2[][,] a1;
      {
        var fieldEntity = classEntity.Members.ToArray()[0] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var array1 = fieldEntity.TypeReference.TargetEntity as ArrayTypeEntity;
        array1.BaseClass.FullyQualifiedName.ShouldEqual("System.Array");
        array1.Members.Count().ShouldEqual(0);
        array1.Name.ShouldEqual("A2");
        array1.FullyQualifiedName.ShouldEqual("A2");
        array1.ToString().ShouldEqual("global::A2[][,]");
        array1.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[1].Parent);
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
        array2.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace.ChildTypes.ToList()[1].Parent);
        array2.Rank.ShouldEqual(1);
        array2.SyntaxNodes.Count.ShouldEqual(0);
        array2.UnderlyingType.GetArrayTypeByRank(1).ShouldEqual(array2);
      }
      // A2**[][,] a2;
      {
        var fieldEntity = classEntity.Members.ToArray()[1] as FieldEntity;
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

      var classA = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");

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
        field.TypeReference.TargetEntity.TemplateEntity.ToString().ShouldEqual("global::C+E`1");
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
        field.TypeReference.TargetEntity.TemplateEntity.ToString().ShouldEqual("global::D+F`1+G`1");
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

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("B");
      var baseTypeRef = classEntity.BaseTypeReferences.ToArray()[0];
      baseTypeRef.ResolutionState.ShouldEqual(ResolutionState.Unresolvable);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving a constructed generic class in mscorlib.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ConstructedGenericClassInMscorlib()
    {
      var project = new CSharpProject(WorkingFolder);
      InvokeParser(project).ShouldBeTrue();

      var comparer = project.SemanticGraph.GlobalNamespace.GetChildNamespace("System").GetChildNamespace("Collections").
        GetChildNamespace("Generic").GetSingleChildType<ClassEntity>("Comparer", 1);
      var icomparer = comparer.BaseInterfaces.Where(x => x.Name == "IComparer" && x.AllTypeParameterCount == 1).First();
      icomparer.ToString().ShouldEqual("global::System.Collections.Generic.IComparer`1[global::System.Collections.Generic.Comparer`1.T]");
      icomparer.IsOpen.ShouldBeTrue();
      icomparer.IsConstructed.ShouldBeTrue();
      icomparer.IsUnbound.ShouldBeFalse();
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

      var classA = project.SemanticGraph.GlobalNamespace.GetChildNamespace("N").GetSingleChildType<ClassEntity>("A", 2);
      var fields = classA.Members.ToArray();
      var classB1 = classA.GetSingleChildType<ClassEntity>("B1");
      var classB2 = classA.GetSingleChildType<ClassEntity>("B2", 1);
      var classB3 = classA.GetSingleChildType<ClassEntity>("B3", 1);

      // public class B1
      {
        var typeParameterMap = classB1.TypeParameterMap;
        typeParameterMap.IsEmpty.ShouldBeFalse();
        typeParameterMap.Count.ShouldEqual(2);
        var typeParameters = typeParameterMap.TypeParameters.ToList();
        typeParameters[0].ToString().ShouldEqual("global::N.A`2.T1");
        typeParameters[1].ToString().ShouldEqual("global::N.A`2.T2");
        var typeArguments = typeParameterMap.TypeArguments.ToList();
        typeArguments[0].ShouldBeNull();
        typeArguments[1].ShouldBeNull();
      }

      // public class B2<T1>
      {
        var typeParameterMap = classB2.TypeParameterMap;
        typeParameterMap.IsEmpty.ShouldBeFalse();
        typeParameterMap.Count.ShouldEqual(3);
        var typeParameters = typeParameterMap.TypeParameters.ToList();
        typeParameters[0].ToString().ShouldEqual("global::N.A`2.T1");
        typeParameters[1].ToString().ShouldEqual("global::N.A`2.T2");
        typeParameters[2].ToString().ShouldEqual("global::N.A`2+B2`1.T1");
        var typeArguments = typeParameterMap.TypeArguments.ToList();
        typeArguments[0].ShouldBeNull();
        typeArguments[1].ShouldBeNull();
        typeArguments[2].ShouldBeNull();
      }

      // public class B3<T3>
      {
        var typeParameterMap = classB3.TypeParameterMap;
        typeParameterMap.IsEmpty.ShouldBeFalse();
        typeParameterMap.Count.ShouldEqual(3);
        var typeParameters = typeParameterMap.TypeParameters.ToList();
        typeParameters[0].ToString().ShouldEqual("global::N.A`2.T1");
        typeParameters[1].ToString().ShouldEqual("global::N.A`2.T2");
        typeParameters[2].ToString().ShouldEqual("global::N.A`2+B3`1.T3");
        var typeArguments = typeParameterMap.TypeArguments.ToList();
        typeArguments[0].ShouldBeNull();
        typeArguments[1].ShouldBeNull();
        typeArguments[2].ShouldBeNull();
      }

      int i = 0;

      // A<A1, A2> a1;
      {
        var fieldEntity = fields[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as ClassEntity;
        typeEntity.Name.ShouldEqual("A");
        typeEntity.FullyQualifiedName.ShouldEqual("N.A");
        typeEntity.ToString().ShouldEqual("global::N.A`2[global::N.A1,global::N.A2]");
        typeEntity.TemplateEntity.ShouldEqual(classA);
        classA.GetConstructedEntity(typeEntity.TypeParameterMap).ShouldEqual(typeEntity);
      }
      // A<T1, T2> a2;
      {
        var fieldEntity = fields[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as ClassEntity;
        typeEntity.Name.ShouldEqual("A");
        typeEntity.FullyQualifiedName.ShouldEqual("N.A");
        typeEntity.ToString().ShouldEqual("global::N.A`2[global::N.A`2.T1,global::N.A`2.T2]");
        typeEntity.TemplateEntity.ShouldEqual(classA);
        classA.GetConstructedEntity(typeEntity.TypeParameterMap).ShouldEqual(typeEntity);
      }
      // A<T1, T2> a3;
      {
        var fieldEntity = fields[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as ClassEntity;
        typeEntity.ToString().ShouldEqual("global::N.A`2[global::N.A`2.T1,global::N.A`2.T2]");
        typeEntity.TemplateEntity.ShouldEqual(classA);
        // Should not create a new constructed entity because the previous field has the same type.
        classA.ConstructedEntities.Count().ShouldEqual(2);
      }
      // A<A1, A2>.B1 b1;
      {
        var fieldEntity = fields[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as ClassEntity;
        typeEntity.Name.ShouldEqual("B1");
        typeEntity.FullyQualifiedName.ShouldEqual("N.A.B1");
        typeEntity.ToString().ShouldEqual("global::N.A`2+B1[global::N.A1,global::N.A2]");
        typeEntity.TemplateEntity.ShouldEqual(classB1);
        classB1.GetConstructedEntity(typeEntity.TypeParameterMap).ShouldEqual(typeEntity);
      }
      // A<A1, A2>.B2<A3> b2;
      {
        var fieldEntity = fields[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as ClassEntity;
        typeEntity.Name.ShouldEqual("B2");
        typeEntity.FullyQualifiedName.ShouldEqual("N.A.B2");
        typeEntity.ToString().ShouldEqual("global::N.A`2+B2`1[global::N.A1,global::N.A2,global::N.A3]");
        typeEntity.TemplateEntity.ShouldEqual(classB2);
        classB2.GetConstructedEntity(typeEntity.TypeParameterMap).ShouldEqual(typeEntity);
      }
      // A<A1, A2>.B3<A4> b3;
      {
        var fieldEntity = fields[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as ClassEntity;
        typeEntity.Name.ShouldEqual("B3");
        typeEntity.FullyQualifiedName.ShouldEqual("N.A.B3");
        typeEntity.ToString().ShouldEqual("global::N.A`2+B3`1[global::N.A1,global::N.A2,global::N.A4]");
        typeEntity.TemplateEntity.ShouldEqual(classB3);
        classB3.GetConstructedEntity(typeEntity.TypeParameterMap).ShouldEqual(typeEntity);
      }

      // A<A1, A2> a1; --> public class B1
      {
        var type = (classA.GetMember<FieldEntity>("a1").Type as ClassEntity).GetSingleChildType<ClassEntity>("B1");
        type.ToString().ShouldEqual("global::N.A`2+B1[global::N.A1,global::N.A2]");
        type.IsOpen.ShouldBeFalse();
        type.IsConstructed.ShouldBeTrue();
        type.IsUnbound.ShouldBeFalse();
      }

      // A<A1, A2> a1; --> public class B2<T1>
      {
        var type = (classA.GetMember<FieldEntity>("a1").Type as ClassEntity).GetSingleChildType<ClassEntity>("B2", 1);
        type.ToString().ShouldEqual("global::N.A`2+B2`1[global::N.A1,global::N.A2,(open)]");
        type.IsOpen.ShouldBeTrue();
        type.IsConstructed.ShouldBeTrue();
        type.IsUnbound.ShouldBeFalse();
      }

      // A<A1, A2> a1; --> A<A1, A2>.B2<A3> b2;
      {
        var type = classA.GetMember<FieldEntity>("a1").Type.GetMember<FieldEntity>("b2").Type;
        type.ToString().ShouldEqual("global::N.A`2+B2`1[global::N.A1,global::N.A2,global::N.A3]");
        type.IsOpen.ShouldBeFalse();
        type.IsConstructed.ShouldBeTrue();
        type.IsUnbound.ShouldBeFalse();
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

      var structEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<StructEntity>("A");
      var fields = structEntity.Members.ToArray();
      int i = 0;

      // B<int> a;
      {
        var fieldEntity = fields[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var typeEntity = fieldEntity.TypeReference.TargetEntity as StructEntity;
        typeEntity.Name.ShouldEqual("B");
        typeEntity.FullyQualifiedName.ShouldEqual("B");
        typeEntity.ToString().ShouldEqual("global::B`1[global::System.Int32]");
        typeEntity.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace);
        typeEntity.IsPointerType.ShouldBeFalse();
        typeEntity.IsReferenceType.ShouldBeFalse();
        typeEntity.IsValueType.ShouldBeTrue();
        typeEntity.TypeParameterMap.TypeArguments.ToList()[0].ToString().ShouldEqual("global::System.Int32");
        typeEntity.TemplateEntity.ToString().ShouldEqual("global::B`1");
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

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A1");

      var fieldEntity = classEntity.Members.ToArray()[0] as FieldEntity;
      fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      var typeEntity = fieldEntity.TypeReference.TargetEntity as ClassEntity;
      typeEntity.ShouldNotBeNull();
      typeEntity.Name.ShouldEqual("A3");
      typeEntity.FullyQualifiedName.ShouldEqual("A2.A3");
      typeEntity.ToString().ShouldEqual("global::A2+A3`1[global::A2+A3`1[global::A4]]");
      typeEntity.TemplateEntity.ToString().ShouldEqual("global::A2+A3`1");

      var typeArgTypeEntity = typeEntity.TypeParameterMap.TypeArguments.ToList()[0] as ClassEntity;
      typeArgTypeEntity.ToString().ShouldEqual("global::A2+A3`1[global::A4]");
      typeArgTypeEntity.TemplateEntity.ToString().ShouldEqual("global::A2+A3`1");

      var typeArgTypeEntity2 = typeArgTypeEntity.TypeParameterMap.TypeArguments.ToList()[0] as ClassEntity;
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

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").ChildTypes.ToList()[0].Members.ToList()[0] as FieldEntity;
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

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").ChildTypes.ToList()[0].Members.ToList()[0] as FieldEntity;
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

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").ChildTypes.ToList()[0].Members.ToList()[0] as FieldEntity;
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

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").ChildTypes.ToList()[0].Members.ToList()[0] as FieldEntity;
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

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").ChildTypes.ToList()[0].Members.ToList()[0] as FieldEntity;
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

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").ChildTypes.ToList()[0].Members.ToList()[0] as FieldEntity;
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

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").ChildTypes.ToList()[0].Members.ToList()[0] as FieldEntity;
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

      var field = project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").ChildTypes.ToList()[0].Members.ToList()[0] as FieldEntity;
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

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var field = classEntity.Members.ToList()[0] as FieldEntity;
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

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");

      var field = classEntity.Members.ToList()[0] as FieldEntity;
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
        var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A1");
        var baseTypeRef = classEntity.BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::C0");
      }
      // class A2 : C0.C1
      {
        var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A2");
        var baseTypeRef = classEntity.BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::C0+C1");
      }
      // class A3 : N1.N1C0
      {
        var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A3");
        var baseTypeRef = classEntity.BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::N1.N1C0");
      }
      // class A4 : N1.N2.N2C0
      {
        var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A4");
        var baseTypeRef = classEntity.BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::N1.N2.N2C0");
      }
      // class A5 : N1.N2.N2C0.N2C1
      {
        var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A5");
        var baseTypeRef = classEntity.BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::N1.N2.N2C0+N2C1");
      }
      // class N3C1 : N3C2
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.GetChildNamespace("N3").ChildTypes.ToList()[0].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::N3.N3C2");
      }
      // class N5C1 : N4C1
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.GetChildNamespace("N4").ChildNamespaces[0].ChildTypes.ToList()[0].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::N4.N4C1");
      }
      // class N5C2 : N6.N6C1
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.GetChildNamespace("N4").ChildNamespaces[0].ChildTypes.ToList()[1].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::N4.N6.N6C1");
      }
      // class N5C2C1 : N5C1
      {
        var baseTypeRef = ((ClassEntity)project.SemanticGraph.GlobalNamespace.GetChildNamespace("N4").ChildNamespaces[0].ChildTypes.ToList()[1])
          .ChildTypes.ToList()[0].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::N4.N5.N5C1");
      }
      // class N5C2C2 : N4C1
      {
        var baseTypeRef = ((ClassEntity)project.SemanticGraph.GlobalNamespace.GetChildNamespace("N4").ChildNamespaces[0].ChildTypes.ToList()[1])
          .ChildTypes.ToList()[1].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::N4.N4C1");
      }
      // class N5C2C3 : N5C2C1
      {
        var baseTypeRef = ((ClassEntity)project.SemanticGraph.GlobalNamespace.GetChildNamespace("N4").ChildNamespaces[0].ChildTypes.ToList()[1])
          .ChildTypes.ToList()[2].BaseTypeReferences.ToArray()[0];
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

      // class A1<T1> : A3<T1, A2<T1>>, I1<A1<T1>>
      {
        var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A1", 1);
        classEntity.BaseClass.ToString().ShouldEqual("global::A3`2[global::A1`1.T1,global::A2`1[global::A1`1.T1]]");
        classEntity.BaseInterfaces.Count.ShouldEqual(1);
        classEntity.BaseInterfaces[0].ToString().ShouldEqual("global::I1`1[global::A1`1[global::A1`1.T1]]");
      }
      // class A2<T2> : A3<int, long>, I1<int>
      {
        var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A2", 1);
        classEntity.BaseClass.ToString().ShouldEqual("global::A3`2[global::System.Int32,global::System.Int64]");
        classEntity.BaseInterfaces.Count.ShouldEqual(1);
        classEntity.BaseInterfaces[0].ToString().ShouldEqual("global::I1`1[global::System.Int32]");
      }
      // class A3<T3, T4>
      {
        var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A3", 2);
        classEntity.BaseClass.ToString().ShouldEqual("global::System.Object");
        classEntity.BaseInterfaces.Count.ShouldEqual(0);
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

      // class A1 : System.Object
      {
        var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A1");
        var baseTypeRef = classEntity.BaseTypeReferences.ToArray()[0];
        baseTypeRef.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        baseTypeRef.TargetEntity.ToString().ShouldEqual("global::System.Object");
      }
      // class A2 : System.Collections.Generic.Dictionary<int,long>
      {
        var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A2");
        var baseTypeRef = classEntity.BaseTypeReferences.ToArray()[0];
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

      // class A1 // implicitly : System.Object
      {
        var entity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A1");
        entity.BaseClass.FullyQualifiedName.ShouldEqual("System.Object");
      }
      // struct A2 // implicitly: System.ValueType
      {
        var entity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<StructEntity>("A2");
        entity.BaseClass.FullyQualifiedName.ShouldEqual("System.ValueType");
      }
      // enum A3 // implicitly: System.Enum
      {
        var entity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<EnumEntity>("A3");
        entity.BaseClass.FullyQualifiedName.ShouldEqual("System.Enum");
      }
      // delegate void A4(); // implicitly: System.MulticastDelegate
      {
        var entity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<DelegateEntity>("A4");
        entity.BaseClass.FullyQualifiedName.ShouldEqual("System.MulticastDelegate");
      }
      // class A5<T>
      {
        var entity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<TypeEntity>("A5", 1);
        entity.BaseClass.FullyQualifiedName.ShouldEqual("System.Object");
      }
      // struct A6<T>
      {
        var entity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<TypeEntity>("A6", 1);
        entity.BaseClass.FullyQualifiedName.ShouldEqual("System.ValueType");
      }
      // A5<int> a5;
      {
        var entity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A8");
        var field = entity.GetMember<FieldEntity>("a5");
        field.Type.BaseClass.FullyQualifiedName.ShouldEqual("System.Object");
      }
      // A6<int> a6;
      {
        var entity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A8");
        var field = entity.GetMember<FieldEntity>("a6");
        field.Type.BaseClass.FullyQualifiedName.ShouldEqual("System.ValueType");
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
      factory.ImportTypesIntoSemanticGraph(Assembly.GetAssembly(typeof(int)).Location, "global");
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
      keyedCollection.BaseClass.TemplateEntity.ShouldEqual(collection);
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
      factory.ImportTypesIntoSemanticGraph(Assembly.GetAssembly(typeof(int)).Location, "global");
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

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("B");
      var baseTypeRef = classEntity.BaseTypeReferences.ToArray()[0];
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

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");

      int i = 0;

      // global::A x1;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("global::A");
      }
      // global::B<int> x2;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("global::B`1[global::System.Int32]");
      }
      // global::C.D x3;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("global::C.D");
      }
      // global::C.E<int> x4;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.TypeReference.TargetEntity.ToString().ShouldEqual("global::C.E`1[global::System.Int32]");
      }
      // global::C.E<int>.F<long> x5;
      {
        var fieldEntity = classEntity.Members.ToArray()[i++] as FieldEntity;
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

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");

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
    /// Using alias resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingAlias()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\UsingAlias.cs");
      InvokeParser(project).ShouldBeTrue();

      var global = project.SemanticGraph.GlobalNamespace;

      var namespaceA = global.GetChildNamespace("A");
      var classB = namespaceA.GetSingleChildType<ClassEntity>("B");
      var namespaceC = global.GetChildNamespace("C");
      var classD = namespaceC.GetSingleChildType<ClassEntity>("D");
      var usingAliasE = namespaceC.GetUsingAliasByNameAndSourcePoint("E", classD.SyntaxNodes[0].SourcePoint);

      usingAliasE.AliasedNamespace.ShouldEqual(namespaceA);
      usingAliasE.AliasedType.ShouldBeNull();

      classD.BaseClass.ShouldEqual(classB);
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

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<TypeEntity>("A");

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

      classEntity = project.SemanticGraph.GlobalNamespace.GetChildNamespace("B").GetChildNamespace("C").ChildTypes.ToList()[0];

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
        var enumEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<EnumEntity>("A");
        enumEntity.UnderlyingTypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        enumEntity.UnderlyingTypeReference.TargetEntity.FullyQualifiedName.ShouldEqual("System.Int32");
        (enumEntity.Members.ToList()[0] as EnumMemberEntity).TypeReference.ShouldEqual(enumEntity.UnderlyingTypeReference);
      }
      {
        var enumEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<EnumEntity>("B");
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
        var delegateEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<DelegateEntity>("A");
        delegateEntity.ReturnTypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        delegateEntity.ReturnType.FullyQualifiedName.ShouldEqual("System.Int32");
      }
      {
        var delegateEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<DelegateEntity>("B");
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

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");

      int i = 0;

      // int B { get; set; }
      {
        var propertyEntity = classEntity.Members.ToList()[i] as PropertyEntity;
        propertyEntity.IsInvocable.ShouldBeFalse();

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

      i++;

      // D DP { get; set; } // D is a delegate, so DP is invocable
      {
        var propertyEntity = classEntity.Members.ToList()[i] as PropertyEntity;
        propertyEntity.IsInvocable.ShouldBeTrue();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Field type resolution.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Field()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\Field.cs");
      InvokeParser(project).ShouldBeTrue();

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");

      int i = 0;

      // int a;
      {
        var fieldEntity = classEntity.Members.ToList()[i] as FieldEntity;
        fieldEntity.IsInvocable.ShouldBeFalse();

        // Check member type resolution
        fieldEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        fieldEntity.Type.FullyQualifiedName.ShouldEqual("System.Int32");

        // Check that the member can be retrieved by name.
        classEntity.GetMember<FieldEntity>(fieldEntity.Name).ShouldEqual(fieldEntity);
      }

      i++;

      // D d; // D is a delegate, so "d" is invocable
      {
        var fieldEntity = classEntity.Members.ToList()[i] as FieldEntity;
        fieldEntity.IsInvocable.ShouldBeTrue();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// ConstantMember type resolution.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ConstantMember()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\ConstantMember.cs");
      InvokeParser(project).ShouldBeTrue();

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");

      int i = 0;

      // const int a = 0;
      {
        var constantEntity = classEntity.Members.ToList()[i] as ConstantMemberEntity;
        constantEntity.IsInvocable.ShouldBeFalse();

        // Check member type resolution
        constantEntity.TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        constantEntity.Type.FullyQualifiedName.ShouldEqual("System.Int32");

        // Check that the member can be retrieved by name.
        classEntity.GetMember<ConstantMemberEntity>(constantEntity.Name).ShouldEqual(constantEntity);
      }

      i++;

      // const D d = null; // D is a delegate, so "d" is invocable
      {
        var constantEntity = classEntity.Members.ToList()[i] as ConstantMemberEntity;
        constantEntity.IsInvocable.ShouldBeTrue();
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

      var classEntity = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A",2);
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
        (parameters[0].Type as ClassEntity).TemplateEntity.ShouldEqual(classEntity);
        parameters[1].TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        parameters[1].Type.ShouldEqual(classEntity.GetOwnTypeParameterByName("T1"));
        parameters[2].TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        parameters[2].Type.ShouldEqual(methodEntity.GetOwnTypeParameterByName("T2"));
        parameters[3].TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        parameters[3].Type.ShouldEqual(methodEntity.GetOwnTypeParameterByName("T3"));

        classEntity.GetMethod(methodEntity.Signature).ShouldEqual(methodEntity);

        // Check TypeParameterMap
        var typeParameterMap = methodEntity.TypeParameterMap;
        typeParameterMap.Count.ShouldEqual(4);
        var typeParameters = typeParameterMap.TypeParameters.ToList();
        typeParameters[0].ToString().ShouldEqual("global::A`2.T1");
        typeParameters[1].ToString().ShouldEqual("global::A`2.T2");
        typeParameters[2].ToString().ShouldEqual("T2");
        typeParameters[3].ToString().ShouldEqual("T3");
        var typeArguments = typeParameterMap.TypeArguments.ToList();
        typeArguments[0].ShouldBeNull();
        typeArguments[1].ShouldBeNull();
        typeArguments[2].ShouldBeNull();
        typeArguments[3].ShouldBeNull();
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
        methodEntity.Interface.ToString().ShouldEqual("global::I`2[global::A`2.T1,global::A`2.T2]");

        // Check parameter type resolution
        var parameters = methodEntity.Parameters.ToList();
        parameters[0].TypeReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        parameters[0].Type.TemplateEntity.ShouldEqual(classEntity);
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
        methodEntity.Interface.ToString().ShouldEqual("global::I`2[global::A`2.T1,global::A`2.T2]");

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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0122: 'N.InternalClass' is inaccessible due to its protection level.
    /// Testing with a qualified reference: "using X2 = N.InternalClass;"
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0122_ClassInaccessible_Qualified()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0122_ClassInaccessible_Qualified.cs");
      project.AddAssemblyReference(TestAssemblyPathAndFilename);
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0122");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0122: 'N.InternalClass' is inaccessible due to its protection level.
    /// Testing with an unqualified reference: "using X2 = InternalClass;"
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0122_ClassInaccessible_Unqualified()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0122_ClassInaccessible_Unqualified.cs");
      project.AddAssemblyReference(TestAssemblyPathAndFilename);
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0122");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Testing that when looking for a nested type in base classes, the accessible one 
    /// beats the inaccessible even if it's in the less derived class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NestedAccessibleType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\NestedAccessibleType.cs");
      InvokeParser(project).ShouldBeTrue();

      var global = project.SemanticGraph.GlobalNamespace;
      var classA = global.GetSingleChildType<ClassEntity>("A");
      var member = classA.GetMember<FieldEntity>("a");
      member.Type.ShouldEqual(global.GetSingleChildType<ClassEntity>("C").GetSingleChildType<ClassEntity>("Nested"));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Testing that when only an inaccessible type is found then CS0122 is reported.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [Ignore] // TODO: CS0246 is reported which seems like conforming to the spec, but csc.exe reports CS0122.
    public void NestedInaccessibleType_Unresolvable()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\NestedInaccessibleType_Unresolvable.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0122");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Testing that when an inaccessible type is found but also an imported type is found with the
    /// same name, then the imported type wins.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NestedInaccessibleType_Resolvable()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\NestedInaccessibleType_Resolvable.cs");
      InvokeParser(project).ShouldBeTrue();

      var global = project.SemanticGraph.GlobalNamespace;
      var classA = global.GetSingleChildType<ClassEntity>("A");
      var member = classA.GetMember<FieldEntity>("a");
      member.Type.ShouldEqual(global.GetChildNamespace("N").GetSingleChildType<ClassEntity>("Nested"));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0547: 'a': property or indexer cannot have void type
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0547_VoidProperty()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0547_VoidProperty.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0547");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0670: Field cannot have void type
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0670_VoidField()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0670_VoidField.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0670");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS1547: Const member cannot be type 'void'
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS1547_VoidConst()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS1547_VoidConst.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS1547");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the resolution of types in constructed class members.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ConstructedClassMembers()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\ConstructedClassMembers.cs");
      InvokeParser(project).ShouldBeTrue();

      var classA = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A", 2);

      // A<int, long> a0;
      {
        var field = classA.GetMember<FieldEntity>("a0");
        var type = field.Type as ClassEntity;
        type.IsGeneric.ShouldBeTrue();
        type.IsUnbound.ShouldBeFalse();
        type.IsConstructed.ShouldBeTrue();

        type.TemplateEntity.ToString().ShouldEqual("global::A`2");
        type.ToString().ShouldEqual("global::A`2[global::System.Int32,global::System.Int64]");

        (type.TemplateEntity as ClassEntity).BaseClass.ToString().ShouldEqual("global::B`1[global::A`2.T1]");
        var baseClass = type.BaseClass;
        baseClass.ToString().ShouldEqual("global::B`1[global::System.Int32]");
        baseClass.TypeParameterMap.Count.ShouldEqual(1);
        baseClass.TypeParameterMap.TypeParameters.First().ToString().ShouldEqual("global::B`1.T");
        baseClass.TypeParameterMap.TypeArguments.First().ToString().ShouldEqual("global::System.Int32");

        var typeArguments = type.TypeParameterMap.TypeArguments.ToList();
        typeArguments[0].BuiltInTypeValue.ShouldEqual(BuiltInType.Int);
        typeArguments[1].BuiltInTypeValue.ShouldEqual(BuiltInType.Long);
      }

      // T1 a1;
      {
        var member = classA.GetMember<FieldEntity>("a1");
        member.TemplateEntity.ShouldBeNull();

        var type = member.Type as TypeParameterEntity;
        type.ToString().ShouldEqual("global::A`2.T1");
        type.IsGeneric.ShouldBeFalse();
        type.IsUnbound.ShouldBeFalse();
        type.IsConstructed.ShouldBeFalse();
        type.TemplateEntity.ShouldBeNull();
      }

      // B<T2> a2 { get; set; }
      {
        var member = classA.GetMember<PropertyEntity>("a2");
        member.Type.ToString().ShouldEqual("global::B`1[global::A`2.T2]");
      }

      // T1? a3;
      {
        var member = classA.GetMember<FieldEntity>("a3");
        member.Type.ToString().ShouldEqual("global::System.Nullable`1[global::A`2.T1]");
      }

      // A<int, T2> M1(T1[] p1, A<int, B<T2>> p2) 
      {
        var member = classA.GetMember<MethodEntity>("M1");
        member.ReturnType.ToString().ShouldEqual("global::A`2[global::System.Int32,global::A`2.T2]");
        var parameters = member.Parameters.ToList();
        {
          var paramType = parameters[0].Type as ArrayTypeEntity;
          paramType.ToString().ShouldEqual("global::A`2.T1[]");
          paramType.UnderlyingType.ToString().ShouldEqual("global::A`2.T1");
          paramType.Rank.ShouldEqual(1);
          paramType.IsOpen.ShouldBeTrue();
          paramType.IsUnbound.ShouldBeTrue();
          paramType.IsConstructed.ShouldBeFalse();
        }
        {
          var paramType = parameters[1].Type as ClassEntity;
          paramType.ToString().ShouldEqual("global::A`2[global::System.Int32,global::B`1[global::A`2.T2]]");
          paramType.IsOpen.ShouldBeTrue();
          paramType.IsUnbound.ShouldBeFalse();
          paramType.IsConstructed.ShouldBeTrue();
        }
      }

      // A<int, long> a0 --> T1 a1;
      {
        var member = classA.GetMember<FieldEntity>("a0").Type.GetMember<FieldEntity>("a1");
        member.Type.ToString().ShouldEqual("global::System.Int32");
      }

      // A<int, long> a0 --> B<T2> a2 { get; set; }
      {
        var member = classA.GetMember<FieldEntity>("a0").Type.GetMember<PropertyEntity>("a2");
        member.Type.ToString().ShouldEqual("global::B`1[global::System.Int64]");
      }

      // A<int, long> a0 --> T1? a3;
      {
        var member = classA.GetMember<FieldEntity>("a0").Type.GetMember<FieldEntity>("a3");
        member.Type.IsNullableType.ShouldBeTrue();
        member.Type.UnderlyingOfNullableType.ToString().ShouldEqual("global::System.Int32");
        member.Type.ToString().ShouldEqual("global::System.Nullable`1[global::System.Int32]");
      }

      // A<int, long> a0 --> A<int, T2> M1(T1[] p1, A<int, B<T2>> p2) 
      {
        var member = classA.GetMember<FieldEntity>("a0").Type.GetMember<MethodEntity>("M1");
        member.ReturnType.ToString().ShouldEqual("global::A`2[global::System.Int32,global::System.Int64]");
        var parameters = member.Parameters.ToList();
        {
          var paramType = parameters[0].Type as ArrayTypeEntity;
          paramType.ToString().ShouldEqual("global::System.Int32[]");
          paramType.UnderlyingType.ToString().ShouldEqual("global::System.Int32");
          paramType.Rank.ShouldEqual(1);
          paramType.IsOpen.ShouldBeFalse();
          paramType.IsConstructed.ShouldBeFalse();
        }
        {
          var paramType = parameters[1].Type as ClassEntity;
          paramType.ToString().ShouldEqual("global::A`2[global::System.Int32,global::B`1[global::System.Int64]]");
          paramType.IsOpen.ShouldBeFalse();
          paramType.IsUnbound.ShouldBeFalse();
          paramType.IsConstructed.ShouldBeTrue();
        }
      }

      var classA2 = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A2", 2);

      // A2<T2, T1> a1;
      {
        var field = classA2.GetMember<FieldEntity>("a1");
        var type = field.Type as ClassEntity;
        type.TemplateEntity.ToString().ShouldEqual("global::A2`2");
        type.ToString().ShouldEqual("global::A2`2[global::A2`2.T2,global::A2`2.T1]");
      }

      // A2<byte, char> a2;
      {
        var field = classA2.GetMember<FieldEntity>("a2");
        var type = field.Type as ClassEntity;
        type.TemplateEntity.ToString().ShouldEqual("global::A2`2");
        type.ToString().ShouldEqual("global::A2`2[global::System.Byte,global::System.Char]");
      }

      // A2<T2, T1> a1 --> A2<T2, T1> a1;
      {
        var member = classA2.GetMember<FieldEntity>("a1").Type.GetMember<FieldEntity>("a1");
        member.Type.ToString().ShouldEqual("global::A2`2[global::A2`2.T1,global::A2`2.T2]");
      }

      // A2<byte, char> a2 --> A2<T2, T1> a1;
      {
        var member = classA2.GetMember<FieldEntity>("a2").Type.GetMember<FieldEntity>("a1");
        member.Type.ToString().ShouldEqual("global::A2`2[global::System.Char,global::System.Byte]");
      }
    }
  }
}