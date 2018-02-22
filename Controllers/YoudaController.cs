using qiushi.Entity;
using qiushi.Model;
using qiushi.Task;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace qiushi.Web.Controllers
{
    public class YoudaController : Controller
    {
        // GET: Youda
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> AddUser(FormCollection form)
        {
            var name = form["name"];
            var phone = form["phone"];
            var idcard = form["idcard"];
            var address = form["address"];
            var gender = int.Parse(form["sex"]);

            var user = new YoudaUserEntity() {
                 Name = name,
                 Phone= phone,
                 Gender =gender,
                 Idcard = idcard,
                 IdcardAddress =address
            };

            var youdaUserModel = new YoudaModel();
            var successfully = await youdaUserModel.Add(user);
            if(successfully)
            {
                return Content("ok");
            }
            return Content("faild");
        }

        public async Task<ActionResult> OpenId(FormCollection form)
        {
            var code = form["code"];

            var appid = "wx7f25f9555b497548";
            var secret = "b7a21c2ca3c9764702f7adf1b6a5859c";
            var url = "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";

            url = string.Format(url, appid, secret, code);

            CurlTask task = new CurlTask();
            var request = task.GetRequest(url);
            var result = await task.GetResult(request);

            return Json(result);
        }
    }
}