// ================================================================================================
// CSharp9ProjectContentProviderTests.cs
//
// Created: 2009.03.05, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.ProjectProvider
{
  [TestClass]
  public class CSharp9ProjectContentProviderTests
  {
    private const string csProjFile =
      @"C:\Work\LINQOverCSharp\CSharpFactoryVS2010\CSharpFactoryTest\ProjectProvider\WinFormsAppTest\TestWinApp.csproj.test";

    [TestMethod]
    public void CSharp9ProviderParseProjectOk()
    {
      var provider = new CSharp9ProjectContentProvider(csProjFile);
      Assert.AreEqual(provider.SourceFiles.Count, 6);
      Assert.AreEqual(provider.References.Count, 10);
    }
  }
}
