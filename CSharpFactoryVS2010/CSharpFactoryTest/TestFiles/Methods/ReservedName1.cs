public class MyClass<T>
{
  public int IntProperty { get { return 0; } }
  public int get_IntProperty() {}
  public void set_IntProperty(int a) { }

  public T TProperty { get { return default(T); } }
  public int get_TProperty() {}
  public string set_TProperty(T par) {}
}