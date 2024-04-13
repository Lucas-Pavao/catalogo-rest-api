using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Interfaces;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriaParams);
    Task<IPagedList<Categoria>> GetCategoriaFiltroNomeAsync(CategoriaFiltroNome categoriaFiltroNome);
}


