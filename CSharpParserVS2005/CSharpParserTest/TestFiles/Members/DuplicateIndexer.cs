public sealed class MyClass
{
  private int[] _Field;

  public int this[int index]
  {
    get { return _Field[index]; }
    set { _Field[index] = value; }
  }

  public int this[int index]
  {
    get { return _Field[index]; }
    set { _Field[index] = value; }
  }

  public int this[string key]
  {
    get { return _Field[0]; }
    set { _Field[0] = Int32.Parse(value); }
  }

  public int this[string key]
  {
    get { return _Field[0]; }
    set { _Field[0] = Int32.Parse(value); }
  }
}