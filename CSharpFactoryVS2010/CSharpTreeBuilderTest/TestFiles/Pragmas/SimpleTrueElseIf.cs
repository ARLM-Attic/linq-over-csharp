#define B
#define D

using System;
class PurchaseTransaction
{
  void Commit()
  {
#if A
    Console.WriteLine("A");
    #if D
    
    #endif
#elif B
    Console.WriteLine("B");
#elif C
    Console.WriteLine("C");
#elif D
    Console.WriteLine("D");
#elif E
    Console.WriteLine("E");
#else
    Console.WriteLine("F");
#endif
  }
}