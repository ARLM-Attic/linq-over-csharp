internal sealed class MyDupPropClass
{
  private int _Prop1;
  private string _Prop2;

  public int Prop1
  {
    get { return _Prop1; }
    set { _Prop1 = value; }
  }

  public string Prop2
  {
    get { return _Prop2; }
    set { _Prop2 = value; }
  }

  public int Prop2
  {
    get { return _Prop1; }
  }

  public string Prop1
  {
    set { _Prop2 = value; }
  }

  private CancelEventHandler _Handler;

  public event CancelEventHandler Handler
  {
    add { _Handler += value; }
    remove { _Handler -= value; }
  }
}