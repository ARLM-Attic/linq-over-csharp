// When finding nested types in base classes, the accessible one beats the inaccessible one.
class A : B
{
  Nested a;
}

class B : C
{
  new private class Nested
  { }
}

class C
{
  public class Nested
  { }
}