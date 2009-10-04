// Next, interface members that are hidden by class members are removed from the set. 
// This step only has an effect if T is a type parameter and T has both an effective base class other than object 
// and a non-empty effective interface set (§10.1.5). 
// For every member S.M in the set, where S is the type in which the member M is declared, 
// the following rules are applied if S is a class declaration other than object:
// - If M is a constant, field, property, event, enumeration member, or type declaration, 
//   then all members declared in an interface declaration are removed from the set.
// - If M is a method, then all non-method members declared in an interface declaration are removed from the set, 
//   and all methods with the same signature as M declared in an interface declaration are removed from the set.

class Base
{
  public int M = 0;
}

interface I1
{
  int M { get; set; }
}

class A<T> where T : Base, I1
{
  void Test(T t)
  {
    t.M = 0;
  }
}