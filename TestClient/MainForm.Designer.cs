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
            grpTaskTrace = new GroupBox();
            tvwTaskTrace = new TreeView();
            grpResponseSummary = new GroupBox();
            lstResponseSummary = new ListBox();
            grpActivityItems = new GroupBox();
            lstActivityItems = new ListBox();
            pnlHyperNodeActivityTop = new Panel();
            txtTaskId = new TextBox();
            btnCancelCurrentTask = new Button();
            lblTaskId = new Label();
            pnlLeft.SuspendLayout();
            grpRealTimeResponse.SuspendLayout();
            grpRealTimeTaskTrace.SuspendLayout();
            grpHyperNodeActivity.SuspendLayout();
            grpTaskTrace.SuspendLayout();
            grpResponseSummary.SuspendLayout();
            grpActivityItems.SuspendLayout();
            pnlHyperNodeActivityTop.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLeft
            // 
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
            grpHyperNodeActivity.Controls.Add(grpTaskTrace);
            grpHyperNodeActivity.Controls.Add(grpResponseSummary);
            grpHyperNodeActivity.Controls.Add(grpActivityItems);
            grpHyperNodeActivity.Controls.Add(pnlHyperNodeActivityTop);
            grpHyperNodeActivity.Dock = DockStyle.Fill;
            grpHyperNodeActivity.Location = new Point(369, 0);
            grpHyperNodeActivity.Name = "grpHyperNodeActivity";
            grpHyperNodeActivity.Size = new Size(566, 664);
            grpHyperNodeActivity.TabIndex = 1;
            grpHyperNodeActivity.TabStop = false;
            grpHyperNodeActivity.Text = "HyperNode Activity";
            // 
            // grpTaskTrace
            // 
            grpTaskTrace.Controls.Add(tvwTaskTrace);
            grpTaskTrace.Location = new Point(284, 386);
            grpTaskTrace.Name = "grpTaskTrace";
            grpTaskTrace.Size = new Size(193, 193);
            grpTaskTrace.TabIndex = 3;
            grpTaskTrace.TabStop = false;
            grpTaskTrace.Text = "Task Trace";
            // 
            // tvwTaskTrace
            // 
            tvwTaskTrace.Dock = DockStyle.Fill;
            tvwTaskTrace.Location = new Point(3, 19);
            tvwTaskTrace.Name = "tvwTaskTrace";
            tvwTaskTrace.Size = new Size(187, 171);
            tvwTaskTrace.TabIndex = 0;
            // 
            // grpResponseSummary
            // 
            grpResponseSummary.Controls.Add(lstResponseSummary);
            grpResponseSummary.Location = new Point(22, 380);
            grpResponseSummary.Name = "grpResponseSummary";
            grpResponseSummary.Size = new Size(215, 210);
            grpResponseSummary.TabIndex = 2;
            grpResponseSummary.TabStop = false;
            grpResponseSummary.Text = "Response Summary";
            // 
            // lstResponseSummary
            // 
            lstResponseSummary.Dock = DockStyle.Fill;
            lstResponseSummary.FormattingEnabled = true;
            lstResponseSummary.ItemHeight = 15;
            lstResponseSummary.Location = new Point(3, 19);
            lstResponseSummary.Name = "lstResponseSummary";
            lstResponseSummary.Size = new Size(209, 188);
            lstResponseSummary.TabIndex = 0;
            // 
            // grpActivityItems
            // 
            grpActivityItems.Controls.Add(lstActivityItems);
            grpActivityItems.Location = new Point(46, 93);
            grpActivityItems.Name = "grpActivityItems";
            grpActivityItems.Size = new Size(306, 160);
            grpActivityItems.TabIndex = 1;
            grpActivityItems.TabStop = false;
            grpActivityItems.Text = "Activity Items";
            // 
            // lstActivityItems
            // 
            lstActivityItems.Dock = DockStyle.Fill;
            lstActivityItems.FormattingEnabled = true;
            lstActivityItems.ItemHeight = 15;
            lstActivityItems.Location = new Point(3, 19);
            lstActivityItems.Name = "lstActivityItems";
            lstActivityItems.Size = new Size(300, 138);
            lstActivityItems.TabIndex = 0;
            // 
            // pnlHyperNodeActivityTop
            // 
            pnlHyperNodeActivityTop.Controls.Add(txtTaskId);
            pnlHyperNodeActivityTop.Controls.Add(btnCancelCurrentTask);
            pnlHyperNodeActivityTop.Controls.Add(lblTaskId);
            pnlHyperNodeActivityTop.Dock = DockStyle.Top;
            pnlHyperNodeActivityTop.Location = new Point(3, 19);
            pnlHyperNodeActivityTop.Name = "pnlHyperNodeActivityTop";
            pnlHyperNodeActivityTop.Size = new Size(560, 24);
            pnlHyperNodeActivityTop.TabIndex = 0;
            // 
            // txtTaskId
            // 
            txtTaskId.Dock = DockStyle.Fill;
            txtTaskId.Location = new Point(43, 0);
            txtTaskId.Name = "txtTaskId";
            txtTaskId.Size = new Size(442, 23);
            txtTaskId.TabIndex = 2;
            // 
            // btnCancelCurrentTask
            // 
            btnCancelCurrentTask.Dock = DockStyle.Right;
            btnCancelCurrentTask.Location = new Point(485, 0);
            btnCancelCurrentTask.Name = "btnCancelCurrentTask";
            btnCancelCurrentTask.Size = new Size(75, 24);
            btnCancelCurrentTask.TabIndex = 1;
            btnCancelCurrentTask.Text = "Cancel";
            btnCancelCurrentTask.UseVisualStyleBackColor = true;
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
            ClientSize = new Size(935, 664);
            Controls.Add(grpHyperNodeActivity);
            Controls.Add(pnlLeft);
            Name = "MainForm";
            Text = "HyperNode Test Client";
            pnlLeft.ResumeLayout(false);
            pnlLeft.PerformLayout();
            grpRealTimeResponse.ResumeLayout(false);
            grpRealTimeTaskTrace.ResumeLayout(false);
            grpHyperNodeActivity.ResumeLayout(false);
            grpTaskTrace.ResumeLayout(false);
            grpResponseSummary.ResumeLayout(false);
            grpActivityItems.ResumeLayout(false);
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
        private GroupBox grpActivityItems;
        private TextBox txtTaskId;
        private GroupBox grpTaskTrace;
        private TreeView tvwTaskTrace;
        private GroupBox grpResponseSummary;
        private ListBox lstResponseSummary;
        private ListBox lstActivityItems;
        private TreeView tvwRealTimeTaskTrace;
        private CheckBox chkCacheProgressInfo;
        private CheckBox chkRunConcurrently;
        private CheckBox chkReturnTaskTrace;
        private Label lblCommandName;
        private ComboBox cboCommandNames;
        private Button btnRefreshCommandList;
        private GroupBox grpRealTimeResponse;
        private GroupBox grpRealTimeTaskTrace;
    }
}