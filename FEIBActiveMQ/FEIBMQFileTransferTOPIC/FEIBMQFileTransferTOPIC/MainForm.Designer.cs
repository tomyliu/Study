namespace FEIBMQFileTransferTOPIC
{
    partial class MainForm
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
            this.btnSendFileToTopic = new System.Windows.Forms.Button();
            this.txtStatusMessage = new System.Windows.Forms.TextBox();
            this.listBoxFileList = new System.Windows.Forms.ListBox();
            this.btnClearFileList = new System.Windows.Forms.Button();
            this.btnCloseForm = new System.Windows.Forms.Button();
            this.lbFileListDesc = new System.Windows.Forms.Label();
            this.lbStatusMessageDesc = new System.Windows.Forms.Label();
            this.cbIsCover = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnSendFileToTopic
            // 
            this.btnSendFileToTopic.Enabled = false;
            this.btnSendFileToTopic.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSendFileToTopic.Location = new System.Drawing.Point(12, 12);
            this.btnSendFileToTopic.Name = "btnSendFileToTopic";
            this.btnSendFileToTopic.Size = new System.Drawing.Size(200, 39);
            this.btnSendFileToTopic.TabIndex = 28;
            this.btnSendFileToTopic.Text = "開始進行傳送";
            this.btnSendFileToTopic.UseVisualStyleBackColor = true;
            this.btnSendFileToTopic.Click += new System.EventHandler(this.btnSendFileToTopic_Click);
            // 
            // txtStatusMessage
            // 
            this.txtStatusMessage.Location = new System.Drawing.Point(12, 239);
            this.txtStatusMessage.Multiline = true;
            this.txtStatusMessage.Name = "txtStatusMessage";
            this.txtStatusMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatusMessage.Size = new System.Drawing.Size(844, 120);
            this.txtStatusMessage.TabIndex = 30;
            // 
            // listBoxFileList
            // 
            this.listBoxFileList.AllowDrop = true;
            this.listBoxFileList.FormattingEnabled = true;
            this.listBoxFileList.ItemHeight = 12;
            this.listBoxFileList.Location = new System.Drawing.Point(12, 90);
            this.listBoxFileList.Name = "listBoxFileList";
            this.listBoxFileList.Size = new System.Drawing.Size(844, 112);
            this.listBoxFileList.TabIndex = 29;
            this.listBoxFileList.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBoxFileList_DragDrop);
            this.listBoxFileList.DragOver += new System.Windows.Forms.DragEventHandler(this.listBoxFileList_DragOver);
            // 
            // btnClearFileList
            // 
            this.btnClearFileList.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnClearFileList.Location = new System.Drawing.Point(347, 12);
            this.btnClearFileList.Name = "btnClearFileList";
            this.btnClearFileList.Size = new System.Drawing.Size(200, 39);
            this.btnClearFileList.TabIndex = 31;
            this.btnClearFileList.Text = "清除檔案列表";
            this.btnClearFileList.UseVisualStyleBackColor = true;
            this.btnClearFileList.Click += new System.EventHandler(this.btnClearFileList_Click);
            // 
            // btnCloseForm
            // 
            this.btnCloseForm.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCloseForm.Location = new System.Drawing.Point(756, 12);
            this.btnCloseForm.Name = "btnCloseForm";
            this.btnCloseForm.Size = new System.Drawing.Size(100, 39);
            this.btnCloseForm.TabIndex = 32;
            this.btnCloseForm.Text = "關閉";
            this.btnCloseForm.UseVisualStyleBackColor = true;
            this.btnCloseForm.Click += new System.EventHandler(this.btnCloseForm_Click);
            // 
            // lbFileListDesc
            // 
            this.lbFileListDesc.AutoSize = true;
            this.lbFileListDesc.Font = new System.Drawing.Font("新細明體", 12F);
            this.lbFileListDesc.Location = new System.Drawing.Point(12, 68);
            this.lbFileListDesc.Name = "lbFileListDesc";
            this.lbFileListDesc.Size = new System.Drawing.Size(344, 16);
            this.lbFileListDesc.TabIndex = 33;
            this.lbFileListDesc.Text = "待傳送檔案列表清單，請將檔案拖曳到下面框中";
            // 
            // lbStatusMessageDesc
            // 
            this.lbStatusMessageDesc.AutoSize = true;
            this.lbStatusMessageDesc.Font = new System.Drawing.Font("新細明體", 12F);
            this.lbStatusMessageDesc.Location = new System.Drawing.Point(12, 216);
            this.lbStatusMessageDesc.Name = "lbStatusMessageDesc";
            this.lbStatusMessageDesc.Size = new System.Drawing.Size(136, 16);
            this.lbStatusMessageDesc.TabIndex = 34;
            this.lbStatusMessageDesc.Text = "傳送結果回覆清單";
            // 
            // cbIsCover
            // 
            this.cbIsCover.AutoSize = true;
            this.cbIsCover.Font = new System.Drawing.Font("新細明體", 12F);
            this.cbIsCover.Location = new System.Drawing.Point(223, 24);
            this.cbIsCover.Name = "cbIsCover";
            this.cbIsCover.Size = new System.Drawing.Size(91, 20);
            this.cbIsCover.TabIndex = 35;
            this.cbIsCover.Text = "強制覆蓋";
            this.cbIsCover.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 371);
            this.Controls.Add(this.cbIsCover);
            this.Controls.Add(this.lbStatusMessageDesc);
            this.Controls.Add(this.lbFileListDesc);
            this.Controls.Add(this.btnCloseForm);
            this.Controls.Add(this.btnClearFileList);
            this.Controls.Add(this.txtStatusMessage);
            this.Controls.Add(this.listBoxFileList);
            this.Controls.Add(this.btnSendFileToTopic);
            this.Name = "MainForm";
            this.Text = "FEIBMQFileTransferTOPIC";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.MainForm_DragOver);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSendFileToTopic;
        private System.Windows.Forms.TextBox txtStatusMessage;
        private System.Windows.Forms.ListBox listBoxFileList;
        private System.Windows.Forms.Button btnClearFileList;
        private System.Windows.Forms.Button btnCloseForm;
        private System.Windows.Forms.Label lbFileListDesc;
        private System.Windows.Forms.Label lbStatusMessageDesc;
        private System.Windows.Forms.CheckBox cbIsCover;
    }
}

