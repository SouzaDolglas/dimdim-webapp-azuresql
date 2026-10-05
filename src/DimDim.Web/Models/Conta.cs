using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DimDim.Web.Models;

[Table("conta")]
public class Conta
{
    [Key, Column("id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o número da conta"), StringLength(20), Column("numero")]
    [Display(Name = "Número")]
    public string Numero { get; set; } = "";

    [Required, StringLength(20), Column("tipo")]
    public string Tipo { get; set; } = "CORRENTE";

    [Column("saldo", TypeName = "decimal(12,2)"), Range(0, 9999999999)]
    public decimal Saldo { get; set; }

    [Column("data_abertura"), Display(Name = "Aberta em")]
    public DateTime DataAbertura { get; set; } = DateTime.UtcNow;

    [Column("cliente_id"), Display(Name = "Cliente")]
    [Range(1, int.MaxValue, ErrorMessage = "Escolha o cliente")]
    public int ClienteId { get; set; }

    [JsonIgnore]
    public Cliente? Cliente { get; set; }
}
