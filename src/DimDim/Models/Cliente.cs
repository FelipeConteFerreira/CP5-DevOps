using System.ComponentModel.DataAnnotations;
namespace DimDim.Models;

public class Cliente
{
    public int Id { get; set; }
    [Required, StringLength(100)] public string Nome { get; set; }
    [Required, EmailAddress, StringLength(150)] public string Email { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.Now;
    public List<Transacao> Transacoes { get; set; } = new();
}
