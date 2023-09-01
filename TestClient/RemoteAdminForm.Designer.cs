namespace TestClient
{
    partial class RemoteAdminForm
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            txtEchoPrompt = new TextBox();
            lblEchoPrompt = new Label();
            btnEcho = new Button();
            chkCacheProgressInfo = new CheckBox();
            chkReturnTaskTrace = new CheckBox();
            cboEndpoints = new ComboBox();
            lblEndpoint = new Label();
            btnConnect = new Button();
            grpTarget = new GroupBox();
            txtEndpointUri = new TextBox();
            pnlTargetLeft = new Panel();
            pnlTargetRight = new Panel();
            btnDisconnect = new Button();
            grpLiveTasks = new GroupBox();
            grdLiveTasks = new DataGridView();
            bsLiveTasks = new BindingSource(components);
            pnlNodeStatusTop = new Panel();
            chkEnableProgressCache = new CheckBox();
            chkEnableDiagnostics = new CheckBox();
            btnRefreshNodeStatus = new Button();
            pnlServiceDetails = new Panel();
            grpCommands = new GroupBox();
            grdCommands = new DataGridView();
            enabledDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
            commandNameDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            commandTypeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            bsCommands = new BindingSource(components);
            tmrRefreshNodeStatus = new System.Windows.Forms.Timer(components);
            CancelDataGridViewButtonColumn = new DataGridViewButtonColumn();
            commandNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            elapsedDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            taskIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            Status = new DataGridViewTextBoxColumn();
            grpTarget.SuspendLayout();
            pnlTargetLeft.SuspendLayout();
            pnlTargetRight.SuspendLayout();
            grpLiveTasks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grdLiveTasks).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bsLiveTasks).BeginInit();
            pnlNodeStatusTop.SuspendLayout();
            pnlServiceDetails.SuspendLayout();
            grpCommands.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grdCommands).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bsCommands).BeginInit();
            SuspendLayout();
            // 
            // txtEchoPrompt
            // 
            txtEchoPrompt.Location = new Point(437, 71);
            txtEchoPrompt.Name = "txtEchoPrompt";
            txtEchoPrompt.Size = new Size(100, 23);
            txtEchoPrompt.TabIndex = 0;
            txtEchoPrompt.Text = "Hello!";
            // 
            // lblEchoPrompt
            // 
            lblEchoPrompt.AutoSize = true;
            lblEchoPrompt.Location = new Point(381, 74);
            lblEchoPrompt.Name = "lblEchoPrompt";
            lblEchoPrompt.Size = new Size(50, 15);
            lblEchoPrompt.TabIndex = 1;
            lblEchoPrompt.Text = "Prompt:";
            // 
            // btnEcho
            // 
            btnEcho.Location = new Point(543, 71);
            btnEcho.Name = "btnEcho";
            btnEcho.Size = new Size(75, 23);
            btnEcho.TabIndex = 2;
            btnEcho.Text = "Echo";
            btnEcho.UseVisualStyleBackColor = true;
            btnEcho.Click += btnEcho_Click;
            // 
            // chkCacheProgressInfo
            // 
            chkCacheProgressInfo.AutoSize = true;
            chkCacheProgressInfo.Location = new Point(381, 132);
            chkCacheProgressInfo.Name = "chkCacheProgressInfo";
            chkCacheProgressInfo.Size = new Size(131, 19);
            chkCacheProgressInfo.TabIndex = 9;
            chkCacheProgressInfo.Text = "Cache Progress Info";
            chkCacheProgressInfo.UseVisualStyleBackColor = true;
            // 
            // chkReturnTaskTrace
            // 
            chkReturnTaskTrace.AutoSize = true;
            chkReturnTaskTrace.Location = new Point(381, 113);
            chkReturnTaskTrace.Name = "chkReturnTaskTrace";
            chkReturnTaskTrace.Size = new Size(116, 19);
            chkReturnTaskTrace.TabIndex = 8;
            chkReturnTaskTrace.Text = "Return Task Trace";
            chkReturnTaskTrace.UseVisualStyleBackColor = true;
            // 
            // cboEndpoints
            // 
            cboEndpoints.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEndpoints.FormattingEnabled = true;
            cboEndpoints.Location = new Point(75, 3);
            cboEndpoints.Name = "cboEndpoints";
            cboEndpoints.Size = new Size(121, 23);
            cboEndpoints.TabIndex = 0;
            cboEndpoints.SelectedIndexChanged += cboEndpoints_SelectedIndexChanged;
            // 
            // lblEndpoint
            // 
            lblEndpoint.AutoSize = true;
            lblEndpoint.Location = new Point(9, 6);
            lblEndpoint.Name = "lblEndpoint";
            lblEndpoint.Size = new Size(58, 15);
            lblEndpoint.TabIndex = 11;
            lblEndpoint.Text = "Endpoint:";
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(3, 2);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(75, 23);
            btnConnect.TabIndex = 0;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // grpTarget
            // 
            grpTarget.Controls.Add(txtEndpointUri);
            grpTarget.Controls.Add(pnlTargetLeft);
            grpTarget.Controls.Add(pnlTargetRight);
            grpTarget.Dock = DockStyle.Top;
            grpTarget.Location = new Point(0, 0);
            grpTarget.Name = "grpTarget";
            grpTarget.Size = new Size(723, 51);
            grpTarget.TabIndex = 0;
            grpTarget.TabStop = false;
            grpTarget.Text = "Target";
            // 
            // txtEndpointUri
            // 
            txtEndpointUri.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtEndpointUri.Location = new Point(214, 22);
            txtEndpointUri.Name = "txtEndpointUri";
            txtEndpointUri.ReadOnly = true;
            txtEndpointUri.Size = new Size(341, 23);
            txtEndpointUri.TabIndex = 1;
            // 
            // pnlTargetLeft
            // 
            pnlTargetLeft.Controls.Add(cboEndpoints);
            pnlTargetLeft.Controls.Add(lblEndpoint);
            pnlTargetLeft.Dock = DockStyle.Left;
            pnlTargetLeft.Location = new Point(3, 19);
            pnlTargetLeft.Name = "pnlTargetLeft";
            pnlTargetLeft.Size = new Size(205, 29);
            pnlTargetLeft.TabIndex = 0;
            // 
            // pnlTargetRight
            // 
            pnlTargetRight.Controls.Add(btnConnect);
            pnlTargetRight.Controls.Add(btnDisconnect);
            pnlTargetRight.Dock = DockStyle.Right;
            pnlTargetRight.Location = new Point(558, 19);
            pnlTargetRight.Name = "pnlTargetRight";
            pnlTargetRight.Size = new Size(162, 29);
            pnlTargetRight.TabIndex = 2;
            // 
            // btnDisconnect
            // 
            btnDisconnect.Enabled = false;
            btnDisconnect.Location = new Point(84, 2);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(75, 23);
            btnDisconnect.TabIndex = 1;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // grpLiveTasks
            // 
            grpLiveTasks.Controls.Add(grdLiveTasks);
            grpLiveTasks.Dock = DockStyle.Bottom;
            grpLiveTasks.Location = new Point(0, 263);
            grpLiveTasks.Name = "grpLiveTasks";
            grpLiveTasks.Size = new Size(723, 308);
            grpLiveTasks.TabIndex = 0;
            grpLiveTasks.TabStop = false;
            grpLiveTasks.Text = "Live Tasks";
            // 
            // grdLiveTasks
            // 
            grdLiveTasks.AllowUserToAddRows = false;
            grdLiveTasks.AllowUserToDeleteRows = false;
            grdLiveTasks.AllowUserToResizeRows = false;
            grdLiveTasks.AutoGenerateColumns = false;
            grdLiveTasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grdLiveTasks.Columns.AddRange(new DataGridViewColumn[] { CancelDataGridViewButtonColumn, commandNameDataGridViewTextBoxColumn, elapsedDataGridViewTextBoxColumn, taskIdDataGridViewTextBoxColumn, Status });
            grdLiveTasks.DataSource = bsLiveTasks;
            grdLiveTasks.Dock = DockStyle.Fill;
            grdLiveTasks.Location = new Point(3, 19);
            grdLiveTasks.MultiSelect = false;
            grdLiveTasks.Name = "grdLiveTasks";
            grdLiveTasks.ReadOnly = true;
            grdLiveTasks.RowHeadersVisible = false;
            grdLiveTasks.RowTemplate.Height = 25;
            grdLiveTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdLiveTasks.Size = new Size(717, 286);
            grdLiveTasks.TabIndex = 0;
            grdLiveTasks.CellContentClick += grdLiveTasks_CellContentClick;
            // 
            // bsLiveTasks
            // 
            bsLiveTasks.DataSource = typeof(ViewModels.LiveTaskStatusViewModel);
            // 
            // pnlNodeStatusTop
            // 
            pnlNodeStatusTop.Controls.Add(chkEnableProgressCache);
            pnlNodeStatusTop.Controls.Add(chkEnableDiagnostics);
            pnlNodeStatusTop.Controls.Add(btnRefreshNodeStatus);
            pnlNodeStatusTop.Dock = DockStyle.Top;
            pnlNodeStatusTop.Location = new Point(0, 0);
            pnlNodeStatusTop.Name = "pnlNodeStatusTop";
            pnlNodeStatusTop.Size = new Size(723, 28);
            pnlNodeStatusTop.TabIndex = 1;
            // 
            // chkEnableProgressCache
            // 
            chkEnableProgressCache.AutoSize = true;
            chkEnableProgressCache.Location = new Point(134, 5);
            chkEnableProgressCache.Name = "chkEnableProgressCache";
            chkEnableProgressCache.Size = new Size(145, 19);
            chkEnableProgressCache.TabIndex = 2;
            chkEnableProgressCache.Text = "Enable Progress Cache";
            chkEnableProgressCache.UseVisualStyleBackColor = true;
            chkEnableProgressCache.CheckedChanged += chkEnableProgressCache_CheckedChanged;
            // 
            // chkEnableDiagnostics
            // 
            chkEnableDiagnostics.AutoSize = true;
            chkEnableDiagnostics.Location = new Point(3, 6);
            chkEnableDiagnostics.Name = "chkEnableDiagnostics";
            chkEnableDiagnostics.Size = new Size(125, 19);
            chkEnableDiagnostics.TabIndex = 1;
            chkEnableDiagnostics.Text = "Enable Diagnostics";
            chkEnableDiagnostics.UseVisualStyleBackColor = true;
            chkEnableDiagnostics.CheckedChanged += chkEnableDiagnostics_CheckedChanged;
            // 
            // btnRefreshNodeStatus
            // 
            btnRefreshNodeStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefreshNodeStatus.Location = new Point(645, 3);
            btnRefreshNodeStatus.Name = "btnRefreshNodeStatus";
            btnRefreshNodeStatus.Size = new Size(75, 23);
            btnRefreshNodeStatus.TabIndex = 0;
            btnRefreshNodeStatus.Text = "Refresh";
            btnRefreshNodeStatus.UseVisualStyleBackColor = true;
            btnRefreshNodeStatus.Click += btnRefreshNodeStatus_Click;
            // 
            // pnlServiceDetails
            // 
            pnlServiceDetails.Controls.Add(grpCommands);
            pnlServiceDetails.Controls.Add(pnlNodeStatusTop);
            pnlServiceDetails.Controls.Add(grpLiveTasks);
            pnlServiceDetails.Controls.Add(txtEchoPrompt);
            pnlServiceDetails.Controls.Add(chkCacheProgressInfo);
            pnlServiceDetails.Controls.Add(lblEchoPrompt);
            pnlServiceDetails.Controls.Add(chkReturnTaskTrace);
            pnlServiceDetails.Controls.Add(btnEcho);
            pnlServiceDetails.Dock = DockStyle.Fill;
            pnlServiceDetails.Enabled = false;
            pnlServiceDetails.Location = new Point(0, 51);
            pnlServiceDetails.Name = "pnlServiceDetails";
            pnlServiceDetails.Size = new Size(723, 571);
            pnlServiceDetails.TabIndex = 1;
            // 
            // grpCommands
            // 
            grpCommands.Controls.Add(grdCommands);
            grpCommands.Dock = DockStyle.Left;
            grpCommands.Location = new Point(0, 28);
            grpCommands.Name = "grpCommands";
            grpCommands.Size = new Size(375, 235);
            grpCommands.TabIndex = 10;
            grpCommands.TabStop = false;
            grpCommands.Text = "Commands";
            // 
            // grdCommands
            // 
            grdCommands.AllowUserToAddRows = false;
            grdCommands.AllowUserToDeleteRows = false;
            grdCommands.AllowUserToResizeRows = false;
            grdCommands.AutoGenerateColumns = false;
            grdCommands.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grdCommands.Columns.AddRange(new DataGridViewColumn[] { enabledDataGridViewCheckBoxColumn, commandNameDataGridViewTextBoxColumn1, commandTypeDataGridViewTextBoxColumn });
            grdCommands.DataSource = bsCommands;
            grdCommands.Dock = DockStyle.Fill;
            grdCommands.Location = new Point(3, 19);
            grdCommands.MultiSelect = false;
            grdCommands.Name = "grdCommands";
            grdCommands.ReadOnly = true;
            grdCommands.RowHeadersVisible = false;
            grdCommands.RowTemplate.Height = 25;
            grdCommands.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdCommands.Size = new Size(369, 213);
            grdCommands.TabIndex = 0;
            grdCommands.CellContentClick += grdCommands_CellContentClick;
            // 
            // enabledDataGridViewCheckBoxColumn
            // 
            enabledDataGridViewCheckBoxColumn.DataPropertyName = "Enabled";
            enabledDataGridViewCheckBoxColumn.HeaderText = "Enabled";
            enabledDataGridViewCheckBoxColumn.Name = "enabledDataGridViewCheckBoxColumn";
            enabledDataGridViewCheckBoxColumn.ReadOnly = true;
            enabledDataGridViewCheckBoxColumn.Width = 60;
            // 
            // commandNameDataGridViewTextBoxColumn1
            // 
            commandNameDataGridViewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            commandNameDataGridViewTextBoxColumn1.DataPropertyName = "CommandName";
            commandNameDataGridViewTextBoxColumn1.HeaderText = "Command";
            commandNameDataGridViewTextBoxColumn1.Name = "commandNameDataGridViewTextBoxColumn1";
            commandNameDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // commandTypeDataGridViewTextBoxColumn
            // 
            commandTypeDataGridViewTextBoxColumn.DataPropertyName = "CommandType";
            commandTypeDataGridViewTextBoxColumn.HeaderText = "Type";
            commandTypeDataGridViewTextBoxColumn.Name = "commandTypeDataGridViewTextBoxColumn";
            commandTypeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bsCommands
            // 
            bsCommands.DataSource = typeof(ViewModels.CommandStatusViewModel);
            // 
            // tmrRefreshNodeStatus
            // 
            tmrRefreshNodeStatus.Interval = 5000;
            tmrRefreshNodeStatus.Tick += tmrRefreshNodeStatus_Tick;
            // 
            // CancelDataGridViewButtonColumn
            // 
            CancelDataGridViewButtonColumn.HeaderText = "Cancel";
            CancelDataGridViewButtonColumn.Name = "CancelDataGridViewButtonColumn";
            CancelDataGridViewButtonColumn.ReadOnly = true;
            CancelDataGridViewButtonColumn.Text = "Cancel";
            CancelDataGridViewButtonColumn.UseColumnTextForButtonValue = true;
            CancelDataGridViewButtonColumn.Width = 80;
            // 
            // commandNameDataGridViewTextBoxColumn
            // 
            commandNameDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            commandNameDataGridViewTextBoxColumn.DataPropertyName = "CommandName";
            commandNameDataGridViewTextBoxColumn.HeaderText = "Command";
            commandNameDataGridViewTextBoxColumn.Name = "commandNameDataGridViewTextBoxColumn";
            commandNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // elapsedDataGridViewTextBoxColumn
            // 
            elapsedDataGridViewTextBoxColumn.DataPropertyName = "Elapsed";
            dataGridViewCellStyle1.Format = "hh\\:mm\\:ss\\.fff";
            elapsedDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            elapsedDataGridViewTextBoxColumn.HeaderText = "Elapsed";
            elapsedDataGridViewTextBoxColumn.Name = "elapsedDataGridViewTextBoxColumn";
            elapsedDataGridViewTextBoxColumn.ReadOnly = true;
            elapsedDataGridViewTextBoxColumn.Width = 80;
            // 
            // taskIdDataGridViewTextBoxColumn
            // 
            taskIdDataGridViewTextBoxColumn.DataPropertyName = "TaskId";
            taskIdDataGridViewTextBoxColumn.HeaderText = "Task ID";
            taskIdDataGridViewTextBoxColumn.Name = "taskIdDataGridViewTextBoxColumn";
            taskIdDataGridViewTextBoxColumn.ReadOnly = true;
            taskIdDataGridViewTextBoxColumn.Width = 250;
            // 
            // Status
            // 
            Status.DataPropertyName = "Status";
            Status.HeaderText = "Status";
            Status.Name = "Status";
            Status.ReadOnly = true;
            Status.Width = 80;
            // 
            // RemoteAdminForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(723, 622);
            Controls.Add(pnlServiceDetails);
            Controls.Add(grpTarget);
            Name = "RemoteAdminForm";
            Text = "Remote Administration";
            grpTarget.ResumeLayout(false);
            grpTarget.PerformLayout();
            pnlTargetLeft.ResumeLayout(false);
            pnlTargetLeft.PerformLayout();
            pnlTargetRight.ResumeLayout(false);
            grpLiveTasks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grdLiveTasks).EndInit();
            ((System.ComponentModel.ISupportInitialize)bsLiveTasks).EndInit();
            pnlNodeStatusTop.ResumeLayout(false);
            pnlNodeStatusTop.PerformLayout();
            pnlServiceDetails.ResumeLayout(false);
            pnlServiceDetails.PerformLayout();
            grpCommands.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grdCommands).EndInit();
            ((System.ComponentModel.ISupportInitialize)bsCommands).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TextBox txtEchoPrompt;
        private Label lblEchoPrompt;
        private Button btnEcho;
        private CheckBox chkCacheProgressInfo;
        private CheckBox chkReturnTaskTrace;
        private ComboBox cboEndpoints;
        private Label lblEndpoint;
        private Button btnConnect;
        private GroupBox grpTarget;
        private TextBox txtEndpointUri;
        private Button btnDisconnect;
        private Panel pnlTargetLeft;
        private Panel pnlTargetRight;
        private GroupBox grpLiveTasks;
        private Panel pnlServiceDetails;
        private DataGridView grdLiveTasks;
        private Panel pnlNodeStatusTop;
        private Button btnRefreshNodeStatus;
        private BindingSource bsLiveTasks;
        private System.Windows.Forms.Timer tmrRefreshNodeStatus;
        private GroupBox grpCommands;
        private DataGridView grdCommands;
        private BindingSource bsCommands;
        private DataGridViewCheckBoxColumn enabledDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn commandNameDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn commandTypeDataGridViewTextBoxColumn;
        private CheckBox chkEnableDiagnostics;
        private CheckBox chkEnableProgressCache;
        private DataGridViewButtonColumn CancelDataGridViewButtonColumn;
        private DataGridViewTextBoxColumn commandNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn elapsedDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn taskIdDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn Status;
    }
}