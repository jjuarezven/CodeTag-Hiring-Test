using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
//using System.Xml.Serialization;
using Codetag.Model;

namespace Codetag.TcpServer
{
	using System.Diagnostics;

	class TcpServer
	{
		const int Port = 27877;
		private const int SecondPort = 27878;
		static int objectsCount;
		static Dictionary<string, List<FakeServer>> dictServer;

		static void Main(string[] args)
		{
			Console.BackgroundColor = ConsoleColor.Cyan;
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.White;
			dictServer = new Dictionary<string, List<FakeServer>>();
			objectsCount = 0;
			ProcessMessages();
			Console.ReadLine();
		}

		private static void ProcessMessages()
		{
			IPAddress serverAddress = System.Net.IPAddress.Parse("127.0.0.1");
			TcpListener listener = new TcpListener(serverAddress, Port);
			listener.Start();
			Console.WriteLine("Tcp Socket Server listening on port {0}", Port.ToString());
			while (true)
			{
				TcpClient tcpClient = listener.AcceptTcpClient();
				NetworkStream stream = tcpClient.GetStream();
				byte[] data = new byte[tcpClient.ReceiveBufferSize];
				int bytesRead = stream.Read(data, 0, Convert.ToInt32(tcpClient.ReceiveBufferSize));
				string msj = Encoding.ASCII.GetString(data, 0, bytesRead);
				JMessage message = JMessage.Deserialize(msj);
				if (message.Type == typeof(FakeServer))
				{
					//Console.WriteLine(msj);
					FakeServer server = message.Value.ToObject<FakeServer>();
					ProcessServer(server);
				}
				else
				{
					listener.Stop();
					Console.WriteLine("Socket closed due to incorrect type.");
					break;
				}
			}
		}

		private static void ProcessServer(FakeServer server)
		{
			string fileName = string.Empty;
			if (!dictServer.ContainsKey(server.ServerName))
			{
				dictServer.Add(server.ServerName, new List<FakeServer>());
			}
			if (server.ServerItemCount <= 2)
			{
				dictServer[server.ServerName].Add(server);
				objectsCount++;
			}
			else
			{
				var valor = dictServer[server.ServerName].Sum(item => item.ServerValue) / 2;

				SendToSecondPort(server);
				Debug.WriteLine("send");
				//fileName = @"C:\tagme\" + server.ServerName + ".xml";
				//var serializer = new XmlSerializer(typeof(FakeServer));
				//using (var writer = new StreamWriter(fileName))
				//{
				//    serializer.Serialize(writer, server);
				//}

				objectsCount = 0;
				dictServer[server.ServerName].Clear();
			}
		}

		private static void SendToSecondPort(FakeServer server)
		{
			using (System.Net.Sockets.TcpClient tcpClient = new System.Net.Sockets.TcpClient())
			{
				tcpClient.Connect("127.0.0.1", SecondPort);
				using (NetworkStream tcpStream = tcpClient.GetStream())
				{
					string mensaje = JMessage.Serialize(JMessage.FromValue(server));
					byte[] data = Encoding.ASCII.GetBytes(mensaje);
					tcpStream.Write(data, 0, data.Length);
				}
			}
			Console.WriteLine(
						"Sending ServerName: {0} ServerValue: {1}",
						server.ServerName,
						server.ServerValue.ToString());
		}
	}
}
