using System.Drawing;
using System.Windows.Forms;
using CSharpParser.ProjectModel;

namespace CSharpParser.CodeExplorer.TreeNodes
{
  // ==================================================================================
  /// <summary>
  /// This class represents an abstract tree controller used by CodeExplorer 
  /// controllers.
  /// </summary>
  // ==================================================================================
  internal abstract class TreeControllerBase
  {
    #region Private fields

    private readonly TreeView _TreeView;
    private readonly PropertyGrid _PropertyGrid;
    private OnDemandTreeNode _RootNode;
    private CompilationUnit _CompilationUnit;
    private string _WorkingFolder;
    private RichTextDocumentor _Documentor;

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the TreeView control that provides the user interface for this controller.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TreeView TreeView
    {
      get { return _TreeView; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the PropertyGrid control that provides the user interface for 
    /// this controller.
    /// </summary>
    // --------------------------------------------------------------------------------
    public PropertyGrid PropertyGrid
    {
      get { return _PropertyGrid; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Get the root node of the controlled tree node.
    /// </summary>
    // --------------------------------------------------------------------------------
    public OnDemandTreeNode RootNode
    {
      get { return _RootNode; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the compilation unit represented by this view.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CompilationUnit CompilationUnit
    {
      get { return _CompilationUnit; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the working folder of the compilation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string WorkingFolder
    {
      get { return _WorkingFolder; }
    }

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an instance of the controller and attaches it to the specifier 
    /// TreeView and PropertyGrid
    /// </summary>
    /// <param name="elements">UI elements used by the controller</param>
    // --------------------------------------------------------------------------------
    public TreeControllerBase(ControlledUIelements elements)
    {
      _TreeView = elements.Tree;
      _PropertyGrid = elements.Properties;
      _TreeView.BeforeExpand += OnBeforeExpand;
      _TreeView.AfterSelect += OnAfterSelect;
      _Documentor = new RichTextDocumentor(elements.DocumentBox);
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the compilation unit and reloads the compilation unit structure.
    /// </summary>
    /// <param name="unit">Compilation unit</param>
    // --------------------------------------------------------------------------------
    public void SetCompilationUnit(CompilationUnit unit)
    {
      _CompilationUnit = unit;
      _WorkingFolder = unit.WorkingFolder;
      InitTree();
    }

    #endregion

    #region Abstract methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates the root node of this controller.
    /// </summary>
    /// <returns>Tree node created.</returns>
    // --------------------------------------------------------------------------------
    protected abstract OnDemandTreeNode CreateRootNode();

    #endregion

    #region Tree event methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// This method is called before a tree node is expanded.
    /// </summary>
    /// <param name="sender">Object sending this event.</param>
    /// <param name="e">Event arguments.</param>
    // --------------------------------------------------------------------------------
    private void OnBeforeExpand(object sender, TreeViewCancelEventArgs e)
    {
      OnDemandTreeNode node = e.Node as OnDemandTreeNode;
      if (node != null)
      {
        // --- This is the only case we handle
        node.ExpandNode();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Displays information about the node when that is selected.
    /// </summary>
    /// <param name="sender">Object sending this event.</param>
    /// <param name="e">Event arguments.</param>
    // --------------------------------------------------------------------------------
    void OnAfterSelect(object sender, TreeViewEventArgs e)
    {
      IPropertyPanelSupport node = e.Node as IPropertyPanelSupport;
      if (node != null)
      {
        PropertyGrid.SelectedObject = node.GetSelectedObject();
        ISupportsDocumentationComment doc = 
          node.GetSelectedObject() as ISupportsDocumentationComment;
        _Documentor.Document(doc);
      }
    } 

    #endregion

    #region Private methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Initializes the view of the compilation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    private void InitTree()
    {
      TreeView.Nodes.Clear();
      _RootNode = CreateRootNode();
      TreeView.Nodes.Add(_RootNode);
      _RootNode.Expand();
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represnts a combination of UI elements used by the tree controllers.
  /// </summary>
  // ==================================================================================
  public sealed class ControlledUIelements
  {
    private readonly TreeView _Tree;
    private readonly PropertyGrid _Properties;
    private readonly RichTextBox _DocumentBox;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an instance of UI element descriptor used by the controllers.
    /// </summary>
    /// <param name="tree">TreeView of the C# project.</param>
    /// <param name="properties">Property grid displaying selected element.</param>
    /// <param name="documentBox">Rich text box for source documentation.</param>
    // --------------------------------------------------------------------------------
    public ControlledUIelements(TreeView tree, PropertyGrid properties, RichTextBox documentBox)
    {
      _Tree = tree;
      _Properties = properties;
      _DocumentBox = documentBox;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the tree view representing the C# project.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TreeView Tree
    {
      get { return _Tree; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the property grid displaying element properties.
    /// </summary>
    // --------------------------------------------------------------------------------
    public PropertyGrid Properties
    {
      get { return _Properties; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the rich text box displaying source documentation.
    /// </summary>
    // --------------------------------------------------------------------------------
    public RichTextBox DocumentBox
    {
      get { return _DocumentBox; }
    }
  }
}
