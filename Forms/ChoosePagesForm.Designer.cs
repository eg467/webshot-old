namespace Webshot
{
    partial class ChoosePagesForm
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
            this.cblAvailableUris = new System.Windows.Forms.CheckedListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.tabContainer = new System.Windows.Forms.TabControl();
            this.tabPages = new System.Windows.Forms.TabPage();
            this.tabErrors = new System.Windows.Forms.TabPage();
            this.lvErrors = new System.Windows.Forms.ListView();
            this.Uri = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Message = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableErrors = new System.Windows.Forms.TableLayoutPanel();
            this.panel1.SuspendLayout();
            this.tabContainer.SuspendLayout();
            this.tabPages.SuspendLayout();
            this.tabErrors.SuspendLayout();
            this.SuspendLayout();
            // 
            // cblAvailableUris
            // 
            this.cblAvailableUris.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cblAvailableUris.FormattingEnabled = true;
            this.cblAvailableUris.Location = new System.Drawing.Point(3, 3);
            this.cblAvailableUris.Name = "cblAvailableUris";
            this.cblAvailableUris.Size = new System.Drawing.Size(1533, 688);
            this.cblAvailableUris.TabIndex = 0;
            this.cblAvailableUris.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CblAvailableUris_ItemCheck);
            this.cblAvailableUris.SelectedIndexChanged += new System.EventHandler(this.CblAvailableUris_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSelectNone);
            this.panel1.Controls.Add(this.btnSelectAll);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSelect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 731);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1547, 134);
            this.panel1.TabIndex = 1;
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Location = new System.Drawing.Point(845, 16);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(155, 72);
            this.btnSelectNone.TabIndex = 3;
            this.btnSelectNone.Text = "Select None";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.BtnSelectNone_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(663, 16);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(155, 72);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.BtnSelectAll_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(378, 31);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(139, 57);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(12, 31);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(349, 57);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Text = "Use Selected Pages";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // tabContainer
            // 
            this.tabContainer.Controls.Add(this.tabPages);
            this.tabContainer.Controls.Add(this.tabErrors);
            this.tabContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabContainer.Location = new System.Drawing.Point(0, 0);
            this.tabContainer.Name = "tabContainer";
            this.tabContainer.SelectedIndex = 0;
            this.tabContainer.Size = new System.Drawing.Size(1547, 731);
            this.tabContainer.TabIndex = 2;
            // 
            // tabPages
            // 
            this.tabPages.Controls.Add(this.cblAvailableUris);
            this.tabPages.Location = new System.Drawing.Point(4, 33);
            this.tabPages.Name = "tabPages";
            this.tabPages.Padding = new System.Windows.Forms.Padding(3);
            this.tabPages.Size = new System.Drawing.Size(1539, 694);
            this.tabPages.TabIndex = 0;
            this.tabPages.Text = "Available Pages";
            this.tabPages.UseVisualStyleBackColor = true;
            // 
            // tabErrors
            // 
            this.tabErrors.AutoScroll = true;
            this.tabErrors.Controls.Add(this.lvErrors);
            this.tabErrors.Controls.Add(this.tableErrors);
            this.tabErrors.Location = new System.Drawing.Point(4, 33);
            this.tabErrors.Name = "tabErrors";
            this.tabErrors.Padding = new System.Windows.Forms.Padding(3);
            this.tabErrors.Size = new System.Drawing.Size(1539, 694);
            this.tabErrors.TabIndex = 1;
            this.tabErrors.Text = "Errors";
            this.tabErrors.UseVisualStyleBackColor = true;
            // 
            // lvErrors
            // 
            this.lvErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Uri,
            this.Status,
            this.Message});
            this.lvErrors.FullRowSelect = true;
            this.lvErrors.HideSelection = false;
            this.lvErrors.Location = new System.Drawing.Point(8, 134);
            this.lvErrors.Name = "lvErrors";
            this.lvErrors.Size = new System.Drawing.Size(1523, 385);
            this.lvErrors.TabIndex = 1;
            this.lvErrors.UseCompatibleStateImageBehavior = false;
            this.lvErrors.View = System.Windows.Forms.View.Details;
            this.lvErrors.DoubleClick += new System.EventHandler(this.LvErrors_DoubleClick);
            // 
            // Uri
            // 
            this.Uri.Text = "Uri";
            this.Uri.Width = 500;
            // 
            // Status
            // 
            this.Status.Text = "Status";
            this.Status.Width = 200;
            // 
            // Message
            // 
            this.Message.Text = "Message";
            this.Message.Width = 500;
            // 
            // tableErrors
            // 
            this.tableErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableErrors.ColumnCount = 3;
            this.tableErrors.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableErrors.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableErrors.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableErrors.Location = new System.Drawing.Point(8, 6);
            this.tableErrors.Name = "tableErrors";
            this.tableErrors.RowCount = 1;
            this.tableErrors.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableErrors.Size = new System.Drawing.Size(1523, 100);
            this.tableErrors.TabIndex = 0;
            // 
            // ChoosePagesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1547, 865);
            this.Controls.Add(this.tabContainer);
            this.Controls.Add(this.panel1);
            this.Name = "ChoosePagesForm";
            this.Text = "Choose Pages";
            this.Load += new System.EventHandler(this.ChoosePagesForm_Load);
            this.panel1.ResumeLayout(false);
            this.tabContainer.ResumeLayout(false);
            this.tabPages.ResumeLayout(false);
            this.tabErrors.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox cblAvailableUris;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.TabControl tabContainer;
        private System.Windows.Forms.TabPage tabPages;
        private System.Windows.Forms.TabPage tabErrors;
        private System.Windows.Forms.TableLayoutPanel tableErrors;
        private System.Windows.Forms.ListView lvErrors;
        private System.Windows.Forms.ColumnHeader Uri;
        private System.Windows.Forms.ColumnHeader Status;
        private System.Windows.Forms.ColumnHeader Message;
    }
}