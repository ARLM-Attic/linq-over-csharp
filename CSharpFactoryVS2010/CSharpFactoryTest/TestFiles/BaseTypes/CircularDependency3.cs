class E : F.G { }
class F : E
{
  public class G { }
}

class Master
{
  class Inner
  {
    class E : F.G { }
    class F : E
    {
      public class G { }
    }
  }
}