internal class InternalMaster
{
  private class Private { }

  private class OtherPrivate
  {
    public Private _Private;
  }

  protected class Protected
  {
    public Private _Private;
  }

  internal class Internal
  {
    public Private _Private;
  }

  protected internal class ProtectedInternal
  {
    public Private _Private;
  }

  public class Public
  {
    public Private _Private;
  }
}