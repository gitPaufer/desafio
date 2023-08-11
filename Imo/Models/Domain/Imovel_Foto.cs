using MessagePack;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imo.Models.Domain
{
    public class Imovel_Foto
    {
        public Guid Id { get; set; }
        public string IdImovel { get; set; }
        public string ContentType { get; set; }
        public string DescricaoFoto { get; set; }
        public string PathFoto { get; set; }
        public DateTime? Date { get; set; }
        public int? IdUser { get; set; }

      //  public Imovel Imovel { get; set; }
    }
}
