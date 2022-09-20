namespace FEIBMQFileTransfer
{
    partial class MainForm
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
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTargetPath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.listBoxFileList = new System.Windows.Forms.ListBox();
            this.btnSendFileToQueue = new System.Windows.Forms.Button();
            this.btnSendFileToTopic = new System.Windows.Forms.Button();
            this.txtStatusMessage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCommand = new System.Windows.Forms.Button();
            this.txtRunPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtArgs = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSummitCommand = new System.Windows.Forms.Button();
            this.btnGetFileFromTopic = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtFileName
            // 
            this.txtFileName.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtFileName.Location = new System.Drawing.Point(571, 39);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(285, 27);
            this.txtFileName.TabIndex = 24;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(395, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(170, 24);
            this.label7.TabIndex = 23;
            this.label7.Text = "Target FileName:";
            // 
            // txtTargetPath
            // 
            this.txtTargetPath.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtTargetPath.Location = new System.Drawing.Point(176, 6);
            this.txtTargetPath.Name = "txtTargetPath";
            this.txtTargetPath.Size = new System.Drawing.Size(680, 27);
            this.txtTargetPath.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(30, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(140, 24);
            this.label6.TabIndex = 21;
            this.label6.Text = "Target Folder:";
            // 
            // listBoxFileList
            // 
            this.listBoxFileList.AllowDrop = true;
            this.listBoxFileList.FormattingEnabled = true;
            this.listBoxFileList.ItemHeight = 12;
            this.listBoxFileList.Location = new System.Drawing.Point(12, 126);
            this.listBoxFileList.Name = "listBoxFileList";
            this.listBoxFileList.Size = new System.Drawing.Size(844, 88);
            this.listBoxFileList.TabIndex = 25;
            this.listBoxFileList.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBoxFileList_DragDrop);
            this.listBoxFileList.DragOver += new System.Windows.Forms.DragEventHandler(this.listBoxFileList_DragOver);
            // 
            // btnSendFileToQueue
            // 
            this.btnSendFileToQueue.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSendFileToQueue.Location = new System.Drawing.Point(12, 71);
            this.btnSendFileToQueue.Name = "btnSendFileToQueue";
            this.btnSendFileToQueue.Size = new System.Drawing.Size(213, 39);
            this.btnSendFileToQueue.TabIndex = 26;
            this.btnSendFileToQueue.Text = "SendFileToQueue";
            this.btnSendFileToQueue.UseVisualStyleBackColor = true;
            this.btnSendFileToQueue.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnSendFileToTopic
            // 
            this.btnSendFileToTopic.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSendFileToTopic.Location = new System.Drawing.Point(244, 71);
            this.btnSendFileToTopic.Name = "btnSendFileToTopic";
            this.btnSendFileToTopic.Size = new System.Drawing.Size(213, 39);
            this.btnSendFileToTopic.TabIndex = 27;
            this.btnSendFileToTopic.Text = "SendFileToTopic";
            this.btnSendFileToTopic.UseVisualStyleBackColor = true;
            this.btnSendFileToTopic.Click += new System.EventHandler(this.btnSendFileToTopic_Click);
            // 
            // txtStatusMessage
            // 
            this.txtStatusMessage.Location = new System.Drawing.Point(12, 254);
            this.txtStatusMessage.Multiline = true;
            this.txtStatusMessage.Name = "txtStatusMessage";
            this.txtStatusMessage.Size = new System.Drawing.Size(844, 78);
            this.txtStatusMessage.TabIndex = 28;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(12, 227);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 24);
            this.label1.TabIndex = 29;
            this.label1.Text = "Status:";
            // 
            // txtCommand
            // 
            this.txtCommand.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtCommand.Location = new System.Drawing.Point(128, 347);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(680, 27);
            this.txtCommand.TabIndex = 31;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(12, 350);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 24);
            this.label2.TabIndex = 30;
            this.label2.Text = "Command:";
            // 
            // btnCommand
            // 
            this.btnCommand.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCommand.Location = new System.Drawing.Point(12, 449);
            this.btnCommand.Name = "btnCommand";
            this.btnCommand.Size = new System.Drawing.Size(213, 39);
            this.btnCommand.TabIndex = 32;
            this.btnCommand.Text = "RunCommand";
            this.btnCommand.UseVisualStyleBackColor = true;
            this.btnCommand.Click += new System.EventHandler(this.btnCommand_Click);
            // 
            // txtRunPath
            // 
            this.txtRunPath.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtRunPath.Location = new System.Drawing.Point(128, 416);
            this.txtRunPath.Name = "txtRunPath";
            this.txtRunPath.Size = new System.Drawing.Size(680, 27);
            this.txtRunPath.TabIndex = 34;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(12, 419);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 24);
            this.label3.TabIndex = 33;
            this.label3.Text = "RunPath";
            // 
            // txtArgs
            // 
            this.txtArgs.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtArgs.Location = new System.Drawing.Point(128, 380);
            this.txtArgs.Name = "txtArgs";
            this.txtArgs.Size = new System.Drawing.Size(680, 27);
            this.txtArgs.TabIndex = 36;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(12, 383);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 24);
            this.label4.TabIndex = 35;
            this.label4.Text = "Args:";
            // 
            // btnSummitCommand
            // 
            this.btnSummitCommand.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSummitCommand.Location = new System.Drawing.Point(260, 449);
            this.btnSummitCommand.Name = "btnSummitCommand";
            this.btnSummitCommand.Size = new System.Drawing.Size(213, 39);
            this.btnSummitCommand.TabIndex = 37;
            this.btnSummitCommand.Text = "RunSummitCommand";
            this.btnSummitCommand.UseVisualStyleBackColor = true;
            this.btnSummitCommand.Click += new System.EventHandler(this.btnSummitCommand_Click);
            // 
            // btnGetFileFromTopic
            // 
            this.btnGetFileFromTopic.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnGetFileFromTopic.Location = new System.Drawing.Point(479, 71);
            this.btnGetFileFromTopic.Name = "btnGetFileFromTopic";
            this.btnGetFileFromTopic.Size = new System.Drawing.Size(213, 39);
            this.btnGetFileFromTopic.TabIndex = 38;
            this.btnGetFileFromTopic.Text = "GetFileFromTopic";
            this.btnGetFileFromTopic.UseVisualStyleBackColor = true;
            this.btnGetFileFromTopic.Click += new System.EventHandler(this.btnGetFileFromTopic_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 735);
            this.Controls.Add(this.btnGetFileFromTopic);
            this.Controls.Add(this.btnSummitCommand);
            this.Controls.Add(this.txtArgs);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtRunPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCommand);
            this.Controls.Add(this.txtCommand);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtStatusMessage);
            this.Controls.Add(this.btnSendFileToTopic);
            this.Controls.Add(this.btnSendFileToQueue);
            this.Controls.Add(this.listBoxFileList);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtTargetPath);
            this.Controls.Add(this.label6);
            this.Name = "MainForm";
            this.Text = "FEIBMQFileTransfer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.MainForm_DragOver);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTargetPath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox listBoxFileList;
        private System.Windows.Forms.Button btnSendFileToQueue;
        private System.Windows.Forms.Button btnSendFileToTopic;
        private System.Windows.Forms.TextBox txtStatusMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCommand;
        private System.Windows.Forms.TextBox txtRunPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtArgs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSummitCommand;
        private System.Windows.Forms.Button btnGetFileFromTopic;
    }
}

