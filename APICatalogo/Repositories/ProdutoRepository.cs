using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    public PagedList<Produto> GetProdutos(ProdutosParameters produtosParams)
    {
        var produtos = GetAll().OrderBy(x => x.ProdutoId).AsQueryable();
        var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParams.PageNumber, produtosParams.PageSize);
        return produtosOrdenados;

    }

    public PagedList<Produto> GetProdutosFiltroValor(ProdutoFiltroValor produtoFiltroValor)
    {
        var produtos = GetAll().AsQueryable();

        if (produtoFiltroValor.Valor.HasValue && !string.IsNullOrEmpty(produtoFiltroValor.ValorCriterio))
        {
            if (produtoFiltroValor.ValorCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Valor > produtoFiltroValor.Valor.Value).OrderBy(p => p.Valor);
            }
            else if (produtoFiltroValor.ValorCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Valor < produtoFiltroValor.Valor.Value).OrderBy(p => p.Valor);
            }
            else if (produtoFiltroValor.ValorCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Valor == produtoFiltroValor.Valor.Value).OrderBy(p => p.Valor);
            }
        }
        var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, produtoFiltroValor.PageNumber,
                                                                                              produtoFiltroValor.PageSize);
        return produtosFiltrados;
    }

    public IEnumerable<Produto> GetProdutosPorCategoria(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
    }

    
}