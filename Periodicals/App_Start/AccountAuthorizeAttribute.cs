using System.Web;
using System.Web.Mvc;

namespace Periodicals.App_Start
{
    /// <summary>
    /// Class creates attribute which allow acsses to controller methods to user whith paticular role
    /// </summary>
    public class AccountAuthorizeAttribute : AuthorizeAttribute
    {
        private string[] _allowedRoles = new string[] { };

        public AccountAuthorizeAttribute() { }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!string.IsNullOrEmpty(Roles))
            {
                _allowedRoles = base.Roles.Split(new char[] { ',' });
                for (int i = 0; i < _allowedRoles.Length; i++)
                {
                    _allowedRoles[i] = _allowedRoles[i].Trim();
                }
            }
            return httpContext.Request.IsAuthenticated && Role(httpContext);
        }

        private bool Role(HttpContextBase httpContext)
        {
            if (_allowedRoles.Length > 0)
            {
                for (int i = 0; i < _allowedRoles.Length; i++)
                {
                    if (httpContext.Request.Cookies["Role"].Value == _allowedRoles[i])
                        return true;
                }
                return false;
            }
            return true;
        }
    }
}
