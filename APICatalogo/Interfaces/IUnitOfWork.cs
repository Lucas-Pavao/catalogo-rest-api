namespace APICatalogo.Interfaces
{
    public interface IUnitOfWork
    {
        IProdutoRepository produtoRepository { get; }
        ICategoriaRepository categoriaRepository { get; }

        void Commit ();

    }
}
