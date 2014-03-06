using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;

namespace vt14_2_1_galleriet.Model
{
    public class Gallery
    {
        // Fields
        private static readonly Regex ApprovedExtensions;
        private static string PhysicalUploadedImagesPath;
        private static Regex SanitizePath;

        // Constructor
        static Gallery()
        { 
            ApprovedExtensions = new Regex("(\\.gif|\\.jpg|\\.png)", RegexOptions.IgnoreCase);
            PhysicalUploadedImagesPath = Path.Combine(AppDomain.CurrentDomain.GetData("APPBASE").ToString(), @"Content/Images");
            
            var invalidChars = new string(Path.GetInvalidFileNameChars());
            SanitizePath = new Regex(string.Format("[{0}]", Regex.Escape(invalidChars)));
        }
        
        // Methods
        public IEnumerable<FileInfo> GetImageNames()
        {
            var dirInfo = new DirectoryInfo(PhysicalUploadedImagesPath);
            return dirInfo.GetFiles();
        }

        static bool ImageExists(string name)
        {
            var dirInfo = new DirectoryInfo(PhysicalUploadedImagesPath);
            var fileInfo = dirInfo.GetFiles();

            foreach (var i in fileInfo)
            {
                if (name == i.Name)
                    return true;
            }
            return false;
        }

        // Check for correct MIME-type
        private bool IsValidImage(System.Drawing.Image image)
        {
            if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Gif.Guid
                || image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid
                || image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Png.Guid)
                return true;
            return false;
        }

        public string SaveImage(Stream stream, string fileName)
        {
            var image = System.Drawing.Image.FromStream(stream);

            fileName = SanitizePath.Replace(fileName, "_");

            // Check validity of upload and creation of image and thumbnail
            if (IsValidImage(image) && ApprovedExtensions.IsMatch(Path.GetExtension(fileName)))
            {
                int index = 1;
                
                while (ImageExists(fileName))
                {
                    string name = Path.GetFileNameWithoutExtension(fileName);

                    if (index > 1)
                        name = name.Substring(0, name.Length - 3);

                    fileName = String.Format("{0}(" + index + "){1}", name, Path.GetExtension(fileName));
                    index++;
                }

                image.Save(Path.Combine(PhysicalUploadedImagesPath, fileName));

                System.Drawing.Image thumbImage = image.GetThumbnailImage(99, 66, null, new System.IntPtr());
                thumbImage.Save(String.Format("{0}/Thumbnails/{1}",PhysicalUploadedImagesPath, fileName));
            }
            else
            {
                throw new ArgumentException();
            }

            return fileName;
        }
    }
}