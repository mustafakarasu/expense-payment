using Microsoft.AspNetCore.Http;

namespace Business.Utilities
{
    public class FileHelper
    {
        private const string FolderResources = "Resources";
        private const string FolderDocuments = "Documents";

        public static string FolderNamePath { get; } = Path.Combine(FolderResources, FolderDocuments);
        public static string PathForSaving { get; } = Path.Combine(Directory.GetCurrentDirectory(), FolderNamePath);

        public static void SaveToPath(IFormFile file, string fileName)
        {
            ExistsAndCreateForDirectory();

            var fullPath = Path.Combine(PathForSaving, fileName);

            using ( var stream = new FileStream(fullPath, FileMode.Create) )
            {
                file.CopyTo(stream);
            }
        }

        private static void ExistsAndCreateForDirectory()
        {
            if ( !Directory.Exists(PathForSaving) )
            {
                Directory.CreateDirectory(PathForSaving);
            }
        }
    }
}
