using ApiCatalogo.Models;
using AutoMapper;

namespace ApiCatalogo.DTO.Mappings
{
    public class DtoMappingProfile : Profile
    {

        public DtoMappingProfile()
        {
            CreateMap<Produto, ProdutoDto>().ReverseMap();
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
        }
    }
}
