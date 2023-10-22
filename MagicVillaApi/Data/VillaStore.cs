using MagicVillaApi.Models.Dtos;

namespace MagicVillaApi.Stores
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
             new VillaDto{ Id = 1, Nombre = "San Juan" },
             new VillaDto{ Id = 2, Nombre = "Lupita" }
        }; 
    }
}
