using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;


namespace Utilities.Helper
{
    public class FileUpload
    {
        public static async Task<string> UploadFile(IFormFile file, string fileName = null, bool useOriginalName = false, string path = null)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            if (string.IsNullOrEmpty(fileName))
            {
                if (useOriginalName)
                {
                    fileName = file.FileName;
                }
                else
                {
                    fileName = Guid.NewGuid().ToString();
                }
            }

            if (!Path.HasExtension(fileName))
            {

                var extension = Path.GetExtension(file.FileName);
                fileName = fileName + extension;
            }

            if (string.IsNullOrEmpty(path))
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/");
            }

            //Create path if not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return path.Replace("wwwroot", "");
        }


        public static async Task<(string path, string ext, string base64)> UploadFileAndReturnBase64(IFormFile file, string fileName = null, bool useOriginalName = false, string path = null)
        {
            if (file == null || file.Length == 0)
            {
                return (null, null, null);
            }

            if (string.IsNullOrEmpty(fileName))
            {
                if (useOriginalName)
                {
                    fileName = file.FileName;
                }
                else
                {
                    fileName = Guid.NewGuid().ToString();
                }
            }
            var ext = "";
            if (!Path.HasExtension(fileName))
            {

                var extension = Path.GetExtension(file.FileName);
                fileName = fileName + extension;
                ext = extension;
            }

            if (string.IsNullOrEmpty(path))
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/");
            }

            //Create path if not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            path = Path.Combine(path, fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            Byte[] bytes = File.ReadAllBytes(path);
            String base_64 = Convert.ToBase64String(bytes);
            return (path: path.Replace("wwwroot", ""), ext, base64: base_64);
        }

        public static async Task<bool> DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            if (File.Exists(filePath))
            {
                await Task.Run(() =>
                {
                    File.Delete(filePath);
                });
                return true;
            }
            return false;
        }

        public static async Task<FileInfo> Base64Upload(string base64Filestring, string folderName, string fileName)
        {
            var file = new FileInfo("");
            var dir = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            byte[] imageBytes = Convert.FromBase64String(base64Filestring);
            var filePath = string.Format("{0}/{1}", dir, fileName);
            File.WriteAllBytes(filePath, imageBytes);

            var fileData = new FileInfo(filePath);

            return await Task.FromResult(fileData);
        }

        public static async Task<IFormFile> GetFormFile(string filePath)
        {
            var webClient = new WebClient();
            byte[] fileByte = webClient.DownloadData(filePath);
            var fileStream = new MemoryStream(fileByte);
            IFormFile newFormFile = new FormFile(fileStream, 0, fileByte.Length, "name", filePath);

            return await Task.FromResult(newFormFile);
        }
    }
}
