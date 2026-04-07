using System;
using System.Net.Http;
using System.Windows.Forms;

namespace ProjectDemoEsignWinForm
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Configuration = System.Configuration.ConfigurationManager.AppSettings;
            DeviceId = Configuration["DeviceId"];

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLogin());
        }

        public static string TOKEN { get; set; }
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string DeviceId { get; set; }

        public static System.Collections.Specialized.NameValueCollection Configuration { get; set; }

        /// <summary>
        /// Tạo HttpClient với header Authorization Bearer (dùng để lấy danh sách CTS)
        /// </summary>
        public static HttpClient CreateHttpClient(string token = null)
        {
            var client = new HttpClient();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
            return client;
        }

        /// <summary>
        /// Tạo HttpClient với header AuthorizationRM + x-clientid + x-clientkey (dùng cho API ký số)
        /// </summary>
        public static HttpClient CreateSigningHttpClient(string token, string clientId, string clientKey)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("AuthorizationRM", $"Bearer {token}");
            client.DefaultRequestHeaders.Add("x-clientid", clientId);
            client.DefaultRequestHeaders.Add("x-clientkey", clientKey);
            return client;
        }
    }
}
