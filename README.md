# Two-Factor Authentication Is Fun (TwoIF)
A desktop-based application that simply generates one-time passwords (much like Google Authenticator does). Support is currently limited to TOTP (Time-based  one-time password) and HOTP (HMAC-based (counter) one-time password) codes - no support for Google Authenticator-like push notification approvals.

# UI
Currently the UI is based on Windows Forms, which is not the best for portability.

# Usage
Clicking anywhere in the main TwoIF client will copy the currently displayed code to the clipboard. The hamburger button will load the token manager window where tokens can be added/removed.

# Build
Compiled & tested in x86-64 Windows 10/11. In Visual Studio, the program cam be compiled and "Published" to a single file.

# Token Database Store
The OTP token database is encrypted using AES-256 and stored in `%APPDATA%\TwoIFClient\database.dat`. The encryption roots itself on a user-specified password, which derives the AES encryption key. Future development efforts will use the TSS.NET (from Microsoft's TSS.MSR project) package to also tie the encryption to the local machine's TPM chip to reduce the vulnerability to offline attacks on the token database.

# Adding Tokens
Tokens can be added three ways:
1. Using a QR code uploaded into the program as an image file (e.g. a screenshot of the QR code).
2. Using an OTP URI, in the form `otpauth://TYPE/LABEL?PARAMETERS`, e.g., `otpauth://totp/ACME:bugs.bunny%40example.com?secret=ABCDEFGHIJKLMNOPQRSTUVWXYZ&issuer=ACME`
3. Manually entering the "Secret" field and the other relevant parameters.

# Future Work
-Integrate TPM support using TSS.NET (from Microsoft's TSS.MSR project) in order to reduce the vulnerability to offline attacks on the token database.
-Port the UI to other platforms. Avalonia UI (https://avaloniaui.net/) is under consideration, as is Mono, etc.