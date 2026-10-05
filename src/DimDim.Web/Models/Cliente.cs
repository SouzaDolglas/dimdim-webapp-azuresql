using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DimDim.Web.Models;

[Table("cliente")]
public class Cliente
{
    [Key, Column("id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome"), StringLength(100), Column("nome")]
    public string Nome { get; set; } = "";

    [Required(ErrorMessage = "Informe o CPF"), StringLength(14), Column("cpf")]
    public string Cpf { get; set; } = "";

    [Required(ErrorMessage = "Informe o e-mail"), EmailAddress, StringLength(120), Column("email")]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = "";

    [Column("data_cadastro"), Display(Name = "Cadastrado em")]
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    public List<Conta> Contas { get; set; } = new();
}
