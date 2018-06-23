using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using qiushi.Entity;
using qiushi.Model;
using qiushi.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace qiushi.Web.Controllers
{
    public class GardenController : Controller
    {
        // GET: Garden
        public ActionResult Index()
        {
            var gardebBL = new GardebModel();
            var gardens = gardebBL.List(1, Constant.numArticlePerPage);

            ViewBag.CurrentPage = 1;
            ViewBag.TotalPage = gardens.Item2;

            return View("Index", gardens.Item1);
        }

        public ActionResult List(int page)
        {
            var gardebBL = new GardebModel();
            var gardens = gardebBL.List(page, Constant.numArticlePerPage);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = gardens.Item2;

            return View("Index", gardens.Item1);
        }

        public async Task<ActionResult> Detail(int id, int page)
        {
            var gardebBL = new GardebModel();

            var garden = await gardebBL.Detail(id, page, 1);

            if(garden == null)
            {
                var garden1 = new GardenEntity() {
                     Attachments= new List<GardenAttachmentEntity>(),
                     Content = "不存在的帖子"
                };
            }

            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = garden.Item2;

            return View(garden.Item1);
        }

        public async Task<ActionResult> Save(string json)
        {
            var garden = JsonConvert.DeserializeObject<GardenEntity>(json);
            garden.Hash = CurlTask.GetHash(garden.Content);
            garden.UpdateBy = 2;

            foreach(var attachment in garden.Attachments)
            {
                attachment.UpdateBy = 2;
            }

            var gardebModel = new GardebModel();
            var result =  await gardebModel.Add(garden);

            return Content(result.ToString());
        }

        public async Task<ActionResult> Check(string json)
        {
           
            string hash = CurlTask.GetHash(json);

            var gardebModel = new GardebModel();
            var result = await gardebModel.Check(hash);
            if(result)
            {
                return Content("Yes");
            }
            return Content("No");
        }


    }
}