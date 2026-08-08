using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DVLD_PresentationLayer.Global_Classes
{
    static public class util
    {

        static public BitmapImage _LoadImageSafely(string imagePath)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad; // يقفل الملف فور الانتهاء من القراءة في الذاكرة
            bitmap.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            bitmap.Freeze(); // يجعل الصورة خفيفة وقابلة للاستخدام عبر الخيوط (Threads)
            return bitmap;
        }

        static public string GenerateGUID()
        {
            Guid NewGuid = Guid.NewGuid(); 
            
            return NewGuid.ToString(); 
        }

        static public bool CreateFolderIfNotExists(string FolderPath) 
        {
            if (!File.Exists(FolderPath))
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);
                    return true; 

                } catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder: " + ex.Message); 
                    return false; 
                }
            }

            return true;  
        }

        static string ReplaceFileNameWithGUID(string SourceFile) 
        {
            string Extension = System.IO.Path.GetExtension(SourceFile);

            string NewFileName = GenerateGUID() + Extension;

            return NewFileName; 
        }

        static bool CopyImageToImagesFolder(ref string SourceFile)
        {

            string ImagesFolder = @"D:\C#\Course 19 DLD\DVLD\DVLD_PresentationLayer\Images\PeopleImages\";

            if (!CreateFolderIfNotExists(ImagesFolder)) return false; 

            string DestinationFile = ImagesFolder + ReplaceFileNameWithGUID(SourceFile);

            try
            {
                File.Copy(SourceFile, DestinationFile, true); 
            } catch(IOException iox) 
            {
                MessageBox.Show(iox.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
                // log the error later; 
            }

            SourceFile = DestinationFile; 
            return true; 
        }
    }
}
