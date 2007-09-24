public class AssemblyReader
{
  private struct Section
  {
    public uint virtualAddress;
    public uint virtualSize;
    public uint fileOffset;
  };

  private Section[] sections;
}
