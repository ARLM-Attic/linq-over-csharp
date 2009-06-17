class PurchaseTransaction
{
  void Commit()
  {

#if false
    /* This is syntactically legal
#elif false
    Console.WriteLine();
  // --- All these pragmas are on the "false" branch, so they are not taken into account as pragmas,
  // --- but only as simple tokens!!!
  #if false
    Console.WriteLine();
    Console.WriteLine();
  #elif true
    #if Debug
      Console.WriteLine();
    #elif Debug
      Console.WriteLine();
    #else
      Console.WriteLine("This");
      Console.WriteLine("This");
      Console.WriteLine("This");
    #endif
  #elif true
    System.Console.WriteLine();
    System.Console.WriteLine();
  #endif
  // --- End of "false" branch
#elif true
  // --- This is the "true" branch, so all pragmas are taken into account as real pragmas.
  #if false
    Console.WriteLine();
    Console.WriteLine();
  #elif true
    #if Debug
      Console.WriteLine();
    #elif Debug
      Console.WriteLine();
    #else
      Console.WriteLine("This");
      Console.WriteLine("This");
      Console.WriteLine("This");
    #endif
  // --- Although the next "#elif" evaluates to true, it is taken into account as false, because
  // --- A preceding "#elif" already evaluated to true.
  #elif true
    Console.WriteLine();
    Console.WriteLine();
  #endif
#else
    Console.WriteLine();
#endif
  }
}