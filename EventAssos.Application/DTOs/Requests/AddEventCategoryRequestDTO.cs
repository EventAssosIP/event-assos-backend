namespace EventAssos.Application.DTOs.Requests
{
    using System.ComponentModel.DataAnnotations;

    namespace EventAssos.Application.DTOs.Requests
    {
        public class AddEventCategoryRequestDTO
        {
            [Required(ErrorMessage = "Category name is required")]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
            public required string Name { get; set; }
        }
    }
}
