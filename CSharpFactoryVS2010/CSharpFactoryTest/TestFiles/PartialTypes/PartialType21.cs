partial class A
{
  partial class B
  {
    abstract partial class C {}
  }
}

sealed partial class A
{
  static partial class B
  {
    partial class C { }
    public partial class C { }
  }
}