class A : B
{
  public C c = new C();

  public class C { }        // A+C
}

class B
{
  public class C { }
}

// warning CS0108: 'A.C' hides inherited member 'B.C'. Use the new keyword if hiding was intended.
