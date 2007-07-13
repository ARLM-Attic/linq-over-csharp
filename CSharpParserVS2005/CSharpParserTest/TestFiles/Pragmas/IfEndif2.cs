#define Debug // Debugging on
class PurchaseTransaction
{
  void Commit()
  {
#if Debug
    Console.WriteLine();
#else
    /* Do something else
#endif
  }
}