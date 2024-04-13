using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    
    private readonly IUnitOfWork _uof;

    public ProdutosController(IUnitOfWork uof)
    {
        
        _uof = uof;
    }

    [HttpGet("produtos/{id}")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosCategoria(int id)
    {
        var produtos = _uof.produtoRepository.GetProdutosPorCategoria(id);
        
        if (produtos is null)
            return NotFound();
        var produtosDto = produtos.ToProdutoDtoList();


        return Ok(produtosDto);    
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProdutoDTO>> Get()
    {
        var produtos = _uof.produtoRepository.GetAll();
        if (produtos is null)
        {
            return NotFound();
        }
        var produtosDto = produtos.ToProdutoDtoList();
        return Ok(produtosDto);
    }

    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _uof.produtoRepository.Get(c=> c.ProdutoId == id);   
        if (produto is null)
        {
            return NotFound("Produto não encontrado...");
        }
        var produtoDto = produto.ToProdutoDto();
        return Ok(produto);
    }

    [HttpPost]
    public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDto)
    {
        if (produtoDto is null)
            return BadRequest();
        var produto = produtoDto.ToProduto();

        var novoProduto = _uof.produtoRepository.Create(produto!);
        _uof.Commit();

        var novoProdutoDto = novoProduto.ToProdutoDto();

        return new CreatedAtRouteResult("ObterProduto",
            new { id = novoProdutoDto?.ProdutoId }, novoProdutoDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDto)
    {
        if (id != produtoDto.ProdutoId)
        {
            return BadRequest();//400
        }

        var produto = produtoDto.ToProduto();

        var produtoAtualizado = _uof.produtoRepository.Update(produto!);
        _uof.Commit();

        var produtoDtoAtualizado = produtoAtualizado.ToProdutoDto();

        return Ok(produtoDtoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProdutoDTO> Delete(int id)
    {
        var produto = _uof.produtoRepository.Get(p => p.ProdutoId == id);
        if (produto is null)
        {
            return NotFound("Produto não encontrado...");
        }

        var produtoDeletado = _uof.produtoRepository.Delete(produto);
        _uof.Commit();
        var produtoDtoDeletado = produtoDeletado.ToProdutoDto();

        return Ok(produtoDtoDeletado);
    }
}