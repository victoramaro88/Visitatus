using Microsoft.AspNetCore.Mvc;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> EnviarEmails([FromBody] List<string> destinatarios)
        {
            var caminhoTemplate = "Templates/EmailTemplate.html";
            var placeholders = new Dictionary<string, string>
                {
                    { "Nome", "Usuário" },
                    { "Mensagem", "Bem-vindo ao nosso serviço!" }
                };

            await _emailService.EnviarEmailsEmMassaAsync(destinatarios, "Bem-vindo!", caminhoTemplate, placeholders);

            return Ok("E-mails enviados!");
        }
    }
}
