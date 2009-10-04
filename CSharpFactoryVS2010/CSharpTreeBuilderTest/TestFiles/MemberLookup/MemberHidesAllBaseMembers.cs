// If M is a constant, field, property, event, or enumeration member, 
// then all members declared in a base type of S are removed from the set.

class Base2
{
  protected class M {} // Hidden by const int M
}

class Base1 : Base2
{
  protected static void M() { } // Hidden by const int M
}

class S : Base1
{
  const int M = 0; // hides all base members

//const int test = S.M; // S.M denotes const int M
  const int test = 0; // this is used as accessing entity, because member access is not yet implemented
}