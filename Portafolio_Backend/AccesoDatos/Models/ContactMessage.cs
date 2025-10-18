namespace AccesoDatos.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; } // Nuevo campo: asunto del mensaje
        public string Message { get; set; }
        public DateTime SentAt { get; set; }

        // Relación opcional con User para identificar a quién va dirigido el mensaje
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}