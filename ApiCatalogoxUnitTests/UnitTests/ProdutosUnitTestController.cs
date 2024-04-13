using APICatalogo.Context;
using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Interfaces;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoxUnitTests.UnitTests;

public class ProdutosUnitTestController
{
    public IUnitOfWork repository;
    public IMapper mapper;
    public static DbContextOptions<AppDbContext> dbContextOptions { get; }
    public static string connectionString =
      "Server=localhost;DataBase=apicatalogodb;Uid=root;Pwd=13Lunac@";
    static ProdutosUnitTestController()
    {
        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
           .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
           .Options;
    }
    public ProdutosUnitTestController()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ProdutoDTOMappingProfile());
        });

        mapper = config.CreateMapper();

        var context = new AppDbContext(dbContextOptions);
        repository = new UnitOfWork(context);
    }

    //criar testes de unidade

    [Fact]
    public async Task DeleteProdutoById_Return_OkResult()
    {
        // Arrange
        var controller = new ProdutosController(repository, mapper);
        var prodId = 3;

        // Act
        var result = await controller.Delete(prodId) as ActionResult<ProdutoDTO>;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<ProdutoDTO>((result.Result as OkObjectResult).Value);
    }

    [Fact]
    public async Task DeleteProdutoById_Return_NotFound()
    {
        // Arrange
        var controller = new ProdutosController(repository, mapper);
        var prodId = 999;

        // Act
        var result = await controller.Delete(prodId) as ActionResult<ProdutoDTO>;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetProdutoById_OKResult()
    {
        // Arrange
        var controller = new ProdutosController(repository, mapper);
        var prodId = 2;

        // Act
        var result = await controller.Get(prodId) as ActionResult<ProdutoDTO>;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<ProdutoDTO>((result.Result as OkObjectResult).Value);
    }

    [Fact]
    public async Task GetProdutoById_Return_NotFound()
    {
        // Arrange
        var controller = new ProdutosController(repository, mapper);
        var prodId = 999;

        // Act
        var result = await controller.Get(prodId) as ActionResult<ProdutoDTO>;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetProdutoById_Return_BadRequest()
    {
        // Arrange
        var controller = new ProdutosController(repository, mapper);
        var prodId = -1;

        // Act
        var result = await controller.Get(prodId) as ActionResult<ProdutoDTO>;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task GetProdutos_Return_ListOfProdutoDTO()
    {
        // Arrange
        var controller = new ProdutosController(repository, mapper);

        // Act
        var result = await controller.Get() as ActionResult<IEnumerable<ProdutoDTO>>;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IEnumerable<ProdutoDTO>>((result.Result as OkObjectResult).Value);
    }

    [Fact]
    public async Task GetProdutos_Return_BadRequestResult()
    {
        // Arrange
        var controller = new ProdutosController(repository, mapper);

        // Act
        var result = await controller.Get() as ActionResult<IEnumerable<ProdutoDTO>>;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IEnumerable<ProdutoDTO>>((result.Result as OkObjectResult).Value);
    }

    [Fact]
    public async Task PostProduto_Return_CreatedStatusCode()
    {
        // Arrange
        var controller = new ProdutosController(repository, mapper);
        var novoProdutoDto = new ProdutoDTO
        {
            Nome = "Novo Produto",
            Descricao = "Descrição do Novo Produto",
            Valor = 10.99m,
            ImagemUrl = "imagemfake1.jpg",
            CategoriaId = 2
        };

        // Act
        var result = await controller.Post(novoProdutoDto) as ActionResult<ProdutoDTO>;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CreatedAtRouteResult>(result.Result);
    }

    [Fact]
    public async Task PostProduto_Return_BadRequest()
    {
        // Arrange
        var controller = new ProdutosController(repository, mapper);
        ProdutoDTO prod = null;

        // Act
        var result = await controller.Post(prod) as ActionResult<ProdutoDTO>;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task PutProduto_Return_OkResult()
    {
        // Arrange
        var controller = new ProdutosController(repository, mapper);
        var prodId = 14;

        var updatedProdutoDto = new ProdutoDTO
        {
            ProdutoId = prodId,
            Nome = "Produto Atualizado - Testes",
            Descricao = "Minha Descricao",
            ImagemUrl = "imagem1.jpg",
            CategoriaId = 2
        };

        // Act
        var result = await controller.Put(prodId, updatedProdutoDto) as ActionResult<ProdutoDTO>;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task PutProduto_Return_BadRequest()
    {
        // Arrange
        var controller = new ProdutosController(repository, mapper);
        var prodId = 1000;

        var meuProduto = new ProdutoDTO
        {
            ProdutoId = 14,
            Nome = "Produto Atualizado - Testes",
            Descricao = "Minha Descricao alterada",
            ImagemUrl = "imagem11.jpg",
            CategoriaId = 2
        };

        // Act
        var result = await controller.Put(prodId, meuProduto) as ActionResult<ProdutoDTO>;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result.Result);
    }

}
