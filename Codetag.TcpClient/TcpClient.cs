using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using Codetag.Model;

namespace Codetag.TcpClient
{
	class TcpClient
	{
		private const int Port = 27877;
		private static string serverName;
		private static int serverMaxValue;
		static int serverItemCount;
		private const int Interval = 20;

		static void Main(string[] args)
		{
			Console.BackgroundColor = ConsoleColor.Magenta;
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.White;

			serverName = args[0];
			serverMaxValue = Convert.ToInt16(args[1]);

			serverItemCount = 0;
			var timer = new System.Timers.Timer(Interval);
			timer.Elapsed += new ElapsedEventHandler(OnTimer);
			timer.AutoReset = true;
			timer.Start();
			Console.WriteLine("Tcp Socket Client writing on port {0}", Port.ToString());
			Console.WriteLine(@"Press 'q' and 'Enter' to quit...");

			while (Console.Read() != 'q')
			{
				Thread.Sleep(Interval);
			}
		}

		static void OnTimer(object sender, ElapsedEventArgs e)
		{
			GenerarMensaje();
		}

		private static void GenerarMensaje()
		{

			using (System.Net.Sockets.TcpClient tcpClient = new System.Net.Sockets.TcpClient())
			{
				tcpClient.Connect("127.0.0.1", Port);
				using (NetworkStream tcpStream = tcpClient.GetStream())
				{
					serverItemCount++;
					var server = new FakeServer
					{
						ServerName = serverName,
						ServerValue = Utility.GetRandomNumber(serverMaxValue),
						ServerItemCount = serverItemCount
					};

					string mensaje = JMessage.Serialize(JMessage.FromValue(server));
					byte[] data = Encoding.ASCII.GetBytes(mensaje);
					tcpStream.Write(data, 0, data.Length);
				}

			}
			if (serverItemCount == 3)
			{
				serverItemCount = 0;
			}
		}
	}
}
