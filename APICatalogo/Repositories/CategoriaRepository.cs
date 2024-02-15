using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {        
    }
}
