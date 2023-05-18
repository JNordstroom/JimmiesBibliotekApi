using System.ComponentModel.DataAnnotations;

namespace MyLibrary.api.ViewModels
{
    public class MoviePostViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Längd måste anges!")]
        public int Lenght { get; set; }

        
    }
}