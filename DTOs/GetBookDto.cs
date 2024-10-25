namespace SzabolcsMolnarBookWebApi.DTOs
{
    public class GetBookDto
    {
        public string Title { get; set; }
        public DateTime PublishedDate { get; set; }

        public string AuthorName { get; set; }
    }
}
