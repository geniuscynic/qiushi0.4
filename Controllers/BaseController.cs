using qiushi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace qiushi.Web.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected UserEntity CurrentUser
        {
            get
            {
                return Session["User"] as UserEntity;
            }
            set
            {
                Session["User"] = value;
            }

        }

        protected List<MessageEntity> MessageEntities
        {
            get
            {
                if(ViewBag.MessageEntities == null)
                {
                    ViewBag.MessageEntities = new List<MessageEntity>();
                }

                return ViewBag.MessageEntities;
            }
        }

        protected void RequiredValidation(string value, string message)
        {
            if(string.IsNullOrEmpty(value))
            {
                AddValidationErrorMessage(message);
            }
        }

        protected void EqualValidation(string value1, string value2, string message)
        {
            if (value1 != value2)
            {
                AddValidationErrorMessage(message);
            }
        }

        private void AddValidationErrorMessage(string message)
        {
            ModelState.AddModelError("key", message);
            MessageEntities.Add(new MessageEntity(message, Constant.MessageType.Error));
        }

        public void AddMessage(MessageEntity message)
        {
            
            MessageEntities.Add(message);
        }

        
    }

   
}
