using System.Web.Mvc;

namespace Codetag.Web.Controllers
{
	using System.Diagnostics;
	using System.Threading;

	using Codetag.Web.Models;

	public class ServerController : Controller
	{
		// GET: /Server/Create

		public ActionResult Create()
		{
			var newServer = new ServerViewModel();
			return View(newServer);
		}

		//
		// POST: /Server/Create

		[HttpPost]
		public ActionResult Create(ServerViewModel newServer)
		{
			try
			{
				string tcpClientFileName = Server.MapPath("/TcpExes") + @"/Codetag.TcpClient.exe";
				var arguments = "\"" + newServer.ServerName + "\" " + newServer.MaxValue.ToString();

				ProcessStartInfo startInfo = new ProcessStartInfo();
				startInfo.UseShellExecute = true;
				startInfo.FileName = tcpClientFileName;
				startInfo.WindowStyle = ProcessWindowStyle.Normal;
				startInfo.Arguments = @arguments;

				Thread.Sleep(500);

				Process.Start(startInfo);
				Session["ServerName"] = newServer.ServerName;
				return RedirectToAction("Create");

			}
			catch
			{
				return View();
			}
		}
	}
}
