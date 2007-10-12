using System;
using System.Windows.Forms;
using CSharpParser.ProjectContent;
using CSharpParser.ProjectModel;
using CSharpParser.CodeExplorer.TreeNodes;
using System.Reflection;

namespace CSharpParser.CodeExplorer
{
  // ====================================================================================
  /// <summary>
  /// Main form of the CodeExplorer utility.
  /// </summary>
  // ====================================================================================
  public partial class MainForm : Form
  {
    private FileTreeController _FileTreeController;
    private NamespaceTreeController _NamespaceTreeController;

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Initializes the form components.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public MainForm()
    {
      InitializeComponent();
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Initializes the application form and creates the controller instances.
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void MainForm_Load(object sender, EventArgs e)
    {
      Text += " v" + Assembly.GetEntryAssembly().GetName().Version;
      _FileTreeController = new FileTreeController(FileTreeView, PropertiyGrid);
      _NamespaceTreeController = new NamespaceTreeController(NamespaceTreeView, PropertiyGrid);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Quits the application.
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void ExitItem_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void OpenProjectFileItem_Click(object sender, EventArgs e)
    {
      if (FileDialog.ShowDialog() != DialogResult.OK) return;

      // --- Creates a compilation unit for the selected folder.
      CSharpProjectContent content = new CSharpProjectContent(FileDialog.FileName);
      CompilationUnit unit = new CompilationUnit(content);
      ParseAndShowCompilation(unit);
    }

    #region Private methods

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Parses the specified compilation unit.
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void ParseAndShowCompilation(CompilationUnit unit)
    {
      StatusLabel.Text = "Parsing project in " + unit.WorkingFolder;
      StatusStrip.Update();
      Cursor = Cursors.WaitCursor;
      try
      {
        // --- Parses the compilation unit
        DateTime start = DateTime.Now;
        unit.Parse();
        if (unit.Errors.Count == 0)
        {
          StatusLabel.Text = String.Format("Parsing finished successfuly in {0} ms.",
            (DateTime.Now - start).TotalMilliseconds);
        }
        else
        {
          StatusLabel.Text = String.Format("{0} erros found during parse.", unit.Errors.Count);
        }
      }
      catch (SystemException ex)
      {
        StatusLabel.Text = ex.Message;
      }
      finally
      {
        Cursor = Cursors.Default;
        StatusStrip.Update();
      }

      // --- Updates the views through the controllers
      _FileTreeController.SetCompilationUnit(unit);
      _NamespaceTreeController.SetCompilationUnit(unit);
    }

    #endregion
  }
}
