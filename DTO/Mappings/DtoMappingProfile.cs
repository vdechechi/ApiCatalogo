using ApiCatalogo.Models;
using ApiCatalogo.Repositorys.Produtos;
using AutoMapper;

namespace ApiCatalogo.DTO.Mappings
{
    public class DtoMappingProfile : Profile
    {

        public DtoMappingProfile()
        {
            CreateMap<Produto, ProdutoDto>().ReverseMap();
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Produto, ProdutoDtoUpdateRequest>().ReverseMap();   
            CreateMap<Produto, ProdutoDtoUpdateResponse>().ReverseMap();
        }
    }
}
