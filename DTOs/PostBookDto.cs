namespace SzabolcsMolnarBookWebApi.DTOs
{
    public class PostBookDto
    {
        public string Title { get; set; }
        public DateTime PublishedDate { get; set; }
        public int AuthorId { get; set; }
    }
}
