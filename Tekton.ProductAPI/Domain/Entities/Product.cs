using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tekton.ProductAPI.Infrastructure.Repositories;

namespace Tekton.ProductAPI.Domain.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int? Stock { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount {  get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set;}
    }
}
