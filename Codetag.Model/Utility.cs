using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Codetag.Model
{
	public class JMessage
	{
		public Type Type
		{
			get;
			set;
		}

		public JToken Value
		{
			get;
			set;
		}

		public static JMessage FromValue<T>(T value)
		{
			return new JMessage
			{
				Type = typeof(T),
				Value = JToken.FromObject(value)
			};
		}

		public static string Serialize(JMessage message)
		{
			return JToken.FromObject(message).ToString();
		}

		public static JMessage Deserialize(string data)
		{
			return JToken.Parse(data).ToObject<JMessage>();
		}
	}

	/// <summary>
	/// Static utility Class 
	/// </summary>
	public static class Utility
	{
		static Random rnd = new Random();
		public static int GetRandomNumber(int maxNumber = 100)
		{
			return rnd.Next(1, maxNumber);
		}

		public static string GetRandomServerName()
		{
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var random = new Random();
			var result = new string(
				Enumerable.Repeat(chars, 8)
						  .Select(s => s[random.Next(s.Length)])
						  .ToArray());
			return result;
		}
	}
}
