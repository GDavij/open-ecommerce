using Azure.Storage.Blobs;
using Core.Modules.HumanResources.Domain.Constants;
using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.JobApplications.GetJobApplication;
using Core.Modules.HumanResources.Domain.Exceptions.JobApplication;
using Core.Modules.HumanResources.Domain.Helpers;
using Core.Modules.Shared.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Queries.JobApplications.GetJobApplication;

internal class GetJobApplicationQueryHandler : IGetJobApplicationQueryHandler
{
    private readonly IHumanResourcesContext _dbContext;
    private readonly BlobServiceClient _blobClient;
    
    public GetJobApplicationQueryHandler(IHumanResourcesContext dbContext, BlobServiceClient blobClient)
    {
        _dbContext = dbContext;
        _blobClient = blobClient;
    }

    public async Task<GetJobApplicationQueryResponse> Handle(GetJobApplicationQuery request, CancellationToken cancellationToken)
    {
        var jobApplication = await _dbContext.JobApplications
            .Include(x => x.SocialLinks)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (jobApplication is null)
        {
            throw new InvalidJobApplicationException(request.Id);
        }

        var containerClient = _blobClient.GetBlobContainerClient(AzureBlobStorageContainers.JobApplicationsResumes);
        
        //TODO: Implement Managed Background Temporary File Cache Service
        var blobName = jobApplication.ResumeURL.Split('/').Last();
        var blobClient = containerClient.GetBlobClient(blobName);
        
        var tempFileName = $"{Guid.NewGuid()}_{blobName}";
        var filePath = TemporaryFileFolders.JobApplicationFolder.AddToPathFile(tempFileName);

        await using (var fileStream = File.OpenWrite(filePath))
        {
            await blobClient.DownloadToAsync(fileStream, cancellationToken);
        }
        
        var fileBytes = await File.ReadAllBytesAsync(filePath, cancellationToken);
        // This should be removed when using the Cache Service
        File.Delete(filePath);
        string base64Resume = Convert.ToBase64String(fileBytes);

        return new GetJobApplicationQueryResponse
        {
            Id = jobApplication.Id,
            FullName = $"{jobApplication.FirstName} {jobApplication.LastName}",
            Age = jobApplication.Age,
            Email = jobApplication.Email,
            Phone = jobApplication.Phone,
            ProcessStep = jobApplication.ProcessStep,
            Sector = jobApplication.Sector,
            Resume = base64Resume,
            SocialLinks = jobApplication.SocialLinks.Select(x => new GetJobApplicationQueryResponse.SocialLink
            {
                Id = x.Id,
                SocialMedia = x.SocialMedia,
                URL = x.URL
            }).ToList(),
            SentAt = jobApplication.CreatedAt
        };
    }
}