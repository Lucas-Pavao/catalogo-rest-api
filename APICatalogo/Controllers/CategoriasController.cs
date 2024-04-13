using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(
        ILogger<CategoriasController> logger, IUnitOfWork uof)
    {

        _logger = logger;
        _uof = uof;
    }

    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParameters)
    {
        var categorias = await _uof.categoriaRepository.GetCategoriasAsync(categoriasParameters);
        var produtosDto = ObterCategorias(categorias);
        return Ok(produtosDto);

    }

    [HttpGet("filter/nome/pagination")]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradas ([FromQuery] CategoriaFiltroNome categoriaFiltroNome)
    {
        var categorias = await _uof.categoriaRepository.GetCategoriaFiltroNomeAsync(categoriaFiltroNome);
        var produtosDto = ObterCategorias(categorias);
        return Ok(produtosDto);
    }

    private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(IPagedList<Categoria> categorias)
    {
        var metaData = new
        {
            categorias.Count,
            categorias.PageSize,
            categorias.PageCount,
            categorias.TotalItemCount,
            categorias.HasNextPage,
            categorias.HasPreviousPage
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));

        var produtosDto = categorias.ToCategoriaDtoList;
        return Ok(produtosDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
    {
        var categorias = await _uof.categoriaRepository.GetAllAsync();

        if(categorias is null)
        {
            return NotFound("Não existem Categorias ...");
        }
        var categoriasDto = categorias.ToCategoriaDtoList();
        return Ok(categoriasDto);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public async Task<ActionResult<CategoriaDTO>> Get(int id)
    {
        var categoria = await _uof.categoriaRepository.GetAsync(c=> c.CategoriaId == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Categoria com id= {id} não encontrada...");
            return NotFound($"Categoria com id= {id} não encontrada...");
        }

        var categoriaDto = categoria.ToCategoriaDto();
        return Ok(categoriaDto);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        var categoria = categoriaDto.ToCategoria();

        var categoriaCriada = _uof.categoriaRepository.Create(categoria);
       await  _uof.CommitAsync();

        var novaCategoriaDto = categoria.ToCategoriaDto();

        return new CreatedAtRouteResult("ObterCategoria", 
            new { id = novaCategoriaDto.CategoriaId },
            novaCategoriaDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDto)
    {
        if (id != categoriaDto.CategoriaId)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        var categoria = categoriaDto.ToCategoria();

        _uof.categoriaRepository.Update(categoria);
        await _uof.CommitAsync();

        var novaCategoriaDto = categoria.ToCategoriaDto();
        return Ok(novaCategoriaDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CategoriaDTO>> Delete(int id)
    {
        var categoria =await _uof.categoriaRepository.GetAsync(c => c.CategoriaId == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Categoria com id={id} não encontrada...");
            return NotFound($"Categoria com id={id} não encontrada...");
        }

        var categoriaExcluida = _uof.categoriaRepository.Delete(categoria);
       await _uof.CommitAsync();

        var categoriaDto = categoria.ToCategoriaDto();
        return Ok(categoriaDto);

    }
}