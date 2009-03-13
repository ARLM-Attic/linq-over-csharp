// ================================================================================================
// FolderContentProviderTests.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.ProjectProvider
{
  [TestClass]
  public class FolderContentProviderTests
  {
    private const string csProjFolder =
      @"C:\Work\LINQOverCSharp\CSharpFactoryVS2010\CSharpFactoryTest\ProjectProvider\WinFormsAppTest";

    [TestMethod]
    public void FolderProviderParseProjectOk()
    {
      var provider = new FolderContentProvider(csProjFolder);
      Assert.AreEqual(provider.SourceFiles.Count, 6);
      Assert.AreEqual(provider.References.Count, 2);
    }
  }
}