# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Ngôn ngữ / Language

Trao đổi và ghi chú (comment, note, tài liệu nội bộ) **bằng tiếng Việt**.

## Tổng quan repo

Repo "Tools" gồm hai thành phần chính:
1. **ProjectDemoEsignWinForm** — Ứng dụng desktop C# WinForms (.NET Framework 4.7.2, x86) demo tích hợp MISA eSign Remote Signing API/SDK.
2. **BMAD tooling** — Công cụ Python hỗ trợ LLM authoring và các skill script của `.claude` (nằm trong `_bmad/` và `.claude/`).

## Build & Run (C# WinForms)

Dự án target .NET Framework 4.7.2, **phải build trên Windows**.

Cấu trúc solution: `ProjectDemoEsignWinForm/` (thư mục solution) chứa `ProjectDemoEsignWinForm.slnx` và thư mục con `ProjectDemoEsignWinForm/` (project thực sự, chứa `*.csproj`, `*.cs`, `App.config`, `Library/`).

**Visual Studio:** Mở `ProjectDemoEsignWinForm/ProjectDemoEsignWinForm.slnx` → Build → Build Solution.

**MSBuild (Developer Command Prompt):**
```bat
msbuild "ProjectDemoEsignWinForm\ProjectDemoEsignWinForm.slnx" /p:Configuration=Debug
```

**Build Release:**
```bat
msbuild "ProjectDemoEsignWinForm\ProjectDemoEsignWinForm.slnx" /p:Configuration=Release
```

**Chạy ứng dụng:**
```bat
ProjectDemoEsignWinForm\ProjectDemoEsignWinForm\bin\Debug\ProjectDemoEsignWinForm.exe
```

> **Lưu ý:** Dự án tham chiếu DLL local trong `ProjectDemoEsignWinForm/ProjectDemoEsignWinForm/Library/` (không dùng NuGet). Không xóa thư mục `Library/`.

> **Build artifacts:** `bin/`, `obj/`, `.vs/` là thư mục sinh ra khi build — không cần đọc/scan khi phân tích code.

## Python Tests (BMAD Tooling)

```bat
python -m venv .venv
.venv\Scripts\activate
pip install pytest pyyaml
```

Chạy tất cả test:
```bat
python -m pytest _bmad\core\bmad-distillator\scripts\tests
python -m pytest _bmad\core\bmad-init\scripts\tests
```

Chạy một test đơn lẻ:
```bat
python -m pytest _bmad\core\bmad-init\scripts\tests\test_bmad_init.py::TestClassName::test_method_name -v
```

> Test phải chạy từ **thư mục gốc repo** để import tương đối hoạt động đúng.

## Kiến trúc: C# WinForms App

**Entry point:** `Program.cs` — Nạp `App.config` vào static `Configuration` (NameValueCollection), set `DeviceId`. Cung cấp hai factory method:
- `CreateHttpClient(token)` — gọi API thông thường (header `Authorization: Bearer`)
- `CreateSigningHttpClient(token, clientId, clientKey)` — gọi API ký số (header `AuthorizationRM`, `x-clientid`, `x-clientkey`)

**Luồng xác thực:**
- `frmLogin.cs` — Hiển thị form đăng nhập, auto-fill từ `App.config`. Nếu `TOKEN` đã có trong config → tự động mở `frmMain` mà không cần đăng nhập lại.
- Đăng nhập thành công → ghi `TOKEN`, `UserName`, `PassWord` vào file `App.config` trên disk, mở `frmMain`.
- Nếu API trả `errorCode == 122` (two-factor) → mở `frmOTP.cs` để nhập OTP, gửi lên `URL_TWO_FACTOR_AUTH`, nhận token và ghi vào config tương tự.

**Luồng ký tài liệu (`frmMain.cs`):**
1. **Lấy cert:** GET `URL_GET_CERT` → populate combo `cbbList_Cert`.
2. **Pre-sign (local hash):** `DocumentUtil.CalculateLocalHash()` từ SDK — tạo DTO riêng cho từng định dạng (`PdfDocToHash`, `XmlDocToHash`, `WordDocToHash`, `ExcelDocToHash`). PDF nhúng font `VietNamese_Font/Hoa_Sen_Typeface.ttf`.
3. **Remote sign:** POST file (base64) + metadata lên `URL_SIGNING_HASHED_FILE` (hoặc `URL_SIGNING_SESSION_HASHED_FILE`) → nhận `TransactionId`.
4. **Poll status:** GET `URL_SIGNING_STATUS` (format với `{0}` = transactionId), `Thread.Sleep(2000)` mỗi vòng, cho đến khi SUCCESS/FAILED.
5. **Post-sign:** `DocumentUtil.AttachSignature()` đính chữ ký vào tài liệu → `SaveFileDialog` trên STA thread riêng để lưu file.

