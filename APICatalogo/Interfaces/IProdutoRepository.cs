using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
    Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams);

    Task<IPagedList<Produto>> GetProdutosFiltroValorAsync(ProdutoFiltroValor produtoFiltroValor);
}
