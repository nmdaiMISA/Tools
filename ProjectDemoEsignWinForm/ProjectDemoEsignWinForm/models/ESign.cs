using System.Collections.Generic;

namespace ProjectDemoEsignWinForm.models
{
    // ==================== Auth ====================

    public class ESignAuthRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public string deviceId { get; set; }
    }

    public class TwoFactorAuthRequest
    {
        public string userName { get; set; }
        public string code { get; set; }
        public int otpType { get; set; }    // 2 = Authenticator app
        public bool remember { get; set; }
        public string deviceId { get; set; }
    }

    public class ResendOtpRequest
    {
        public string userName { get; set; }
        public string language { get; set; }
    }

    public class ESignAuthResponse
    {
        public AuthStatus status { get; set; }
        public AuthData data { get; set; }
    }

    public class AuthStatus
    {
        public string type { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public bool error { get; set; }
        public int errorCode { get; set; }
    }

    public class AuthData
    {
        public string accessToken { get; set; }
        public string remoteSigningAccessToken { get; set; }
        public string tokenType { get; set; }
        public int expiresIn { get; set; }
        public string refreshToken { get; set; }
        public AuthUser user { get; set; }
    }

    public class AuthUser
    {
        public string id { get; set; }
        public string email { get; set; }
        public object phoneNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public object username { get; set; }
    }

    // Note: Certificate model đã được thay thế bởi UserCertificateGetDto trong SDK
    // MISA.eSign.RemoteSigning.SDK.Model.UserCertificateGetDto

    // ==================== Sign Hash ====================

    public class SignHashRequest
    {
        public string DataToBeDisplayed { get; set; }
        public string UserId { get; set; }
        public string CertAlias { get; set; }
        public List<SignHashDocumentDto> Documents { get; set; }
    }

    public class SignHashDocumentDto
    {
        public string DocumentId { get; set; }
        public string FileToSign { get; set; }
        public string DocumentName { get; set; }
    }
}
