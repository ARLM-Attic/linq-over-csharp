using X1 = PublicClass;
using X2 = PublicClass.PublicNestedClass;
using X3 = PublicClass.InternalNestedClass;
using X4 = PublicClass.ProtectedInternalNestedClass;
//using X = PublicClass.ProtectedClass;
//using X = PublicClass.PrivateClass;

using X5 = InternalClass;
using X6 = InternalClass.PublicNestedInInternalClass;

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
}

internal class InternalClass
{
  public class PublicNestedInInternalClass
  { }
}