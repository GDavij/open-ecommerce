using System;
using System.Threading.Tasks;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Core.Modules.UserAccess.Tests.Services.SecurityService;

public class EncryptionTests
{
    private readonly IAppConfigService _appConfigService;
    private readonly ISecurityService _securityService;

    public EncryptionTests()
    {
        _appConfigService = Substitute.For<IAppConfigService>();
        _securityService = new Application.Services.SecurityService(_appConfigService);
    }

    [Theory]
    [InlineData("secret-unit-test-password")]
    [InlineData("RFskX23!2w#c9rwWbVanNk4#g7QM%TVfvWTu$j6CR%nSBthweB#gGW6R4mCpCnk@T&9oq5Eo#TcgDp^maQiLwG$FKBcUXLR7LMCeX#bUUH4cHK7m2mEnNLYC@5#Dxo&w")]
    [InlineData("WtqCKru7kxXTSiRrkw6rNY4JeJGBJXWp97DK6WvMnawG*uUwM%NMtT&83vYLGdCoL7neB3$sg!ch@K$EuJ@n6B#pATa6UdPTJZ3RSHnxHcMU$*vn!LMNUw^W*wup7TNK")]
    [InlineData("enclosure-sleek-scuba-tile-marbling-eardrum-awning9-coat-devious-strut-tannery-mystify-handprint-gracious-anchor-violate-ounce-dropper-epidural-urgency")]
    [InlineData("Obvious@Unruly@Purist@Chewable@Revisit@Ooze@Preformed@Flask@Idealism@Swerve@Living@Valium9@Obscure@Saffron@Dealmaker@Boaster@Alike@Subpanel@Caliber@Clarify")]
    internal async Task ShouldEncodeAndDecodeBase64PasswordWithZeroLoss(string password)
    {
        //Arrange
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";
        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");

        byte[] userSecretKey = _securityService.GenerateSecurityKey();
        byte[] derivedPassphrase = await _securityService.DerivePassword(password, userSecretKey, default);
        
        //Act
        var encodedBase64Token = Convert.ToBase64String(derivedPassphrase);

        byte[] decodedBase64Token = Convert.FromBase64String(encodedBase64Token);
        
        //Assert
        decodedBase64Token
            .Should()
            .BeEquivalentTo(derivedPassphrase);
    }
}