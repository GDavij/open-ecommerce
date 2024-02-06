namespace Core.Modules.HumanResources.Domain.Constants;

internal static class TemporaryFileFolders
{
   private const string ApplicationFolderName = "open-ecommerce-file-cache";

   public static readonly string JobApplicationFolder = Path.Join(Path.GetTempPath(), ApplicationFolderName, "job-applications");
}