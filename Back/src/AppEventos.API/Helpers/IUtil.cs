namespace AppEventos.API.Helpers
{
    public interface IUtil
    {
        Task<string> SaveImage(IFormFile image, string folderName);
        void DeleteImage(string imageName, string folderName);
    }
}
