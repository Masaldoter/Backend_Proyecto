namespace AccesoDatos.Models
{
    public class Point
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        // La propiedad Project solo se usa para navegación, no debe ser requerida ni enviada en el POST
        public Project Project { get; set; }
    }
}