public class MyClass
{
  internal int InternalProperty1
  {
    public get { return 0; }
    set { return; }
  }

  internal int InternalProperty2
  {
    protected get { return 0; }
    set { return; }
  }

  internal int InternalProperty3
  {
    internal get { return 0; }
    set { return; }
  }

  internal int InternalProperty4
  {
    protected internal get { return 0; }
    set { return; }
  }

  internal int InternalProperty5
  {
    private get { return 0; }
    set { return; }
  }
}