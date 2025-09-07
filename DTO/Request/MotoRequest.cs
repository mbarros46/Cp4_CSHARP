namespace MottuCrudAPI.DTO.Request
{
    public class MotoRequest
    {
        public string Modelo { get; set; }
        public string Placa { get; set; }
        public string Status { get; set; }
        public int Ano { get; set; }
        public Guid? PatioId { get; set; }

        public MotoRequest() { }

        public MotoRequest(string modelo, string placa, string status, int ano, Guid? patioId = null)
        {
            Modelo = modelo;
            Placa = placa;
            Status = status;
            Ano = ano;
            PatioId = patioId;
        }
    }
}
