using ApiCatalogo.Models;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;

namespace ApiCatalogo.DTO.Mappings
{
    public static class CategoriaDtoMappingExtensions
    {
        public static CategoriaDto? ToCategoriaDto(this Categoria categoria)
        {
            if(categoria == null) return null;

            return new CategoriaDto()
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            };

        }

        public static Categoria? ToCategoria (this CategoriaDto categoriaDto)

        { 
            if (categoriaDto == null)
            {
                return null;
            }

            return new Categoria()
            {
                Id = categoriaDto.Id,
                Nome = categoriaDto.Nome,
                ImagemUrl = categoriaDto.ImagemUrl

            };

        }

        public static IEnumerable<CategoriaDto> toCategoriaDtoList (this IEnumerable<Categoria> categorias)
        {
            if(categorias == null || !categorias.Any())
            {
                return new List<CategoriaDto>();
            }

            return categorias.Select(categoria => new CategoriaDto
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            }).ToList();
        }

    }
}
