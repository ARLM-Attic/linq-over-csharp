public class A
{
  public static int PublicMember;

  public class PublicNestedClass
  {
  }

  // Accessing protected type
  PublicNestedClass a1;

  // Accessing protected field
  //static int a2 = PublicMember;
}

public class B
{
  A.PublicNestedClass b1;
  //static int b2 = A.PublicMember;
}
