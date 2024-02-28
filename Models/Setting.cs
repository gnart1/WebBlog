namespace WebBlog.Models
{
    public class Setting
    {
        public int Id { get; set; }
        public string? SiteName { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? ImageUrl { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? GithubUrl { get; set; }
    }
}
