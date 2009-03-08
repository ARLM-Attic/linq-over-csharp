public class PublicMaster
{
  internal class Internal { }

  private class Private
  {
    public Internal _Internal;
  }

  protected class Protected
  {
    public Internal _Internal;
  }

  internal class OtherInternal
  {
    public Internal _Internal;
  }

  protected internal class ProtectedInternal
  {
    public Internal _Internal;
  }

  public class Public
  {
    public Internal _Internal;
  }
}