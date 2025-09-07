namespace MottuCrudAPI.DTO.Response
{
    public class MotoResponse
    {
        public Guid Id { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        public string Status { get; set; }
        public int Ano { get; set; }
        public Guid? PatioId { get; set; }

        public MotoResponse() { }

        public MotoResponse(Guid id, string modelo, string placa, string status, int ano, Guid? patioId)
        {
            Id = id;
            Modelo = modelo;
            Placa = placa;
            Status = status;
            Ano = ano;
            PatioId = patioId;
        }
    }
}
