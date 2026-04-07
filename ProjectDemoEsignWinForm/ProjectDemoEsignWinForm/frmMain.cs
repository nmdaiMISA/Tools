using MISA.eSign.RemoteSigning.SDK.Model;
using MISA.eSign.RemoteSigning.SDK.Utils;
using Newtonsoft.Json;
using ProjectDemoEsignWinForm.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ProjectDemoEsignWinForm
{
    public partial class frmMain : Form
    {
        private List<UserCertificateGetDto> _listCert = new List<UserCertificateGetDto>();
        private string _clientId;
        private string _clientKey;
        private DocumentType _typeFile;

        private frmLogin _frmLogin;

        // Lưu kết quả trung gian giữa các bước ký
        private SignHashResDto _signResponse;
        private UserCertificateGetDto _selectedCert;
        private CalculateLocalHashResult _hashResult;

        public frmMain(frmLogin frmLogin)
        {
            InitializeComponent();
            _clientId = Program.Configuration["ClientID"];
            _clientKey = Program.Configuration["ClientKEY"];
            _frmLogin = frmLogin;
        }

        // ==================== Lấy danh sách chứng thư số ====================

        private void frmMain_Shown(object sender, EventArgs e)
        {
            GetCertificates();
        }

        private void btnGetCer_Click(object sender, EventArgs e)
        {
            GetCertificates();
        }

        private void GetCertificates()
        {
            SetStatus("Đang lấy danh sách chữ ký số...");
            RunBackground(() =>
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Program.TOKEN}");
                        client.DefaultRequestHeaders.Add("x-clientid", _clientId);
                        client.DefaultRequestHeaders.Add("x-clientkey", _clientKey);

                        var response = client.GetAsync(Program.Configuration["URL_GET_CERT"]).Result;
                        string body = response.Content.ReadAsStringAsync().Result;

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var certs = JsonConvert.DeserializeObject<List<UserCertificateGetDto>>(body);
                            if (certs != null && certs.Count > 0)
                            {
                                _listCert = certs;
                                // Xóa ký tự xuống dòng trong certificate
                                _listCert.ForEach(c =>
                                {
                                    if (c.Certificate != null)
                                        c.Certificate = c.Certificate.Replace("\r\n", "").Replace("\n", "");
                                });

                                InvokeUI(() =>
                                {
                                    cbbList_Cert.Items.Clear();
                                    foreach (var cert in _listCert)
                                        cbbList_Cert.Items.Add(cert.KeyAlias ?? cert.UserId);
                                    cbbList_Cert.SelectedIndex = 0;

                                    SetControlsEnabled(true);
                                    SetStatus("");
                                });
                            }
                            else
                            {
                                SetStatus("Không có chữ ký số nào trong tài khoản!");
                            }
                        }
                        else
                        {
                            SetStatus("Lỗi khi lấy danh sách chữ ký số. Ấn 'Lấy CTS' để thử lại!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    SetStatus("Lỗi: " + ex.Message);
                }
            });
        }

        // ==================== Chọn file ====================

        private void btnPathChooseSign_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Supported files (*.pdf;*.xml;*.docx;*.xlsx)|*.pdf;*.xml;*.docx;*.xlsx";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtPathSign.Text = ofd.FileName;
                    string ext = Path.GetExtension(ofd.FileName).ToLower();
                    switch (ext)
                    {
                        case ".pdf":  _typeFile = DocumentType.Pdf;   break;
                        case ".xml":  _typeFile = DocumentType.Xml;   break;
                        case ".docx": _typeFile = DocumentType.Word;  break;
                        case ".xlsx": _typeFile = DocumentType.Excel; break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnImgSignature_Click(object sender, EventArgs e)
        {
            OpenImageFile(txtImgSignature);
        }

        private void btnImgLogo_Click(object sender, EventArgs e)
        {
            OpenImageFile(txtImgLogo);
        }

        private void OpenImageFile(System.Windows.Forms.TextBox target)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Image file (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
                if (ofd.ShowDialog() == DialogResult.OK)
                    target.Text = ofd.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // ==================== Bước 1: Hash tài liệu (Presign) ====================

        /// <summary>
        /// Dùng DocumentUtil.CalculateLocalHash() từ SDK để tính hash tài liệu cục bộ
        /// </summary>
        private CalculateLocalHashResult HashDocument(UserCertificateGetDto cert)
        {
            var pdfDocs   = new List<PdfDocToHash>();
            var xmlDocs   = new List<XmlDocToHash>();
            var wordDocs  = new List<WordDocToHash>();
            var excelDocs = new List<ExcelDocToHash>();

            switch (_typeFile)
            {
                case DocumentType.Pdf:
                    var signatureImageBase64 = string.IsNullOrWhiteSpace(txtImgSignature.Text)
                        ? null
                        : Convert.ToBase64String(File.ReadAllBytes(txtImgSignature.Text));
                    var logoImageBytes = string.IsNullOrWhiteSpace(txtImgLogo.Text)
                        ? null
                        : File.ReadAllBytes(txtImgLogo.Text);

                    var renderMode = signatureImageBase64 != null
                        ? RenderingMode.GraphicAndDescription
                        : RenderingMode.Description;

                    var pdfSigInfo = new PdfSignatureInfo(
                        positionX: 380,
                        positionY: 160,
                        width: 200,
                        height: 50,
                        fontSize: 10,
                        textColor: PDFSignatureColor.Black,
                        renderingMode: renderMode,
                        signatureImage: signatureImageBase64,
                        signatureDescription: new PdfSignatureDescription
                        {
                            DisplayText = txtContentSign.Text
                        },
                        page: 1,
                        signatureName: Guid.NewGuid().ToString(),
                        logoImage: logoImageBytes
                    );

                    // Set font tiếng Việt — bắt buộc, SDK sẽ NullReferenceException nếu thiếu
                    string fontPath = Path.Combine(
                        Path.GetDirectoryName(Application.ExecutablePath),
                        "VietNamese_Font", "Hoa_Sen_Typeface.ttf");
                    pdfSigInfo.FontPath = fontPath;
                    pdfSigInfo.FontData = File.ReadAllBytes(fontPath);

                    pdfDocs.Add(new PdfDocToHash
                    {
                        DocumentId = Guid.NewGuid().ToString(),
                        FileToSign = File.ReadAllBytes(txtPathSign.Text),
                        SignatureInfo = pdfSigInfo
                    });
                    break;

                case DocumentType.Xml:
                    xmlDocs.Add(new XmlDocToHash
                    {
                        DocumentId = Guid.NewGuid().ToString(),
                        FileToSign = File.ReadAllText(txtPathSign.Text),
                        SignatureInfo = new XmlSignatureInfoDto
                        {
                            Id = "seller",
                            NodeToSign = new List<string> { "#1VUPFJ59D", "#SignTime_1" },
                            SignatureLocation = "//HDon/DSCKS/NBan",
                            ShowSigningTime = true,
                            TimeNodeId = "SignTime_1"
                        }
                    });
                    break;

                case DocumentType.Word:
                    wordDocs.Add(new WordDocToHash
                    {
                        DocumentId = Guid.NewGuid().ToString(),
                        FileToSign = File.ReadAllBytes(txtPathSign.Text)
                    });
                    break;

                case DocumentType.Excel:
                    excelDocs.Add(new ExcelDocToHash
                    {
                        DocumentId = Guid.NewGuid().ToString(),
                        FileToSign = File.ReadAllBytes(txtPathSign.Text)
                    });
                    break;
            }

            var data = new CalculateLocalHashData
            {
                Certificate = Convert.FromBase64String(cert.Certificate),
                CertificateChain = cert.CertiticateChain,
                DocumentType = _typeFile,
                PdfDocs = pdfDocs,
                XmlDocs = xmlDocs,
                WordDocs = wordDocs,
                ExcelDocs = excelDocs
            };

            // Gọi SDK để tính hash cục bộ
            return DocumentUtil.CalculateLocalHash(data);
        }

        // ==================== Bước 2: Gửi file để ký (SignHash) ====================

        /// <summary>
        /// Gọi API để ký — gửi toàn bộ nội dung file (base64), người dùng xác nhận trên app mobile
        /// </summary>
        private SignHashResDto SignHashedFile(UserCertificateGetDto cert, string fileName)
        {
            string documentId;
            switch (_typeFile)
            {
                case DocumentType.Pdf:   documentId = _hashResult.PdfDocs[0].DocumentId;   break;
                case DocumentType.Xml:   documentId = _hashResult.XmlDocs[0].DocumentId;   break;
                case DocumentType.Word:  documentId = _hashResult.WordDocs[0].DocumentId;  break;
                case DocumentType.Excel: documentId = _hashResult.ExcelDocs[0].DocumentId; break;
                default:                 documentId = Guid.NewGuid().ToString();             break;
            }

            string fileBase64 = Convert.ToBase64String(File.ReadAllBytes(txtPathSign.Text));

            var request = new SignHashRequest
            {
                DataToBeDisplayed = $"Ký file {fileName}",
                UserId            = cert.UserId,
                CertAlias         = cert.KeyAlias,
                Documents         = new List<SignHashDocumentDto>
                {
                    new SignHashDocumentDto
                    {
                        DocumentId   = documentId,
                        FileToSign   = fileBase64,
                        DocumentName = fileName
                    }
                }
            };

            string json = JsonConvert.SerializeObject(request);
            using (var client = Program.CreateSigningHttpClient(Program.TOKEN, _clientId, _clientKey))
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync(Program.Configuration["URL_SIGNING_HASHED_FILE"], content).Result;
                string body = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode != HttpStatusCode.OK)
                    return null;

                return JsonConvert.DeserializeObject<SignHashResDto>(body);
            }
        }

        // ==================== Bước 2b: Kiểm tra trạng thái ký ====================

        private SigningStatusResDto GetSigningStatus(string transactionId)
        {
            string url = string.Format(Program.Configuration["URL_SIGNING_STATUS"], transactionId);
            using (var client = Program.CreateSigningHttpClient(Program.TOKEN, _clientId, _clientKey))
            {
                var response = client.GetAsync(url).Result;
                string body = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<SigningStatusResDto>(body);
            }
        }

        // ==================== Bước 3: Gắn chữ ký vào file (PostSign) ====================

        /// <summary>
        /// Dùng DocumentUtil.AttachSignature() từ SDK để gắn chữ ký số vào tài liệu
        /// </summary>
        private void AttachAndSaveSignature(UserCertificateGetDto cert, CalculateLocalHashResult hashResult, SigningStatusResDto statusResult)
        {
            var attachPdfDatas   = new List<AttachSignaturePdfData>();
            var attachXmlDatas   = new List<AttachSignatureXmlData>();
            var attachWordDatas  = new List<AttachSignatureWordData>();
            var attachExcelDatas = new List<AttachSignatureExcelData>();
            string filter = "";

            switch (_typeFile)
            {
                case DocumentType.Pdf:
                    filter = "PDF File|*.pdf";
                    attachPdfDatas.Add(new AttachSignaturePdfData
                    {
                        Digest        = hashResult.PdfDocs[0].Digest,
                        DocumentHash  = hashResult.PdfDocs[0].DocumentHash,
                        Sh            = hashResult.PdfDocs[0].Sh,
                        Signature     = statusResult.Signatures[0].Signature,
                        SignatureName = hashResult.PdfDocs[0].SignatureName,
                        DocumentBytes = hashResult.PdfDocs[0].DocumentBytes,
                        DocumentId    = hashResult.PdfDocs[0].DocumentId
                    });
                    break;

                case DocumentType.Xml:
                    filter = "XML File|*.xml";
                    attachXmlDatas.Add(new AttachSignatureXmlData
                    {
                        Digest      = hashResult.XmlDocs[0].Digest,
                        Sh          = hashResult.XmlDocs[0].Sh,
                        Signature   = statusResult.Signatures[0].Signature,
                        DocumentId  = hashResult.XmlDocs[0].DocumentId,
                        Document    = hashResult.XmlDocs[0].Document,
                        SignatureId = hashResult.XmlDocs[0].SignatureId
                    });
                    break;

                case DocumentType.Word:
                    filter = "DOCX File|*.docx";
                    attachWordDatas.Add(new AttachSignatureWordData
                    {
                        Digest        = hashResult.WordDocs[0].Digest,
                        Signature     = statusResult.Signatures[0].Signature,
                        DocumentId    = hashResult.WordDocs[0].DocumentId,
                        SignatureId   = hashResult.WordDocs[0].SignatureId,
                        DocumentBytes = hashResult.WordDocs[0].DocumentBytes,
                        MainDom       = hashResult.WordDocs[0].MainDom
                    });
                    break;

                case DocumentType.Excel:
                    filter = "XLSX File|*.xlsx";
                    attachExcelDatas.Add(new AttachSignatureExcelData
                    {
                        Digest        = hashResult.ExcelDocs[0].Digest,
                        Signature     = statusResult.Signatures[0].Signature,
                        DocumentId    = hashResult.ExcelDocs[0].DocumentId,
                        SignatureId   = hashResult.ExcelDocs[0].SignatureId,
                        DocumentBytes = hashResult.ExcelDocs[0].DocumentBytes,
                        MainDom       = hashResult.ExcelDocs[0].MainDom
                    });
                    break;
            }

            // Gọi SDK để gắn chữ ký vào file
            var attachResult = DocumentUtil.AttachSignature(new AttachSignatureData
            {
                PdfDocs           = attachPdfDatas,
                XmlDocs           = attachXmlDatas,
                WordDocs          = attachWordDatas,
                ExcelDocs         = attachExcelDatas,
                Certififcate      = Convert.FromBase64String(cert.Certificate),
                CertififcateChain = cert.CertiticateChain,
                DocumentType      = _typeFile
            });

            // Mở hộp thoại lưu file trên STA thread
            string filterCopy = filter;
            var t = new Thread(() =>
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = filterCopy,
                    Title = "Lưu tài liệu đã ký",
                    FileName = "Signed_" + Path.GetFileName(txtPathSign.Text),
                    RestoreDirectory = true
                };

                if (sfd.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(sfd.FileName))
                {
                    try
                    {
                        switch (_typeFile)
                        {
                            case DocumentType.Pdf:
                                File.WriteAllBytes(sfd.FileName, attachResult.PdfDocs[0].Document);
                                break;
                            case DocumentType.Xml:
                                File.WriteAllText(sfd.FileName, attachResult.XmlDocs[0].Document);
                                break;
                            case DocumentType.Word:
                                File.WriteAllBytes(sfd.FileName, attachResult.WordDocs[0].Document);
                                break;
                            case DocumentType.Excel:
                                File.WriteAllBytes(sfd.FileName, attachResult.ExcelDocs[0].Document);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lưu file: " + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }

        // ==================== Nút Ký ====================

        private void btnSign_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPathSign.Text))
            {
                MessageBox.Show("Vui lòng chọn file cần ký.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_listCert.Count == 0)
            {
                MessageBox.Show("Chưa có chữ ký số. Vui lòng lấy CTS trước.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string fileName = Path.GetFileName(txtPathSign.Text);
            _selectedCert = _listCert[cbbList_Cert.SelectedIndex];

            SetControlsEnabled(false);
            SetStatus("Đang thực hiện ký...");

            RunBackground(() =>
            {
                try
                {
                    // Bước 1: Tính hash tài liệu cục bộ
                    _hashResult = HashDocument(_selectedCert);

                    // Bước 2: Gửi file lên API để ký
                    _signResponse = SignHashedFile(_selectedCert, fileName);

                    if (_signResponse != null && !string.IsNullOrEmpty(_signResponse.TransactionId))
                    {
                        InvokeUI(() =>
                        {
                            txtTransID.Text = _signResponse.TransactionId;
                            btnSeachTransID.Enabled = true;
                            SetStatus("Đã gửi yêu cầu ký. Vui lòng xác nhận trên app mobile, rồi nhấn 'Tra cứu'.");
                        });
                    }
                    else
                    {
                        InvokeUI(() =>
                        {
                            SetControlsEnabled(true);
                            SetStatus("Gọi API ký thất bại!");
                        });
                        MessageBox.Show("Gọi API ký thất bại!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    InvokeUI(() =>
                    {
                        SetControlsEnabled(true);
                        SetStatus("Lỗi: " + ex.Message);
                    });
                    MessageBox.Show("Đã có lỗi xảy ra: " + ex.ToString(), "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }

        // ==================== Nút Tra cứu trạng thái ====================

        private void btnSeachTransID_Click(object sender, EventArgs e)
        {
            if (_signResponse == null || string.IsNullOrEmpty(_signResponse.TransactionId))
            {
                MessageBox.Show("Không có giao dịch nào để tra cứu.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSeachTransID.Enabled = false;
            SetStatus("Đang kiểm tra trạng thái ký...");

            RunBackground(() =>
            {
                string message = "";
                bool isSigned = false;
                SigningStatusResDto finalStatus = null;

                try
                {
                    bool done = false;
                    while (!done)
                    {
                        var status = GetSigningStatus(_signResponse.TransactionId);

                        if (status?.Status == "SUCCESS")
                        {
                            isSigned = true;
                            done = true;
                            finalStatus = status;
                            message = "Ký thành công!";
                        }
                        else if (status?.Status == "FAILED")
                        {
                            done = true;
                            message = "Phiên ký bị từ chối!";
                        }
                        else
                        {
                            Thread.Sleep(2000);
                        }
                    }

                    if (isSigned)
                    {
                        MessageBox.Show(message, "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Bước 3: Gắn chữ ký số vào tài liệu
                        AttachAndSaveSignature(_selectedCert, _hashResult, finalStatus);
                    }
                    else
                    {
                        MessageBox.Show(message, "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã có lỗi xảy ra: " + ex.ToString(), "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                InvokeUI(() =>
                {
                    SetStatus(message);
                    SetControlsEnabled(true);
                    btnSeachTransID.Enabled = true;
                });
            });
        }

        // ==================== Đăng xuất ====================

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            config.AppSettings.Settings.Remove("TOKEN");
            config.AppSettings.Settings.Add("TOKEN", "");
            config.Save(ConfigurationSaveMode.Modified);
            Program.TOKEN = string.Empty;

            _frmLogin.Show();
            this.Hide();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            _frmLogin.Close();
        }

        // ==================== Helpers ====================

        private void RunBackground(Action action)
        {
            var bw = new BackgroundWorker();
            bw.DoWork += (s, ev) => action();
            bw.RunWorkerAsync();
        }

        private void InvokeUI(Action action)
        {
            if (lblNotification.InvokeRequired)
                lblNotification.Invoke(action);
            else
                action();
        }

        private void SetStatus(string message)
        {
            InvokeUI(() => lblNotification.Text = message);
        }

        private void SetControlsEnabled(bool enabled)
        {
            InvokeUI(() =>
            {
                btnLogout.Enabled         = enabled;
                btnPathChooseSign.Enabled = enabled;
                btnImgLogo.Enabled        = enabled;
                btnImgSignature.Enabled   = enabled;
                btnSign.Enabled           = enabled;
                cbbList_Cert.Enabled      = enabled;
                txtPathSign.Enabled       = enabled;
                txtImgLogo.Enabled        = enabled;
                txtImgSignature.Enabled   = enabled;
                txtContentSign.Enabled    = enabled;
            });
        }
    }
}
