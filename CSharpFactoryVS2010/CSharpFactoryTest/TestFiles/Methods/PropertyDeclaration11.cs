public class MyClass
{
  private int PrivateProperty1
  {
    public get { return 0; }
    set { return; }
  }

  private int PrivateProperty2
  {
    protected get { return 0; }
    set { return; }
  }

  private int PrivateProperty3
  {
    internal get { return 0; }
    set { return; }
  }

  private int PrivateProperty4
  {
    protected internal get { return 0; }
    set { return; }
  }

  private int PrivateProperty5
  {
    private get { return 0; }
    set { return; }
  }
}