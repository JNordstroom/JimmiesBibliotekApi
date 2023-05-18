using System.ComponentModel.DataAnnotations;

namespace MyLibrary.api.ViewModels
{
    public class SeriePostViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Antal säsonger måste anges!")]
        public int Seasons { get; set; }

       
    }
}