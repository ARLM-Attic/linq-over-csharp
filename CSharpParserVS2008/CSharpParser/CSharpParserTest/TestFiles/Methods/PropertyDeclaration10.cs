public class MyClass
{
  protected internal int ProtectedProperty1
  {
    public get { return 0; }
    set { return; }
  }

  protected internal int ProtectedProperty2
  {
    protected get { return 0; }
    set { return; }
  }

  protected internal int ProtectedProperty3
  {
    internal get { return 0; }
    set { return; }
  }

  protected internal int ProtectedProperty4
  {
    protected internal get { return 0; }
    set { return; }
  }

  protected internal int ProtectedProperty5
  {
    private get { return 0; }
    set { return; }
  }
}