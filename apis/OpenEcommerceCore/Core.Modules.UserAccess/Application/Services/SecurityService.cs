using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Models;
using JWT.Algorithms;
using JWT.Builder;

namespace Core.Modules.UserAccess.Application.Services;

internal class SecurityService : ISecurityService
{
    private readonly IAppConfigService _appConfigService;

    public SecurityService(IAppConfigService appConfigService)
    {
        _appConfigService = appConfigService;
    }

    public Task<byte[]> DerivePassword(string password, byte[] userSecurityKey, CancellationToken cancellationToken)
    {
        // Refactor this Method
        return Task.Run(() =>
        {

            string systemSecurityKeyEnvironmentVariableName = "USER_ACCESS_DERIVATION_SECURITY_KEY";
            string systemSecurityKey =
                _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName);

            byte[] systemSecurityKeyBytes = Encoding.UTF8.GetBytes(systemSecurityKey);

            byte[] saltBytes = AppendBytes(userSecurityKey, systemSecurityKeyBytes);

            const int derivedPasswordLength = 512;

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            byte[] derivedPassword = Rfc2898DeriveBytes.Pbkdf2(
                passwordBytes,
                saltBytes,
                10000,
                HashAlgorithmName.SHA512,
                derivedPasswordLength);

            return derivedPassword;
        }, cancellationToken);
    }


    public byte[] GenerateSecurityKey()
    {
        var randomNumberGenerator = RandomNumberGenerator.Create();

        const int securityKeySize = 512;

        byte[] securityKey = new byte[securityKeySize];

        randomNumberGenerator.GetBytes(securityKey, 0, securityKeySize);
        return securityKey;
    }

    public string EncodeToken(Token token)
    {
        string secretValidationKey = _appConfigService.GetEnvironmentVariable("USER_ACCESS_JWT_SECURITY_KEY");

        return JwtBuilder.Create()
            .WithAlgorithm(new HMACSHA512Algorithm())
            .WithSecret(secretValidationKey)
            .Encode(token);
    }

    public bool TryParseEncodedToken(string encodedToken, out Token token)
    {
        string secretValidationKey = _appConfigService.GetEnvironmentVariable("USER_ACCESS_JWT_SECURITY_KEY");

        try
        {
            token = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA512Algorithm())
                .WithSecret(secretValidationKey)
                .Decode<Token>(encodedToken);

            return true;
        }
        catch (Exception ex)
        {
            token = null!;
            return false;
        }
    }

    private byte[] AppendBytes(byte[] array, byte[] bytesToAppend)
    {
        int totalSize = array.Length + bytesToAppend.Length;
        byte[] appendedArray = new byte[totalSize];
        Buffer.BlockCopy(array, 0, appendedArray, 0, array.Length);
        Buffer.BlockCopy(bytesToAppend, 0, appendedArray, array.Length, bytesToAppend.Length);

        return appendedArray;
    }
}