namespace Core.Modules.HumanResources.Domain.Helpers;

internal static class AddFileToPathHelper
{
    public static string AddToPathFile(this string path, string fileName)
    {
        ReadOnlySpan<string> paths = path.Split(Path.PathSeparator);
        Span<string> newPath = new string[paths.Length + 1];

        for (int i = 0; i < paths.Length; i++)
        {
            newPath[i] = paths[i];
        }

        newPath[^1] = fileName;
        return Path.Join(newPath.ToArray());
    }
}