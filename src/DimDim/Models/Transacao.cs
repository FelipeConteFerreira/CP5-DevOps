using System.ComponentModel.DataAnnotations;
namespace DimDim.Models;

public class Transacao
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Escolha um cliente")] public int? ClienteId { get; set; }
    [Range(0.01, 999999999, ErrorMessage = "Informe um valor maior que zero")] public decimal Valor { get; set; }
    [StringLength(200)] public string Descricao { get; set; }
    public DateTime DataTransacao { get; set; } = DateTime.Now;
    public Cliente Cliente { get; set; }
}
