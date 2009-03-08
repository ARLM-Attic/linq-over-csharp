internal class InternalMaster
{
  protected class Protected { }

  private class Private
  {
    public Protected _Protected;
  }

  protected class OtherProtected
  {
    public Protected _Protected;
  }

  internal class Internal
  {
    public Protected _Protected;
  }

  protected internal class ProtectedInternal
  {
    public Protected _Protected;
  }

  public class Public
  {
    public Protected _Protected;
  }
}