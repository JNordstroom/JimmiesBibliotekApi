

using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibrary.api.Entities
{
    public class Movie : BaseEntity
    {
        public int Lenght { get; set; }
        
    }
       
}