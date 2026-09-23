using System.Web;
using System.Web.Mvc;

namespace IPC2_Proy02_202602_202407617
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
