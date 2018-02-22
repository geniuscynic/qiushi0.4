using qiushi.Entity;
using qiushi.Model;
using qiushi.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace qiushi.Web.Controllers
{
    public class CommentController : BaseController
    {
        // GET: Commment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetComments(int id)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            CommentModel commentModel = new CommentModel();
            var discuss = commentModel.GetComments2(id).ToList();
            sw.Stop();

            //System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            //sw1.Start();
            //CommentModel commentModel1 = new CommentModel();
            //var discuss2 = commentModel1.GetComments(id).ToList();
            //sw1.Stop();

          
            // Console.WriteLine(sw.ElapsedMilliseconds);


            // Console.WriteLine(sw1.ElapsedMilliseconds);

            //var a = sw1.ElapsedMilliseconds - sw.ElapsedMilliseconds;
            ViewBag.ArticleId = id;

            return PartialView("comments", discuss);
        }

        [MyAuthen]
        [HttpPost]
        public async Task<ActionResult> SaveComments(FormCollection form)
        {
            var articleId = int.Parse(form["articleId"]);
            var commentid = int.Parse(form["commentId"]);
            var content = form["content"];


            var comment = new CommentEntity()
            {
                ArticleId = articleId,
                Content = content,
                Floor = 0,
                UpdateBy = CurrentUser.Id,
                GoodNum = 0,
                BadNum = 0

            };

           // comment.InitializeCommonField();


            CommentModel commentModel = new CommentModel();
            await commentModel.SaveComments(comment, commentid);

            return Content("Successfully");
        }

        [MyAuthen]
        public ActionResult List(int page)
        {
          
            CommentModel commentModel = new CommentModel();
            var discuss = commentModel.List(CurrentUser.Id, page, Constant.numArticlePerPage);

            ViewBag.TotalPage = discuss.Item2;
            ViewBag.CurrentPage = page;
            return View("List", discuss.Item1);
        }


        [MyAuthen]
        public ActionResult Answer(int page)
        {

            CommentModel commentModel = new CommentModel();
            var discuss = commentModel.Answer(CurrentUser.Id, page, Constant.numArticlePerPage);

            ViewBag.TotalPage = discuss.Item2;
            ViewBag.CurrentPage = page;
            return View("Answer", discuss.Item1);
        }
    }
}