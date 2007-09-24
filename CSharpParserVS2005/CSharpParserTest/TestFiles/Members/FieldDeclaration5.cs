public class MyClass1
{
  private MyFieldClass1 Field;
}

public class MyFieldClass1 {}

public class MasterClass
{
  public class Subclass
  {
  }
}

public class MasterClass1: MasterClass
{
}

public class MasterClass2: MasterClass1
{
  private Subclass field;
}