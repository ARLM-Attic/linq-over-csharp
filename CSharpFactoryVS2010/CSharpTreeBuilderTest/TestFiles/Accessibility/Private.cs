public class A
{
  private static int PrivateMember;

  private class PrivateNestedClass
  { }

  // Accessing private type
  PrivateNestedClass a1;

  // Accessing private field
  //private static int a2 = PrivateMember;
}

public class B
{
  int b1;

  // Accessing private type (not allowed)
  //A.PrivateNestedClass b1;

  // Accessing private field (not allowed)
  //private static int b2 = A.PrivateMember;
}