using C = B;

class A
{
  C::D x1;
}

class B
{
  public class D{}
}

// error CS0431: Cannot use alias 'C' with '::' since the alias references a type. Use '.' instead.