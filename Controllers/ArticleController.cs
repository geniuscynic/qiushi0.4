using qiushi.Entity;
using qiushi.Model;
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
    public class ArticleController : BaseController
    {
       

        // GET: Article
        public ActionResult Index()
        {
           return View(new ArticleEntity());
        }

        public ActionResult New()
        {
            return View("Index",new ArticleEntity());
        }

        [HttpPost]
       
        public async Task<ActionResult> Save(FormCollection form)
        {
            var id = form["Id"];
            var content = form["content"];
            content = Server.HtmlEncode(content);
            content = content.Replace ("\r\n", "<br />");

            var articleBl = new ArticleModel();

            var article = new ArticleEntity()
            {
                Content = content,
                Status =  Constant.ArticleStatus.Pending,
                UpdateBy = CurrentUser.Id
                
            };

           // article.InitializeCommonField();



            RequiredValidation(content, "内容不能为空");
            if (MessageEntities .Count() > 0)
            {

                return View("Index", article);
            }

            article.Attachments = new List<AttachmentsEntity>();
            
            var imgUrl = form["imgUrl"];
            var videoUrl = form["videoUrl"];
            var fileUrl = form["fileUrl"];
            
            if(imgUrl != "")
            {
                var fileName = imgUrl.Substring(imgUrl.LastIndexOf("/") + 1);
                var attachments = new AttachmentsEntity()
                {
                    FileName = fileName,
                    Url = imgUrl,
                    Type = Constant .AttachmentType .Picture 
                };

                //attachments.InitializeCommonField();
                attachments.UpdateBy = CurrentUser.Id;


                article.Attachments.Add(attachments);
            }
            else if(videoUrl != "")
            {
                var fileName = videoUrl.Substring(videoUrl.LastIndexOf("/") + 1);
                var attachments = new AttachmentsEntity()
                {
                    FileName = fileName,
                    Url = videoUrl
                };

                //attachments.InitializeCommonField();
                attachments.UpdateBy = CurrentUser.Id;


                article.Attachments.Add(attachments);
            }
            else if (fileUrl != "")
            {
                var fileName = fileUrl.Substring(fileUrl.LastIndexOf("/") + 1);
                var attachments = new AttachmentsEntity()
                {
                    FileName = fileName,
                    Url = fileUrl
                };

                //attachments.InitializeCommonField();
                attachments.UpdateBy = CurrentUser.Id;


                article.Attachments.Add(attachments);
            }


            if (id == "0")
            {
                if (await articleBl.Add(article))
                {
                    MessageEntities.Add(new MessageEntity("内容发布成功", Constant.MessageType.Successfully));
                }
            }
            //ViewBag.Message = new Message()
            //{
            //    MessageContent = "发布成功",
            //    MessageType = MessageType.Successfully
            //};

            return View("Index", new ArticleEntity());
        }


        public ActionResult List(string status, int page)
        {
            var articleBl = new ArticleModel();
            var articlesTuple = articleBl.List(CurrentUser.Id, Constant.GetArticleStatus(status), page, Constant.numArticlePerPage);
            var articles = articlesTuple.Item1;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = articlesTuple.Item2;

            return View(articles);
        }


        public ActionResult NeedVerify()
        {
            var articleBl = new ArticleModel();
            var articles = articleBl.NeedVerify(CurrentUser.Id);
            return View(articles);
        }

        [HttpPost]
        public async Task<ActionResult> Verify(FormCollection form)
        {
            var id = int.Parse(form["id"]);
            var pass = bool.Parse (form["value"]);

            var articleBl = new ArticleModel();

            var articleDetail = new ArticlelVerifyTrackEntity()
            {
                ArticleId = id,
                UpdateBy = CurrentUser.Id,
                IsPass = pass,
                Score = CurrentUser.UserRole.First().Role.Score
            };
           // articleDetail.InitializeCommonField();
            articleDetail.UpdateBy = CurrentUser.Id;

            await articleBl.VerifyArticle(articleDetail);

            return Content(bool.TrueString);
        }

        [HttpPost]
        public async Task<ContentResult> GoodBad(int id, int value)
        {
            var articleBl = new ArticleModel();
            var i = await articleBl.GoodBad(id, value, CurrentUser.Id);
            return Content(i.ToString());
        }



        [HttpPost]
        public async Task<ContentResult> UploadFile(HttpPostedFileBase file)
        {
            PhotoTask task = new PhotoTask();
            var url = await task.UploadPhoto(file.FileName, file.ContentType, file.InputStream);

            //var json = "{" +
            //        "\"errno\": 0," +
            //        "\"data\": [" +
            //            "\"{0}\"" +
            //        "]"+
            //    "}";
            //json = json.Replace("{0}", url.Item2);
            return Content(url.Item2);
        }

        public ActionResult FootTrack( int page)
        {
            var articleBl = new ArticleModel();
            var articlesTuple = articleBl.FootTrack(CurrentUser.Id, page, Constant.numArticlePerPage);
            var articles = articlesTuple.Item1;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = articlesTuple.Item2;

            return View(articles);
        }
    }
}
