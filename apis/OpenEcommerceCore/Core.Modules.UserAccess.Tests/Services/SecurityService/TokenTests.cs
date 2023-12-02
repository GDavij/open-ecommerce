using System;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Models;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Core.Modules.UserAccess.Tests.Services.SecurityService;

public class TokenTests
{
    private readonly IAppConfigService _appConfigService;
    private readonly ISecurityService _securityService;

    public TokenTests()
    {
        _appConfigService = Substitute.For<IAppConfigService>();
        _securityService = new Application.Services.SecurityService(_appConfigService);
    }
    
    [Theory]
    [InlineData(ETokenType.Client)]
    [InlineData(ETokenType.Collaborator)]
    internal void ShouldEncodeAndDecodeTokenWithSuccessWithAnyUserType(ETokenType tokenType)
    {
        // Arrange
        DateTimeOffset mockedIntantTime = DateTimeOffset.UtcNow;
        
        Guid mockedGuid = Guid.NewGuid();
        byte[] mockedPassword = new byte[] { 24, 0, 4, 3, 9, 10, 3, 255, 255, 128, 64, 32, 16 };// Not Encrypted Just For Tests

        long expirationDate = mockedIntantTime.AddDays(1).ToUnixTimeMilliseconds();
        Token token = Token.Create(
            mockedGuid,
            mockedPassword,
            tokenType,
            expirationDate);
        
        // Mock EnvironmentVariables 
        string jwtSecurityKeyName = "user_access::JWT_SECURITY_KEY";
        
        _appConfigService.GetEnvironmentVariable(jwtSecurityKeyName)
            .Returns("jwt-secret-unit-tests-password-key");
        
        // Act
        string encodedToken = _securityService.EncodeToken(token);

        Token parsedToken;
        bool isTokenParsed = _securityService.TryParseEncodedToken(encodedToken, out parsedToken);
        
        // Assert
        isTokenParsed
            .Should()
            .BeTrue();

        parsedToken.Id
            .Should()
            .Be(mockedGuid);

        string expectedPassword = Convert.ToBase64String(mockedPassword);
        parsedToken.Password
            .Should()
            .Be(expectedPassword);

        parsedToken.Exp
            .Should()
            .Be(expirationDate);
    }
}