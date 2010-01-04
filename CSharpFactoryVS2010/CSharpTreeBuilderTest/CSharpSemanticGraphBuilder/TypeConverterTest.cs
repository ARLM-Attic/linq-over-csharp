using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the type conversion logic.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public sealed class TypeConverterTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the identity conversion.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void IdentityConversion()
    {
      var project = new CSharpProject(WorkingFolder);
      InvokeParser(project);

      var arrayClass = project.SemanticGraph.GetEntityByMetadataObject(typeof(System.Array)) as TypeEntity;
      var valueTypeClass = project.SemanticGraph.GetEntityByMetadataObject(typeof(System.ValueType)) as TypeEntity;

      TypeConverter.ImplicitConversionExists(arrayClass, arrayClass).ShouldBeTrue();
      TypeConverter.ImplicitConversionExists(arrayClass, valueTypeClass).ShouldBeFalse();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the implicit numeric conversion.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ImplicitNumericConversion()
    {
      var project = new CSharpProject(WorkingFolder);
      InvokeParser(project);

      var classFloat = project.SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Float);
      var classDouble = project.SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Double);

      TypeConverter.ImplicitConversionExists(classFloat, classDouble).ShouldBeTrue();
      TypeConverter.ImplicitConversionExists(classDouble, classFloat).ShouldBeFalse();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the implicit enumeration conversion.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ImplicitEnumerationConversion()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeConverter\ImplicitEnumerationConversion.cs");
      InvokeParser(project);

      var classA = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");

      // E e1 = 0;     // success
      {
        var field = classA.GetOwnMember<FieldEntity>("e1");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // E? e2 = 0;    // success
      {
        var field = classA.GetOwnMember<FieldEntity>("e2");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // X<E> e3 = 0;  // fails, not enum, or nullable enum
      {
        var field = classA.GetOwnMember<FieldEntity>("e3");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeFalse();
      }
      // E e4 = 1;     // fails, not decimal zero
      {
        var field = classA.GetOwnMember<FieldEntity>("e4");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeFalse();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the implicit nullable conversion.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ImplicitNullableConversion()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeConverter\ImplicitNullableConversion.cs");
      InvokeParser(project);

      var classA = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");

      // static int? i1 = 1;
      {
        var field = classA.GetOwnMember<FieldEntity>("i1");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      //  static float? i2 = 2;
      {
        var field = classA.GetOwnMember<FieldEntity>("i2");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static double? i3 = i2;
      {
        var field = classA.GetOwnMember<FieldEntity>("i3");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static float? i4 = i3;  // fails
      {
        var field = classA.GetOwnMember<FieldEntity>("i4");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeFalse();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the null literal conversion.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NullLiteralConversion()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeConverter\NullLiteralConversion.cs");
      InvokeParser(project);

      var classA = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");

      // static int? i1 = null;
      {
        var field = classA.GetOwnMember<FieldEntity>("i1");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static int i2 = null; // fails
      {
        var field = classA.GetOwnMember<FieldEntity>("i2");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeFalse();
      }
      // static A i3 = null;
      {
        var field = classA.GetOwnMember<FieldEntity>("i3");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the implicit reference conversion.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ImplicitReferenceConversion()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeConverter\ImplicitReferenceConversion.cs");
      InvokeParser(project);

      var classA = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");

      // static object i1 = a;   // From any reference-type to object.
      {
        var field = classA.GetOwnMember<FieldEntity>("i1");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static Base1 i2 = a;     // From any class-type S to any class-type T, provided S is derived from T.
      {
        var field = classA.GetOwnMember<FieldEntity>("i2");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static Base2 i3 = a;     // From any class-type S to any class-type T, provided S is derived from T. (indirectly)
      {
        var field = classA.GetOwnMember<FieldEntity>("i3");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static NonBase i4 = a;  // Fails
      {
        var field = classA.GetOwnMember<FieldEntity>("i4");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeFalse();
      }
      // static I1 i3 = a;       // From any class-type S to any interface-type T, provided S implements T.
      {
        var field = classA.GetOwnMember<FieldEntity>("i5");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static I2 i4 = a;       // From any class-type S to any interface-type T, provided S implements T. (indirectly)
      {
        var field = classA.GetOwnMember<FieldEntity>("i6");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static I2 i5 = i3;      // From any interface-type S to any interface-type T, provided S is derived from T.
      {
        var field = classA.GetOwnMember<FieldEntity>("i7");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static INotImplemented i8 = a;  // Fails
      {
        var field = classA.GetOwnMember<FieldEntity>("i8");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeFalse();
      }

      // static A[] arr3 = arr1;      // Success
      {
        var field = classA.GetOwnMember<FieldEntity>("arr3");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static Base1[] arr4 = arr1;  // Success
      {
        var field = classA.GetOwnMember<FieldEntity>("arr4");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static Base2[] arr5 = arr1;  // Success
      {
        var field = classA.GetOwnMember<FieldEntity>("arr5");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static A[] arr6 = arr2;      // Fails, rank mismatch
      {
        var field = classA.GetOwnMember<FieldEntity>("arr6");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeFalse();
      }
      //  static System.Array arr7 = arr2;  // Success
      {
        var field = classA.GetOwnMember<FieldEntity>("arr7");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static System.ICloneable arr8 = arr2; // Success
      {
        var field = classA.GetOwnMember<FieldEntity>("arr8");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      
      // static System.Collections.Generic.IList<A> arr9 = arr1;
      {
        var field = classA.GetOwnMember<FieldEntity>("arr9");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static System.Collections.Generic.IList<Base1> arr10 = arr1;
      {
        var field = classA.GetOwnMember<FieldEntity>("arr10");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static System.Collections.Generic.IEnumerable<A> arr11 = arr1;
      {
        var field = classA.GetOwnMember<FieldEntity>("arr11");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static System.Collections.IEnumerable arr12 = arr1;
      {
        var field = classA.GetOwnMember<FieldEntity>("arr12");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static System.Collections.Generic.IEnumerable<Base1> arr13 = arr1;
      {
        var field = classA.GetOwnMember<FieldEntity>("arr13");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeTrue();
      }
      // static System.Collections.Generic.IEnumerable<NonBase> arr14 = arr1;
      {
        var field = classA.GetOwnMember<FieldEntity>("arr14");
        var type = field.Type;
        var expression = (field.Initializer as ScalarInitializerEntity).Expression;
        TypeConverter.ImplicitConversionExists(expression, type).ShouldBeFalse();
      }
    }
  }
}