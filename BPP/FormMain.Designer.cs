namespace BPP
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generatePatchHistoryFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byEXEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.hackNamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hackFilenamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editHackButton = new System.Windows.Forms.Button();
            this.previewButton = new System.Windows.Forms.Button();
            this.chooseSelectedEXEButton = new System.Windows.Forms.Button();
            this.selectedEXETextBox = new System.Windows.Forms.TextBox();
            this.reloadButton = new System.Windows.Forms.Button();
            this.undoButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.queuedHacksListBox = new System.Windows.Forms.ListBox();
            this.selectedHackPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.availableHacksTreeView = new System.Windows.Forms.TreeView();
            this.hackDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.chooseHackDirectoryButton = new System.Windows.Forms.Button();
            this.baseAddressNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.baseAddressNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.generatePatchHistoryFileToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            resources.ApplyResources(this.saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // generatePatchHistoryFileToolStripMenuItem
            // 
            resources.ApplyResources(this.generatePatchHistoryFileToolStripMenuItem, "generatePatchHistoryFileToolStripMenuItem");
            this.generatePatchHistoryFileToolStripMenuItem.Name = "generatePatchHistoryFileToolStripMenuItem";
            this.generatePatchHistoryFileToolStripMenuItem.Click += new System.EventHandler(this.generatePatchHistoryFileToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.byEXEToolStripMenuItem,
            this.byDirectoryToolStripMenuItem,
            this.toolStripSeparator1,
            this.hackNamesToolStripMenuItem,
            this.hackFilenamesToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
            // 
            // byEXEToolStripMenuItem
            // 
            this.byEXEToolStripMenuItem.Name = "byEXEToolStripMenuItem";
            resources.ApplyResources(this.byEXEToolStripMenuItem, "byEXEToolStripMenuItem");
            this.byEXEToolStripMenuItem.Click += new System.EventHandler(this.byEXEToolStripMenuItem_Click);
            // 
            // byDirectoryToolStripMenuItem
            // 
            this.byDirectoryToolStripMenuItem.Name = "byDirectoryToolStripMenuItem";
            resources.ApplyResources(this.byDirectoryToolStripMenuItem, "byDirectoryToolStripMenuItem");
            this.byDirectoryToolStripMenuItem.Click += new System.EventHandler(this.byDirectoryToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // hackNamesToolStripMenuItem
            // 
            this.hackNamesToolStripMenuItem.Name = "hackNamesToolStripMenuItem";
            resources.ApplyResources(this.hackNamesToolStripMenuItem, "hackNamesToolStripMenuItem");
            this.hackNamesToolStripMenuItem.Click += new System.EventHandler(this.hackNamesToolStripMenuItem_Click);
            // 
            // hackFilenamesToolStripMenuItem
            // 
            this.hackFilenamesToolStripMenuItem.Name = "hackFilenamesToolStripMenuItem";
            resources.ApplyResources(this.hackFilenamesToolStripMenuItem, "hackFilenamesToolStripMenuItem");
            this.hackFilenamesToolStripMenuItem.Click += new System.EventHandler(this.hackFilenamesToolStripMenuItem_Click);
            // 
            // editHackButton
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.editHackButton, 2);
            resources.ApplyResources(this.editHackButton, "editHackButton");
            this.editHackButton.Name = "editHackButton";
            this.editHackButton.UseVisualStyleBackColor = true;
            this.editHackButton.Click += new System.EventHandler(this.editHackButton_Click);
            // 
            // previewButton
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.previewButton, 2);
            resources.ApplyResources(this.previewButton, "previewButton");
            this.previewButton.Name = "previewButton";
            this.previewButton.UseVisualStyleBackColor = true;
            this.previewButton.Click += new System.EventHandler(this.previewButton_Click);
            // 
            // chooseSelectedEXEButton
            // 
            resources.ApplyResources(this.chooseSelectedEXEButton, "chooseSelectedEXEButton");
            this.chooseSelectedEXEButton.Name = "chooseSelectedEXEButton";
            this.chooseSelectedEXEButton.UseVisualStyleBackColor = true;
            this.chooseSelectedEXEButton.Click += new System.EventHandler(this.selectEXEButton_Click);
            // 
            // selectedEXETextBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.selectedEXETextBox, 3);
            resources.ApplyResources(this.selectedEXETextBox, "selectedEXETextBox");
            this.selectedEXETextBox.Name = "selectedEXETextBox";
            this.selectedEXETextBox.ReadOnly = true;
            // 
            // reloadButton
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.reloadButton, 2);
            resources.ApplyResources(this.reloadButton, "reloadButton");
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.UseVisualStyleBackColor = true;
            this.reloadButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // undoButton
            // 
            resources.ApplyResources(this.undoButton, "undoButton");
            this.undoButton.Name = "undoButton";
            this.undoButton.UseVisualStyleBackColor = true;
            this.undoButton.Click += new System.EventHandler(this.undoButton_Click);
            // 
            // applyButton
            // 
            resources.ApplyResources(this.applyButton, "applyButton");
            this.applyButton.Name = "applyButton";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.tableLayoutPanel1.SetColumnSpan(this.label3, 4);
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Name = "label1";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.queuedHacksListBox, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.selectedHackPropertyGrid, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.applyButton, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.undoButton, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.reloadButton, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.selectedEXETextBox, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.chooseSelectedEXEButton, 7, 1);
            this.tableLayoutPanel1.Controls.Add(this.previewButton, 6, 3);
            this.tableLayoutPanel1.Controls.Add(this.editHackButton, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.availableHacksTreeView, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.hackDirectoryTextBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chooseHackDirectoryButton, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.baseAddressNumericUpDown, 2, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // queuedHacksListBox
            // 
            this.queuedHacksListBox.AllowDrop = true;
            this.tableLayoutPanel1.SetColumnSpan(this.queuedHacksListBox, 2);
            resources.ApplyResources(this.queuedHacksListBox, "queuedHacksListBox");
            this.queuedHacksListBox.FormattingEnabled = true;
            this.queuedHacksListBox.Name = "queuedHacksListBox";
            this.queuedHacksListBox.SelectedValueChanged += new System.EventHandler(this.queuedHacksListBox_SelectedValueChanged);
            this.queuedHacksListBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.queuedHacksListBox_DragDrop);
            this.queuedHacksListBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.queuedHacksListBox_DragEnter);
            this.queuedHacksListBox.DragOver += new System.Windows.Forms.DragEventHandler(this.queuedHacksListBox_DragOver);
            this.queuedHacksListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.queuedHacksListBox_MouseDoubleClick);
            this.queuedHacksListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.queuedHacksListBox_MouseDown);
            this.queuedHacksListBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.queuedHacksListBox_MouseMove);
            this.queuedHacksListBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.queuedHacksListBox_MouseUp);
            // 
            // selectedHackPropertyGrid
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.selectedHackPropertyGrid, 4);
            resources.ApplyResources(this.selectedHackPropertyGrid, "selectedHackPropertyGrid");
            this.selectedHackPropertyGrid.Name = "selectedHackPropertyGrid";
            // 
            // availableHacksTreeView
            // 
            this.availableHacksTreeView.AllowDrop = true;
            this.tableLayoutPanel1.SetColumnSpan(this.availableHacksTreeView, 2);
            resources.ApplyResources(this.availableHacksTreeView, "availableHacksTreeView");
            this.availableHacksTreeView.Name = "availableHacksTreeView";
            this.availableHacksTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.availableHacksTreeView_ItemDrag);
            this.availableHacksTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.availableHacksTreeView_NodeMouseClick);
            this.availableHacksTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.availableHacksTreeView_NodeMouseDoubleClick);
            this.availableHacksTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.availableHacksTreeView_DragDrop);
            this.availableHacksTreeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.availableHacksTreeView_DragEnter);
            // 
            // hackDirectoryTextBox
            // 
            resources.ApplyResources(this.hackDirectoryTextBox, "hackDirectoryTextBox");
            this.hackDirectoryTextBox.Name = "hackDirectoryTextBox";
            this.hackDirectoryTextBox.ReadOnly = true;
            // 
            // chooseHackDirectoryButton
            // 
            resources.ApplyResources(this.chooseHackDirectoryButton, "chooseHackDirectoryButton");
            this.chooseHackDirectoryButton.Name = "chooseHackDirectoryButton";
            this.chooseHackDirectoryButton.UseVisualStyleBackColor = true;
            this.chooseHackDirectoryButton.Click += new System.EventHandler(this.changeHackFolderToolStripMenuItem_Click);
            // 
            // baseAddressNumericUpDown
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.baseAddressNumericUpDown, 2);
            resources.ApplyResources(this.baseAddressNumericUpDown, "baseAddressNumericUpDown");
            this.baseAddressNumericUpDown.Hexadecimal = true;
            this.baseAddressNumericUpDown.Maximum = new decimal(new int[] {
            -1,
            -1,
            0,
            0});
            this.baseAddressNumericUpDown.Name = "baseAddressNumericUpDown";
            // 
            // FormMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.baseAddressNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generatePatchHistoryFileToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Button undoButton;
        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.TextBox selectedEXETextBox;
        private System.Windows.Forms.Button chooseSelectedEXEButton;
        private System.Windows.Forms.Button previewButton;
        private System.Windows.Forms.Button editHackButton;
        private System.Windows.Forms.NumericUpDown baseAddressNumericUpDown;
        private System.Windows.Forms.TextBox hackDirectoryTextBox;
        private System.Windows.Forms.Button chooseHackDirectoryButton;
        private System.Windows.Forms.ListBox queuedHacksListBox;
        private System.Windows.Forms.PropertyGrid selectedHackPropertyGrid;
        private System.Windows.Forms.TreeView availableHacksTreeView;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem byDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byEXEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hackNamesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hackFilenamesToolStripMenuItem;
    }
}

