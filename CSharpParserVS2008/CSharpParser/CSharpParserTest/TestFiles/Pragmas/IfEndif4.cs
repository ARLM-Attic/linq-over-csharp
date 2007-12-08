#define Debug // Debugging on
class PurchaseTransaction
{
  void Commit()
  {

#if true
    Console.WriteLine("This");
#else
    /* This is syntactically legal
#endif
  }
}