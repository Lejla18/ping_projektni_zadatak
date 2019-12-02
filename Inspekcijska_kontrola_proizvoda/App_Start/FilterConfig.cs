using System.Web;
using System.Web.Mvc;

namespace Inspekcijska_kontrola_proizvoda
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