**SDK:** `MISA.eSign.RemoteSigning.SDK.dll` (local) cung cấp `DocumentUtil` (hash/attach) và các DTO (`UserCertificateGetDto`, `SignHashResDto`, `SigningStatusResDto`, …).

**Định dạng hỗ trợ:** PDF, XML, DOCX, XLSX.

**Models:** `models/ESign.cs` — request/response DTO cho auth (`ESignAuthRequest`, `TwoFactorAuthRequest`, `ResendOtpRequest`, `ESignAuthResponse`). `models/SignModel.cs` — DTO cho signing (hầu hết lấy từ SDK).

**Cấu hình (`App.config`):**
- Đọc: `Program.Configuration["key"]` (nạp một lần khi khởi động, không tự cập nhật khi ghi)
- Ghi: `ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath)` → `Remove/Add` → `config.Save(ConfigurationSaveMode.Modified)`
- Keys chính: `URL_LOGIN`, `URL_GET_CERT`, `URL_SIGNING_HASHED_FILE`, `URL_SIGNING_SESSION_HASHED_FILE`, `URL_SIGNING_STATUS`, `URL_ATTACH_SIGNATURE`, `URL_TWO_FACTOR_AUTH`, `URL_RESEND_OTP`, `TOKEN`, `UserName`, `PassWord`, `ClientID`, `ClientKEY`, `DeviceId`

**Thư viện DLL chính (trong `Library/`):** `MISA.eSign.RemoteSigning.SDK`, `Newtonsoft.Json`, `itextsharp`, `itext.*`, `BouncyCastle.Crypto`, `RestSharp`, `Polly`.

## Quy ước code (C# WinForms)

**Đặt tên:**
- Form class: tiền tố `frm` + PascalCase — `frmLogin`, `frmMain`, `frmOTP`
- Control: tiền tố theo loại + tên — `txtUsername`, `btnLogin`, `lblNotification`, `cbbList_Cert`
- Private field: tiền tố `_` + camelCase — `_listCert`, `_clientId`, `_signResponse`
- Static field đặc biệt: `TOKEN` (viết hoa hoàn toàn)
- Event handler: `controlName_EventName` — `btnLogin_Click`, `frmMain_Shown`
- Property DTO: PascalCase theo chuẩn C#, hoặc camelCase nếu cần map đúng JSON field của API

**HTTP pattern:**
- Tạo `HttpClient` mới cho mỗi request (không dùng singleton)
- Lấy URL từ `Program.Configuration["URL_..."]`
- Serialize: `JsonConvert.SerializeObject(request)` → `StringContent(..., "application/json")`
- Gọi `.PostAsync(...).Result` / `.GetAsync(...).Result` (blocking, chạy trong background thread)
- Deserialize: `JsonConvert.DeserializeObject<T>(body)`

**UI threading:**
- Network/IO nặng chạy trong `BackgroundWorker` hoặc `Thread` mới
- Cập nhật UI từ thread nền qua helper `InvokeUI(Action)` (kiểm tra `InvokeRequired` + `Invoke`)
- `SaveFileDialog` yêu cầu STA thread riêng: `new Thread(...)` + `SetApartmentState(ApartmentState.STA)` + `Start()` + `Join()`

**Xử lý lỗi:**
- `try/catch` bao quanh block network và file IO
- Hiển thị lỗi bằng `MessageBox.Show(...)` (tiếng Việt) hoặc cập nhật label qua `SetStatus(...)`

## Kiến trúc: BMAD / .claude Tooling

- `_bmad/` — Nguồn định nghĩa BMAD skill, template và Python script. Cấu trúc theo stage: `bmm/1-analysis`, `bmm/2-plan-workflows`, `bmm/3-solutioning`, `bmm/4-implementation`.
- `_bmad-output/` — Thư mục output được tạo bởi BMAD workflows.
- `.claude/skills/` — Bản **deploy/mirror** của BMAD skill docs từ `_bmad/`. Chứa cả `SKILL.md`, templates, và bản sao scripts/tests (ví dụ `test_analyze_sources.py` xuất hiện cả ở `_bmad/core/bmad-distillator/scripts/tests/` lẫn `.claude/skills/bmad-distillator/scripts/tests/`). Khi sửa skill, sửa nguồn ở `_bmad/` rồi sync sang `.claude/skills/`.
- `.claude/settings.json` — Bật plugin `misa-speckit@misa-plugins`.
- `docs/` — Tài liệu dự án bổ sung.

**`analyze_sources.py` SKIP_DIRS** (các thư mục bị bỏ qua khi scan):
```python
{"node_modules", ".git", "__pycache__", ".venv", "venv", ".claude", "_bmad-output", ".cursor", ".vscode"}
```
Lưu ý: `bin/`, `obj/`, `.vs/` **không** nằm trong danh sách skip mặc định — cần cẩn thận khi chạy distillator trên toàn repo.
