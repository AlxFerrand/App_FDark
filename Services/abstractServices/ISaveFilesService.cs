using App_FDark.Services.concretServices;

namespace App_FDark.Services.abstractServices
{
    public interface ISaveFilesService
    {
        public string SaveFileToTempDirectory(IFormFile file, string newFileName);
        public string SaveFileToImgDirectory(IFormFile file, string newFileName);
        public void DeleteFileToImgDirectory(string fileName);
        public void CleanTempFiles(int id);
        public void CleanStringsFiles(string files);
    }
}
