using Microsoft.Extensions.Hosting;

namespace AppEventos.API.Helpers
{
    public class Util : IUtil
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public Util(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public void DeleteImage(string imageName, string folderName)
        {
            if(!string.IsNullOrEmpty(imageName))
            {
                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources/{folderName}", imageName);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }
        }

        public async Task<string> SaveImage(IFormFile image, string folderName)
        {
            var imageName = new string(Path.GetFileNameWithoutExtension(image.FileName)
                                        .Take(10)
                                        .ToArray())
                                        .Replace(' ', '-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymssfff")}{Path.GetExtension(image.FileName)}";
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources/{folderName}", imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return imageName;
        }
    }
}
