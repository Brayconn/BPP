namespace BPP
{
    partial class FormDiff
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDiff));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.OKbutton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.previousButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.originalRichTextBox = new ScintillaNET.Scintilla();
            this.newRichTextBox = new ScintillaNET.Scintilla();
            this.preShiftLeftButton = new System.Windows.Forms.Button();
            this.preShiftRightButton = new System.Windows.Forms.Button();
            this.postShiftLeftButton = new System.Windows.Forms.Button();
            this.postShiftRightButton = new System.Windows.Forms.Button();
            this.preBufferLabel = new System.Windows.Forms.Label();
            this.postBufferLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x86ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sixteenbitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thirtytwobitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sixtyfourbitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.OKbutton, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.nextButton, 8, 1);
            this.tableLayoutPanel1.Controls.Add(this.previousButton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.preShiftLeftButton, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.preShiftRightButton, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.postShiftLeftButton, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.postShiftRightButton, 7, 1);
            this.tableLayoutPanel1.Controls.Add(this.preBufferLabel, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.postBufferLabel, 6, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // OKbutton
            // 
            this.OKbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.OKbutton, "OKbutton");
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.UseVisualStyleBackColor = true;
            // 
            // nextButton
            // 
            resources.ApplyResources(this.nextButton, "nextButton");
            this.nextButton.Name = "nextButton";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // previousButton
            // 
            resources.ApplyResources(this.previousButton, "previousButton");
            this.previousButton.Name = "previousButton";
            this.previousButton.UseVisualStyleBackColor = true;
            this.previousButton.Click += new System.EventHandler(this.previousButton_Click);
            // 
            // splitContainer1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.splitContainer1, 9);
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.originalRichTextBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.newRichTextBox);
            // 
            // originalRichTextBox
            // 
            resources.ApplyResources(this.originalRichTextBox, "originalRichTextBox");
            this.originalRichTextBox.Name = "originalRichTextBox";
            this.originalRichTextBox.ReadOnly = true;
            this.originalRichTextBox.WrapMode = ScintillaNET.WrapMode.Word;
            // 
            // newRichTextBox
            // 
            resources.ApplyResources(this.newRichTextBox, "newRichTextBox");
            this.newRichTextBox.Name = "newRichTextBox";
            this.newRichTextBox.ReadOnly = true;
            this.newRichTextBox.WrapMode = ScintillaNET.WrapMode.Word;
            // 
            // preShiftLeftButton
            // 
            resources.ApplyResources(this.preShiftLeftButton, "preShiftLeftButton");
            this.preShiftLeftButton.Name = "preShiftLeftButton";
            this.preShiftLeftButton.UseVisualStyleBackColor = true;
            this.preShiftLeftButton.Click += new System.EventHandler(this.preShiftLeftButton_Click);
            // 
            // preShiftRightButton
            // 
            resources.ApplyResources(this.preShiftRightButton, "preShiftRightButton");
            this.preShiftRightButton.Name = "preShiftRightButton";
            this.preShiftRightButton.UseVisualStyleBackColor = true;
            this.preShiftRightButton.Click += new System.EventHandler(this.preShiftRightButton_Click);
            // 
            // postShiftLeftButton
            // 
            resources.ApplyResources(this.postShiftLeftButton, "postShiftLeftButton");
            this.postShiftLeftButton.Name = "postShiftLeftButton";
            this.postShiftLeftButton.UseVisualStyleBackColor = true;
            this.postShiftLeftButton.Click += new System.EventHandler(this.postShiftLeftButton_Click);
            // 
            // postShiftRightButton
            // 
            resources.ApplyResources(this.postShiftRightButton, "postShiftRightButton");
            this.postShiftRightButton.Name = "postShiftRightButton";
            this.postShiftRightButton.UseVisualStyleBackColor = true;
            this.postShiftRightButton.Click += new System.EventHandler(this.postShiftRightButton_Click);
            // 
            // preBufferLabel
            // 
            resources.ApplyResources(this.preBufferLabel, "preBufferLabel");
            this.preBufferLabel.Name = "preBufferLabel";
            // 
            // postBufferLabel
            // 
            resources.ApplyResources(this.postBufferLabel, "postBufferLabel");
            this.postBufferLabel.Name = "postBufferLabel";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hexToolStripMenuItem,
            this.textToolStripMenuItem,
            this.x86ToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
            // 
            // hexToolStripMenuItem
            // 
            this.hexToolStripMenuItem.Name = "hexToolStripMenuItem";
            resources.ApplyResources(this.hexToolStripMenuItem, "hexToolStripMenuItem");
            this.hexToolStripMenuItem.Click += new System.EventHandler(this.hexToolStripMenuItem_Click);
            // 
            // textToolStripMenuItem
            // 
            this.textToolStripMenuItem.Name = "textToolStripMenuItem";
            resources.ApplyResources(this.textToolStripMenuItem, "textToolStripMenuItem");
            this.textToolStripMenuItem.Click += new System.EventHandler(this.textToolStripMenuItem_Click);
            // 
            // x86ToolStripMenuItem
            // 
            this.x86ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sixteenbitToolStripMenuItem,
            this.thirtytwobitToolStripMenuItem,
            this.sixtyfourbitToolStripMenuItem});
            this.x86ToolStripMenuItem.Name = "x86ToolStripMenuItem";
            resources.ApplyResources(this.x86ToolStripMenuItem, "x86ToolStripMenuItem");
            this.x86ToolStripMenuItem.Click += new System.EventHandler(this.x86ToolStripMenuItem_Click);
            // 
            // sixteenbitToolStripMenuItem
            // 
            this.sixteenbitToolStripMenuItem.Name = "sixteenbitToolStripMenuItem";
            resources.ApplyResources(this.sixteenbitToolStripMenuItem, "sixteenbitToolStripMenuItem");
            this.sixteenbitToolStripMenuItem.Click += new System.EventHandler(this.sixteenbitToolStripMenuItem_Click);
            // 
            // thirtytwobitToolStripMenuItem
            // 
            this.thirtytwobitToolStripMenuItem.Name = "thirtytwobitToolStripMenuItem";
            resources.ApplyResources(this.thirtytwobitToolStripMenuItem, "thirtytwobitToolStripMenuItem");
            this.thirtytwobitToolStripMenuItem.Click += new System.EventHandler(this.thirtytwobitToolStripMenuItem_Click);
            // 
            // sixtyfourbitToolStripMenuItem
            // 
            this.sixtyfourbitToolStripMenuItem.Name = "sixtyfourbitToolStripMenuItem";
            resources.ApplyResources(this.sixtyfourbitToolStripMenuItem, "sixtyfourbitToolStripMenuItem");
            this.sixtyfourbitToolStripMenuItem.Click += new System.EventHandler(this.sixtyfourbitToolStripMenuItem_Click);
            // 
            // FormDiff
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormDiff";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button OKbutton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button previousButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button preShiftLeftButton;
        private System.Windows.Forms.Button preShiftRightButton;
        private System.Windows.Forms.Button postShiftLeftButton;
        private System.Windows.Forms.Button postShiftRightButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x86ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sixteenbitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thirtytwobitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sixtyfourbitToolStripMenuItem;
        private System.Windows.Forms.Label preBufferLabel;
        private System.Windows.Forms.Label postBufferLabel;
        private ScintillaNET.Scintilla originalRichTextBox;
        private ScintillaNET.Scintilla newRichTextBox;
        private System.Windows.Forms.ToolStripMenuItem textToolStripMenuItem;
    }
}