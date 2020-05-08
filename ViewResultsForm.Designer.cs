namespace Webshot
{
    partial class ViewResultsForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeFiles = new System.Windows.Forms.TreeView();
            this.pnlPicture = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.hscrollImg = new System.Windows.Forms.HScrollBar();
            this.vscrollImg = new System.Windows.Forms.VScrollBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbImageAutoWidth = new System.Windows.Forms.CheckBox();
            this.btnToggleAutoScroll = new System.Windows.Forms.Button();
            this.lblScrollSpeed = new System.Windows.Forms.Label();
            this.trackScrollSpeed = new System.Windows.Forms.TrackBar();
            this.lblZoom = new System.Windows.Forms.Label();
            this.trackZoom = new System.Windows.Forms.TrackBar();
            this.btnShowInExplorer = new System.Windows.Forms.Button();
            this.lnkUrl = new System.Windows.Forms.LinkLabel();
            this.timerScroll = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlPicture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackScrollSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeFiles);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pnlPicture);
            this.splitContainer1.Panel2.Controls.Add(this.hscrollImg);
            this.splitContainer1.Panel2.Controls.Add(this.vscrollImg);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1953, 1073);
            this.splitContainer1.SplitterDistance = 466;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeFiles
            // 
            this.treeFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeFiles.Location = new System.Drawing.Point(0, 0);
            this.treeFiles.Name = "treeFiles";
            this.treeFiles.Size = new System.Drawing.Size(466, 1073);
            this.treeFiles.TabIndex = 0;
            this.treeFiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeFiles_AfterSelect);
            // 
            // pnlPicture
            // 
            this.pnlPicture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlPicture.Controls.Add(this.pictureBox1);
            this.pnlPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPicture.Location = new System.Drawing.Point(0, 183);
            this.pnlPicture.Name = "pnlPicture";
            this.pnlPicture.Size = new System.Drawing.Size(1453, 860);
            this.pnlPicture.TabIndex = 2;
            this.pnlPicture.Resize += new System.EventHandler(this.PnlPicture_Resize);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(500, 500);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseUp);
            // 
            // hscrollImg
            // 
            this.hscrollImg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hscrollImg.LargeChange = 1000;
            this.hscrollImg.Location = new System.Drawing.Point(0, 1043);
            this.hscrollImg.Maximum = 10000;
            this.hscrollImg.Name = "hscrollImg";
            this.hscrollImg.Size = new System.Drawing.Size(1453, 30);
            this.hscrollImg.SmallChange = 50;
            this.hscrollImg.TabIndex = 4;
            this.hscrollImg.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HscrollImg_Scroll);
            // 
            // vscrollImg
            // 
            this.vscrollImg.Dock = System.Windows.Forms.DockStyle.Right;
            this.vscrollImg.LargeChange = 1000;
            this.vscrollImg.Location = new System.Drawing.Point(1453, 183);
            this.vscrollImg.Maximum = 1000;
            this.vscrollImg.Name = "vscrollImg";
            this.vscrollImg.Size = new System.Drawing.Size(30, 890);
            this.vscrollImg.SmallChange = 50;
            this.vscrollImg.TabIndex = 3;
            this.vscrollImg.Scroll += new System.Windows.Forms.ScrollEventHandler(this.VscrollImg_Scroll);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbImageAutoWidth);
            this.panel1.Controls.Add(this.btnToggleAutoScroll);
            this.panel1.Controls.Add(this.lblScrollSpeed);
            this.panel1.Controls.Add(this.trackScrollSpeed);
            this.panel1.Controls.Add(this.lblZoom);
            this.panel1.Controls.Add(this.trackZoom);
            this.panel1.Controls.Add(this.btnShowInExplorer);
            this.panel1.Controls.Add(this.lnkUrl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1483, 183);
            this.panel1.TabIndex = 0;
            // 
            // cbImageAutoWidth
            // 
            this.cbImageAutoWidth.AutoSize = true;
            this.cbImageAutoWidth.Checked = true;
            this.cbImageAutoWidth.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbImageAutoWidth.Location = new System.Drawing.Point(377, 54);
            this.cbImageAutoWidth.Name = "cbImageAutoWidth";
            this.cbImageAutoWidth.Size = new System.Drawing.Size(171, 29);
            this.cbImageAutoWidth.TabIndex = 7;
            this.cbImageAutoWidth.Text = "Auto-fit to width";
            this.cbImageAutoWidth.UseVisualStyleBackColor = true;
            this.cbImageAutoWidth.CheckedChanged += new System.EventHandler(this.CbImageAutoWidth_CheckedChanged);
            // 
            // btnToggleAutoScroll
            // 
            this.btnToggleAutoScroll.Location = new System.Drawing.Point(1021, 3);
            this.btnToggleAutoScroll.Name = "btnToggleAutoScroll";
            this.btnToggleAutoScroll.Size = new System.Drawing.Size(257, 59);
            this.btnToggleAutoScroll.TabIndex = 6;
            this.btnToggleAutoScroll.Text = "Start AutoScroll";
            this.btnToggleAutoScroll.UseVisualStyleBackColor = true;
            this.btnToggleAutoScroll.Click += new System.EventHandler(this.BtnToggleAutoScroll_Click);
            // 
            // lblScrollSpeed
            // 
            this.lblScrollSpeed.Location = new System.Drawing.Point(719, 49);
            this.lblScrollSpeed.Name = "lblScrollSpeed";
            this.lblScrollSpeed.Size = new System.Drawing.Size(295, 34);
            this.lblScrollSpeed.TabIndex = 5;
            this.lblScrollSpeed.Text = "Scroll Speed";
            this.lblScrollSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackScrollSpeed
            // 
            this.trackScrollSpeed.Location = new System.Drawing.Point(724, 3);
            this.trackScrollSpeed.Maximum = 15;
            this.trackScrollSpeed.Minimum = 1;
            this.trackScrollSpeed.Name = "trackScrollSpeed";
            this.trackScrollSpeed.Size = new System.Drawing.Size(290, 80);
            this.trackScrollSpeed.TabIndex = 4;
            this.trackScrollSpeed.Value = 5;
            // 
            // lblZoom
            // 
            this.lblZoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZoom.Location = new System.Drawing.Point(372, 79);
            this.lblZoom.Name = "lblZoom";
            this.lblZoom.Size = new System.Drawing.Size(315, 29);
            this.lblZoom.TabIndex = 3;
            this.lblZoom.Text = "Zoom";
            this.lblZoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackZoom
            // 
            this.trackZoom.LargeChange = 25;
            this.trackZoom.Location = new System.Drawing.Point(377, 3);
            this.trackZoom.Maximum = 150;
            this.trackZoom.Name = "trackZoom";
            this.trackZoom.Size = new System.Drawing.Size(310, 80);
            this.trackZoom.SmallChange = 10;
            this.trackZoom.TabIndex = 2;
            this.trackZoom.TickFrequency = 25;
            this.trackZoom.Value = 100;
            this.trackZoom.Scroll += new System.EventHandler(this.TrackZoom_Scroll);
            // 
            // btnShowInExplorer
            // 
            this.btnShowInExplorer.Location = new System.Drawing.Point(27, 12);
            this.btnShowInExplorer.Name = "btnShowInExplorer";
            this.btnShowInExplorer.Size = new System.Drawing.Size(275, 50);
            this.btnShowInExplorer.TabIndex = 1;
            this.btnShowInExplorer.Text = "Show in Explorer";
            this.btnShowInExplorer.UseVisualStyleBackColor = true;
            this.btnShowInExplorer.Click += new System.EventHandler(this.BtnShowInExplorer_Click);
            // 
            // lnkUrl
            // 
            this.lnkUrl.AutoSize = true;
            this.lnkUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkUrl.Location = new System.Drawing.Point(19, 112);
            this.lnkUrl.Name = "lnkUrl";
            this.lnkUrl.Size = new System.Drawing.Size(95, 44);
            this.lnkUrl.TabIndex = 0;
            this.lnkUrl.TabStop = true;
            this.lnkUrl.Text = "URL";
            this.lnkUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkUrl_LinkClicked);
            // 
            // timerScroll
            // 
            this.timerScroll.Interval = 2;
            this.timerScroll.Tick += new System.EventHandler(this.TimerScroll_Tick);
            // 
            // ViewResultsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1953, 1073);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ViewResultsForm";
            this.Text = "ViewResults";
            this.Load += new System.EventHandler(this.ViewResultsForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnlPicture.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackScrollSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackZoom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView treeFiles;
        private System.Windows.Forms.LinkLabel lnkUrl;
        private System.Windows.Forms.Panel pnlPicture;
        private System.Windows.Forms.Button btnShowInExplorer;
        private System.Windows.Forms.Label lblZoom;
        private System.Windows.Forms.TrackBar trackZoom;
        private System.Windows.Forms.Label lblScrollSpeed;
        private System.Windows.Forms.TrackBar trackScrollSpeed;
        private System.Windows.Forms.Button btnToggleAutoScroll;
        private System.Windows.Forms.Timer timerScroll;
        private System.Windows.Forms.HScrollBar hscrollImg;
        private System.Windows.Forms.VScrollBar vscrollImg;
        private System.Windows.Forms.CheckBox cbImageAutoWidth;
    }
}