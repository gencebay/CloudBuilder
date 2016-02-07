using System;
using System.ComponentModel.DataAnnotations;

namespace Cloud.Common.Models
{
    public class ClientViewModel
    {
        [Required]
        public Guid ClientId { get; set; }
        public ClientType ClientType { get; set; }
    }
}
