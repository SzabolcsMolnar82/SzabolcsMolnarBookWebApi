using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SzabolcsMolnarBookWebApi.Entities
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [StringLength(255)]

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string? UserId { get; set; }
        [JsonIgnore]

        public virtual ApplicationUser? User { get; set; }


    }
}
