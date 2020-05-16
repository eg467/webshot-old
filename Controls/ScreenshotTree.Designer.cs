namespace Webshot.Controls
{
    partial class ScreenshotTree
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblLegend = new System.Windows.Forms.Label();
            this.treeFiles = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // lblLegend
            // 
            this.lblLegend.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblLegend.Location = new System.Drawing.Point(0, 1111);
            this.lblLegend.Name = "lblLegend";
            this.lblLegend.Size = new System.Drawing.Size(861, 155);
            this.lblLegend.TabIndex = 0;
            // 
            // treeFiles
            // 
            this.treeFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeFiles.Location = new System.Drawing.Point(0, 0);
            this.treeFiles.Name = "treeFiles";
            this.treeFiles.Size = new System.Drawing.Size(861, 1111);
            this.treeFiles.TabIndex = 1;
            this.treeFiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeFiles_AfterSelect);
            // 
            // ScreenshotTree
            // 
            this.Controls.Add(this.treeFiles);
            this.Controls.Add(this.lblLegend);
            this.Name = "ScreenshotTree";
            this.Size = new System.Drawing.Size(861, 1266);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblLegend;
        private System.Windows.Forms.TreeView treeFiles;
    }
}
