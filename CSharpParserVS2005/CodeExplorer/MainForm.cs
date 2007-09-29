using System;
using System.Windows.Forms;
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
    /// Openc a new C# project.
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void OpenProjectFolderItem_Click(object sender, EventArgs e)
    {
      if (FolderDialog.ShowDialog() != DialogResult.OK) return;

      // --- Creates a compilation unit for the selected folder.
      CompilationUnit unit = new CompilationUnit(FolderDialog.SelectedPath, true);
      StatusLabel.Text = "Parsing project in " + FolderDialog.SelectedPath;
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
      catch (SystemException) { }
      finally
      {
        Cursor = Cursors.Default;
        StatusStrip.Update();
      }
      
      // --- Updates the views through the controllers
      _FileTreeController.SetCompilationUnit(unit, FolderDialog.SelectedPath);
      _NamespaceTreeController.SetCompilationUnit(unit, FolderDialog.SelectedPath);
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
  }
}
