using Newtonsoft.Json;
using ProjectDemoEsignWinForm.models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace ProjectDemoEsignWinForm
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            txtUsername.Text = Program.Configuration["UserName"];
            txtPassword.Text = Program.Configuration["PassWord"];

            Program.Username = Program.Configuration["UserName"];
            Program.Password = Program.Configuration["PassWord"];
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var requestData = new ESignAuthRequest
                {
                    username = txtUsername.Text.Trim(),
                    password = txtPassword.Text,
                    deviceId = Program.DeviceId
                };

                string json = JsonConvert.SerializeObject(requestData);
                using (var client = new HttpClient())
                {
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PostAsync(Program.Configuration["URL_LOGIN"], content).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    var res = JsonConvert.DeserializeObject<ESignAuthResponse>(responseBody);

                    if (res?.data?.remoteSigningAccessToken != null)
                    {
                        // Đăng nhập thành công trực tiếp
                        SaveTokenAndOpenMain(res.data.remoteSigningAccessToken);
                    }
                    else if (res?.status?.errorCode == 122)
                    {
                        // Cần xác thực OTP 2 bước
                        string userName = res.data?.user?.username?.ToString() ?? txtUsername.Text.Trim();
                        string message = res.status.message;

                        using (var otpForm = new frmOTP(userName, message))
                        {
                            if (otpForm.ShowDialog() == DialogResult.OK)
                            {
                                OpenMain();
                            }
                        }
                    }
                    else
                    {
                        string msg = res?.status?.message ?? "Đăng nhập thất bại.";
                        MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveTokenAndOpenMain(string token)
        {
            Program.TOKEN = token;

            // Lưu token và thông tin đăng nhập vào config
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

            if (txtUsername.Text.Trim() != Program.Username)
            {
                config.AppSettings.Settings.Remove("UserName");
                config.AppSettings.Settings.Add("UserName", txtUsername.Text.Trim());
            }
            if (txtPassword.Text != Program.Password)
            {
                config.AppSettings.Settings.Remove("PassWord");
                config.AppSettings.Settings.Add("PassWord", txtPassword.Text);
            }

            config.AppSettings.Settings.Remove("TOKEN");
            config.AppSettings.Settings.Add("TOKEN", Program.TOKEN);
            config.Save(ConfigurationSaveMode.Modified);

            OpenMain();
        }

        private void OpenMain()
        {
            frmMain frmMain = new frmMain(this);
            frmMain.Closed += (s, args) => this.Close();
            frmMain.Show();
            this.Hide();
        }

        private void frmLogin_Shown(object sender, EventArgs e)
        {
            // Auto-login nếu đã có token lưu sẵn
            string savedToken = Program.Configuration["TOKEN"];
            if (!string.IsNullOrEmpty(savedToken))
            {
                Program.TOKEN = savedToken;
                OpenMain();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
