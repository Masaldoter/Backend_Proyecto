namespace AccesoDatos.Models
{
    public class ProjectImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}