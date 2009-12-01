class A
{
  static int a1 = B.b;
}

class B
{
  private  int b = 0;
}

// error CS0122: 'B.b' is inaccessible due to its protection level
