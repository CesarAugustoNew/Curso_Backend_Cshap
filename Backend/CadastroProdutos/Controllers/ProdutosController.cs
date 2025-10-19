using CadastroProdutos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CadastroProdutos.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {

        private IProdutosServices produtosService;

        public ProdutosController(IProdutosServices produtosServices)
        {
            this.produtosService = produtosServices;
        }

        [HttpGet]
        public ActionResult<List<Produto>> Get()
        {
            return Ok(produtosService.ObterTodos());
        }

        [HttpGet("{id}")]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = produtosService.ObterPorId(id);

            if (produto is null)
            {
                return NotFound($"Produto com ID {id} não encotrado");
            }

            return Ok(produto);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Post(Produto novoProduto)
        {
            try
            {
                produtosService.Adicionar(novoProduto);

                return Created();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public ActionResult<Produto> Put(int id, Produto produtoAtualizado)
        {
            try
            {
                var produto = produtosService.Atualizar(id, produtoAtualizado);

                if (produto is null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                return Ok(produto);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [Authorize(Roles ="admin")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var deletou = produtosService.Remover(id);

            if (deletou == false)
            {
                return NotFound($"Produto com ID {id} não encontrado.");
            }

            return NoContent();
        }
    }
}
