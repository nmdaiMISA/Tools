using Newtonsoft.Json;
using ProjectDemoEsignWinForm.models;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace ProjectDemoEsignWinForm
{
    public partial class frmOTP : Form
    {
        private readonly string _userName;
        private readonly string _serverMessage;

        public frmOTP(string userName, string serverMessage)
        {
            InitializeComponent();
            _userName = userName;
            _serverMessage = serverMessage;
            lblMessage.Text = serverMessage;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOtp.Text))
            {
                MessageBox.Show("Vui lòng nhập mã OTP.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var data = new TwoFactorAuthRequest
                {
                    userName = _userName,
                    code = txtOtp.Text.Trim(),
                    otpType = 2,
                    remember = chkRemember.Checked,
                    deviceId = Program.DeviceId
                };

                string json = JsonConvert.SerializeObject(data);
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("clientId", Program.Configuration["ClientID"]);
                    client.DefaultRequestHeaders.Add("clientKey", Program.Configuration["ClientKEY"]);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PostAsync(Program.Configuration["URL_TWO_FACTOR_AUTH"], content).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    var res = JsonConvert.DeserializeObject<ESignAuthResponse>(responseBody);

                    if (res?.data?.remoteSigningAccessToken == null)
                    {
                        string msg = res?.status?.message ?? "Xác thực OTP thất bại. Vui lòng thử lại.";
                        MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    Program.TOKEN = res.data.remoteSigningAccessToken;

                    // Lưu token vào config
                    Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                    config.AppSettings.Settings.Remove("TOKEN");
                    config.AppSettings.Settings.Add("TOKEN", Program.TOKEN);
                    config.Save(ConfigurationSaveMode.Modified);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnResend_Click(object sender, EventArgs e)
        {
            try
            {
                var data = new ResendOtpRequest
                {
                    userName = _userName,
                    language = "vi-VN"
                };

                string json = JsonConvert.SerializeObject(data);
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("clientId", Program.Configuration["ClientID"]);
                    client.DefaultRequestHeaders.Add("clientKey", Program.Configuration["ClientKEY"]);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PostAsync(Program.Configuration["URL_RESEND_OTP"], content).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    var res = JsonConvert.DeserializeObject<ESignAuthResponse>(responseBody);

                    if (response.IsSuccessStatusCode)
                    {
                        txtOtp.Clear();
                        txtOtp.Focus();
                        MessageBox.Show("Mã OTP đã được gửi lại. Vui lòng kiểm tra ứng dụng xác thực.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string msg = res?.status?.message ?? "Gửi lại OTP thất bại. Vui lòng thử lại.";
                        MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
