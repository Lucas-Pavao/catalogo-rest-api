using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams)
    {
        var produtos = await GetAllAsync();
        var produtosOrdenados = produtos.OrderBy(x => x.ProdutoId).AsQueryable();
        var resultado = await produtosOrdenados.ToPagedListAsync( produtosParams.PageNumber, produtosParams.PageSize);
        return resultado;

    }

    public async Task<IPagedList<Produto>> GetProdutosFiltroValorAsync(ProdutoFiltroValor produtoFiltroValor)
    {
        var produtos = await GetAllAsync();

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
        var produtosFiltrados = await produtos.ToPagedListAsync(produtoFiltroValor.PageNumber, produtoFiltroValor.PageSize);
        return produtosFiltrados;
    }

    public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
    {
        var produtos = await GetAllAsync();
        return produtos.Where(c => c.CategoriaId == id);
    }

    
}