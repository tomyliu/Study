namespace ActiveMQMonitor
{
    partial class TopicTest
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
            this.txtMQPort = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnBrowserQueue = new System.Windows.Forms.Button();
            this.btnGetQueueList = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnSaveSetting = new System.Windows.Forms.Button();
            this.txtAdminPort = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtActiveMQIP = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtConsumers = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDequeued = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPending = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEnqueued = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listQueueName = new System.Windows.Forms.ListBox();
            this.txtQueryQueueItem = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnQueryQueue = new System.Windows.Forms.Button();
            this.btnPullMessage1 = new System.Windows.Forms.Button();
            this.txtInMessage = new System.Windows.Forms.TextBox();
            this.btnPushMessage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtMQPort
            // 
            this.txtMQPort.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtMQPort.Location = new System.Drawing.Point(623, 21);
            this.txtMQPort.Name = "txtMQPort";
            this.txtMQPort.Size = new System.Drawing.Size(53, 27);
            this.txtMQPort.TabIndex = 58;
            this.txtMQPort.Text = "61616";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.Location = new System.Drawing.Point(521, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 24);
            this.label10.TabIndex = 57;
            this.label10.Text = "MQ Port:";
            // 
            // btnBrowserQueue
            // 
            this.btnBrowserQueue.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnBrowserQueue.Location = new System.Drawing.Point(429, 99);
            this.btnBrowserQueue.Name = "btnBrowserQueue";
            this.btnBrowserQueue.Size = new System.Drawing.Size(167, 41);
            this.btnBrowserQueue.TabIndex = 56;
            this.btnBrowserQueue.Text = "ListenTopic";
            this.btnBrowserQueue.UseVisualStyleBackColor = true;
            this.btnBrowserQueue.Click += new System.EventHandler(this.btnBrowserQueue_Click);
            // 
            // btnGetQueueList
            // 
            this.btnGetQueueList.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnGetQueueList.Location = new System.Drawing.Point(25, 99);
            this.btnGetQueueList.Name = "btnGetQueueList";
            this.btnGetQueueList.Size = new System.Drawing.Size(167, 41);
            this.btnGetQueueList.TabIndex = 55;
            this.btnGetQueueList.Text = "GetTopicList";
            this.btnGetQueueList.UseVisualStyleBackColor = true;
            this.btnGetQueueList.Click += new System.EventHandler(this.btnGetQueueList_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtPassword.Location = new System.Drawing.Point(462, 60);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(171, 27);
            this.txtPassword.TabIndex = 54;
            this.txtPassword.Text = "admin";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.Location = new System.Drawing.Point(357, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(103, 24);
            this.label8.TabIndex = 53;
            this.label8.Text = "Password:";
            // 
            // txtUserID
            // 
            this.txtUserID.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtUserID.Location = new System.Drawing.Point(170, 60);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(171, 27);
            this.txtUserID.TabIndex = 52;
            this.txtUserID.Text = "admin";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.Location = new System.Drawing.Point(105, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 24);
            this.label9.TabIndex = 51;
            this.label9.Text = "User:";
            // 
            // btnSaveSetting
            // 
            this.btnSaveSetting.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSaveSetting.Location = new System.Drawing.Point(718, 13);
            this.btnSaveSetting.Name = "btnSaveSetting";
            this.btnSaveSetting.Size = new System.Drawing.Size(167, 41);
            this.btnSaveSetting.TabIndex = 50;
            this.btnSaveSetting.Text = "SaveSetting";
            this.btnSaveSetting.UseVisualStyleBackColor = true;
            this.btnSaveSetting.Click += new System.EventHandler(this.btnSaveSetting_Click);
            // 
            // txtAdminPort
            // 
            this.txtAdminPort.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtAdminPort.Location = new System.Drawing.Point(429, 18);
            this.txtAdminPort.Name = "txtAdminPort";
            this.txtAdminPort.Size = new System.Drawing.Size(53, 27);
            this.txtAdminPort.TabIndex = 49;
            this.txtAdminPort.Text = "8161";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(301, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(122, 24);
            this.label7.TabIndex = 48;
            this.label7.Text = "Admin Port:";
            // 
            // txtActiveMQIP
            // 
            this.txtActiveMQIP.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtActiveMQIP.Location = new System.Drawing.Point(170, 18);
            this.txtActiveMQIP.Name = "txtActiveMQIP";
            this.txtActiveMQIP.Size = new System.Drawing.Size(112, 27);
            this.txtActiveMQIP.TabIndex = 47;
            this.txtActiveMQIP.Text = "10.48.167.52";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(26, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(138, 24);
            this.label6.TabIndex = 46;
            this.label6.Text = "ActiveMQ IP:";
            // 
            // txtConsumers
            // 
            this.txtConsumers.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtConsumers.Location = new System.Drawing.Point(406, 272);
            this.txtConsumers.Name = "txtConsumers";
            this.txtConsumers.Size = new System.Drawing.Size(137, 27);
            this.txtConsumers.TabIndex = 45;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(283, 269);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 24);
            this.label4.TabIndex = 44;
            this.label4.Text = "Consumers:";
            // 
            // txtDequeued
            // 
            this.txtDequeued.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtDequeued.Location = new System.Drawing.Point(406, 231);
            this.txtDequeued.Name = "txtDequeued";
            this.txtDequeued.Size = new System.Drawing.Size(137, 27);
            this.txtDequeued.TabIndex = 43;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(295, 231);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 24);
            this.label5.TabIndex = 42;
            this.label5.Text = "Dequeued:";
            // 
            // txtPending
            // 
            this.txtPending.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtPending.Location = new System.Drawing.Point(132, 269);
            this.txtPending.Name = "txtPending";
            this.txtPending.Size = new System.Drawing.Size(137, 27);
            this.txtPending.TabIndex = 41;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(37, 266);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 24);
            this.label3.TabIndex = 40;
            this.label3.Text = "Pending:";
            // 
            // txtEnqueued
            // 
            this.txtEnqueued.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtEnqueued.Location = new System.Drawing.Point(132, 228);
            this.txtEnqueued.Name = "txtEnqueued";
            this.txtEnqueued.Size = new System.Drawing.Size(137, 27);
            this.txtEnqueued.TabIndex = 39;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(21, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 24);
            this.label2.TabIndex = 38;
            this.label2.Text = "Enqueued:";
            // 
            // listQueueName
            // 
            this.listQueueName.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.listQueueName.FormattingEnabled = true;
            this.listQueueName.ItemHeight = 19;
            this.listQueueName.Location = new System.Drawing.Point(25, 322);
            this.listQueueName.Name = "listQueueName";
            this.listQueueName.Size = new System.Drawing.Size(860, 289);
            this.listQueueName.TabIndex = 37;
            this.listQueueName.SelectedIndexChanged += new System.EventHandler(this.listQueueName_SelectedIndexChanged);
            // 
            // txtQueryQueueItem
            // 
            this.txtQueryQueueItem.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtQueryQueueItem.Location = new System.Drawing.Point(25, 187);
            this.txtQueryQueueItem.Name = "txtQueryQueueItem";
            this.txtQueryQueueItem.Size = new System.Drawing.Size(860, 27);
            this.txtQueryQueueItem.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(21, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 24);
            this.label1.TabIndex = 35;
            this.label1.Text = "QueryQueueName:";
            // 
            // btnQueryQueue
            // 
            this.btnQueryQueue.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnQueryQueue.Location = new System.Drawing.Point(234, 99);
            this.btnQueryQueue.Name = "btnQueryQueue";
            this.btnQueryQueue.Size = new System.Drawing.Size(167, 41);
            this.btnQueryQueue.TabIndex = 34;
            this.btnQueryQueue.Text = "QueryTopic";
            this.btnQueryQueue.UseVisualStyleBackColor = true;
            this.btnQueryQueue.Click += new System.EventHandler(this.btnQueryQueue_Click);
            // 
            // btnPullMessage1
            // 
            this.btnPullMessage1.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnPullMessage1.Location = new System.Drawing.Point(847, 886);
            this.btnPullMessage1.Name = "btnPullMessage1";
            this.btnPullMessage1.Size = new System.Drawing.Size(167, 41);
            this.btnPullMessage1.TabIndex = 33;
            this.btnPullMessage1.Text = "PullMessage";
            this.btnPullMessage1.UseVisualStyleBackColor = true;
            // 
            // txtInMessage
            // 
            this.txtInMessage.Location = new System.Drawing.Point(904, 187);
            this.txtInMessage.Multiline = true;
            this.txtInMessage.Name = "txtInMessage";
            this.txtInMessage.Size = new System.Drawing.Size(250, 424);
            this.txtInMessage.TabIndex = 32;
            // 
            // btnPushMessage
            // 
            this.btnPushMessage.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnPushMessage.Location = new System.Drawing.Point(944, 99);
            this.btnPushMessage.Name = "btnPushMessage";
            this.btnPushMessage.Size = new System.Drawing.Size(167, 41);
            this.btnPushMessage.TabIndex = 31;
            this.btnPushMessage.Text = "PushMessage";
            this.btnPushMessage.UseVisualStyleBackColor = true;
            this.btnPushMessage.Click += new System.EventHandler(this.btnPushMessage_Click);
            // 
            // TopicTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1190, 629);
            this.Controls.Add(this.txtMQPort);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnBrowserQueue);
            this.Controls.Add(this.btnGetQueueList);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnSaveSetting);
            this.Controls.Add(this.txtAdminPort);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtActiveMQIP);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtConsumers);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDequeued);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPending);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtEnqueued);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listQueueName);
            this.Controls.Add(this.txtQueryQueueItem);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnQueryQueue);
            this.Controls.Add(this.btnPullMessage1);
            this.Controls.Add(this.txtInMessage);
            this.Controls.Add(this.btnPushMessage);
            this.Name = "TopicTest";
            this.Text = "TopicTest";
            this.Load += new System.EventHandler(this.TopicTest_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtMQPort;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnBrowserQueue;
        private System.Windows.Forms.Button btnGetQueueList;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnSaveSetting;
        private System.Windows.Forms.TextBox txtAdminPort;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtActiveMQIP;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtConsumers;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDequeued;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPending;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEnqueued;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listQueueName;
        private System.Windows.Forms.TextBox txtQueryQueueItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnQueryQueue;
        private System.Windows.Forms.Button btnPullMessage1;
        private System.Windows.Forms.TextBox txtInMessage;
        private System.Windows.Forms.Button btnPushMessage;
    }
}