using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PublishUtilityWebApp.Common
{
    /// <summary>
    /// 文件夹操作辅助类
    /// </summary>
    public class FolderHelper
    {
        /// <summary>
        /// 删除掉空文件夹
        /// 所有没有子“文件系统”的都将被删除
        /// </summary>
        /// <param name="storagepath"></param>
        public static void DeleteEmptyDirectory(string storagepath)
        {
            DirectoryInfo dir = new DirectoryInfo(storagepath);
            DirectoryInfo[] subdirs = dir.GetDirectories("*.*", SearchOption.AllDirectories);
            foreach (DirectoryInfo subdir in subdirs)
            {
                FileSystemInfo[] subFiles = subdir.GetFileSystemInfos();
                if (subFiles.Count() == 0)
                {
                    subdir.Delete();
                }
            }
        }
    }
}
