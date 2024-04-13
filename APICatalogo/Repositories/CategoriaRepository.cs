using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {        
    }

    public PagedList<Categoria> GetCategoriaFiltroNome(CategoriaFiltroNome categoriaFiltroNome)
    {
        var categorias = GetAll().AsQueryable();
        if(!string.IsNullOrEmpty(categoriaFiltroNome.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriaFiltroNome.Nome));
        }

        var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, categoriaFiltroNome.PageSize, categoriaFiltroNome.PageNumber);
        return categoriasFiltradas;
    }

    public PagedList<Categoria> GetCategorias(CategoriasParameters categoriaParams)
    {

            var categorias = GetAll().OrderBy(x => x.CategoriaId).AsQueryable();
            var categoriasOrdenados = PagedList<Categoria>.ToPagedList(categorias, categoriaParams.PageNumber, categoriaParams.PageSize);
            return categoriasOrdenados;

        
    }
}
