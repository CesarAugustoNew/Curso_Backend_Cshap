using System;
using System.ComponentModel.DataAnnotations;

namespace CadastroProdutos.Models;

public class Produto
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "O nome do produto é obrigatorio.")]
    [StringLength(100, ErrorMessage = "O nome pode ter no maximo 100 caracteres.")]
    public string Nome { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
    public decimal Preco { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo")]
    public int Estoque { get; set; }
}
