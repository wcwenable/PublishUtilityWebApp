﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PublishUtilityWebApp.Models;

namespace PublishUtilityWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult ProcessForPublish(string sourceDir)
        {
            string msg = string.Empty;//默认成功
            msg = CheckSourceDirExists(sourceDir, msg);
            if (string.IsNullOrWhiteSpace(msg))//源目录存在
            {
                ViewBag.sourceDir = sourceDir;   
            }
            return View();
        }

        [HttpPost]
        public IActionResult DoSearchForPublish(string sourceDir,string searchKey)
        {
            string msg = string.Empty;//默认成功
            msg = CheckSourceDirExists(sourceDir, msg);
            if (string.IsNullOrWhiteSpace(msg))//源目录存在
            {
                //todo:在源目录中查找所有这样的文件（需要包含文件目录）：其文件名中含有搜索关键字
            }
            return Content(msg);
        }

        private static string CheckSourceDirExists(string sourceDir, string msg)
        {
            if (!Directory.Exists(sourceDir))
            {
                msg = "源目录不存在";
            }

            return msg;
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
