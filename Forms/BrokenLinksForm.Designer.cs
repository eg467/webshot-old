namespace Webshot.Forms
{
    partial class BrokenLinksForm
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
            this.treeLinks = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeLinks
            // 
            this.treeLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeLinks.Location = new System.Drawing.Point(0, 0);
            this.treeLinks.Name = "treeLinks";
            this.treeLinks.Size = new System.Drawing.Size(800, 450);
            this.treeLinks.TabIndex = 0;
            this.treeLinks.DoubleClick += new System.EventHandler(this.treeLinks_DoubleClick);
            // 
            // BrokenLinksForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.treeLinks);
            this.Name = "BrokenLinksForm";
            this.Text = "BrokenLinksForm";
            this.Load += new System.EventHandler(this.BrokenLinksForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeLinks;
    }
}