using APICatalogo.Interfaces;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    
    private readonly IProdutoRepository _repository;
    
    public ProdutosController(
        IProdutoRepository repository)
    {
        _repository = repository;
       
    }

    [HttpGet("produtos/{id}")]
    public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
    {
        var produtos = _repository.GetProdutosPorCategoria(id);
        
        if (produtos is null)
            return NotFound();

        return Ok(produtos);    
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _repository.GetAll();
        if (produtos is null)
        {
            return NotFound();
        }
        return Ok(produtos);
    }

    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _repository.Get(c=> c.ProdutoId == id);   
        if (produto is null)
        {
            return NotFound("Produto não encontrado...");
        }
        return Ok(produto);
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto is null)
            return BadRequest();

        var novoProduto = _repository.Create(produto);

        return new CreatedAtRouteResult("ObterProduto",
            new { id = novoProduto.ProdutoId }, novoProduto);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
        {
            return BadRequest();//400
        }

        var produtoAtualizado = _repository.Update(produto);

       return Ok(produtoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _repository.Get(p => p.ProdutoId == id);
        if (produto is null)
        {
            return NotFound("Produto não encontrado...");
        }

        var produtoDeletado = _repository.Delete(produto);
        return Ok(produtoDeletado);
    }
}