// If M is a method, then all non-method members declared in a base type of S are removed from the set.

class Base3
{
  protected void M() { } // Hidden by Base2+M
}

class Base2 : Base3
{
  protected class M { } // Hidden by Base1.M
}

class Base1 : Base2
{
  protected int M; // Hidden by Base0.M()
}

class Base0 : Base1
{
  protected static void M() { } // Not hidden
}

class S : Base0
{
  static void M() { } // Hides all but Base0.M()

  const int test = 0; 


}