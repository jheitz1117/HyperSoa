namespace TestClient
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlLeft = new Panel();
            btnHostingTestForm = new Button();
            btnLaunchRemoteAdmin = new Button();
            grpRealTimeResponse = new GroupBox();
            lstRealTimeResponse = new ListBox();
            grpRealTimeTaskTrace = new GroupBox();
            tvwRealTimeTaskTrace = new TreeView();
            btnRefreshCommandList = new Button();
            lblCommandName = new Label();
            cboCommandNames = new ComboBox();
            chkCacheProgressInfo = new CheckBox();
            chkRunConcurrently = new CheckBox();
            chkReturnTaskTrace = new CheckBox();
            btnRunCommand = new Button();
            grpHyperNodeActivity = new GroupBox();
            spcHyperNodeActivity = new SplitContainer();
            grpCachedActivity = new GroupBox();
            lstCachedActivityItems = new ListBox();
            grpCachedTaskTrace = new GroupBox();
            tvwCachedTaskTrace = new TreeView();
            grpCachedResponseSummary = new GroupBox();
            lstCachedResponseSummary = new ListBox();
            pnlHyperNodeActivityTop = new Panel();
            txtTaskId = new TextBox();
            btnCancelCurrentTask = new Button();
            lblTaskId = new Label();
            pnlLeft.SuspendLayout();
            grpRealTimeResponse.SuspendLayout();
            grpRealTimeTaskTrace.SuspendLayout();
            grpHyperNodeActivity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spcHyperNodeActivity).BeginInit();
            spcHyperNodeActivity.Panel1.SuspendLayout();
            spcHyperNodeActivity.Panel2.SuspendLayout();
            spcHyperNodeActivity.SuspendLayout();
            grpCachedActivity.SuspendLayout();
            grpCachedTaskTrace.SuspendLayout();
            grpCachedResponseSummary.SuspendLayout();
            pnlHyperNodeActivityTop.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLeft
            // 
            pnlLeft.Controls.Add(btnHostingTestForm);
            pnlLeft.Controls.Add(btnLaunchRemoteAdmin);
            pnlLeft.Controls.Add(grpRealTimeResponse);
            pnlLeft.Controls.Add(grpRealTimeTaskTrace);
            pnlLeft.Controls.Add(btnRefreshCommandList);
            pnlLeft.Controls.Add(lblCommandName);
            pnlLeft.Controls.Add(cboCommandNames);
            pnlLeft.Controls.Add(chkCacheProgressInfo);
            pnlLeft.Controls.Add(chkRunConcurrently);
            pnlLeft.Controls.Add(chkReturnTaskTrace);
            pnlLeft.Controls.Add(btnRunCommand);
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(369, 664);
            pnlLeft.TabIndex = 0;
            // 
            // btnHostingTestForm
            // 
            btnHostingTestForm.Location = new Point(3, 3);
            btnHostingTestForm.Name = "btnHostingTestForm";
            btnHostingTestForm.Size = new Size(141, 23);
            btnHostingTestForm.TabIndex = 14;
            btnHostingTestForm.Text = "Hosting Test Form";
            btnHostingTestForm.UseVisualStyleBackColor = true;
            btnHostingTestForm.Click += btnHostingTestForm_Click;
            // 
            // btnLaunchRemoteAdmin
            // 
            btnLaunchRemoteAdmin.Location = new Point(258, 3);
            btnLaunchRemoteAdmin.Name = "btnLaunchRemoteAdmin";
            btnLaunchRemoteAdmin.Size = new Size(108, 23);
            btnLaunchRemoteAdmin.TabIndex = 13;
            btnLaunchRemoteAdmin.Text = "Remote Admin";
            btnLaunchRemoteAdmin.UseVisualStyleBackColor = true;
            btnLaunchRemoteAdmin.Click += btnLaunchRemoteAdmin_Click;
            // 
            // grpRealTimeResponse
            // 
            grpRealTimeResponse.Controls.Add(lstRealTimeResponse);
            grpRealTimeResponse.Dock = DockStyle.Bottom;
            grpRealTimeResponse.Location = new Point(0, 227);
            grpRealTimeResponse.Name = "grpRealTimeResponse";
            grpRealTimeResponse.Size = new Size(369, 221);
            grpRealTimeResponse.TabIndex = 11;
            grpRealTimeResponse.TabStop = false;
            grpRealTimeResponse.Text = "Real Time Response";
            // 
            // lstRealTimeResponse
            // 
            lstRealTimeResponse.Dock = DockStyle.Fill;
            lstRealTimeResponse.FormattingEnabled = true;
            lstRealTimeResponse.ItemHeight = 15;
            lstRealTimeResponse.Location = new Point(3, 19);
            lstRealTimeResponse.Name = "lstRealTimeResponse";
            lstRealTimeResponse.Size = new Size(363, 199);
            lstRealTimeResponse.TabIndex = 3;
            // 
            // grpRealTimeTaskTrace
            // 
            grpRealTimeTaskTrace.Controls.Add(tvwRealTimeTaskTrace);
            grpRealTimeTaskTrace.Dock = DockStyle.Bottom;
            grpRealTimeTaskTrace.Location = new Point(0, 448);
            grpRealTimeTaskTrace.Name = "grpRealTimeTaskTrace";
            grpRealTimeTaskTrace.Size = new Size(369, 216);
            grpRealTimeTaskTrace.TabIndex = 12;
            grpRealTimeTaskTrace.TabStop = false;
            grpRealTimeTaskTrace.Text = "Real Time Task Trace";
            // 
            // tvwRealTimeTaskTrace
            // 
            tvwRealTimeTaskTrace.Dock = DockStyle.Fill;
            tvwRealTimeTaskTrace.Location = new Point(3, 19);
            tvwRealTimeTaskTrace.Name = "tvwRealTimeTaskTrace";
            tvwRealTimeTaskTrace.Size = new Size(363, 194);
            tvwRealTimeTaskTrace.TabIndex = 4;
            // 
            // btnRefreshCommandList
            // 
            btnRefreshCommandList.Location = new Point(291, 37);
            btnRefreshCommandList.Name = "btnRefreshCommandList";
            btnRefreshCommandList.Size = new Size(75, 23);
            btnRefreshCommandList.TabIndex = 10;
            btnRefreshCommandList.Text = "Refresh";
            btnRefreshCommandList.UseVisualStyleBackColor = true;
            btnRefreshCommandList.Click += btnRefreshCommandList_Click;
            // 
            // lblCommandName
            // 
            lblCommandName.AutoSize = true;
            lblCommandName.Location = new Point(12, 41);
            lblCommandName.Name = "lblCommandName";
            lblCommandName.Size = new Size(64, 15);
            lblCommandName.TabIndex = 9;
            lblCommandName.Text = "Command";
            // 
            // cboCommandNames
            // 
            cboCommandNames.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCommandNames.FormattingEnabled = true;
            cboCommandNames.Location = new Point(82, 38);
            cboCommandNames.Name = "cboCommandNames";
            cboCommandNames.Size = new Size(203, 23);
            cboCommandNames.TabIndex = 8;
            // 
            // chkCacheProgressInfo
            // 
            chkCacheProgressInfo.AutoSize = true;
            chkCacheProgressInfo.Checked = true;
            chkCacheProgressInfo.CheckState = CheckState.Checked;
            chkCacheProgressInfo.Location = new Point(169, 131);
            chkCacheProgressInfo.Name = "chkCacheProgressInfo";
            chkCacheProgressInfo.Size = new Size(131, 19);
            chkCacheProgressInfo.TabIndex = 7;
            chkCacheProgressInfo.Text = "Cache Progress Info";
            chkCacheProgressInfo.UseVisualStyleBackColor = true;
            // 
            // chkRunConcurrently
            // 
            chkRunConcurrently.AutoSize = true;
            chkRunConcurrently.Checked = true;
            chkRunConcurrently.CheckState = CheckState.Checked;
            chkRunConcurrently.Location = new Point(169, 112);
            chkRunConcurrently.Name = "chkRunConcurrently";
            chkRunConcurrently.Size = new Size(144, 19);
            chkRunConcurrently.TabIndex = 6;
            chkRunConcurrently.Text = "Run Task Concurrently";
            chkRunConcurrently.UseVisualStyleBackColor = true;
            // 
            // chkReturnTaskTrace
            // 
            chkReturnTaskTrace.AutoSize = true;
            chkReturnTaskTrace.Location = new Point(169, 93);
            chkReturnTaskTrace.Name = "chkReturnTaskTrace";
            chkReturnTaskTrace.Size = new Size(116, 19);
            chkReturnTaskTrace.TabIndex = 5;
            chkReturnTaskTrace.Text = "Return Task Trace";
            chkReturnTaskTrace.UseVisualStyleBackColor = true;
            // 
            // btnRunCommand
            // 
            btnRunCommand.Location = new Point(291, 66);
            btnRunCommand.Name = "btnRunCommand";
            btnRunCommand.Size = new Size(75, 23);
            btnRunCommand.TabIndex = 0;
            btnRunCommand.Text = "Run";
            btnRunCommand.UseVisualStyleBackColor = true;
            btnRunCommand.Click += btnRunCommand_Click;
            // 
            // grpHyperNodeActivity
            // 
            grpHyperNodeActivity.Controls.Add(spcHyperNodeActivity);
            grpHyperNodeActivity.Controls.Add(pnlHyperNodeActivityTop);
            grpHyperNodeActivity.Dock = DockStyle.Fill;
            grpHyperNodeActivity.Location = new Point(369, 0);
            grpHyperNodeActivity.Name = "grpHyperNodeActivity";
            grpHyperNodeActivity.Size = new Size(825, 664);
            grpHyperNodeActivity.TabIndex = 1;
            grpHyperNodeActivity.TabStop = false;
            grpHyperNodeActivity.Text = "HyperNode Activity";
            // 
            // spcHyperNodeActivity
            // 
            spcHyperNodeActivity.Dock = DockStyle.Fill;
            spcHyperNodeActivity.Location = new Point(3, 43);
            spcHyperNodeActivity.Name = "spcHyperNodeActivity";
            spcHyperNodeActivity.Orientation = Orientation.Horizontal;
            // 
            // spcHyperNodeActivity.Panel1
            // 
            spcHyperNodeActivity.Panel1.Controls.Add(grpCachedActivity);
            // 
            // spcHyperNodeActivity.Panel2
            // 
            spcHyperNodeActivity.Panel2.Controls.Add(grpCachedTaskTrace);
            spcHyperNodeActivity.Panel2.Controls.Add(grpCachedResponseSummary);
            spcHyperNodeActivity.Size = new Size(819, 618);
            spcHyperNodeActivity.SplitterDistance = 314;
            spcHyperNodeActivity.TabIndex = 4;
            // 
            // grpCachedActivity
            // 
            grpCachedActivity.Controls.Add(lstCachedActivityItems);
            grpCachedActivity.Dock = DockStyle.Fill;
            grpCachedActivity.Location = new Point(0, 0);
            grpCachedActivity.Name = "grpCachedActivity";
            grpCachedActivity.Size = new Size(819, 314);
            grpCachedActivity.TabIndex = 1;
            grpCachedActivity.TabStop = false;
            grpCachedActivity.Text = "Cached Activity";
            // 
            // lstCachedActivityItems
            // 
            lstCachedActivityItems.Dock = DockStyle.Fill;
            lstCachedActivityItems.FormattingEnabled = true;
            lstCachedActivityItems.ItemHeight = 15;
            lstCachedActivityItems.Location = new Point(3, 19);
            lstCachedActivityItems.Name = "lstCachedActivityItems";
            lstCachedActivityItems.Size = new Size(813, 292);
            lstCachedActivityItems.TabIndex = 0;
            // 
            // grpCachedTaskTrace
            // 
            grpCachedTaskTrace.Controls.Add(tvwCachedTaskTrace);
            grpCachedTaskTrace.Dock = DockStyle.Fill;
            grpCachedTaskTrace.Location = new Point(292, 0);
            grpCachedTaskTrace.Name = "grpCachedTaskTrace";
            grpCachedTaskTrace.Size = new Size(527, 300);
            grpCachedTaskTrace.TabIndex = 3;
            grpCachedTaskTrace.TabStop = false;
            grpCachedTaskTrace.Text = "Cached Task Trace";
            // 
            // tvwCachedTaskTrace
            // 
            tvwCachedTaskTrace.Dock = DockStyle.Fill;
            tvwCachedTaskTrace.Location = new Point(3, 19);
            tvwCachedTaskTrace.Name = "tvwCachedTaskTrace";
            tvwCachedTaskTrace.Size = new Size(521, 278);
            tvwCachedTaskTrace.TabIndex = 0;
            // 
            // grpCachedResponseSummary
            // 
            grpCachedResponseSummary.Controls.Add(lstCachedResponseSummary);
            grpCachedResponseSummary.Dock = DockStyle.Left;
            grpCachedResponseSummary.Location = new Point(0, 0);
            grpCachedResponseSummary.Name = "grpCachedResponseSummary";
            grpCachedResponseSummary.Size = new Size(292, 300);
            grpCachedResponseSummary.TabIndex = 2;
            grpCachedResponseSummary.TabStop = false;
            grpCachedResponseSummary.Text = "Cached Response Summary";
            // 
            // lstCachedResponseSummary
            // 
            lstCachedResponseSummary.Dock = DockStyle.Fill;
            lstCachedResponseSummary.FormattingEnabled = true;
            lstCachedResponseSummary.ItemHeight = 15;
            lstCachedResponseSummary.Location = new Point(3, 19);
            lstCachedResponseSummary.Name = "lstCachedResponseSummary";
            lstCachedResponseSummary.Size = new Size(286, 278);
            lstCachedResponseSummary.TabIndex = 0;
            // 
            // pnlHyperNodeActivityTop
            // 
            pnlHyperNodeActivityTop.Controls.Add(txtTaskId);
            pnlHyperNodeActivityTop.Controls.Add(btnCancelCurrentTask);
            pnlHyperNodeActivityTop.Controls.Add(lblTaskId);
            pnlHyperNodeActivityTop.Dock = DockStyle.Top;
            pnlHyperNodeActivityTop.Location = new Point(3, 19);
            pnlHyperNodeActivityTop.Name = "pnlHyperNodeActivityTop";
            pnlHyperNodeActivityTop.Size = new Size(819, 24);
            pnlHyperNodeActivityTop.TabIndex = 0;
            // 
            // txtTaskId
            // 
            txtTaskId.Dock = DockStyle.Fill;
            txtTaskId.Location = new Point(43, 0);
            txtTaskId.Name = "txtTaskId";
            txtTaskId.Size = new Size(701, 23);
            txtTaskId.TabIndex = 2;
            // 
            // btnCancelCurrentTask
            // 
            btnCancelCurrentTask.Dock = DockStyle.Right;
            btnCancelCurrentTask.Location = new Point(744, 0);
            btnCancelCurrentTask.Name = "btnCancelCurrentTask";
            btnCancelCurrentTask.Size = new Size(75, 24);
            btnCancelCurrentTask.TabIndex = 1;
            btnCancelCurrentTask.Text = "Cancel";
            btnCancelCurrentTask.UseVisualStyleBackColor = true;
            btnCancelCurrentTask.Click += btnCancelCurrentTask_Click;
            // 
            // lblTaskId
            // 
            lblTaskId.Dock = DockStyle.Left;
            lblTaskId.Location = new Point(0, 0);
            lblTaskId.Name = "lblTaskId";
            lblTaskId.Size = new Size(43, 24);
            lblTaskId.TabIndex = 0;
            lblTaskId.Text = "Task ID";
            lblTaskId.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1194, 664);
            Controls.Add(grpHyperNodeActivity);
            Controls.Add(pnlLeft);
            Name = "MainForm";
            Text = "HyperNode Test Client";
            pnlLeft.ResumeLayout(false);
            pnlLeft.PerformLayout();
            grpRealTimeResponse.ResumeLayout(false);
            grpRealTimeTaskTrace.ResumeLayout(false);
            grpHyperNodeActivity.ResumeLayout(false);
            spcHyperNodeActivity.Panel1.ResumeLayout(false);
            spcHyperNodeActivity.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)spcHyperNodeActivity).EndInit();
            spcHyperNodeActivity.ResumeLayout(false);
            grpCachedActivity.ResumeLayout(false);
            grpCachedTaskTrace.ResumeLayout(false);
            grpCachedResponseSummary.ResumeLayout(false);
            pnlHyperNodeActivityTop.ResumeLayout(false);
            pnlHyperNodeActivityTop.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlLeft;
        private Button btnRunCommand;
        private ListBox lstRealTimeResponse;
        private GroupBox grpHyperNodeActivity;
        private Panel pnlHyperNodeActivityTop;
        private Button btnCancelCurrentTask;
        private Label lblTaskId;
        private GroupBox grpCachedActivity;
        private TextBox txtTaskId;
        private GroupBox grpCachedTaskTrace;
        private TreeView tvwCachedTaskTrace;
        private GroupBox grpCachedResponseSummary;
        private ListBox lstCachedResponseSummary;
        private ListBox lstCachedActivityItems;
        private TreeView tvwRealTimeTaskTrace;
        private CheckBox chkCacheProgressInfo;
        private CheckBox chkRunConcurrently;
        private CheckBox chkReturnTaskTrace;
        private Label lblCommandName;
        private ComboBox cboCommandNames;
        private Button btnRefreshCommandList;
        private GroupBox grpRealTimeResponse;
        private GroupBox grpRealTimeTaskTrace;
        private SplitContainer spcHyperNodeActivity;
        private Button btnLaunchRemoteAdmin;
        private Button btnHostingTestForm;
    }
}