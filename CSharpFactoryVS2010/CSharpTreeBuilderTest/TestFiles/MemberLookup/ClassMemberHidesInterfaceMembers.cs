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
  // Hides I1.M1, I2.M1, I3.M1
  public int M1 = 0;
  // Hides I1.M2, I2.M2, but not I3.M2 (different signature)
  public void M2()
  {
  }
}

interface I1
{
  // Hidden by Base.M1
  int M1 { get; set; }
  // Hidden by Base.M2
  void M2();
}

interface I2
{
  // Hidden by Base.M1
  void M1();
  // Hidden by Base.M2
  int M2 { get; set; }
}

interface I3
{
  // Hidden by Base.M1
  void M1(int a);
  // Visible
  void M2(int a);
}

class A<T> where T : Base, I1, I2, I3
{
  // Selecting members of T (contextType) that are accessible in class A, named "M1", "M2"
  T t;

  void Test(T t)
  {
    t.M1 = 0;
    t.M2();
    t.M2(1);
  }
}