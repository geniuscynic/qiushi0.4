using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace qiushi.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");



            // routes.MapRoute(
            //    name: "commment",
            //    url: "{controller}/{action}/{page}",
            //    defaults: new
            //    {
            //        controller = "Comment",
            //        action = "List",

            //        page = 1
            //    }
            //);
            // routes.MapRoute(
            //    name: "verifyPage",
            //    url: "Article/Verify/{id}/{pass}",
            //    defaults: new
            //    {
            //        controller = "Article",
            //        action = "Verify"

            //    }
            //);

            //routes.MapRoute(
            //     name: "goodbad",
            //     url: "Article/GoodBad/{id}/{value}",
            //     defaults: new
            //     {
            //         controller = "Article",
            //         action = "GoodBad"
            //     }
            // );

            //routes.MapRoute(
            //    name: "detailPage",
            //    url: "Home/Detail/{id}",
            //    defaults: new
            //    {
            //        controller = "Home",
            //        action = "Detail",

            //    }
            //);


            //routes.MapRoute(
            //    name: "commentsPage",
            //    url: "Home/Comments/{id}",
            //    defaults: new
            //    {
            //        controller = "Home",
            //        Action = "GetComments",

            //    }
            //);

            //routes.MapRoute(
            //    name: "homePage",
            //    url: "Home/{status}/{page}",
            //    defaults: new
            //    {
            //        controller = "Home",
            //        action = "List",
            //        status = "All"
            //    }
            //);

            routes.MapRoute(
               name: "Comment",
               url: "Comment/{action}/{page}",
               defaults: new
               {
                   controller = "Comment",
                  
                   page = 1
               },
               constraints: new { page = @"\d+" }
           );


            routes.MapRoute(
               name: "FootTrack",
               url: "FootTrack/{page}",
               defaults: new
               {
                   controller = "Article",
                   action = "FootTrack",
                   page = 1
               },
               constraints: new { page = @"\d+" }
           );

            routes.MapRoute(
                name: "articlePage",
                url: "Article/List/{status}/{page}",
                defaults: new
                {
                    controller = "Article",
                    action = "List",
                    status = "All",
                    page = 1
                },
               constraints: new { page = @"\d+" }
                 
            );



           routes.MapRoute(
               name: "home",
               url: "latest/{page}",
               defaults: new { controller = "Home", action = "List"},
               constraints: new { page = @"\d+" }
           );

           routes.MapRoute(
               name: "hot",
               url: "hot/{page}",
               defaults: new { controller = "Home", action = "Hot" },
               constraints: new { page = @"\d+" }
           );

            routes.MapRoute(
               name: "TravelBack",
               url: "chuanyue/{page}",
               defaults: new { controller = "Home", action = "TravelBack" },
               constraints: new { page = @"\d+" }
           );


            routes.MapRoute(
               name: "GetUser",
               url: "User/{id}/{page}",
               defaults: new { controller = "User", action = "GetUser" },
               constraints: new { id = @"\d+", page = @"\d+" }
           );

            routes.MapRoute(
               name: "UserList",
               url: "User/List/{page}",
               defaults: new { controller = "User", action = "List" },
               constraints: new { page = @"\d+" }
           );

          //  routes.MapRoute(
          //    name: "Enable",
          //    url: "User/Disable/{id}/{page}",
          //    defaults: new { controller = "User", action = "Disable" },
          //    constraints: new { id = @"\d+", page = @"\d+" }
          //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
