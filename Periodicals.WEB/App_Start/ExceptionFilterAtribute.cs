using System.Web.Mvc;

namespace Periodicals.App_Start
{
    /// <summary>
    /// Class creates exception atribute, which on exception creates log whith full information
    /// </summary>
    public class ExceptionFilterAtribute : FilterAttribute, IExceptionFilter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void OnException(ExceptionContext filterContext)
        {
            string requestUrl = filterContext.HttpContext.Request.Url.ToString();
            string exceptionStack = filterContext.Exception.StackTrace;
            string exceptionMessage = filterContext.Exception.Message;
            log.Error($"Request url: {requestUrl} exeption thrown: \n {exceptionMessage} \n {exceptionStack}");
            filterContext.ExceptionHandled = true;
        }
    }
}