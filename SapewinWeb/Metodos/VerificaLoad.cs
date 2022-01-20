using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SapewinWeb.Metodos
{
    public class VerificaLoad : ActionFilterAttribute
    {
        public static bool IsAjax(HttpRequestBase Request)
        {
            if (Request.Headers.AllKeys.Contains("X-Requested-With") && Request.Headers.GetValues("X-Requested-With").Where(x=>x == "XMLHttpRequest").Count() > 0)
            {
                return true;
            }else
            {
                return false;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Request.Headers.AllKeys.Contains("X-Requested-With")
                    && HttpContext.Current.Request.Headers.GetValues("X-Requested-With")
                    .Where(x => x == "XMLHttpRequest").Count() > 0) {
                return;            
            }else
            {
                

                filterContext.Result = new HttpUnauthorizedResult();
            }           
        } 
        
         
    }
}