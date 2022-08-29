using System;
using System.ComponentModel.DataAnnotations;

namespace ProviderService.Api.Models
{
    public class Provider
    {
        [Required]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public Guid AlternateIdentifier { get; set; }
        public bool IsActive { get; set; }
    }
}
