internal class InternalMaster
{
  public class Public { }

  private class Private
  {
    public Public _Public;
  }

  protected class Protected
  {
    public Public _Public;
  }

  internal class Internal
  {
    public Public _Public;
  }

  protected internal class ProtectedInternal
  {
    public Public _Public;
  }

  public class OtherPublic
  {
    public Public _Public;
  }
}