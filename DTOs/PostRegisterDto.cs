using System.ComponentModel.DataAnnotations;

namespace SzabolcsMolnarBookWebApi.DTOs
{
    public class PostRegisterDto
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; }
    }
}
