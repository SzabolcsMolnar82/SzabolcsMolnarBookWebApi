namespace SzabolcsMolnarBookWebApi.DTOs
{
    public class PutBookDto
    {
        public string Title { get; set; }
        public DateTime PupblishedDate { get; set; }
        public int AuthorId { get; set; }
    }
}
