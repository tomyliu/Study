namespace ActiveMQMonitor
{
    partial class QueueTest
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnPushMessage = new System.Windows.Forms.Button();
            this.txtInMessage = new System.Windows.Forms.TextBox();
            this.btnPullMessage1 = new System.Windows.Forms.Button();
            this.btnQueryQueue = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtQueryQueueItem = new System.Windows.Forms.TextBox();
            this.listQueueName = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEnqueued = new System.Windows.Forms.TextBox();
            this.txtPending = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtConsumers = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDequeued = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtActiveMQIP = new System.Windows.Forms.TextBox();
            this.txtAdminPort = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSaveSetting = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnGetQueueList = new System.Windows.Forms.Button();
            this.btnBrowserQueue = new System.Windows.Forms.Button();
            this.txtMQPort = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnCleanQueue = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPushMessage
            // 
            this.btnPushMessage.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnPushMessage.Location = new System.Drawing.Point(930, 106);
            this.btnPushMessage.Name = "btnPushMessage";
            this.btnPushMessage.Size = new System.Drawing.Size(167, 41);
            this.btnPushMessage.TabIndex = 0;
            this.btnPushMessage.Text = "PushMessage";
            this.btnPushMessage.UseVisualStyleBackColor = true;
            this.btnPushMessage.Click += new System.EventHandler(this.btnPushMessage_Click);
            // 
            // txtInMessage
            // 
            this.txtInMessage.Location = new System.Drawing.Point(890, 194);
            this.txtInMessage.Multiline = true;
            this.txtInMessage.Name = "txtInMessage";
            this.txtInMessage.Size = new System.Drawing.Size(250, 424);
            this.txtInMessage.TabIndex = 3;
            // 
            // btnPullMessage1
            // 
            this.btnPullMessage1.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnPullMessage1.Location = new System.Drawing.Point(833, 893);
            this.btnPullMessage1.Name = "btnPullMessage1";
            this.btnPullMessage1.Size = new System.Drawing.Size(167, 41);
            this.btnPullMessage1.TabIndex = 4;
            this.btnPullMessage1.Text = "PullMessage";
            this.btnPullMessage1.UseVisualStyleBackColor = true;
            this.btnPullMessage1.Click += new System.EventHandler(this.btnPullMessage1_Click);
            // 
            // btnQueryQueue
            // 
            this.btnQueryQueue.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnQueryQueue.Location = new System.Drawing.Point(220, 106);
            this.btnQueryQueue.Name = "btnQueryQueue";
            this.btnQueryQueue.Size = new System.Drawing.Size(167, 41);
            this.btnQueryQueue.TabIndex = 5;
            this.btnQueryQueue.Text = "QueryQueue";
            this.btnQueryQueue.UseVisualStyleBackColor = true;
            this.btnQueryQueue.Click += new System.EventHandler(this.btnQueryQueue_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(7, 167);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "QueryQueueName:";
            // 
            // txtQueryQueueItem
            // 
            this.txtQueryQueueItem.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtQueryQueueItem.Location = new System.Drawing.Point(11, 194);
            this.txtQueryQueueItem.Name = "txtQueryQueueItem";
            this.txtQueryQueueItem.Size = new System.Drawing.Size(860, 27);
            this.txtQueryQueueItem.TabIndex = 7;
            // 
            // listQueueName
            // 
            this.listQueueName.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.listQueueName.FormattingEnabled = true;
            this.listQueueName.ItemHeight = 19;
            this.listQueueName.Location = new System.Drawing.Point(11, 329);
            this.listQueueName.Name = "listQueueName";
            this.listQueueName.Size = new System.Drawing.Size(860, 289);
            this.listQueueName.Sorted = true;
            this.listQueueName.TabIndex = 8;
            this.listQueueName.SelectedIndexChanged += new System.EventHandler(this.listQueueName_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(7, 235);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 24);
            this.label2.TabIndex = 9;
            this.label2.Text = "Enqueued:";
            // 
            // txtEnqueued
            // 
            this.txtEnqueued.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtEnqueued.Location = new System.Drawing.Point(118, 235);
            this.txtEnqueued.Name = "txtEnqueued";
            this.txtEnqueued.Size = new System.Drawing.Size(137, 27);
            this.txtEnqueued.TabIndex = 10;
            // 
            // txtPending
            // 
            this.txtPending.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtPending.Location = new System.Drawing.Point(118, 276);
            this.txtPending.Name = "txtPending";
            this.txtPending.Size = new System.Drawing.Size(137, 27);
            this.txtPending.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(23, 273);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 24);
            this.label3.TabIndex = 11;
            this.label3.Text = "Pending:";
            // 
            // txtConsumers
            // 
            this.txtConsumers.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtConsumers.Location = new System.Drawing.Point(392, 279);
            this.txtConsumers.Name = "txtConsumers";
            this.txtConsumers.Size = new System.Drawing.Size(137, 27);
            this.txtConsumers.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(269, 276);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 24);
            this.label4.TabIndex = 15;
            this.label4.Text = "Consumers:";
            // 
            // txtDequeued
            // 
            this.txtDequeued.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtDequeued.Location = new System.Drawing.Point(392, 238);
            this.txtDequeued.Name = "txtDequeued";
            this.txtDequeued.Size = new System.Drawing.Size(137, 27);
            this.txtDequeued.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(281, 238);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 24);
            this.label5.TabIndex = 13;
            this.label5.Text = "Dequeued:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(12, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(138, 24);
            this.label6.TabIndex = 17;
            this.label6.Text = "ActiveMQ IP:";
            // 
            // txtActiveMQIP
            // 
            this.txtActiveMQIP.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtActiveMQIP.Location = new System.Drawing.Point(156, 25);
            this.txtActiveMQIP.Name = "txtActiveMQIP";
            this.txtActiveMQIP.Size = new System.Drawing.Size(112, 27);
            this.txtActiveMQIP.TabIndex = 18;
            this.txtActiveMQIP.Text = "10.48.167.52";
            // 
            // txtAdminPort
            // 
            this.txtAdminPort.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtAdminPort.Location = new System.Drawing.Point(415, 25);
            this.txtAdminPort.Name = "txtAdminPort";
            this.txtAdminPort.Size = new System.Drawing.Size(53, 27);
            this.txtAdminPort.TabIndex = 20;
            this.txtAdminPort.Text = "8161";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(287, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(122, 24);
            this.label7.TabIndex = 19;
            this.label7.Text = "Admin Port:";
            // 
            // btnSaveSetting
            // 
            this.btnSaveSetting.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSaveSetting.Location = new System.Drawing.Point(704, 20);
            this.btnSaveSetting.Name = "btnSaveSetting";
            this.btnSaveSetting.Size = new System.Drawing.Size(167, 41);
            this.btnSaveSetting.TabIndex = 21;
            this.btnSaveSetting.Text = "SaveSetting";
            this.btnSaveSetting.UseVisualStyleBackColor = true;
            this.btnSaveSetting.Click += new System.EventHandler(this.btnSaveSetting_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtPassword.Location = new System.Drawing.Point(448, 67);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(171, 27);
            this.txtPassword.TabIndex = 25;
            this.txtPassword.Text = "admin";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.Location = new System.Drawing.Point(343, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(103, 24);
            this.label8.TabIndex = 24;
            this.label8.Text = "Password:";
            // 
            // txtUserID
            // 
            this.txtUserID.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtUserID.Location = new System.Drawing.Point(156, 67);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(171, 27);
            this.txtUserID.TabIndex = 23;
            this.txtUserID.Text = "admin";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.Location = new System.Drawing.Point(91, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 24);
            this.label9.TabIndex = 22;
            this.label9.Text = "User:";
            // 
            // btnGetQueueList
            // 
            this.btnGetQueueList.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnGetQueueList.Location = new System.Drawing.Point(11, 106);
            this.btnGetQueueList.Name = "btnGetQueueList";
            this.btnGetQueueList.Size = new System.Drawing.Size(167, 41);
            this.btnGetQueueList.TabIndex = 26;
            this.btnGetQueueList.Text = "GetQueueList";
            this.btnGetQueueList.UseVisualStyleBackColor = true;
            this.btnGetQueueList.Click += new System.EventHandler(this.btnGetQueueList_Click);
            // 
            // btnBrowserQueue
            // 
            this.btnBrowserQueue.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnBrowserQueue.Location = new System.Drawing.Point(415, 106);
            this.btnBrowserQueue.Name = "btnBrowserQueue";
            this.btnBrowserQueue.Size = new System.Drawing.Size(167, 41);
            this.btnBrowserQueue.TabIndex = 27;
            this.btnBrowserQueue.Text = "BrowserQueue";
            this.btnBrowserQueue.UseVisualStyleBackColor = true;
            this.btnBrowserQueue.Click += new System.EventHandler(this.btnBrowserQueue_Click);
            // 
            // txtMQPort
            // 
            this.txtMQPort.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtMQPort.Location = new System.Drawing.Point(609, 28);
            this.txtMQPort.Name = "txtMQPort";
            this.txtMQPort.Size = new System.Drawing.Size(53, 27);
            this.txtMQPort.TabIndex = 29;
            this.txtMQPort.Text = "61616";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.Location = new System.Drawing.Point(507, 28);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 24);
            this.label10.TabIndex = 28;
            this.label10.Text = "MQ Port:";
            // 
            // btnCleanQueue
            // 
            this.btnCleanQueue.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCleanQueue.Location = new System.Drawing.Point(609, 106);
            this.btnCleanQueue.Name = "btnCleanQueue";
            this.btnCleanQueue.Size = new System.Drawing.Size(167, 41);
            this.btnCleanQueue.TabIndex = 30;
            this.btnCleanQueue.Text = "CleanQueue";
            this.btnCleanQueue.UseVisualStyleBackColor = true;
            this.btnCleanQueue.Click += new System.EventHandler(this.btnCleanQueue_Click);
            // 
            // QueueTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1277, 621);
            this.Controls.Add(this.btnCleanQueue);
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
            this.Name = "QueueTest";
            this.Text = "CheckQueue";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPushMessage;
        private System.Windows.Forms.TextBox txtInMessage;
        private System.Windows.Forms.Button btnPullMessage1;
        private System.Windows.Forms.Button btnQueryQueue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtQueryQueueItem;
        private System.Windows.Forms.ListBox listQueueName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEnqueued;
        private System.Windows.Forms.TextBox txtPending;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtConsumers;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDequeued;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtActiveMQIP;
        private System.Windows.Forms.TextBox txtAdminPort;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnSaveSetting;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnGetQueueList;
        private System.Windows.Forms.Button btnBrowserQueue;
        private System.Windows.Forms.TextBox txtMQPort;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnCleanQueue;
    }
}

