public class A 
{
  int a1;
  //PublicClass.ProtectedInternalNestedClass b1;
  //static int b3 = PublicClass.protectedInternalNestedMember;
}

public class B : PublicClass
{
  PublicClass.ProtectedInternalNestedClass b1;
  //PublicClass.InternalNestedClass b2;
  //static int b3 = PublicClass.protectedInternalNestedMember;
  //static int b4 = PublicClass.internalNestedMember;
}
