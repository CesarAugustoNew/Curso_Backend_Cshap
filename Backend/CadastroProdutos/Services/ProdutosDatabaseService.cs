using System;
using CadastroProdutos.Database;
using CadastroProdutos.Models;

namespace CadastroProdutos.Services;

public class ProdutosDatabaseService : IProdutosServices
{
    private ApplicationDbContext banco;

    public ProdutosDatabaseService(ApplicationDbContext banco)
    {
        this.banco = banco;
    }

    public void Adicionar(Produto novoProduto)
    {
        ValidarProdutos(novoProduto);
        banco.Produtos.Add(novoProduto);
        banco.SaveChanges();
    }

    public Produto Atualizar(int id, Produto produtoAtualizado)
    {
        ValidarProdutos(produtoAtualizado);
        var produto = banco.Produtos.FirstOrDefault(x => x.Id == id);

        if (produto is null)
        {
            return null;
        }

        produto.Nome = produtoAtualizado.Nome;
        produto.Preco = produtoAtualizado.Preco;
        produto.Estoque = produtoAtualizado.Estoque;

        banco.SaveChanges();

        return produto;
    }

    public Produto ObterPorId(int id)
    {
        return banco.Produtos.FirstOrDefault(x => x.Id == id);
    }

    public List<Produto> ObterTodos()
    {
        return banco.Produtos.ToList();
    }

    public bool Remover(int id)
    {
        var produto = banco.Produtos.FirstOrDefault(x => x.Id == id);
        if (produto is null)
        {
            return false;
        }

        banco.Produtos.Remove(produto);

        banco.SaveChanges();

        return true;
    }
    
    private void ValidarProdutos(Produto produto)
    {
        if (produto.Nome == "Produto Padrão")
        {
            throw new Exception("Não é permitido cadastrar um produto com o nome: Produto Padrão.");
        }

        if (produto.Estoque > 1000)
        {
            throw new Exception("O estoque não pode ser maior que 1000 unidades.");
        }
    }
}
