using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Areas.PrivateSite.Controllers
{
    [CustomAuthentication]
    [CustomAuthorize(Roles = "quản trị, quản lý, nhân viên")]
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}