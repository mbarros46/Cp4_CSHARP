namespace MottuCrudAPI.DTO.Response
{
    public class PatioResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public int Capacidade { get; set; }
        public int OcupacaoAtual { get; set; }
    }
}
