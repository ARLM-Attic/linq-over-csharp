using System.ComponentModel;
using System.Windows.Forms;
using CSharpParser.CodeExplorer.Entities;

namespace CSharpParser.CodeExplorer
{
  public partial class ParsingTimeStatistics : Form
  {
    public ParsingTimeStatistics(BindingList<FileParsingData> data)
    {
      InitializeComponent();
      ParseTimeGrid.DataSource = data;
    }
  }
}