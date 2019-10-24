namespace StateMachine
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.GetMdlFilePathButton = new System.Windows.Forms.Button();
            this.ParseMdlFileButton = new System.Windows.Forms.Button();
            this.MdlFilePathTextBox = new System.Windows.Forms.TextBox();
            this.OutPutTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // GetMdlFilePathButton
            // 
            this.GetMdlFilePathButton.Location = new System.Drawing.Point(100, 134);
            this.GetMdlFilePathButton.Name = "GetMdlFilePathButton";
            this.GetMdlFilePathButton.Size = new System.Drawing.Size(199, 56);
            this.GetMdlFilePathButton.TabIndex = 0;
            this.GetMdlFilePathButton.Text = "Get mdl File Path";
            this.GetMdlFilePathButton.UseVisualStyleBackColor = true;
            this.GetMdlFilePathButton.Click += new System.EventHandler(this.GetMdlFilePathButton_Click);
            // 
            // ParseMdlFileButton
            // 
            this.ParseMdlFileButton.Location = new System.Drawing.Point(100, 231);
            this.ParseMdlFileButton.Name = "ParseMdlFileButton";
            this.ParseMdlFileButton.Size = new System.Drawing.Size(199, 57);
            this.ParseMdlFileButton.TabIndex = 1;
            this.ParseMdlFileButton.Text = "Parse mdl File";
            this.ParseMdlFileButton.UseVisualStyleBackColor = true;
            this.ParseMdlFileButton.Click += new System.EventHandler(this.ParseMdlFileButton_Click);
            // 
            // MdlFilePathTextBox
            // 
            this.MdlFilePathTextBox.Location = new System.Drawing.Point(31, 71);
            this.MdlFilePathTextBox.Name = "MdlFilePathTextBox";
            this.MdlFilePathTextBox.Size = new System.Drawing.Size(718, 28);
            this.MdlFilePathTextBox.TabIndex = 2;
            this.MdlFilePathTextBox.Text = "D:\\GitHub\\StateMachine\\10-8.mdl";
            // 
            // OutPutTextBox
            // 
            this.OutPutTextBox.Location = new System.Drawing.Point(329, 134);
            this.OutPutTextBox.Multiline = true;
            this.OutPutTextBox.Name = "OutPutTextBox";
            this.OutPutTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.OutPutTextBox.Size = new System.Drawing.Size(445, 291);
            this.OutPutTextBox.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.OutPutTextBox);
            this.Controls.Add(this.MdlFilePathTextBox);
            this.Controls.Add(this.ParseMdlFileButton);
            this.Controls.Add(this.GetMdlFilePathButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GetMdlFilePathButton;
        private System.Windows.Forms.Button ParseMdlFileButton;
        private System.Windows.Forms.TextBox MdlFilePathTextBox;
        private System.Windows.Forms.TextBox OutPutTextBox;
    }
}

