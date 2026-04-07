namespace ProjectDemoEsignWinForm
{
    partial class frmOTP
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblOtp = new System.Windows.Forms.Label();
            this.txtOtp = new System.Windows.Forms.TextBox();
            this.chkRemember = new System.Windows.Forms.CheckBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnResend = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblMessage
            //
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblMessage.Location = new System.Drawing.Point(12, 12);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(396, 36);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "";
            //
            // lblOtp
            //
            this.lblOtp.AutoSize = true;
            this.lblOtp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblOtp.Location = new System.Drawing.Point(12, 58);
            this.lblOtp.Name = "lblOtp";
            this.lblOtp.Size = new System.Drawing.Size(62, 17);
            this.lblOtp.TabIndex = 1;
            this.lblOtp.Text = "Mã OTP:";
            //
            // txtOtp
            //
            this.txtOtp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtOtp.Location = new System.Drawing.Point(90, 55);
            this.txtOtp.MaxLength = 6;
            this.txtOtp.Name = "txtOtp";
            this.txtOtp.Size = new System.Drawing.Size(318, 23);
            this.txtOtp.TabIndex = 0;
            //
            // chkRemember
            //
            this.chkRemember.AutoSize = true;
            this.chkRemember.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.chkRemember.Location = new System.Drawing.Point(12, 90);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.Size = new System.Drawing.Size(180, 21);
            this.chkRemember.TabIndex = 1;
            this.chkRemember.Text = "Nhớ thiết bị này";
            this.chkRemember.UseVisualStyleBackColor = true;
            //
            // btnConfirm
            //
            this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnConfirm.Location = new System.Drawing.Point(12, 122);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(126, 30);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "Xác nhận";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            //
            // btnResend
            //
            this.btnResend.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnResend.Location = new System.Drawing.Point(153, 122);
            this.btnResend.Name = "btnResend";
            this.btnResend.Size = new System.Drawing.Size(126, 30);
            this.btnResend.TabIndex = 3;
            this.btnResend.Text = "Gửi lại OTP";
            this.btnResend.UseVisualStyleBackColor = true;
            this.btnResend.Click += new System.EventHandler(this.btnResend_Click);
            //
            // btnCancel
            //
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(294, 122);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(114, 30);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // frmOTP
            //
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 165);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblOtp);
            this.Controls.Add(this.txtOtp);
            this.Controls.Add(this.chkRemember);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnResend);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmOTP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Xác thực OTP";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblOtp;
        private System.Windows.Forms.TextBox txtOtp;
        private System.Windows.Forms.CheckBox chkRemember;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnResend;
        private System.Windows.Forms.Button btnCancel;
    }
}
