namespace DuplicateImageFinder.GUI
{
	partial class FrmMain
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
			this.grpBoxPreview = new System.Windows.Forms.GroupBox();
			this.grpBoxActions = new System.Windows.Forms.GroupBox();
			this.grpBoxScan = new System.Windows.Forms.GroupBox();
			this.listView = new System.Windows.Forms.ListView();
			this.btnSelectAll = new System.Windows.Forms.Button();
			this.btnClearAll = new System.Windows.Forms.Button();
			this.btnDeleteSelected = new System.Windows.Forms.Button();
			this.lblLocation = new System.Windows.Forms.Label();
			this.txtLocation = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.btnScan = new System.Windows.Forms.Button();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.grpBoxPreview.SuspendLayout();
			this.grpBoxActions.SuspendLayout();
			this.grpBoxScan.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpBoxPreview
			// 
			this.grpBoxPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpBoxPreview.Controls.Add(this.listView);
			this.grpBoxPreview.Location = new System.Drawing.Point(12, 12);
			this.grpBoxPreview.Name = "grpBoxPreview";
			this.grpBoxPreview.Size = new System.Drawing.Size(595, 342);
			this.grpBoxPreview.TabIndex = 0;
			this.grpBoxPreview.TabStop = false;
			this.grpBoxPreview.Text = "Preview Files";
			// 
			// grpBoxActions
			// 
			this.grpBoxActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpBoxActions.Controls.Add(this.btnSelectAll);
			this.grpBoxActions.Controls.Add(this.btnClearAll);
			this.grpBoxActions.Controls.Add(this.btnDeleteSelected);
			this.grpBoxActions.Location = new System.Drawing.Point(613, 12);
			this.grpBoxActions.Name = "grpBoxActions";
			this.grpBoxActions.Size = new System.Drawing.Size(159, 342);
			this.grpBoxActions.TabIndex = 1;
			this.grpBoxActions.TabStop = false;
			this.grpBoxActions.Text = "Actions";
			// 
			// grpBoxScan
			// 
			this.grpBoxScan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpBoxScan.Controls.Add(this.btnScan);
			this.grpBoxScan.Controls.Add(this.btnBrowse);
			this.grpBoxScan.Controls.Add(this.txtLocation);
			this.grpBoxScan.Controls.Add(this.lblLocation);
			this.grpBoxScan.Location = new System.Drawing.Point(12, 360);
			this.grpBoxScan.Name = "grpBoxScan";
			this.grpBoxScan.Size = new System.Drawing.Size(760, 60);
			this.grpBoxScan.TabIndex = 1;
			this.grpBoxScan.TabStop = false;
			this.grpBoxScan.Text = "Scan";
			// 
			// listView
			// 
			this.listView.CheckBoxes = true;
			this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView.Location = new System.Drawing.Point(3, 17);
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(589, 322);
			this.listView.TabIndex = 2;
			this.listView.UseCompatibleStateImageBehavior = false;
			// 
			// btnSelectAll
			// 
			this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSelectAll.AutoSize = true;
			this.btnSelectAll.Location = new System.Drawing.Point(6, 20);
			this.btnSelectAll.Name = "btnSelectAll";
			this.btnSelectAll.Size = new System.Drawing.Size(147, 25);
			this.btnSelectAll.TabIndex = 3;
			this.btnSelectAll.Text = "Select All";
			this.btnSelectAll.UseVisualStyleBackColor = true;
			// 
			// btnClearAll
			// 
			this.btnClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClearAll.AutoSize = true;
			this.btnClearAll.Location = new System.Drawing.Point(6, 49);
			this.btnClearAll.Name = "btnClearAll";
			this.btnClearAll.Size = new System.Drawing.Size(147, 25);
			this.btnClearAll.TabIndex = 4;
			this.btnClearAll.Text = "Clear All";
			this.btnClearAll.UseVisualStyleBackColor = true;
			// 
			// btnDeleteSelected
			// 
			this.btnDeleteSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDeleteSelected.AutoSize = true;
			this.btnDeleteSelected.Location = new System.Drawing.Point(6, 78);
			this.btnDeleteSelected.Name = "btnDeleteSelected";
			this.btnDeleteSelected.Size = new System.Drawing.Size(147, 25);
			this.btnDeleteSelected.TabIndex = 5;
			this.btnDeleteSelected.Text = "Delete Selected";
			this.btnDeleteSelected.UseVisualStyleBackColor = true;
			// 
			// lblLocation
			// 
			this.lblLocation.AutoSize = true;
			this.lblLocation.Location = new System.Drawing.Point(6, 24);
			this.lblLocation.Name = "lblLocation";
			this.lblLocation.Size = new System.Drawing.Size(98, 15);
			this.lblLocation.TabIndex = 0;
			this.lblLocation.Text = "Images Location";
			// 
			// txtLocation
			// 
			this.txtLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.txtLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
			this.txtLocation.Location = new System.Drawing.Point(107, 21);
			this.txtLocation.Name = "txtLocation";
			this.txtLocation.Size = new System.Drawing.Size(485, 21);
			this.txtLocation.TabIndex = 1;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.AutoSize = true;
			this.btnBrowse.Location = new System.Drawing.Point(598, 19);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 25);
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "&Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// btnScan
			// 
			this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnScan.AutoSize = true;
			this.btnScan.Location = new System.Drawing.Point(679, 19);
			this.btnScan.Name = "btnScan";
			this.btnScan.Size = new System.Drawing.Size(75, 25);
			this.btnScan.TabIndex = 3;
			this.btnScan.Text = "&Scan";
			this.btnScan.UseVisualStyleBackColor = true;
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
			this.statusStrip.Location = new System.Drawing.Point(0, 440);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(784, 22);
			this.statusStrip.TabIndex = 2;
			this.statusStrip.Text = "statusStrip1";
			// 
			// lblStatus
			// 
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(39, 17);
			this.lblStatus.Text = "Ready";
			// 
			// FrmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 462);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.grpBoxScan);
			this.Controls.Add(this.grpBoxActions);
			this.Controls.Add(this.grpBoxPreview);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MinimumSize = new System.Drawing.Size(800, 500);
			this.Name = "FrmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Duplicate Image Finder";
			this.grpBoxPreview.ResumeLayout(false);
			this.grpBoxActions.ResumeLayout(false);
			this.grpBoxActions.PerformLayout();
			this.grpBoxScan.ResumeLayout(false);
			this.grpBoxScan.PerformLayout();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox grpBoxPreview;
		private System.Windows.Forms.GroupBox grpBoxActions;
		private System.Windows.Forms.GroupBox grpBoxScan;
		private System.Windows.Forms.ListView listView;
		private System.Windows.Forms.Button btnSelectAll;
		private System.Windows.Forms.Button btnClearAll;
		private System.Windows.Forms.Button btnDeleteSelected;
		private System.Windows.Forms.Button btnScan;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TextBox txtLocation;
		private System.Windows.Forms.Label lblLocation;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
	}
}

