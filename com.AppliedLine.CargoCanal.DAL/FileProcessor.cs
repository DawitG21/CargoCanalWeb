using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Security.AccessControl;

namespace com.AppliedLine.CargoCanal.DAL
{
    public class FileProcessor
    {
        public static string GetFileBase64String(string path)
        {
            byte[] imageByte = null;
            Image im = Image.FromFile(path);

            using (MemoryStream ms = new MemoryStream())
            {
                im.Save(ms, im.RawFormat);
                im.Dispose();
                imageByte = ms.ToArray(); // what's in the byte
                return Convert.ToBase64String(imageByte);
            }
        }

        public static byte[] GetFileByte(string path)
        {
            byte[] file = File.ReadAllBytes(path);
            return file;
        }

        public static void DeleteFileOnDisc(string filepath)
        {
            if (string.IsNullOrEmpty(filepath)) return; // no file to delete

            if (File.Exists(filepath)) File.Delete(filepath);
        }

        private static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                var writeData = new FileSystemAccessRule("everyone", FileSystemRights.WriteData, AccessControlType.Allow);
                var createFile = new FileSystemAccessRule("everyone", FileSystemRights.CreateFiles, AccessControlType.Allow);
                var createDir = new FileSystemAccessRule("everyone", FileSystemRights.CreateDirectories, AccessControlType.Allow);
                var executeDeny = new FileSystemAccessRule("everyone", FileSystemRights.ExecuteFile, AccessControlType.Deny);

                var dirSecurity = new System.Security.AccessControl.DirectorySecurity();
                dirSecurity.AddAccessRule(createFile);
                dirSecurity.AddAccessRule(createDir);
                dirSecurity.AddAccessRule(writeData);
                dirSecurity.AddAccessRule(executeDeny);
                Directory.CreateDirectory(path, dirSecurity);
            }
        }

        public static Dictionary<string, string> SaveFileOnDisc(string path, System.Net.Http.HttpContent multipartItem, string filename = null)
        {
            string partFilename = multipartItem.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
            int length = (int)multipartItem.Headers.ContentLength;

            string fileExt = partFilename.Substring(partFilename.LastIndexOf('.')).ToLower();
            string oldfilename = partFilename.Substring(0, partFilename.LastIndexOf('.'));
            string newfilename = string.Empty;
            if (oldfilename.Length > 10) oldfilename = oldfilename.Substring(0, 10);

            if (string.IsNullOrEmpty(filename)) newfilename = oldfilename + Guid.NewGuid().ToString() + fileExt;
            else newfilename = filename;

            newfilename = newfilename.Replace(" ", string.Empty);

            // Create directory if it does not exist exempting Execute permission
            CreateDirectory(path);

            // Create a FileStream object to write a stream to a file
            using (FileStream fileStream = File.Create(Path.Combine(path, newfilename), length))
            {
                // Use FileStream object to write bytes[] array to the specified file
                byte[] bt = multipartItem.ReadAsByteArrayAsync().Result;
                fileStream.Write(bt, 0, bt.Length);


                // return filename and base64String
                return new Dictionary<string, string>
                {
                    { newfilename, Convert.ToBase64String(bt) }
                };
            }
        }


        public static Dictionary<string, string> CreateFileFromByteOnDisc(string path, string filename, byte[] data)
        {
            if (!File.Exists(Path.Combine(path, filename)))
            {
                // Create directory if it does not exist exempting Execute permission
                CreateDirectory(path);

                // Create a FileStream object to write a stream to a file
                using (FileStream fileStream = File.Create(Path.Combine(path, filename), data.Length))
                {
                    // Use FileStream object to write bytes[] array to the specified file
                    fileStream.Write(data, 0, data.Length);

                    // return filename and base64String
                    return new Dictionary<string, string>
                    {
                        { filename, Convert.ToBase64String(data) }
                    };
                }

            }
            return null;
        }

        /// <summary>
        /// Save an Image file and returns how many files were saved
        /// </summary>
        /// <param name="path"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public static Dictionary<int, string> SaveFileOnDisc(string path, HttpFileCollection files)
        {
            int count = 0;
            Dictionary<int, string> filesSaved = new Dictionary<int, string>();
            string filename;
            string fileExt;
            string filepath;

            foreach (var item in files)
            {
                Console.Write(item);
            }
            foreach (HttpPostedFile file in files)
            {
                fileExt = Path.GetExtension(file.FileName);
                filename = Guid.NewGuid().ToString() + fileExt;
                filepath = Path.Combine(HttpContext.Current.Server.MapPath(path), filename);
                file.SaveAs(filepath);
                filesSaved.Add(count++, filepath);
            }

            return filesSaved;
        }

        public static FileInfo GetFileInfo(string path, string filename)
        {
            var filepath = Path.Combine(path, filename);
            if (File.Exists(filepath))
            {
                return new FileInfo(filepath);
            }

            return null;
        }
    }
}
