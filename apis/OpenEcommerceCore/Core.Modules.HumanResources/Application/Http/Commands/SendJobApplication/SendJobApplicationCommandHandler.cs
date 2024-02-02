using Azure.Storage.Blobs;
using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.SendJobApplication;
using Core.Modules.HumanResources.Domain.Entities;
using Core.Modules.HumanResources.Domain.Enums;
using Core.Modules.HumanResources.Domain.Exceptions.JobApplication;
using Core.Modules.Shared.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Commands.SendJobApplication;

internal class SendJobApplicationCommandHandler : ISendJobApplicationCommandHandler
{
    private readonly IHumanResourcesContext _dbContext;
    private readonly BlobServiceClient _blobClient;

    public SendJobApplicationCommandHandler(IHumanResourcesContext dbContext, BlobServiceClient blobClient)
    {
        _dbContext = dbContext;
        _blobClient = blobClient;
    }

    public async Task Handle(SendJobApplicationCommand request, CancellationToken cancellationToken)
    {
        var existentJobApplication = await _dbContext.JobApplications
            .FirstOrDefaultAsync(j => (j.Email == request.Email || j.Phone == request.Phone) && j.Sector == request.Sector, cancellationToken);
        
        if (existentJobApplication is not null)
        {
            throw new AlreadyExistentJobApplicationForSector(request.Sector, request.Email, request.Phone);
        }
        
        var container = _blobClient.GetBlobContainerClient(AzureBlobStorageContainers.JobApplicationsResumes);
        var blobName = $"{Guid.NewGuid()}.pdf";

        await container.UploadBlobAsync(blobName, request.Resume.OpenReadStream(), cancellationToken);

        var jobApplication = new JobApplication
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            Age = request.Age,
            Sector = request.Sector,
            ProcessStep = ApplicationProcess.OnDatabase,
            ResumeURL = $"{container.Uri.AbsoluteUri}/{blobName}",
            CreatedAt = DateTime.UtcNow,
            SocialLinks = request.SocialLinks.Select(s => new SocialLink
            {
                Id = Guid.NewGuid(),
                SocialMedia = s.SocialMedia,
                URL = s.Url
            }).ToList()
        };

        _dbContext.JobApplications.Add(jobApplication);
        
        await _dbContext.SaveChangesAsync();
    }
}