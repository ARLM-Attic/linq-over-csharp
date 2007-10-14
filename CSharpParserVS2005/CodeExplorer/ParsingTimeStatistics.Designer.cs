namespace CSharpParser.CodeExplorer
{
  partial class ParsingTimeStatistics
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.ParseTimeGrid = new System.Windows.Forms.DataGridView();
      this.fileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.timeFromStartDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.parseTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fileParsingDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.ParseTimeGrid)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.fileParsingDataBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // ParseTimeGrid
      // 
      this.ParseTimeGrid.AllowUserToAddRows = false;
      this.ParseTimeGrid.AllowUserToDeleteRows = false;
      this.ParseTimeGrid.AllowUserToResizeRows = false;
      this.ParseTimeGrid.AutoGenerateColumns = false;
      this.ParseTimeGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.ParseTimeGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fileNameDataGridViewTextBoxColumn,
            this.timeFromStartDataGridViewTextBoxColumn,
            this.parseTimeDataGridViewTextBoxColumn});
      this.ParseTimeGrid.DataSource = this.fileParsingDataBindingSource;
      this.ParseTimeGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ParseTimeGrid.Location = new System.Drawing.Point(4, 4);
      this.ParseTimeGrid.Name = "ParseTimeGrid";
      this.ParseTimeGrid.ReadOnly = true;
      this.ParseTimeGrid.RowTemplate.Height = 24;
      this.ParseTimeGrid.Size = new System.Drawing.Size(663, 406);
      this.ParseTimeGrid.TabIndex = 0;
      // 
      // fileNameDataGridViewTextBoxColumn
      // 
      this.fileNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.fileNameDataGridViewTextBoxColumn.DataPropertyName = "FileName";
      this.fileNameDataGridViewTextBoxColumn.FillWeight = 3F;
      this.fileNameDataGridViewTextBoxColumn.HeaderText = "Source File Name";
      this.fileNameDataGridViewTextBoxColumn.Name = "fileNameDataGridViewTextBoxColumn";
      this.fileNameDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // timeFromStartDataGridViewTextBoxColumn
      // 
      this.timeFromStartDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.timeFromStartDataGridViewTextBoxColumn.DataPropertyName = "TimeFromStart";
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      dataGridViewCellStyle1.Format = "N0";
      dataGridViewCellStyle1.NullValue = null;
      this.timeFromStartDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
      this.timeFromStartDataGridViewTextBoxColumn.FillWeight = 1F;
      this.timeFromStartDataGridViewTextBoxColumn.HeaderText = "From Start";
      this.timeFromStartDataGridViewTextBoxColumn.Name = "timeFromStartDataGridViewTextBoxColumn";
      this.timeFromStartDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // parseTimeDataGridViewTextBoxColumn
      // 
      this.parseTimeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.parseTimeDataGridViewTextBoxColumn.DataPropertyName = "ParseTime";
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      dataGridViewCellStyle2.Format = "N0";
      dataGridViewCellStyle2.NullValue = null;
      this.parseTimeDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
      this.parseTimeDataGridViewTextBoxColumn.FillWeight = 1F;
      this.parseTimeDataGridViewTextBoxColumn.HeaderText = "Parse Time";
      this.parseTimeDataGridViewTextBoxColumn.Name = "parseTimeDataGridViewTextBoxColumn";
      this.parseTimeDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // fileParsingDataBindingSource
      // 
      this.fileParsingDataBindingSource.DataSource = typeof(CSharpParser.CodeExplorer.Entities.FileParsingData);
      // 
      // ParsingTimeStatistics
      // 
      this.ClientSize = new System.Drawing.Size(671, 414);
      this.Controls.Add(this.ParseTimeGrid);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "ParsingTimeStatistics";
      this.Padding = new System.Windows.Forms.Padding(4);
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Syntax Parsing Time Statistics";
      ((System.ComponentModel.ISupportInitialize)(this.ParseTimeGrid)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.fileParsingDataBindingSource)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView ParseTimeGrid;
    private System.Windows.Forms.BindingSource fileParsingDataBindingSource;
    private System.Windows.Forms.DataGridViewTextBoxColumn fileNameDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn timeFromStartDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn parseTimeDataGridViewTextBoxColumn;
  }
}