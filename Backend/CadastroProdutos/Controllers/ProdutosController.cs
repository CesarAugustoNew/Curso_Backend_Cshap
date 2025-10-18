using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CadastroProdutos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private static List<Produto> produtos = new List<Produto>()
        {
            new Produto () { Id = 1, Nome = "Mouse sem fio", Preco = 99.90M, Estoque = 50},
            new Produto () { Id = 2, Nome = "Teclado", Preco = 249.90M, Estoque = 30}
        };

        [HttpGet]
        public ActionResult<List<Produto>> Get()
        {
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = produtos.FirstOrDefault(x => x.Id == id);

            if (produto is null)
            {
                return NotFound($"Produto com ID {id} não encotrado");
            }

            return Ok(produto);
        }

        [HttpPost]
        public ActionResult Post(Produto novoProduto)
        {
            produtos.Add(novoProduto);

            return Created();
        }

        [HttpPut("{id}")]
        public ActionResult<Produto> Put(int id, Produto produtoAtualizado)
        {
            var produto = produtos.FirstOrDefault(x => x.Id == id);

            if (produto is null)
            {
                return NotFound($"Produto com ID {id} não encontrado.");
            }

            produto.Nome = produtoAtualizado.Nome;
            produto.Preco = produtoAtualizado.Preco;
            produto.Estoque = produtoAtualizado.Estoque;

            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var produto = produtos.FirstOrDefault(x => x.Id == id);

            if (produto is null)
            {
                return NotFound($"Produto com ID {id} não encontrado.");
            }

            produtos.Remove(produto);

            return NoContent();
        }
    }
}
