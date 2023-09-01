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
            CancelDataGridViewButtonColumn = new DataGridViewButtonColumn();
            commandNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            elapsedDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            taskIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            Status = new DataGridViewTextBoxColumn();
            bsLiveTasks = new BindingSource(components);
            panel1 = new Panel();
            btnRefreshLiveTasks = new Button();
            pnlServiceDetails = new Panel();
            tmrRefreshLiveTasks = new System.Windows.Forms.Timer(components);
            grpTarget.SuspendLayout();
            pnlTargetLeft.SuspendLayout();
            pnlTargetRight.SuspendLayout();
            grpLiveTasks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grdLiveTasks).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bsLiveTasks).BeginInit();
            panel1.SuspendLayout();
            pnlServiceDetails.SuspendLayout();
            SuspendLayout();
            // 
            // txtEchoPrompt
            // 
            txtEchoPrompt.Location = new Point(189, 60);
            txtEchoPrompt.Name = "txtEchoPrompt";
            txtEchoPrompt.Size = new Size(100, 23);
            txtEchoPrompt.TabIndex = 0;
            txtEchoPrompt.Text = "Hello!";
            // 
            // lblEchoPrompt
            // 
            lblEchoPrompt.AutoSize = true;
            lblEchoPrompt.Location = new Point(133, 63);
            lblEchoPrompt.Name = "lblEchoPrompt";
            lblEchoPrompt.Size = new Size(50, 15);
            lblEchoPrompt.TabIndex = 1;
            lblEchoPrompt.Text = "Prompt:";
            // 
            // btnEcho
            // 
            btnEcho.Location = new Point(295, 60);
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
            chkCacheProgressInfo.Location = new Point(133, 121);
            chkCacheProgressInfo.Name = "chkCacheProgressInfo";
            chkCacheProgressInfo.Size = new Size(131, 19);
            chkCacheProgressInfo.TabIndex = 9;
            chkCacheProgressInfo.Text = "Cache Progress Info";
            chkCacheProgressInfo.UseVisualStyleBackColor = true;
            // 
            // chkReturnTaskTrace
            // 
            chkReturnTaskTrace.AutoSize = true;
            chkReturnTaskTrace.Location = new Point(133, 102);
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
            grpTarget.Size = new Size(691, 51);
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
            txtEndpointUri.Size = new Size(309, 23);
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
            pnlTargetRight.Location = new Point(526, 19);
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
            grpLiveTasks.Controls.Add(panel1);
            grpLiveTasks.Dock = DockStyle.Bottom;
            grpLiveTasks.Location = new Point(0, 263);
            grpLiveTasks.Name = "grpLiveTasks";
            grpLiveTasks.Size = new Size(691, 308);
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
            grdLiveTasks.Location = new Point(3, 47);
            grdLiveTasks.MultiSelect = false;
            grdLiveTasks.Name = "grdLiveTasks";
            grdLiveTasks.ReadOnly = true;
            grdLiveTasks.RowHeadersVisible = false;
            grdLiveTasks.RowTemplate.Height = 25;
            grdLiveTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdLiveTasks.Size = new Size(685, 258);
            grdLiveTasks.TabIndex = 0;
            grdLiveTasks.CellContentClick += grdLiveTasks_CellContentClick;
            // 
            // CancelDataGridViewButtonColumn
            // 
            CancelDataGridViewButtonColumn.HeaderText = "Cancel";
            CancelDataGridViewButtonColumn.Name = "CancelDataGridViewButtonColumn";
            CancelDataGridViewButtonColumn.ReadOnly = true;
            CancelDataGridViewButtonColumn.Text = "Cancel";
            CancelDataGridViewButtonColumn.UseColumnTextForButtonValue = true;
            // 
            // commandNameDataGridViewTextBoxColumn
            // 
            commandNameDataGridViewTextBoxColumn.DataPropertyName = "CommandName";
            commandNameDataGridViewTextBoxColumn.HeaderText = "Command";
            commandNameDataGridViewTextBoxColumn.Name = "commandNameDataGridViewTextBoxColumn";
            commandNameDataGridViewTextBoxColumn.ReadOnly = true;
            commandNameDataGridViewTextBoxColumn.Width = 175;
            // 
            // elapsedDataGridViewTextBoxColumn
            // 
            elapsedDataGridViewTextBoxColumn.DataPropertyName = "Elapsed";
            dataGridViewCellStyle1.Format = "hh\\:mm\\:ss\\.fff";
            elapsedDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            elapsedDataGridViewTextBoxColumn.HeaderText = "Elapsed";
            elapsedDataGridViewTextBoxColumn.Name = "elapsedDataGridViewTextBoxColumn";
            elapsedDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // taskIdDataGridViewTextBoxColumn
            // 
            taskIdDataGridViewTextBoxColumn.DataPropertyName = "TaskId";
            taskIdDataGridViewTextBoxColumn.HeaderText = "Task ID";
            taskIdDataGridViewTextBoxColumn.Name = "taskIdDataGridViewTextBoxColumn";
            taskIdDataGridViewTextBoxColumn.ReadOnly = true;
            taskIdDataGridViewTextBoxColumn.Width = 200;
            // 
            // Status
            // 
            Status.DataPropertyName = "Status";
            Status.HeaderText = "Status";
            Status.Name = "Status";
            Status.ReadOnly = true;
            // 
            // bsLiveTasks
            // 
            bsLiveTasks.DataSource = typeof(ViewModels.LiveTaskStatusViewModel);
            // 
            // panel1
            // 
            panel1.Controls.Add(btnRefreshLiveTasks);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(3, 19);
            panel1.Name = "panel1";
            panel1.Size = new Size(685, 28);
            panel1.TabIndex = 1;
            // 
            // btnRefreshLiveTasks
            // 
            btnRefreshLiveTasks.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefreshLiveTasks.Location = new Point(607, 3);
            btnRefreshLiveTasks.Name = "btnRefreshLiveTasks";
            btnRefreshLiveTasks.Size = new Size(75, 23);
            btnRefreshLiveTasks.TabIndex = 0;
            btnRefreshLiveTasks.Text = "Refresh";
            btnRefreshLiveTasks.UseVisualStyleBackColor = true;
            btnRefreshLiveTasks.Click += btnRefreshLiveTasks_Click;
            // 
            // pnlServiceDetails
            // 
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
            pnlServiceDetails.Size = new Size(691, 571);
            pnlServiceDetails.TabIndex = 1;
            // 
            // tmrRefreshLiveTasks
            // 
            tmrRefreshLiveTasks.Interval = 5000;
            tmrRefreshLiveTasks.Tick += tmrRefreshLiveTasks_Tick;
            // 
            // RemoteAdminForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(691, 622);
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
            panel1.ResumeLayout(false);
            pnlServiceDetails.ResumeLayout(false);
            pnlServiceDetails.PerformLayout();
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
        private Panel panel1;
        private Button btnRefreshLiveTasks;
        private BindingSource bsLiveTasks;
        private System.Windows.Forms.Timer tmrRefreshLiveTasks;
        private DataGridViewButtonColumn CancelDataGridViewButtonColumn;
        private DataGridViewTextBoxColumn commandNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn elapsedDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn taskIdDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn Status;
    }
}