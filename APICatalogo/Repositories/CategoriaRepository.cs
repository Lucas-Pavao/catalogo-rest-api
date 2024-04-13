using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {        
    }

    public async Task<IPagedList<Categoria>> GetCategoriaFiltroNomeAsync(CategoriaFiltroNome categoriaFiltroNome)
    {
        var categorias = await GetAllAsync();
        
        if(!string.IsNullOrEmpty(categoriaFiltroNome.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriaFiltroNome.Nome));
        }

        var categoriasFiltradas = await categorias.ToPagedListAsync(categoriaFiltroNome.PageSize, categoriaFiltroNome.PageNumber);
        return categoriasFiltradas;
    }

    public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriaParams)
    {

            var categorias = await GetAllAsync();
            var categoriaOrdenadas = categorias.OrderBy(x => x.CategoriaId).AsQueryable();
            var resultado = await categoriaOrdenadas.ToPagedListAsync(categoriaParams.PageNumber, categoriaParams.PageSize);
            return resultado;

        
    }
}
