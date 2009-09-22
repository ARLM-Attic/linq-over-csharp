class A : B
{
  Nested a;
}

class B
{
  private class Nested { }
}

// error CS0122: 'B.Nested' is inaccessible due to its protection level