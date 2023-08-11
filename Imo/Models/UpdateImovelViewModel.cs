namespace Imo.Models
{
    public class UpdateImovelViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string Localizacao { get; set; }
        public string Preco { get; set; }
        public string Descricao { get; set; }
        public string ContactoFone { get; set; }
        public string ContactoEmail { get; set; }
        public string TipoNegocio { get; set; }
        public DateTime? Date { get; set; }
    }
}
