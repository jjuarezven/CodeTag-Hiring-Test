
namespace Codetag.Web.Models
{
	using System.ComponentModel.DataAnnotations;

	public class ServerViewModel
	{
		public ServerViewModel()
		{
			MaxValue = 100;
		}

		[Display(Name = "Fake Server Name")]
		[Required]
		public string ServerName { get; set; }

		[Required]
		[Range(10, 100)]
		[Display(Name = "Max Random Value for Y Axis")]
		public int MaxValue { get; set; }
	}
}