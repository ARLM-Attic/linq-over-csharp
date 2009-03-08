using System.Collections.Generic;

public class A<T> : Dictionary<U, V> {}

public class B<U, V>
{
  public class C: Dictionary<U, V> {}
}