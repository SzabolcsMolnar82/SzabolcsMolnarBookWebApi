using System.ComponentModel.DataAnnotations;

namespace SzabolcsMolnarBookWebApi.DTOs
{
    public class PostLoginDto
    {
        [Required]
        public string Username { get; set; }



        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; }
    }
}
