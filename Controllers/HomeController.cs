using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublishUtilityWebApp.Common;
using PublishUtilityWebApp.Models;

namespace PublishUtilityWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessForPublish(string sourceDir, IList<string> reservedFiles)
        {
            try
            {
                string msg = string.Empty;//默认成功
                var rootPath = string.Format(@"\\{0}\{1}", HttpContext.GetClientUserIp(), sourceDir);
                rootPath = sourceDir;
                msg = CheckSourceDirExists(rootPath, msg);
                if (string.IsNullOrWhiteSpace(msg))//源目录存在
                {
                    if (reservedFiles.Any())
                    {
                        FolderHelper.DeleteAllNonRelevantFiles(rootPath, reservedFiles);
                    }
                    else
                    {
                        msg = "保存失败：保留区中并无可保留文件。";
                    }
                }
                return Json(new
                {
                    bRet = string.IsNullOrWhiteSpace(msg),
                    msg,
                    data = string.IsNullOrWhiteSpace(msg) ? "保存成功" : null
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    bRet = false,
                    msg=ex.ToString(),
                    data = "保存失败"
                });
            }
        }

        [HttpPost]
        public IActionResult DoSearchForPublish(string sourceDir,string searchKey)
        {
            try
            {
                var lRet = new List<string>();
                string msg = string.Empty;//默认成功
                var rootPath = string.Format(@"\\{0}\{1}", HttpContext.GetClientUserIp(), sourceDir);
                rootPath = sourceDir;
                msg = CheckSourceDirExists(rootPath, msg);
                if (string.IsNullOrWhiteSpace(msg))//源目录存在
                {
                    if (!string.IsNullOrWhiteSpace(searchKey))
                    {
                        //在源目录中查找所有这样的文件（需要包含文件目录）：其文件名中含有搜索关键字
                        FolderHelper.GetAllFileByKeyWord(rootPath, searchKey, lRet);
                    }
                    else
                    {
                        msg = "请先输入文件名关键字！";
                    }
                }
                return Json(new
                {
                    bRet = string.IsNullOrWhiteSpace(msg),
                    msg,
                    data = string.IsNullOrWhiteSpace(msg) ? lRet : null
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    bRet = false,
                    msg = ex.ToString(),
                    data = "搜索失败"
                });
            }
        }

        private static string CheckSourceDirExists(string sourceDir, string msg)
        {
            if (!Directory.Exists(sourceDir))
            {
                msg = "源目录不存在";
            }

            return msg;
            //return FolderHelper.RemoteFileExists(sourceDir);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
