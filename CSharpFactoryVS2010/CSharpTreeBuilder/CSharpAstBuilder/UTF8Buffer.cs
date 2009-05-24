// ================================================================================================
// UTF8Buffer.cs
//
// Created: 2009.05.22, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ==================================================================================
  /// <summary>
  /// This class represents a memory buffer that uses UTF8 encoded files.
  /// </summary>
  // ==================================================================================
  public class UTF8Buffer : Buffer
  {
    #region Lifecycle methods

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new UTF8Buffer instance based on a Buffer instance.
    /// </summary>
    /// <param name="b">Base Buffer instance</param>
    //-----------------------------------------------------------------------------------
    public UTF8Buffer(Buffer b) : base(b)
    {
    }

    #endregion

    #region Overridden methods

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Reads the next value from the UTF8Buffer.
    /// </summary>
    /// <returns>Value read.</returns>
    //-----------------------------------------------------------------------------------
    public override int Read()
    {
      int ch;
      do
      {
        ch = base.Read();
        // until we find a uft8 start (0xxxxxxx or 11xxxxxx)
      } while ((ch >= 128) && ((ch & 0xC0) != 0xC0) && (ch != EOF));
      if (ch < 128 || ch == EOF)
      {
        // nothing to do, first 127 chars are the same in ascii and utf8
        // 0xxxxxxx or end of file character
      }
      else if ((ch & 0xF0) == 0xF0)
      {
        // 11110xxx 10xxxxxx 10xxxxxx 10xxxxxx
        int c1 = ch & 0x07;
        ch = base.Read();
        int c2 = ch & 0x3F;
        ch = base.Read();
        int c3 = ch & 0x3F;
        ch = base.Read();
        int c4 = ch & 0x3F;
        ch = (((((c1 << 6) | c2) << 6) | c3) << 6) | c4;
      }
      else if ((ch & 0xE0) == 0xE0)
      {
        // 1110xxxx 10xxxxxx 10xxxxxx
        int c1 = ch & 0x0F;
        ch = base.Read();
        int c2 = ch & 0x3F;
        ch = base.Read();
        int c3 = ch & 0x3F;
        ch = (((c1 << 6) | c2) << 6) | c3;
      }
      else if ((ch & 0xC0) == 0xC0)
      {
        // 110xxxxx 10xxxxxx
        int c1 = ch & 0x1F;
        ch = base.Read();
        int c2 = ch & 0x3F;
        ch = (c1 << 6) | c2;
      }
      return ch;
    }

    #endregion
  }
}