public class MyClass
{
  protected int ProtectedProperty1
  {
    public get { return 0; }
    set { return; }
  }

  protected int ProtectedProperty2
  {
    protected get { return 0; }
    set { return; }
  }

  protected int ProtectedProperty3
  {
    internal get { return 0; }
    set { return; }
  }

  protected int ProtectedProperty4
  {
    protected internal get { return 0; }
    set { return; }
  }

  protected int ProtectedProperty5
  {
    private get { return 0; }
    set { return; }
  }
}