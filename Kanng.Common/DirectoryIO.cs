using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kanng.Common
{
    public class DirectoryIO
    {

        public static void CopyFolder(string from, string to)
        {
            if (!Directory.Exists(to))
                Directory.CreateDirectory(to);

            // 子文件夹
            foreach (string sub in Directory.GetDirectories(from))
            CopyFolder(sub + "\\", to + Path.GetFileName(sub) + "\\");

            // 文件
            foreach (string file in Directory.GetFiles(from))
            File.Copy(file, to + Path.GetFileName(file), true);
           
        }


        public static void UpdateFolderName(string from, string to)
        {
            if (!Directory.Exists(from))
            {
                Directory.Move(from, to);
            }         
        }
    }
}
