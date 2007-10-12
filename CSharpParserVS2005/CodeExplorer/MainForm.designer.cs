namespace CSharpParser.CodeExplorer
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("No project loaded.");
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("No project loaded.");
      this.MainStrip = new System.Windows.Forms.MenuStrip();
      this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.ExitItem = new System.Windows.Forms.ToolStripMenuItem();
      this.SplitContainer = new System.Windows.Forms.SplitContainer();
      this.ProjectViewTabControl = new System.Windows.Forms.TabControl();
      this.FileTabPage = new System.Windows.Forms.TabPage();
      this.FileTreeView = new System.Windows.Forms.TreeView();
      this.SmallImages = new System.Windows.Forms.ImageList(this.components);
      this.NamespaceTabView = new System.Windows.Forms.TabPage();
      this.NamespaceTreeView = new System.Windows.Forms.TreeView();
      this.PropertiyGrid = new System.Windows.Forms.PropertyGrid();
      this.StatusStrip = new System.Windows.Forms.StatusStrip();
      this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.OpenProjectFileItem = new System.Windows.Forms.ToolStripMenuItem();
      this.FileDialog = new System.Windows.Forms.OpenFileDialog();
      this.MainStrip.SuspendLayout();
      this.SplitContainer.Panel1.SuspendLayout();
      this.SplitContainer.Panel2.SuspendLayout();
      this.SplitContainer.SuspendLayout();
      this.ProjectViewTabControl.SuspendLayout();
      this.FileTabPage.SuspendLayout();
      this.NamespaceTabView.SuspendLayout();
      this.StatusStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // MainStrip
      // 
      this.MainStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu});
      this.MainStrip.Location = new System.Drawing.Point(0, 0);
      this.MainStrip.Name = "MainStrip";
      this.MainStrip.Size = new System.Drawing.Size(688, 28);
      this.MainStrip.TabIndex = 0;
      this.MainStrip.Text = "menuStrip1";
      // 
      // FileMenu
      // 
      this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenProjectFileItem,
            this.toolStripMenuItem1,
            this.ExitItem});
      this.FileMenu.Name = "FileMenu";
      this.FileMenu.Size = new System.Drawing.Size(44, 24);
      this.FileMenu.Text = "&File";
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(220, 6);
      // 
      // ExitItem
      // 
      this.ExitItem.Name = "ExitItem";
      this.ExitItem.Size = new System.Drawing.Size(223, 24);
      this.ExitItem.Text = "E&xit";
      this.ExitItem.Click += new System.EventHandler(this.ExitItem_Click);
      // 
      // SplitContainer
      // 
      this.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.SplitContainer.Location = new System.Drawing.Point(0, 28);
      this.SplitContainer.Name = "SplitContainer";
      // 
      // SplitContainer.Panel1
      // 
      this.SplitContainer.Panel1.Controls.Add(this.ProjectViewTabControl);
      // 
      // SplitContainer.Panel2
      // 
      this.SplitContainer.Panel2.Controls.Add(this.PropertiyGrid);
      this.SplitContainer.Size = new System.Drawing.Size(688, 477);
      this.SplitContainer.SplitterDistance = 347;
      this.SplitContainer.TabIndex = 1;
      // 
      // ProjectViewTabControl
      // 
      this.ProjectViewTabControl.Controls.Add(this.FileTabPage);
      this.ProjectViewTabControl.Controls.Add(this.NamespaceTabView);
      this.ProjectViewTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ProjectViewTabControl.Location = new System.Drawing.Point(0, 0);
      this.ProjectViewTabControl.Name = "ProjectViewTabControl";
      this.ProjectViewTabControl.SelectedIndex = 0;
      this.ProjectViewTabControl.Size = new System.Drawing.Size(347, 477);
      this.ProjectViewTabControl.TabIndex = 0;
      // 
      // FileTabPage
      // 
      this.FileTabPage.Controls.Add(this.FileTreeView);
      this.FileTabPage.Location = new System.Drawing.Point(4, 25);
      this.FileTabPage.Name = "FileTabPage";
      this.FileTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.FileTabPage.Size = new System.Drawing.Size(339, 448);
      this.FileTabPage.TabIndex = 0;
      this.FileTabPage.Text = "File View";
      this.FileTabPage.UseVisualStyleBackColor = true;
      // 
      // FileTreeView
      // 
      this.FileTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.FileTreeView.ImageKey = "CSharpProject";
      this.FileTreeView.ImageList = this.SmallImages;
      this.FileTreeView.Location = new System.Drawing.Point(3, 3);
      this.FileTreeView.Name = "FileTreeView";
      treeNode1.ImageKey = "CSharpProject";
      treeNode1.Name = "Node0";
      treeNode1.Text = "No project loaded.";
      this.FileTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
      this.FileTreeView.SelectedImageKey = "CSharpProject";
      this.FileTreeView.Size = new System.Drawing.Size(333, 442);
      this.FileTreeView.TabIndex = 0;
      // 
      // SmallImages
      // 
      this.SmallImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("SmallImages.ImageStream")));
      this.SmallImages.TransparentColor = System.Drawing.Color.Fuchsia;
      this.SmallImages.Images.SetKeyName(0, "FolderClosed");
      this.SmallImages.Images.SetKeyName(1, "FolderOpen");
      this.SmallImages.Images.SetKeyName(2, "CSharpProject");
      this.SmallImages.Images.SetKeyName(3, "CSharpFile");
      this.SmallImages.Images.SetKeyName(4, "Namespace");
      this.SmallImages.Images.SetKeyName(5, "Class");
      this.SmallImages.Images.SetKeyName(6, "Delegate");
      this.SmallImages.Images.SetKeyName(7, "Enum");
      this.SmallImages.Images.SetKeyName(8, "Interface");
      this.SmallImages.Images.SetKeyName(9, "Struct");
      this.SmallImages.Images.SetKeyName(10, "Constant");
      this.SmallImages.Images.SetKeyName(11, "Event");
      this.SmallImages.Images.SetKeyName(12, "Field");
      this.SmallImages.Images.SetKeyName(13, "Method");
      this.SmallImages.Images.SetKeyName(14, "Operator");
      this.SmallImages.Images.SetKeyName(15, "Property");
      this.SmallImages.Images.SetKeyName(16, "EnumValue");
      // 
      // NamespaceTabView
      // 
      this.NamespaceTabView.Controls.Add(this.NamespaceTreeView);
      this.NamespaceTabView.Location = new System.Drawing.Point(4, 25);
      this.NamespaceTabView.Name = "NamespaceTabView";
      this.NamespaceTabView.Padding = new System.Windows.Forms.Padding(3);
      this.NamespaceTabView.Size = new System.Drawing.Size(339, 448);
      this.NamespaceTabView.TabIndex = 1;
      this.NamespaceTabView.Text = "Namespace view";
      this.NamespaceTabView.UseVisualStyleBackColor = true;
      // 
      // NamespaceTreeView
      // 
      this.NamespaceTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.NamespaceTreeView.ImageKey = "CSharpProject";
      this.NamespaceTreeView.ImageList = this.SmallImages;
      this.NamespaceTreeView.Location = new System.Drawing.Point(3, 3);
      this.NamespaceTreeView.Name = "NamespaceTreeView";
      treeNode2.ImageKey = "CSharpProject";
      treeNode2.Name = "Node0";
      treeNode2.Text = "No project loaded.";
      this.NamespaceTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
      this.NamespaceTreeView.SelectedImageKey = "CSharpProject";
      this.NamespaceTreeView.Size = new System.Drawing.Size(333, 442);
      this.NamespaceTreeView.TabIndex = 0;
      // 
      // PropertiyGrid
      // 
      this.PropertiyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.PropertiyGrid.Location = new System.Drawing.Point(0, 0);
      this.PropertiyGrid.Name = "PropertiyGrid";
      this.PropertiyGrid.Size = new System.Drawing.Size(337, 477);
      this.PropertiyGrid.TabIndex = 0;
      // 
      // StatusStrip
      // 
      this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
      this.StatusStrip.Location = new System.Drawing.Point(0, 483);
      this.StatusStrip.Name = "StatusStrip";
      this.StatusStrip.Size = new System.Drawing.Size(688, 22);
      this.StatusStrip.TabIndex = 2;
      this.StatusStrip.Text = "statusStrip1";
      // 
      // StatusLabel
      // 
      this.StatusLabel.Name = "StatusLabel";
      this.StatusLabel.Size = new System.Drawing.Size(0, 17);
      // 
      // OpenProjectFileItem
      // 
      this.OpenProjectFileItem.Name = "OpenProjectFileItem";
      this.OpenProjectFileItem.Size = new System.Drawing.Size(223, 24);
      this.OpenProjectFileItem.Text = "Open Project File...";
      this.OpenProjectFileItem.Click += new System.EventHandler(this.OpenProjectFileItem_Click);
      // 
      // FileDialog
      // 
      this.FileDialog.FileName = "C:\\Work\\LINQOverCSharp\\CSharpParserVS2005\\CSharpParser\\CSharpParser.csproj";
      this.FileDialog.Filter = "C# project files (*.csproj)|*.csproj";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(688, 505);
      this.Controls.Add(this.StatusStrip);
      this.Controls.Add(this.SplitContainer);
      this.Controls.Add(this.MainStrip);
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "LINQ Over C# Code Explorer";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.MainStrip.ResumeLayout(false);
      this.MainStrip.PerformLayout();
      this.SplitContainer.Panel1.ResumeLayout(false);
      this.SplitContainer.Panel2.ResumeLayout(false);
      this.SplitContainer.ResumeLayout(false);
      this.ProjectViewTabControl.ResumeLayout(false);
      this.FileTabPage.ResumeLayout(false);
      this.NamespaceTabView.ResumeLayout(false);
      this.StatusStrip.ResumeLayout(false);
      this.StatusStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip MainStrip;
    private System.Windows.Forms.ToolStripMenuItem FileMenu;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem ExitItem;
    private System.Windows.Forms.SplitContainer SplitContainer;
    private System.Windows.Forms.TabControl ProjectViewTabControl;
    private System.Windows.Forms.TabPage FileTabPage;
    private System.Windows.Forms.TabPage NamespaceTabView;
    private System.Windows.Forms.TreeView FileTreeView;
    private System.Windows.Forms.TreeView NamespaceTreeView;
    private System.Windows.Forms.ImageList SmallImages;
    private System.Windows.Forms.StatusStrip StatusStrip;
    private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
    private System.Windows.Forms.PropertyGrid PropertiyGrid;
    private System.Windows.Forms.ToolStripMenuItem OpenProjectFileItem;
    private System.Windows.Forms.OpenFileDialog FileDialog;
  }
}

