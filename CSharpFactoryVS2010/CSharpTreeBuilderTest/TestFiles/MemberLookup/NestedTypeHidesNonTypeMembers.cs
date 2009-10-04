// If M is a type declaration, then all non-types declared in a base type of S are removed from the set, 
// and all type declarations with the same number of type parameters as M declared in a base type of S are removed from the set.

class Base2
{
  protected enum M {} // Hidden by class M
  protected struct M2<T> { } // Hidden by class M2<T>
}

class Base1 : Base2
{
  protected static void M() { } // Hidden by class M
  protected static void M2<T>() { } // Hidden by class M2<T>
}

class S : Base1
{
  class M {} // hides all non-type members and type members with 0 type params

  class M2<T> { } // hides all non-type members and type members with 1 type params 

  const int test = 0; // this is used as accessing entity, because member access is not yet implemented
}