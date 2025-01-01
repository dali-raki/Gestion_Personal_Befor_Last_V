using System.ComponentModel.DataAnnotations;

namespace Gestion_personal.Components.Models.Login
{
	public class LoginModel
	{
		
		public int EmployeId { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        public string Password { get; set; }

        public int FunctionId { get; set; }

	}
}
