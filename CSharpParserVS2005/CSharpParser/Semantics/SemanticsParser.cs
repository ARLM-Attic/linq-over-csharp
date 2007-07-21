using CSharpParser.ProjectModel;

namespace CSharpParser
{
  public sealed class SemanticsParser
  {
    private CompilationUnit _Project;

    public SemanticsParser(CompilationUnit project)
    {
      _Project = project;
    }

    public CompilationUnit Project
    {
      get { return _Project; }
    }

    public void CheckSemantics()
    {
    }
  }
}
