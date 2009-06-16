using System;

class PurchaseTransaction
{
  void Commit()
  {
#if false
    /* This is syntactically legal
#elif true
    Console.WriteLine("This");
#elif true
    Environment.Exit(1);
    Environment.Exit(1);
#else
    Environment.Exit(1);
#endif
  }
}