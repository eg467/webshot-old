namespace Webshot.Controls
{
    partial class LargeImageViewer
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
            this.components = new System.ComponentModel.Container();
            this.pnlPic = new System.Windows.Forms.Panel();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.hscrollImg = new System.Windows.Forms.HScrollBar();
            this.vscrollImg = new System.Windows.Forms.VScrollBar();
            this.timerAutoscroll = new System.Windows.Forms.Timer(this.components);
            this.pnlPic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlPic
            // 
            this.pnlPic.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlPic.Controls.Add(this.picImage);
            this.pnlPic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPic.Location = new System.Drawing.Point(0, 0);
            this.pnlPic.Name = "pnlPic";
            this.pnlPic.Size = new System.Drawing.Size(1035, 861);
            this.pnlPic.TabIndex = 0;
            this.pnlPic.Resize += new System.EventHandler(this.PnlPic_Resize);
            // 
            // picImage
            // 
            this.picImage.Location = new System.Drawing.Point(3, 3);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(293, 233);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picImage.TabIndex = 0;
            this.picImage.TabStop = false;
            this.picImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Picture_MouseDown);
            this.picImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Picture_MouseMove);
            // 
            // hscrollImg
            // 
            this.hscrollImg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hscrollImg.Location = new System.Drawing.Point(0, 861);
            this.hscrollImg.Name = "hscrollImg";
            this.hscrollImg.Size = new System.Drawing.Size(1065, 30);
            this.hscrollImg.TabIndex = 1;
            this.hscrollImg.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HscrollImg_Scroll);
            // 
            // vscrollImg
            // 
            this.vscrollImg.Dock = System.Windows.Forms.DockStyle.Right;
            this.vscrollImg.Location = new System.Drawing.Point(1035, 0);
            this.vscrollImg.Name = "vscrollImg";
            this.vscrollImg.Size = new System.Drawing.Size(30, 861);
            this.vscrollImg.TabIndex = 2;
            this.vscrollImg.Scroll += new System.Windows.Forms.ScrollEventHandler(this.VscrollImg_Scroll);
            // 
            // timerAutoscroll
            // 
            this.timerAutoscroll.Interval = 5;
            this.timerAutoscroll.Tick += new System.EventHandler(this.TimerAutoscroll_Tick);
            // 
            // LargeImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlPic);
            this.Controls.Add(this.vscrollImg);
            this.Controls.Add(this.hscrollImg);
            this.Name = "LargeImageViewer";
            this.Size = new System.Drawing.Size(1065, 891);
            this.Load += new System.EventHandler(this.LargeImageViewer_Load);
            this.pnlPic.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlPic;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.HScrollBar hscrollImg;
        private System.Windows.Forms.VScrollBar vscrollImg;
        private System.Windows.Forms.Timer timerAutoscroll;
    }
}
