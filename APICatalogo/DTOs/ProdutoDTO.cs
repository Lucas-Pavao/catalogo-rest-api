using APICatalogo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APICatalogo.DTOs
{
    public class ProdutoDTO
    {

        public int ProdutoId { get; set; }
        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }
        [Required]
        [StringLength(300)]
        public string? Descricao { get; set; }
        [Required]
        public decimal Valor { get; set; }
        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }
        public int CategoriaId { get; set; }
        
        
    }
}
