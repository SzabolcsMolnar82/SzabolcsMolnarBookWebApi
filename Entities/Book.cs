using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SzabolcsMolnarBookWebApi.Entities
{
    public class Book
    {
        public int Id
        {
            get; set;
        }
        [StringLength(255)]

        public string Title
        {
            get; set;
        }
        public DateTime PublishedDate
        {
            get; set;
        }
        public int AuthorId
        {
            get; set;
        }
        [JsonIgnore]
        public virtual Author Author
        {
            get; set;
        }
    }
}
