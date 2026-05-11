using OnlineCourses.Application.Interfaces.Services;

namespace OnlineCourses.Application.Services;

public class EmailTemplateService
    : IEmailTemplateService
{
    public string GetResetPasswordTemplate(
        string username,
        string code)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='UTF-8'>
<meta name='viewport' content='width=device-width, initial-scale=1.0'>
<style>
    body {{
        margin: 0;
        padding: 0;
        background: #f2f4f8;
        font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Arial, sans-serif;
    }}

    .container {{
        max-width: 480px;
        margin: 40px auto;
        background: #ffffff;
        border-radius: 16px;
        overflow: hidden;
        box-shadow: 0 10px 30px rgba(0,0,0,0.08);
    }}

    .header {{
        background: linear-gradient(135deg, #4f46e5, #7c3aed);
        padding: 30px;
        text-align: center;
    }}

    .header h1 {{
        color: white;
        margin: 0;
        font-size: 22px;
        letter-spacing: 0.5px;
    }}

    .body {{
        padding: 30px;
        text-align: center;
    }}

    .body p {{
        color: #444;
        font-size: 15px;
        line-height: 1.6;
        margin: 10px 0;
    }}

    .code-box {{
        margin: 25px 0;
    }}

    .code {{
        display: inline-block;
        font-size: 34px;
        font-weight: bold;
        color: #4f46e5;
        background: #f5f7ff;
        padding: 14px 30px;
        border-radius: 10px;
        letter-spacing: 10px;
        border: 1px solid #e0e7ff;
    }}

    .button {{
        display: inline-block;
        margin-top: 20px;
        padding: 12px 25px;
        background: #4f46e5;
        color: white;
        text-decoration: none;
        border-radius: 8px;
        font-size: 14px;
    }}

    .warning {{
        color: #888;
        font-size: 13px;
        margin-top: 15px;
    }}

    .footer {{
        background: #fafafa;
        padding: 15px;
        text-align: center;
        color: #aaa;
        font-size: 12px;
        border-top: 1px solid #eee;
    }}

    @media (max-width: 500px) {{
        .container {{
            margin: 20px;
        }}
    }}
</style>
</head>

<body>
    <div class='container'>
        <div class='header'>
            <h1>🔐 Password Reset</h1>
        </div>

        <div class='body'>
            <p>Hello, <strong>{username}</strong></p>
            <p>We received a request to reset your password.</p>

            <div class='code-box'>
                <div class='code'>{code}</div>
            </div>

            <p>If the code doesn’t work, copy it manually.</p>

            <p class='warning'>⏱ This code will expire in <strong>3 minutes</strong>.</p>
            <p class='warning'>If you didn’t request this, you can safely ignore this email.</p>
        </div>

        <div class='footer'>
            © 2026 OnlineCourses — Secure Authentication System
        </div>
    </div>
</body>
</html>";
    }
}