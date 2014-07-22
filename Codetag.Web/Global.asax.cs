using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Codetag.Web
{
	using System.Diagnostics;

	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			AuthConfig.RegisterAuth();

			CreateTcpServers();
		}

		private void CreateTcpServers()
		{
			string tcpToWebServer = Server.MapPath("/TcpExes") + @"/Codetag.TcpToWeb.exe";
			string tcpServer = Server.MapPath("/TcpExes") + @"/Codetag.TcpServer.exe";
			Process[] pname = Process.GetProcessesByName("Codetag.TcpToWeb");
			if (pname.Length != 0)
			{
				pname[0].Kill();
			}
			pname = Process.GetProcessesByName("Codetag.TcpServer");
			if (pname.Length != 0)
			{
				pname[0].Kill();
			}


			Process.Start(tcpToWebServer);
			Process.Start(tcpServer);
		}
	}
}