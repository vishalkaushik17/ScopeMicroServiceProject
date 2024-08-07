
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace GenericFunction
{

    public static class FileExtensionMethods
    {
        public static string GetDisplayNameOfEnum(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .GetName();
        }

        /// <summary>
        /// Upload file to the given path
        /// </summary>
        /// <param name="filename">IFormFile extension parameter</param>
        /// <param name="path">Path where to store</param>
        /// <param name="subfolder"></param>
        /// <returns>Return null if fails, return full name of the newly save file including path.</returns>
        public static string UploadFile(this IFormFile filename, string path, string subfolder)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filename.FileName) || string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(subfolder))
                    return string.Empty;
                string uploadPath = Path.Combine(path, subfolder);
                string newFilename = Guid.NewGuid().ToString() + filename;
                string filePath = Path.Combine(uploadPath, newFilename);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    filename.CopyTo(fileStream);

                }
                return subfolder + newFilename;

            }
            catch (Exception)
            {
                return string.Empty;
            }


        }


    }
}