using API_Visitatus.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IConfiguration configuration)
    {
        _smtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
    }

    public async Task EnviarEmailsEmMassaAsync(List<string> destinatarios, string assunto, string caminhoTemplate, Dictionary<string, string> placeholders)
    {
        var htmlCorpo = await CarregarTemplateEmailAsync(caminhoTemplate, placeholders);

        foreach (var destinatario in destinatarios)
        {
            await EnviarEmailAsync(destinatario, assunto, htmlCorpo);
        }
    }

    public async Task EnviarEmailAsync(string destinatario, string assunto, string htmlCorpo)
    {
        using var client = new SmtpClient
        {
            Host = _smtpSettings.Host!,
            Port = _smtpSettings.Port,
            EnableSsl = true,
            Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password),
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        // Desabilitar validação de certificado (somente se necessário)
        //ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, sslPolicyErrors) => true;

        var mensagem = new MailMessage
        {
            From = new MailAddress(_smtpSettings.UserName!),
            Subject = assunto,
            Body = htmlCorpo,
            IsBodyHtml = true
        };

        mensagem.To.Add(destinatario);

        try
        {
            await client.SendMailAsync(mensagem);
            Console.WriteLine($"E-mail enviado para: {destinatario}");
        }
        catch (SmtpException ex)
        {
            Console.WriteLine($"Erro ao enviar e-mail para {destinatario}: {ex.Message}");
            throw;
        }
    }

    public async Task<string> CarregarTemplateEmailAsync(string caminhoTemplate, Dictionary<string, string> placeholders)
    {
        // Simula carregamento do arquivo do template
        var template = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <h1>Olá, {{Nome}}!</h1>\r\n    <p>{{Mensagem}}</p>\r\n</body>\r\n</html>";

        foreach (var placeholder in placeholders)
        {
            template = template.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
        }

        return template;
    }
}
