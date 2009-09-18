// Nested types

class A : Parent
{
  class B
  {}

  public class B2
  {}

  internal class B3
  { }

  protected class B4
  { }

  protected internal class B5
  { }

  private class B6
  { }

  new class B7
  { }
}

// Not nested types

namespace C
{
  namespace C2
  {
  }

  class D:A
  {
  }
}

public class A2
{ }

internal class A3
{ }

static class A4
{}

abstract class A5
{}

sealed class A6
{}

class Parent
{
  public int B7;
}