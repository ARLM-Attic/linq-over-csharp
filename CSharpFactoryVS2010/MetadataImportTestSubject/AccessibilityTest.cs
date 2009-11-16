public class PublicClass
{
  public class PublicNestedClass
  { }

  internal class InternalNestedClass
  { }

  protected class ProtectedNestedClass
  { }

  protected internal class ProtectedInternalNestedClass
  { }

  private class PrivateNestedClass
  { }

  public static int publicNestedMember;
  internal static int internalNestedMember;
  protected static int protectedNestedMember;
  protected internal static int protectedInternalNestedMember;
  private static int privateNestedCMember;

}

internal class InternalClass
{
  public class PublicNestedInInternalClass
  { }
}
