using System;
using System.ComponentModel.DataAnnotations;

namespace CadastroProdutos.Models;

public class Login
{
    [Required]
    public string Usuario { get; set; }

    [Required]
    public string Senha { get; set; }
}
