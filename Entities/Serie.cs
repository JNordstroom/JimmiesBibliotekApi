using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibrary.api.Entities
{
    public class Serie : BaseEntity
    {
        public int Seasons { get; set; }
    }
}