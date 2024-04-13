using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public ProdutosController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet("produtos/{id}")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosCategoria(int id)
    {
        var produtos = _uof.produtoRepository.GetProdutosPorCategoria(id);
        
        if (produtos is null)
            return NotFound();

        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

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

        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDto);
    }

    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<ProdutoDTO> Get(int id)
    {
        var produto = _uof.produtoRepository.Get(c=> c.ProdutoId == id);   
        if (produto is null)
        {
            return NotFound("Produto não encontrado...");
        }
        var produtoDto = _mapper.Map<ProdutoDTO>(produto);

        return Ok(produtoDto);
    }

    [HttpPost]
    public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDto)
    {
        if (produtoDto is null)
            return BadRequest();

        var produto = _mapper.Map<Produto>(produtoDto);

        var novoProduto = _uof.produtoRepository.Create(produto);
        _uof.Commit();

        var novoProdutoDto = _mapper.Map<ProdutoDTO>(novoProduto);

        return new CreatedAtRouteResult("ObterProduto",
            new { id = novoProdutoDto?.ProdutoId }, novoProdutoDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDto)
    {
        if (id != produtoDto.ProdutoId)
            return BadRequest();//400
        

        var produto = _mapper.Map<Produto>(produtoDto);

        var produtoAtualizado = _uof.produtoRepository.Update(produto);
        _uof.Commit();

        var produtoDtoAtualizado = _mapper.Map<ProdutoDTO>(produtoAtualizado);

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

        var produtoDtoDeletado = _mapper.Map<ProdutoDTO>(produtoDeletado);
        return Ok(produtoDtoDeletado);
    }
}