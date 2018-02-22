using qiushi.Task;
using qiushi.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace qiushi.Web.Controllers
{
    [MyAuthen]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Fetch()
        {
            QiushiCurlTask task = new QiushiCurlTask();

            var content =  await task.GetContent();

            return Content(content.ToString());
        }
    }
}