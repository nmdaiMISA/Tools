namespace ProjectDemoEsignWinForm
{
    partial class frmMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.cbbList_Cert = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPathChooseSign = new System.Windows.Forms.Button();
            this.txtPathSign = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSign = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblNotification = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtImgLogo = new System.Windows.Forms.TextBox();
            this.btnImgLogo = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtImgSignature = new System.Windows.Forms.TextBox();
            this.btnImgSignature = new System.Windows.Forms.Button();
            this.txtContentSign = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTransID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSeachTransID = new System.Windows.Forms.Button();
            this.btnGetCer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbbList_Cert
            // 
            this.cbbList_Cert.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbList_Cert.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cbbList_Cert.FormattingEnabled = true;
            this.cbbList_Cert.Location = new System.Drawing.Point(151, 8);
            this.cbbList_Cert.Name = "cbbList_Cert";
            this.cbbList_Cert.Size = new System.Drawing.Size(292, 28);
            this.cbbList_Cert.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(15, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Chọn chữ ký số";
            // 
            // btnPathChooseSign
            // 
            this.btnPathChooseSign.Enabled = false;
            this.btnPathChooseSign.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnPathChooseSign.Location = new System.Drawing.Point(453, 39);
            this.btnPathChooseSign.Name = "btnPathChooseSign";
            this.btnPathChooseSign.Size = new System.Drawing.Size(97, 29);
            this.btnPathChooseSign.TabIndex = 3;
            this.btnPathChooseSign.Text = "Chọn file ký";
            this.btnPathChooseSign.UseVisualStyleBackColor = true;
            this.btnPathChooseSign.Click += new System.EventHandler(this.btnPathChooseSign_Click);
            // 
            // txtPathSign
            // 
            this.txtPathSign.Enabled = false;
            this.txtPathSign.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtPathSign.Location = new System.Drawing.Point(151, 42);
            this.txtPathSign.Name = "txtPathSign";
            this.txtPathSign.Size = new System.Drawing.Size(292, 26);
            this.txtPathSign.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(15, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Đường dẫn file";
            // 
            // btnSign
            // 
            this.btnSign.Enabled = false;
            this.btnSign.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnSign.Location = new System.Drawing.Point(151, 202);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(134, 29);
            this.btnSign.TabIndex = 8;
            this.btnSign.Text = "Ký";
            this.btnSign.UseVisualStyleBackColor = true;
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Enabled = false;
            this.btnLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnLogout.Location = new System.Drawing.Point(346, 202);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(97, 29);
            this.btnLogout.TabIndex = 9;
            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // lblNotification
            // 
            this.lblNotification.AutoSize = true;
            this.lblNotification.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblNotification.Location = new System.Drawing.Point(15, 240);
            this.lblNotification.MaximumSize = new System.Drawing.Size(540, 0);
            this.lblNotification.Name = "lblNotification";
            this.lblNotification.Size = new System.Drawing.Size(81, 17);
            this.lblNotification.TabIndex = 7;
            this.lblNotification.Text = "Thông báo:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(15, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "File ảnh logo";
            // 
            // txtImgLogo
            // 
            this.txtImgLogo.Enabled = false;
            this.txtImgLogo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtImgLogo.Location = new System.Drawing.Point(151, 74);
            this.txtImgLogo.Name = "txtImgLogo";
            this.txtImgLogo.Size = new System.Drawing.Size(292, 26);
            this.txtImgLogo.TabIndex = 2;
            this.txtImgLogo.Text = "Samples/logo.png";
            // 
            // btnImgLogo
            // 
            this.btnImgLogo.Enabled = false;
            this.btnImgLogo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnImgLogo.Location = new System.Drawing.Point(453, 71);
            this.btnImgLogo.Name = "btnImgLogo";
            this.btnImgLogo.Size = new System.Drawing.Size(97, 29);
            this.btnImgLogo.TabIndex = 5;
            this.btnImgLogo.Text = "Chọn ảnh logo";
            this.btnImgLogo.UseVisualStyleBackColor = true;
            this.btnImgLogo.Click += new System.EventHandler(this.btnImgLogo_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label4.Location = new System.Drawing.Point(15, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 20);
            this.label4.TabIndex = 13;
            this.label4.Text = "File ảnh cks";
            // 
            // txtImgSignature
            // 
            this.txtImgSignature.Enabled = false;
            this.txtImgSignature.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtImgSignature.Location = new System.Drawing.Point(151, 105);
            this.txtImgSignature.Name = "txtImgSignature";
            this.txtImgSignature.Size = new System.Drawing.Size(292, 26);
            this.txtImgSignature.TabIndex = 4;
            this.txtImgSignature.Text = "Samples/signature.png";
            // 
            // btnImgSignature
            // 
            this.btnImgSignature.Enabled = false;
            this.btnImgSignature.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnImgSignature.Location = new System.Drawing.Point(453, 102);
            this.btnImgSignature.Name = "btnImgSignature";
            this.btnImgSignature.Size = new System.Drawing.Size(97, 29);
            this.btnImgSignature.TabIndex = 7;
            this.btnImgSignature.Text = "Chọn ảnh cks";
            this.btnImgSignature.UseVisualStyleBackColor = true;
            this.btnImgSignature.Click += new System.EventHandler(this.btnImgSignature_Click);
            // 
            // txtContentSign
            // 
            this.txtContentSign.Enabled = false;
            this.txtContentSign.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtContentSign.Location = new System.Drawing.Point(151, 137);
            this.txtContentSign.Name = "txtContentSign";
            this.txtContentSign.Size = new System.Drawing.Size(292, 26);
            this.txtContentSign.TabIndex = 6;
            this.txtContentSign.Text = "Test ký số";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label5.Location = new System.Drawing.Point(15, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 20);
            this.label5.TabIndex = 15;
            this.label5.Text = "Nội dung cks";
            // 
            // txtTransID
            // 
            this.txtTransID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtTransID.Location = new System.Drawing.Point(151, 169);
            this.txtTransID.Name = "txtTransID";
            this.txtTransID.ReadOnly = true;
            this.txtTransID.Size = new System.Drawing.Size(292, 26);
            this.txtTransID.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label6.Location = new System.Drawing.Point(15, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 20);
            this.label6.TabIndex = 15;
            this.label6.Text = "TransID file ký";
            // 
            // btnSeachTransID
            // 
            this.btnSeachTransID.Enabled = false;
            this.btnSeachTransID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnSeachTransID.Location = new System.Drawing.Point(453, 166);
            this.btnSeachTransID.Name = "btnSeachTransID";
            this.btnSeachTransID.Size = new System.Drawing.Size(97, 29);
            this.btnSeachTransID.TabIndex = 7;
            this.btnSeachTransID.Text = "Tra cứu";
            this.btnSeachTransID.UseVisualStyleBackColor = true;
            this.btnSeachTransID.Click += new System.EventHandler(this.btnSeachTransID_Click);
            // 
            // btnGetCer
            // 
            this.btnGetCer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnGetCer.Location = new System.Drawing.Point(453, 8);
            this.btnGetCer.Name = "btnGetCer";
            this.btnGetCer.Size = new System.Drawing.Size(97, 29);
            this.btnGetCer.TabIndex = 16;
            this.btnGetCer.Text = "Lấy CTS";
            this.btnGetCer.UseVisualStyleBackColor = true;
            this.btnGetCer.Click += new System.EventHandler(this.btnGetCer_Click);
            // 
            // frmMain
            // 
            this.AcceptButton = this.btnSign;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 432);
            this.Controls.Add(this.btnGetCer);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtTransID);
            this.Controls.Add(this.txtContentSign);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtImgSignature);
            this.Controls.Add(this.btnSeachTransID);
            this.Controls.Add(this.btnImgSignature);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtImgLogo);
            this.Controls.Add(this.btnImgLogo);
            this.Controls.Add(this.lblNotification);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnSign);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPathSign);
            this.Controls.Add(this.btnPathChooseSign);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbbList_Cert);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Demo Ký Số - MISA eSign SDK";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbbList_Cert;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPathChooseSign;
        private System.Windows.Forms.TextBox txtPathSign;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblNotification;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtImgLogo;
        private System.Windows.Forms.Button btnImgLogo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtImgSignature;
        private System.Windows.Forms.Button btnImgSignature;
        private System.Windows.Forms.TextBox txtContentSign;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTransID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSeachTransID;
        private System.Windows.Forms.Button btnGetCer;
    }
}
