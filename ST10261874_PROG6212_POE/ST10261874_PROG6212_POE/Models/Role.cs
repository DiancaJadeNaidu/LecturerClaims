using System.ComponentModel.DataAnnotations;

namespace ST10261874_PROG6212_POE.Models
{
    public class Role
    {
        public string Id { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public string Description { get; set; }
    }
}