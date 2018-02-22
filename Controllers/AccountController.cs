using qiushi.Entity;
using qiushi.Entity.Utility;
using qiushi.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace qiushi.Web.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        public ActionResult Index()
        {
            return View(new UserEntity());
        }

        public ActionResult LogOff()
        {
            CurrentUser = null;
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Login()
        {

            return View("Login", new UserEntity());
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {

            var account = form["account"];
            var password = form["password"];

           // var a= DESEncrypt.Encode(password);
            //var b = DESEncrypt.Decode(a);
            RequiredValidation(account, "帐号不能为空");
            RequiredValidation(password, "密码不能为空");

            if (MessageEntities.Count() > 0)
            {
                var userEntity = new UserEntity()
                {
                    LoginName = account,
                    Password = password
                };

                return View(userEntity);
            }

            var userBl = new UserModel();


            var user = userBl.FindUser(account, password);


            if (user.Message.MessageType == Constant.MessageType.Error)
            {
                AddMessage(user.Message);
                return View(user);
            }

            CurrentUser = user;
            return RedirectToAction("Index", "User");

        }

        [HttpPost]
        public async Task<ActionResult> Signup(FormCollection form)
        {

            //ModelState.AddModelError("login", "帐号不能为空");

            var userEntity = new UserEntity()
            {
                NickName = form["nickName"],
                LoginName = form["login"],
                Password = form["password"],
                Score = 0,
                LastLoginTime = DateTime.Now
            };

            //userEntity.InitializeCommonField();

            RequiredValidation(userEntity.LoginName, "帐号不能为空");
            RequiredValidation(userEntity.NickName, "昵称不能为空");
            RequiredValidation(userEntity.Password, "密码不能为空");
            RequiredValidation(userEntity.Password, "确认密码不能为空");
            EqualValidation(userEntity.Password, form["password2"], "密码不一致");
            EqualValidation(form["imgCode"].ToLower(), Session["CheckCode"].ToString().ToLower(), "验证码不对");

            if (!ModelState.IsValid)
            {

                return View("Index", userEntity);
            }


            var userBl = new UserModel();

            var message = userBl.CheckUserExists(userEntity.LoginName, userEntity.NickName);

            if (message.MessageType == Constant.MessageType.Error)
            {
               // AddValidationErrorMessage(message.Message);
                return View("Index", userEntity);
            }


            await userBl.Add(userEntity);

            CurrentUser = userEntity;
            return RedirectToAction("Index", "User");
        }
    }
}