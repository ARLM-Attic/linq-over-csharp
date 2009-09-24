class A : B
{
  protected int B
  {
    get { return 0; }
    private set { }
  }
  
  int C { get; set; }

  static int D { get; set; }

  public virtual int E { get; set; }

  public override int F { get; set; }

  public new int G { get; set; }
}

class B
{
  public virtual int F { get; set; }
  public int G { get; set; }
}

interface I
{
  int I { get; set; }
}