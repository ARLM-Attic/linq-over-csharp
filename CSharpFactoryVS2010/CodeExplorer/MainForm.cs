using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using CSharpFactory.CodeExplorer.Entities;
using CSharpFactory.CodeExplorer.TreeNodes;
using CSharpFactory.ProjectContent;
using CSharpFactory.ProjectModel;
using System.Reflection;

namespace CSharpFactory.CodeExplorer
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
    private CompilationUnit _Unit;
    private Stopwatch _Watch;
    private readonly BindingList<FileParsingData> _ParsingTimeStats = 
      new BindingList<FileParsingData>();
    private FileParsingData _CurrentFileData;

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
      ControlledUIelements uiElements = 
        new ControlledUIelements(FileTreeView, PropertiyGrid, DocumentationBox);
      _FileTreeController = new FileTreeController(uiElements);
      uiElements = new ControlledUIelements(NamespaceTreeView, PropertiyGrid, DocumentationBox); 
      _NamespaceTreeController = new NamespaceTreeController(uiElements);
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

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Opens the specified project file.
    /// </summary>
    // ----------------------------------------------------------------------------------
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
      _Unit = null;
      StatusLabel.Text = "Parsing project in " + unit.WorkingFolder;
      StatusStrip.Update();
      Cursor = Cursors.WaitCursor;
      try
      {
        // --- Parses the compilation unit
        DateTime start = DateTime.Now;

        // --- Setup events
        try
        {
          unit.AfterInitParse += OnAfterInitParse;
          unit.BeforeParseFile += OnBeforeParseFile;
          unit.AfterParseFile += OnAfterParseFile;
          unit.Parse();
        }
        finally
        {
          unit.AfterInitParse -= OnAfterInitParse;
          unit.BeforeParseFile -= OnBeforeParseFile;
          unit.AfterParseFile -= OnAfterParseFile;
        }

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
      _Unit = unit;
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Displays the parser time statistics
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void ParsingTimesItems_Click(object sender, EventArgs e)
    {
      if (_Unit == null)
      {
        MessageBox.Show("No successfully compiled unit to show statictics about.", 
          "No statistics");
        return;
      }
      ParsingTimeStatistics form = new ParsingTimeStatistics(_ParsingTimeStats);
      form.ShowDialog();
    }

    #endregion

    #region Events related to parsing

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Init the parser watch
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void OnAfterInitParse(object sender, ParseCancelEventArgs e)
    {
      _Watch = new Stopwatch();
      _Watch.Start();
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Collect start time information for the file
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void OnBeforeParseFile(object sender, ParseFileEventArgs e)
    {
      _CurrentFileData = new FileParsingData();
      _CurrentFileData.FileName = e.File.Name;
      _CurrentFileData.TimeFromStart = _Watch.ElapsedMilliseconds;
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Collect start time information for the file
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void OnAfterParseFile(object sender, ParseFileEventArgs e)
    {
      _CurrentFileData.ParseTime = 
        _Watch.ElapsedMilliseconds - _CurrentFileData.TimeFromStart;
      _ParsingTimeStats.Add(_CurrentFileData);
    }

    #endregion

  }
}
