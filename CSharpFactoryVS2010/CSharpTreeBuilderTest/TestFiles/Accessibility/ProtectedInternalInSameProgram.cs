public class A
{
  protected internal static int ProtectedInternalMember;

  protected internal class ProtectedInternalNestedClass
  {
  }
}

public class B
{
  A.ProtectedInternalNestedClass b1;
//  static int b3 = A.ProtectedInternalMember;
}
