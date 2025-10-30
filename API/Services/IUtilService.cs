using Microsoft.AspNetCore.Identity;

namespace API_Visitatus.Services
{
    public interface IUtilService
    {
        PasswordVerificationResult VerifyPassword(string hashedPassword, string plainPassword);
        string CriptografarSenha(string senha);
    }
}
