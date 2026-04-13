using System.ComponentModel.DataAnnotations;

namespace EventAssos.Application.DTOs.Requests
{
    public class UpdateEventCategoryRequestDTO
    {
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
        public string? Name { get; set; }
    }
}
