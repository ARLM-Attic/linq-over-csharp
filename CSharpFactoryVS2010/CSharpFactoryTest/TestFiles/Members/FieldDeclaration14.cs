internal class InternalMaster
{
  protected internal class ProtectedInternal { }

  private class Private
  {
    public ProtectedInternal _ProtectedInternal;
  }

  protected class OtherProtected
  {
    public ProtectedInternal _ProtectedInternal;
  }

  internal class Internal
  {
    public ProtectedInternal _ProtectedInternal;
  }

  protected internal class OtherProtectedInternal
  {
    public ProtectedInternal _ProtectedInternal;
  }

  public class Public
  {
    public ProtectedInternal _ProtectedInternal;
  }
}