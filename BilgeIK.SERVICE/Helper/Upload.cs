using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeIK.SERVICE.Helper
{
    public class Upload
    {

        public static string ImageUpload(List<IFormFile> files, IHostingEnvironment env, out bool result)
        {
            result = false;
            var uploads = Path.Combine(env.WebRootPath, "uploads");

            foreach (var file in files)
            {
                if (file.ContentType.Contains("image"))
                {
                    if (file.Length <= 2097152)
                    {
                        string uniqueName = $"{Guid.NewGuid().ToString().Replace("-", "_").ToLower()}.{file.ContentType.Split('/')[1]}";

                        // ~/uploads/A8242141-4B77-4A34-ACE2-19BDDAAF9F99.jpg
                        var filePath = Path.Combine(uploads, uniqueName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                            result = true;
                            //return filePath.Substring(filePath.IndexOf("\\uploads\\"));
                            return uniqueName;
                        }
                    }
                    else
                    {
                        return $"2 MB'tan büyük boyutta resim yükleyemezsiniz!";
                    }
                }
                else
                {
                    return $"Lütfen sadece resim dosyası yükleyiniz!";
                }

            }
            // Dosya yoksa ilgili return
            return $"Dosya bulunamadı! Lütfen en az bir tane dosya seçiniz...";
        }

        public static string FileUpload(List<IFormFile> files, IHostingEnvironment env, out bool result)
        {
            result = false;
            var uploads = Path.Combine(env.WebRootPath, "uploads");

            foreach (var file in files)
            {
                var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "tiff" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (supportedTypes.Contains(fileExt))
                {
                    if (file.Length <= 209715200)
                    {
                        string uniqueName = $"{Guid.NewGuid().ToString().Replace("-", "_").ToLower()}.{fileExt}";

                        // ~/uploads/A8242141-4B77-4A34-ACE2-19BDDAAF9F99.jpg
                        var filePath = Path.Combine(uploads, uniqueName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                            result = true;
                            //return filePath.Substring(filePath.IndexOf("\\uploads\\"));
                            return uniqueName;
                        }
                    }
                    else
                    {
                        return $"200 MB'tan büyük boyutta dosya yükleyemezsiniz!";
                    }
                }
                else
                {
                    return $"Lütfen sadece txt, doc, docx, pdf, xls, xlsx, tiff uzantılı dosyası yükleyiniz!";
                }

            }
            // Dosya yoksa ilgili return
            return $"Dosya bulunamadı! Lütfen en az bir tane dosya seçiniz...";
        }

        public static string ImageAndFileUpload(List<IFormFile> files, IHostingEnvironment env, out bool result)
        {
            result = false;
            var uploads = Path.Combine(env.WebRootPath, "uploads");

            foreach (var file in files)
            {
                var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "png", "tiff" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (supportedTypes.Contains(fileExt))
                {
                    if (file.Length <= 209715200)
                    {
                        string uniqueName = $"{Guid.NewGuid().ToString().Replace("-", "_").ToLower()}.{fileExt}";

                        // ~/uploads/A8242141-4B77-4A34-ACE2-19BDDAAF9F99.jpg
                        var filePath = Path.Combine(uploads, uniqueName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                            result = true;
                            //return filePath.Substring(filePath.IndexOf("\\uploads\\"));
                            return uniqueName;
                        }
                    }
                    else
                    {
                        return $"200 MB'tan büyük boyutta dosya yükleyemezsiniz!";
                    }
                }
                else
                {
                    return $"Lütfen sadece txt, doc, docx, pdf, xls, xlsx, tiff, png, jpg uzantılı dosyası yükleyiniz!";
                }

            }
            // Dosya yoksa ilgili return
            return $"Dosya bulunamadı! Lütfen en az bir tane dosya seçiniz...";
        }


    }
}
