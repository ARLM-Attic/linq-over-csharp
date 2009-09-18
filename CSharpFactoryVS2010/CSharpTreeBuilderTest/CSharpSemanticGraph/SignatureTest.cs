using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilderTest.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// Tests signature class.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public sealed class SignatureTest
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// A full signature
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void FullSignature()
    {
      var signature = new Signature("M", 1, new List<ParameterEntity>() 
      { new ParameterEntity("a", new DirectSemanticEntityReference<TypeEntity>(new ClassEntity(null, "A")), ParameterKind.Value),
        new ParameterEntity("b", new DirectSemanticEntityReference<TypeEntity>(new ClassEntity(null, "B")), ParameterKind.Reference),
        new ParameterEntity("c", new DirectSemanticEntityReference<TypeEntity>(new ClassEntity(null, "C")), ParameterKind.Output)
      });
      signature.Name.ShouldEqual("M");
      signature.TypeParameterCount.ShouldEqual(1);
      signature.Parameters.Count().ShouldEqual(3);
      signature.ToString().ShouldEqual("M`1(A, ref B, out C)");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Null parameters
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NullParameters()
    {
      var signature = new Signature("M", 1, null);
      signature.Name.ShouldEqual("M");
      signature.TypeParameterCount.ShouldEqual(1);
      signature.Parameters.Count().ShouldEqual(0);
      signature.ToString().ShouldEqual("M`1()");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Zero type argument count parameters
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ZeroTypeArgumentCount()
    {
      var signature = new Signature("M", 0, null);
      signature.Name.ShouldEqual("M");
      signature.TypeParameterCount.ShouldEqual(0);
      signature.Parameters.Count().ShouldEqual(0);
      signature.ToString().ShouldEqual("M()");
    }
  }
}
