using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebApi.Models
{
    public class CreateProjectFormModel : IValidatableObject
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public string[]? Technologies { get; set; }

        [Required]
        public IFormFile Image { get; set; } = default!; // principal obligatoria en creación

        public IFormFile[]? Images { get; set; }

        [Url]
        public string? ProjectUrl { get; set; }

        [Required]
        public int UserId { get; set; }

        public bool IsFeatured { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(ProjectUrl))
            {
                if (!Uri.TryCreate(ProjectUrl, UriKind.Absolute, out var uri) ||
                    (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                {
                    yield return new ValidationResult("ProjectUrl debe usar http o https.", new[] { nameof(ProjectUrl) });
                }
            }

            if (Image != null && !Image.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                yield return new ValidationResult("El archivo principal debe ser una imagen.", new[] { nameof(Image) });
            }

            if (Images != null)
            {
                foreach (var img in Images)
                {
                    if (img != null && !img.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                        yield return new ValidationResult("Todas las imágenes adicionales deben ser de tipo imagen.", new[] { nameof(Images) });
                }
            }
        }
    }
}
