﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PublishUtilityWebApp.Common
{
    /// <summary>
    /// 文件夹操作辅助类
    /// </summary>
    public static class FolderHelper
    {
        /// <summary>
        /// 删除掉空文件夹
        /// 所有没有子“文件系统”的都将被删除
        /// </summary>
        /// <param name="storagepath"></param>
        private static void DeleteEmptyDirectory(string storagepath)
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
        /// <summary>
        /// 获取客户Ip
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientUserIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }


        public static void DeleteAllNonRelevantFiles(string path, IList<string> reservedFiles)
        {
            DeleteFiles(path, reservedFiles);
            DeleteEmptyDirectory(path);
        }


        /// <summary>
        /// 用于获取指定路径（path）下的文件名中包含关键词的文件全名列表
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="searchKey">关键词</param>
        /// <param name="lRet">文件名中包含关键词的文件全名列表</param>
        public static void GetAllFileByKeyWord(string path, string searchKey, IList<string> lRet)
        {
            DirectoryInfo theFolder = new DirectoryInfo(@path);

            //遍历文件
            foreach (FileInfo NextFile in theFolder.GetFiles())
            {
                if (NextFile.Name.Contains(searchKey.Trim()))
                {
                    lRet.Add(NextFile.FullName);
                }
            }

            //遍历文件夹
            foreach (DirectoryInfo NextFolder in theFolder.GetDirectories())
            {
                GetAllFileByKeyWord(NextFolder.FullName, searchKey, lRet);
            }
        }

        private static void DeleteFiles(string path, IList<string> reservedFiles)
        {

            DirectoryInfo theFolder = new DirectoryInfo(@path);

            //遍历文件
            foreach (FileInfo NextFile in theFolder.GetFiles())
            {
                if (!reservedFiles.Contains(NextFile.FullName))
                {
                    File.Delete(NextFile.FullName);
                }
            }

            //遍历文件夹
            foreach (DirectoryInfo NextFolder in theFolder.GetDirectories())
            {
                DeleteFiles(NextFolder.FullName, reservedFiles);
            }
        }



        #region 判断远程文件是否存在
        /// <summary>
        /// 判断远程文件是否存在
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        public static string RemoteFileExists(string fileUrl)
        {
            HttpWebRequest re = null;
            HttpWebResponse res = null;
            try
            {
                re = (HttpWebRequest)WebRequest.Create(fileUrl);
                res = (HttpWebResponse)re.GetResponse();
                if (res.ContentLength != 0)
                {
                    //MessageBox.Show("文件存在");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("无此文件");
                return ex.ToString();
            }
            finally
            {
                if (re != null)
                {
                    re.Abort();//销毁关闭连接
                }
                if (res != null)
                {
                    res.Close();//销毁关闭响应
                }
            }
            return "目录或文件不存在";
        }
        #endregion
    }
}
