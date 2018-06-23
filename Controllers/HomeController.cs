using qiushi.Entity;
using qiushi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static qiushi.Entity.Constant;

namespace qiushi.Web.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            
           var articleBl = new ArticleModel();
            var articles = articleBl.List(0, ArticleStatus.Pass, 1, Constant.numArticlePerPage);

            ViewBag.CurrentPage = 1;
            ViewBag.TotalPage = articles.Item2;

            return View(articles.Item1);

        }

        [HttpGet]
        public ActionResult List(int page)
        {
            var articleBl = new ArticleModel();
            var articles = articleBl.List(0, ArticleStatus.Pass, page, Constant.numArticlePerPage);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = articles.Item2;

            return View("Index", articles.Item1);
        }

        [HttpGet]
        public ActionResult Hot(int page)
        {
            var articleBl = new ArticleModel();
            var articles = articleBl.Hot (0 , page, Constant.numArticlePerPage);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = articles.Item2;

            return View("Index", articles.Item1);
        }

        [HttpGet]
        public ActionResult TravelBack(int page)
        {
            var articleBl = new ArticleModel();
            var articles = articleBl.TravelBack(0, page, Constant.numArticlePerPage);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = articles.Item2;

            return View("Index", articles.Item1);
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var articleBl = new ArticleModel();
            var article = articleBl.Find(id);

            return View(article);
        }


        public ActionResult ListArticle(int userId, int page)
        {
            var articleBl = new ArticleModel();
            var articlesTuple = articleBl.List(userId, Constant.ArticleStatus.Pass, page, Constant.numArticlePerPage);
            var articles = articlesTuple.Item1;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = articlesTuple.Item2;

            return PartialView("Index", articles);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Mian()
        {
            return View();
        }
    }
}