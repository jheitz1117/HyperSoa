namespace TestClient
{
    partial class HostingTestClientForm
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
            btnAsyncCommandNoContracts = new Button();
            btnAsyncCommandRequestOnly = new Button();
            grpTarget = new GroupBox();
            txtEndpointUri = new TextBox();
            pnlTargetRight = new Panel();
            btnConnect = new Button();
            btnDisconnect = new Button();
            pnlTargetLeft = new Panel();
            cboEndpoints = new ComboBox();
            lblEndpoint = new Label();
            pnlServiceDetails = new Panel();
            btnSyncCommandNoContracts = new Button();
            btnSyncCommandRequestOnly = new Button();
            btnSyncCommandRequestResponse = new Button();
            btnSyncCommandResponseOnly = new Button();
            chkCacheProgressInfo = new CheckBox();
            chkReturnTaskTrace = new CheckBox();
            grpTarget.SuspendLayout();
            pnlTargetRight.SuspendLayout();
            pnlTargetLeft.SuspendLayout();
            pnlServiceDetails.SuspendLayout();
            SuspendLayout();
            // 
            // btnAsyncCommandNoContracts
            // 
            btnAsyncCommandNoContracts.Location = new Point(43, 79);
            btnAsyncCommandNoContracts.Name = "btnAsyncCommandNoContracts";
            btnAsyncCommandNoContracts.Size = new Size(122, 51);
            btnAsyncCommandNoContracts.TabIndex = 0;
            btnAsyncCommandNoContracts.Text = "Async Command (No Contracts)";
            btnAsyncCommandNoContracts.UseVisualStyleBackColor = true;
            btnAsyncCommandNoContracts.Click += btnAsyncCommandNoContracts_Click;
            // 
            // btnAsyncCommandRequestOnly
            // 
            btnAsyncCommandRequestOnly.Location = new Point(43, 136);
            btnAsyncCommandRequestOnly.Name = "btnAsyncCommandRequestOnly";
            btnAsyncCommandRequestOnly.Size = new Size(122, 51);
            btnAsyncCommandRequestOnly.TabIndex = 1;
            btnAsyncCommandRequestOnly.Text = "Async Command (Request Only)";
            btnAsyncCommandRequestOnly.UseVisualStyleBackColor = true;
            btnAsyncCommandRequestOnly.Click += btnAsyncCommandRequestOnly_Click;
            // 
            // grpTarget
            // 
            grpTarget.Controls.Add(txtEndpointUri);
            grpTarget.Controls.Add(pnlTargetRight);
            grpTarget.Controls.Add(pnlTargetLeft);
            grpTarget.Dock = DockStyle.Top;
            grpTarget.Location = new Point(0, 0);
            grpTarget.Name = "grpTarget";
            grpTarget.Size = new Size(800, 51);
            grpTarget.TabIndex = 4;
            grpTarget.TabStop = false;
            grpTarget.Text = "Target";
            // 
            // txtEndpointUri
            // 
            txtEndpointUri.Dock = DockStyle.Fill;
            txtEndpointUri.Location = new Point(208, 19);
            txtEndpointUri.Name = "txtEndpointUri";
            txtEndpointUri.ReadOnly = true;
            txtEndpointUri.Size = new Size(427, 23);
            txtEndpointUri.TabIndex = 1;
            // 
            // pnlTargetRight
            // 
            pnlTargetRight.Controls.Add(btnConnect);
            pnlTargetRight.Controls.Add(btnDisconnect);
            pnlTargetRight.Dock = DockStyle.Right;
            pnlTargetRight.Location = new Point(635, 19);
            pnlTargetRight.Name = "pnlTargetRight";
            pnlTargetRight.Size = new Size(162, 29);
            pnlTargetRight.TabIndex = 2;
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
            // pnlServiceDetails
            // 
            pnlServiceDetails.Controls.Add(btnSyncCommandNoContracts);
            pnlServiceDetails.Controls.Add(btnSyncCommandRequestOnly);
            pnlServiceDetails.Controls.Add(btnSyncCommandRequestResponse);
            pnlServiceDetails.Controls.Add(btnSyncCommandResponseOnly);
            pnlServiceDetails.Controls.Add(chkCacheProgressInfo);
            pnlServiceDetails.Controls.Add(chkReturnTaskTrace);
            pnlServiceDetails.Controls.Add(btnAsyncCommandNoContracts);
            pnlServiceDetails.Controls.Add(btnAsyncCommandRequestOnly);
            pnlServiceDetails.Dock = DockStyle.Fill;
            pnlServiceDetails.Enabled = false;
            pnlServiceDetails.Location = new Point(0, 51);
            pnlServiceDetails.Name = "pnlServiceDetails";
            pnlServiceDetails.Size = new Size(800, 399);
            pnlServiceDetails.TabIndex = 5;
            // 
            // btnSyncCommandNoContracts
            // 
            btnSyncCommandNoContracts.Location = new Point(231, 79);
            btnSyncCommandNoContracts.Name = "btnSyncCommandNoContracts";
            btnSyncCommandNoContracts.Size = new Size(122, 51);
            btnSyncCommandNoContracts.TabIndex = 12;
            btnSyncCommandNoContracts.Text = "Sync Command (No Contracts)";
            btnSyncCommandNoContracts.UseVisualStyleBackColor = true;
            btnSyncCommandNoContracts.Click += btnSyncCommandNoContracts_Click;
            // 
            // btnSyncCommandRequestOnly
            // 
            btnSyncCommandRequestOnly.Location = new Point(231, 136);
            btnSyncCommandRequestOnly.Name = "btnSyncCommandRequestOnly";
            btnSyncCommandRequestOnly.Size = new Size(122, 51);
            btnSyncCommandRequestOnly.TabIndex = 13;
            btnSyncCommandRequestOnly.Text = "Sync Command (Request Only)";
            btnSyncCommandRequestOnly.UseVisualStyleBackColor = true;
            btnSyncCommandRequestOnly.Click += btnSyncCommandRequestOnly_Click;
            // 
            // btnSyncCommandRequestResponse
            // 
            btnSyncCommandRequestResponse.Location = new Point(231, 250);
            btnSyncCommandRequestResponse.Name = "btnSyncCommandRequestResponse";
            btnSyncCommandRequestResponse.Size = new Size(122, 51);
            btnSyncCommandRequestResponse.TabIndex = 15;
            btnSyncCommandRequestResponse.Text = "Sync Command (Request/Response)";
            btnSyncCommandRequestResponse.UseVisualStyleBackColor = true;
            btnSyncCommandRequestResponse.Click += btnSyncCommandRequestResponse_Click;
            // 
            // btnSyncCommandResponseOnly
            // 
            btnSyncCommandResponseOnly.Location = new Point(231, 193);
            btnSyncCommandResponseOnly.Name = "btnSyncCommandResponseOnly";
            btnSyncCommandResponseOnly.Size = new Size(122, 51);
            btnSyncCommandResponseOnly.TabIndex = 14;
            btnSyncCommandResponseOnly.Text = "Sync Command (Response Only)";
            btnSyncCommandResponseOnly.UseVisualStyleBackColor = true;
            btnSyncCommandResponseOnly.Click += btnSyncCommandResponseOnly_Click;
            // 
            // chkCacheProgressInfo
            // 
            chkCacheProgressInfo.AutoSize = true;
            chkCacheProgressInfo.Location = new Point(43, 34);
            chkCacheProgressInfo.Name = "chkCacheProgressInfo";
            chkCacheProgressInfo.Size = new Size(131, 19);
            chkCacheProgressInfo.TabIndex = 11;
            chkCacheProgressInfo.Text = "Cache Progress Info";
            chkCacheProgressInfo.UseVisualStyleBackColor = true;
            // 
            // chkReturnTaskTrace
            // 
            chkReturnTaskTrace.AutoSize = true;
            chkReturnTaskTrace.Location = new Point(43, 15);
            chkReturnTaskTrace.Name = "chkReturnTaskTrace";
            chkReturnTaskTrace.Size = new Size(116, 19);
            chkReturnTaskTrace.TabIndex = 10;
            chkReturnTaskTrace.Text = "Return Task Trace";
            chkReturnTaskTrace.UseVisualStyleBackColor = true;
            // 
            // HostingTestClientForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pnlServiceDetails);
            Controls.Add(grpTarget);
            Name = "HostingTestClientForm";
            Text = "HostingTestClientForm";
            grpTarget.ResumeLayout(false);
            grpTarget.PerformLayout();
            pnlTargetRight.ResumeLayout(false);
            pnlTargetLeft.ResumeLayout(false);
            pnlTargetLeft.PerformLayout();
            pnlServiceDetails.ResumeLayout(false);
            pnlServiceDetails.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button btnAsyncCommandNoContracts;
        private Button btnAsyncCommandRequestOnly;
        private GroupBox grpTarget;
        private TextBox txtEndpointUri;
        private Panel pnlTargetLeft;
        private ComboBox cboEndpoints;
        private Label lblEndpoint;
        private Panel pnlTargetRight;
        private Button btnConnect;
        private Button btnDisconnect;
        private Panel pnlServiceDetails;
        private CheckBox chkCacheProgressInfo;
        private CheckBox chkReturnTaskTrace;
        private Button btnSyncCommandNoContracts;
        private Button btnSyncCommandRequestOnly;
        private Button btnSyncCommandRequestResponse;
        private Button btnSyncCommandResponseOnly;
    }
}