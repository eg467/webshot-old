﻿namespace Webshot
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbFitImage = new System.Windows.Forms.CheckBox();
            this.btnToggleAutoScroll = new System.Windows.Forms.Button();
            this.lblScrollSpeed = new System.Windows.Forms.Label();
            this.trackScrollSpeed = new System.Windows.Forms.TrackBar();
            this.lblZoom = new System.Windows.Forms.Label();
            this.trackZoom = new System.Windows.Forms.TrackBar();
            this.btnShowInExplorer = new System.Windows.Forms.Button();
            this.lnkUrl = new System.Windows.Forms.LinkLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboResultSets = new System.Windows.Forms.ComboBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeScreenshots = new Webshot.Controls.ScreenshotTree();
            this.imageViewer = new Webshot.Controls.LargeImageViewer();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackScrollSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackZoom)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbFitImage);
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
            // cbFitImage
            // 
            this.cbFitImage.AutoSize = true;
            this.cbFitImage.Checked = true;
            this.cbFitImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFitImage.Location = new System.Drawing.Point(336, 63);
            this.cbFitImage.Name = "cbFitImage";
            this.cbFitImage.Size = new System.Drawing.Size(171, 29);
            this.cbFitImage.TabIndex = 7;
            this.cbFitImage.Text = "Auto-fit to width";
            this.cbFitImage.UseVisualStyleBackColor = true;
            this.cbFitImage.CheckedChanged += new System.EventHandler(this.CbFitImage_CheckedChanged);
            // 
            // btnToggleAutoScroll
            // 
            this.btnToggleAutoScroll.Location = new System.Drawing.Point(1214, 3);
            this.btnToggleAutoScroll.Name = "btnToggleAutoScroll";
            this.btnToggleAutoScroll.Size = new System.Drawing.Size(257, 59);
            this.btnToggleAutoScroll.TabIndex = 6;
            this.btnToggleAutoScroll.Text = "Start AutoScroll";
            this.btnToggleAutoScroll.UseVisualStyleBackColor = true;
            this.btnToggleAutoScroll.Click += new System.EventHandler(this.BtnToggleAutoScroll_Click);
            // 
            // lblScrollSpeed
            // 
            this.lblScrollSpeed.Location = new System.Drawing.Point(913, 49);
            this.lblScrollSpeed.Name = "lblScrollSpeed";
            this.lblScrollSpeed.Size = new System.Drawing.Size(295, 34);
            this.lblScrollSpeed.TabIndex = 5;
            this.lblScrollSpeed.Text = "Scroll Speed";
            this.lblScrollSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackScrollSpeed
            // 
            this.trackScrollSpeed.Location = new System.Drawing.Point(918, 3);
            this.trackScrollSpeed.Maximum = 15;
            this.trackScrollSpeed.Minimum = 1;
            this.trackScrollSpeed.Name = "trackScrollSpeed";
            this.trackScrollSpeed.Size = new System.Drawing.Size(290, 80);
            this.trackScrollSpeed.TabIndex = 4;
            this.trackScrollSpeed.Value = 5;
            this.trackScrollSpeed.Scroll += new System.EventHandler(this.TrackScrollSpeed_Scroll);
            // 
            // lblZoom
            // 
            this.lblZoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZoom.Location = new System.Drawing.Point(331, 9);
            this.lblZoom.Name = "lblZoom";
            this.lblZoom.Size = new System.Drawing.Size(260, 41);
            this.lblZoom.TabIndex = 3;
            this.lblZoom.Text = "Zoom";
            this.lblZoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackZoom
            // 
            this.trackZoom.LargeChange = 25;
            this.trackZoom.Location = new System.Drawing.Point(584, 3);
            this.trackZoom.Maximum = 150;
            this.trackZoom.Minimum = 10;
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
            // panel2
            // 
            this.panel2.Controls.Add(this.comboResultSets);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(466, 50);
            this.panel2.TabIndex = 3;
            // 
            // comboResultSets
            // 
            this.comboResultSets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboResultSets.FormattingEnabled = true;
            this.comboResultSets.Location = new System.Drawing.Point(0, 0);
            this.comboResultSets.Name = "comboResultSets";
            this.comboResultSets.Size = new System.Drawing.Size(466, 32);
            this.comboResultSets.TabIndex = 0;
            this.comboResultSets.SelectedIndexChanged += new System.EventHandler(this.comboResultSets_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeScreenshots);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.imageViewer);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1953, 1073);
            this.splitContainer1.SplitterDistance = 466;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeScreenshots
            // 
            this.treeScreenshots.AutoSize = true;
            this.treeScreenshots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeScreenshots.Location = new System.Drawing.Point(0, 50);
            this.treeScreenshots.Name = "treeScreenshots";
            this.treeScreenshots.SelectedFile = null;
            this.treeScreenshots.Size = new System.Drawing.Size(466, 1023);
            this.treeScreenshots.TabIndex = 4;
            this.treeScreenshots.ScreenshotSelected += new System.EventHandler(this.ScreenshotTree1_ScreenshotSelected);
            // 
            // imageViewer
            // 
            this.imageViewer.AutoscrollImage = false;
            this.imageViewer.AutoScrollImageDelay = 1000;
            this.imageViewer.AutoScrollImageSpeed = 2;
            this.imageViewer.ConstrainImageWidth = false;
            this.imageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewer.Image = null;
            this.imageViewer.ImageScale = 100;
            this.imageViewer.Location = new System.Drawing.Point(0, 183);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.Size = new System.Drawing.Size(1483, 890);
            this.imageViewer.TabIndex = 1;
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackScrollSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackZoom)).EndInit();
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox cbFitImage;
        private System.Windows.Forms.Button btnToggleAutoScroll;
        private System.Windows.Forms.Label lblScrollSpeed;
        private System.Windows.Forms.TrackBar trackScrollSpeed;
        private System.Windows.Forms.Label lblZoom;
        private System.Windows.Forms.TrackBar trackZoom;
        private System.Windows.Forms.Button btnShowInExplorer;
        private System.Windows.Forms.LinkLabel lnkUrl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox comboResultSets;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Controls.LargeImageViewer imageViewer;
        private Controls.ScreenshotTree treeScreenshots;
    }
}