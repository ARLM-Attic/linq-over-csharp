public class A
{
  protected static int ProtectedMember;

  protected class ProtectedNestedClass
  {
  }

  // Accessing protected type
  ProtectedNestedClass a1;

  // Accessing protected field
  //static int a3 = ProtectedMember;
}

public class B : A
{
  ProtectedNestedClass b1;
  //static int b3 = ProtectedMember;
}

public class C : B
{
  ProtectedNestedClass c1;
  //static int c3 = ProtectedMember;
}

public class D
{
  int d1;
  //ProtectedNestedClass d1;
  //static int d3 = ProtectedMember;
}