using System;

namespace MottuCrudAPI.Persistense
{
    public class Patio
    {
        public Guid Id { get; private set; }

        public string Nome { get; private set; }

        public string Endereco { get; private set; }

        public int Capacidade { get; private set; }

        public int OcupacaoAtual { get; private set; }

        private Patio(string nome, string endereco, int capacidade)
        {
            ValidateNome(nome);
            ValidateEndereco(endereco);
            ValidateCapacidade(capacidade);

            Nome = nome;
            Endereco = endereco;
            Capacidade = capacidade;
            OcupacaoAtual = 0;
        }

        public static Patio Create(string nome, string endereco, int capacidade)
        {
            return new Patio(nome, endereco, capacidade);
        }

        private void ValidateNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.");
        }

        private void ValidateEndereco(string endereco)
        {
            if (string.IsNullOrWhiteSpace(endereco))
                throw new ArgumentException("Endereço é obrigatório.");
        }

        private void ValidateCapacidade(int capacidade)
        {
            if (capacidade <= 0)
                throw new ArgumentException("Capacidade deve ser maior que zero.");
        }

        public void AtualizarPatio(string nome, string endereco, int capacidade)
        {
            ValidateNome(nome);
            ValidateEndereco(endereco);
            ValidateCapacidade(capacidade);

            if (capacidade < OcupacaoAtual)
                throw new InvalidOperationException("Nova capacidade não pode ser menor que a ocupação atual.");

            Nome = nome;
            Endereco = endereco;
            Capacidade = capacidade;
        }

        public void IncrementarOcupacao(int quantidade = 1)
        {
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade para incrementar deve ser maior que zero.");

            if (OcupacaoAtual + quantidade > Capacidade)
                throw new InvalidOperationException("Capacidade máxima do pátio excedida.");

            OcupacaoAtual += quantidade;
        }

        public void DecrementarOcupacao(int quantidade = 1)
        {
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade para decrementar deve ser maior que zero.");

            if (OcupacaoAtual - quantidade < 0)
                throw new InvalidOperationException("Ocupação atual não pode ser negativa.");

            OcupacaoAtual -= quantidade;
        }
    }
}
