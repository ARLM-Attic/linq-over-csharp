using System.Drawing;
using System.Windows.Forms;
using CSharpParser.ProjectModel;

namespace CSharpParser.CodeExplorer
{
  public class RichTextDocumentor
  {
    private RichTextBox _Box;
    private int _LastDocPosition;
    private readonly Font _TitleFont;
    private readonly Font _ParagraphFont;

    public RichTextDocumentor(RichTextBox _Box)
    {
      this._Box = _Box;
      _TitleFont = new Font("Tahoma", 14, FontStyle.Bold);
      _ParagraphFont = new Font("Tahoma", 10);
    }

    public void Document(ISupportsDocumentationComment element)
    {
      ClearDocumentContent();
      if (element == null) return;

      DocumentationComment comment = element.DocumentationComment;

      DisplayDocumentTitle("Summary");
      DisplayDocumentParagraph(comment.Summary.XmlText);

      if (comment.Parameters.Count > 0)
      {
        DisplayDocumentTitle("Parameters");
        foreach (ParamTag param in comment.Parameters)
        {
          DisplayDocumentParagraph(param.Name + ": " + param.XmlText);
        }
        DisplayDocumentParagraph("");
      }
      DisplayDocumentTitle("Remarks");
      DisplayDocumentParagraph(comment.Remarks.XmlText);
    }

    private void ClearDocumentContent()
    {
      _Box.Text = string.Empty;
      _LastDocPosition = 0;
    }

    private void DisplayDocumentTitle(string title)
    {
      _Box.AppendText(title);
      _Box.Select(_LastDocPosition, _Box.Text.Length);
      _Box.SelectionIndent = 0;
      _Box.SelectionFont = _TitleFont;
      _Box.AppendText("\n");
      _LastDocPosition = _Box.Text.Length;
    }

    private void DisplayDocumentParagraph(string para)
    {
      _Box.AppendText(para);
      _Box.Select(_LastDocPosition, _Box.Text.Length);
      _Box.SelectionIndent = 24;
      _Box.SelectionFont = _ParagraphFont;
      _Box.AppendText("\n");
      _LastDocPosition = _Box.Text.Length;
    }
  }
}
