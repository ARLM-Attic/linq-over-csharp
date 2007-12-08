using System; // Comment 1   

// Comment 2
class X
{
  ~X()
  {
    Console.WriteLine("DESTRUCTOR!");
    // Comment 3
  }

  // -------------------------------------------------
  /// This is a test method
  // ------------------------------------------------
  public static int Test1()
  {
    try
    {
      // Try block
      return 8;
    }
    catch (Exception) { }
    System.Console.WriteLine("Shouldn't get here");
    return 9;
  }

  public static void Test2()
  {
    int[] vars = { 3, 4, 5 };

    foreach (int a in vars)
    {
      try
      {
        continue;
      }
      catch (Exception)
      {
        break;
      }
    }
  }

  public static void Main()
  {
    Test1();
    Test2();

    // Within the main method
    try
    {
      return;
    }
    catch (Exception) { }
    System.Console.WriteLine("Shouldn't get here");
    return;
  }
}
