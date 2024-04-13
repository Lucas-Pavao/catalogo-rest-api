using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    IEnumerable<Produto> GetProdutosPorCategoria(int id);
    PagedList<Produto> GetProdutos(ProdutosParameters produtosParams);

    PagedList<Produto> GetProdutosFiltroValor(ProdutoFiltroValor produtoFiltroValor);
}
