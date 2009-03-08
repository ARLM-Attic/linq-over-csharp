using System.ComponentModel;
using System.Windows.Forms;
using CSharpFactory.CodeExplorer.Entities;

namespace CSharpFactory.CodeExplorer
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