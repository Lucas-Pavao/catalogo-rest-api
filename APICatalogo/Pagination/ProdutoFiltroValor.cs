namespace APICatalogo.Pagination
{
    public class ProdutoFiltroValor : QuaryStringParameters
    {
        public decimal? Valor { get; set; }
        public string? ValorCriterio { get; set; }

    }
}
