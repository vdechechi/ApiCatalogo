namespace ApiCatalogo.Repositorys.Produtos
{
    public interface IProdutosRepository
    {

        ICollection<Produto> GetProdutos();
        Produto GetProduto(int id);
        Produto Create(Produto produto);   
        Produto Update(Produto produto);   
        Produto Delete(int id);

    }
}
