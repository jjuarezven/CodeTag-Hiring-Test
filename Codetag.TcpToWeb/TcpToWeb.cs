using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Codetag.Model;
using Fleck;


namespace Codetag.TcpToWeb
{
	class TcpToWeb
	{
		private const int Port = 27878;
		static List<IWebSocketConnection> allSockets;
		static string temp;


		static void Main(string[] args)
		{
			Console.BackgroundColor = ConsoleColor.Gray;
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.White;

			InitializeFleck();
			System.Net.IPAddress serverAddress = System.Net.IPAddress.Parse("127.0.0.1");
			TcpListener listener = new TcpListener(serverAddress, Port);
			listener.Start();

			Console.WriteLine("Tcp Socket Server listening on port {0}", Port.ToString());
			while (true)
			{
				TcpClient tpcClient = listener.AcceptTcpClient();
				NetworkStream stream = tpcClient.GetStream();
				byte[] data = new byte[tpcClient.ReceiveBufferSize];
				int bytesRead = stream.Read(data, 0, System.Convert.ToInt32(tpcClient.ReceiveBufferSize));
				foreach (var socket in allSockets.ToList())
				{
					temp = string.Empty;
					string msj = Encoding.ASCII.GetString(data, 0, bytesRead);
					JMessage server = JMessage.Deserialize(msj);

					if (server.Type == typeof(FakeServer))
					{
						socket.Send(server.Value.ToString());
						Console.WriteLine("Send to Web: {0}", server.Value.ToString());
					}
					else
					{
						listener.Stop();
						Console.WriteLine("Socket closed due to incorrect type.");
						break;
					}
				}
			}
		}

		private static void InitializeFleck()
		{
			FleckLog.Level = LogLevel.Debug;
			allSockets = new List<IWebSocketConnection>();
			var server = new Fleck.WebSocketServer("ws://127.0.0.1:27879");
			server.Start(socket =>
				{
					socket.OnOpen = () =>
						{
							Console.WriteLine("Open!");
							allSockets.Add(socket);
						};
					socket.OnClose = () =>
						{
							Console.WriteLine("Close!");
							allSockets.Remove(socket);
						};
					socket.OnMessage = message =>
						{
							Console.WriteLine(message);
							allSockets.ToList().ForEach(s => s.Send("Echo: " + message));
						};
				});
		}
	}
}
