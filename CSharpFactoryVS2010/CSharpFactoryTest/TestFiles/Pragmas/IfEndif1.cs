#define Debug // Debugging on
#undef Trace // Tracing off
class PurchaseTransaction
{
  void Commit()
  {
#if Debug
    CheckConsistency();
  #if Trace
    #if Debug
    WriteToLog(this.ToString());
    #endif
  #endif
#endif
    CommitHelper();
  }
}