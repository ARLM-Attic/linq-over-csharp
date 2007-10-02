public class MyClass
{
  public int A
  {
    new get { return 0; }
    new set { return; }
  }

  public int B
  {
    static get { return 0; }
    static set { return; }
  }

  public int C
  {
    readonly get { return 0; }
    readonly set { return; }
  }

  public int D
  {
    volatile get { return 0; }
    volatile set { return; }
  }

  public int E
  {
    virtual get { return 0; }
    virtual set { return; }
  }

  public int F
  {
    sealed get { return 0; }
    sealed set { return; }
  }

  public int G
  {
    override get { return 0; }
    override set { return; }
  }

  public int H
  {
    abstract get { return 0; }
    abstract set { return; }
  }

  public int I
  {
    extern get { return 0; }
    extern set { return; }
  }
}