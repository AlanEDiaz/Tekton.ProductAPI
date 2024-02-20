using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tekton.ProductAPI.Infrastructure.Repositories;

namespace Tekton.ProductAPI.Domain.Entities
{
    public class CommandStore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public Guid ProductId { get; set; }
        public string Type { get; set; }

        public string Data { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
