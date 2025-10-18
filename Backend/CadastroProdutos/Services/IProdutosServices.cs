using System;

namespace CadastroProdutos.Services;

public interface IProdutosServices
{
    public List<Produto> ObterTodos();
    public Produto ObterPorId(int id);
    public void Adicionar(Produto novoProduto);
    public Produto Atualizar(int id, Produto produtoAtualizado);
    public bool Remover(int id);

}
