using APICatalogo.Models;

namespace APICatalogo.DTOs.Mappings
{
    public static class ProdutoDtosMappingExtensions
    {
        public static ProdutoDTO? ToProdutoDto(this Produto produto)
        {
            if (produto is null)
                return null;
            return new ProdutoDTO
            {
                ProdutoId = produto.ProdutoId,
                Nome = produto.Nome,
                ImagemUrl = produto.ImagemUrl,
                Categoria = produto.Categoria,
                DataCadastro = produto.DataCadastro,
                Descricao = produto.Descricao,
                Estoque = produto.Estoque,
                Valor = produto.Valor
            };

        }

        public static Produto? ToProduto(this ProdutoDTO produtoDto)
        {

            if (produtoDto is null)
                return null;
            return new Produto
            {
                
                ProdutoId = produtoDto.ProdutoId,
                Nome = produtoDto.Nome,
                ImagemUrl = produtoDto.ImagemUrl,
                Categoria = produtoDto.Categoria,
                DataCadastro = produtoDto.DataCadastro,
                Descricao = produtoDto.Descricao,
                Estoque = produtoDto.Estoque,
                Valor = produtoDto.Valor
            };
        }

        public static IEnumerable<ProdutoDTO> ToProdutoDtoList(this IEnumerable<Produto> produtos)
        {
            if (produtos is null || !produtos.Any())
            {
                return Enumerable.Empty<ProdutoDTO>();
            }
            return produtos.Select(
                produto => new ProdutoDTO
                {
                    ProdutoId = produto.ProdutoId,
                    Nome = produto.Nome,
                    ImagemUrl = produto.ImagemUrl,
                    Categoria = produto.Categoria,
                    DataCadastro = produto.DataCadastro,
                    Descricao = produto.Descricao,
                    Estoque = produto.Estoque,
                    Valor = produto.Valor
                }).ToList();
        }
    }
}
