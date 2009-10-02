class A
{
  // Looking up members of B (context entity) at this field (accessing entity) by the name "C".
  B b;
}

class B
{
  // This member will be selected for type parameter count == 0.
  public class C { }

  // This member will be selected for type parameter count == 1.
  public class C<T> { }
}
