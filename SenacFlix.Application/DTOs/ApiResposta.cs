// Nome do arquivo: ApiResposta.cs
// Objetivo: DTO padronizado para respostas da API.
// Camada: Application
// Como participa: Todas as respostas da API vao utilizar este wrapper para garantir um formato consistente de retorno.

using System.Collections.Generic;

namespace SenacFlix.Application.DTOs
{
    // Classe generica para resposta da API
    public class ApiResposta<T>
    {
        // Indica se a operacao foi bem-sucedida
        public bool Sucesso { get; set; }

        // Mensagem descritiva do resultado da operacao
        public string Mensagem { get; set; } = string.Empty;

        // Dados retornados pela operacao (pode ser nulo em caso de erro)
        public T? Dados { get; set; }

        // Lista de erros de validacao ou regras de negocio
        public List<string>? Erros { get; set; }

        // Metodo estatico para retornar sucesso com dados
        public static ApiResposta<T> Ok(T dados, string mensagem = "Operacao realizada com sucesso.")
        {
            return new ApiResposta<T>
            {
                Sucesso = true,
                Mensagem = mensagem,
                Dados = dados
            };
        }

        // Metodo estatico para retornar falha sem erros especificos
        public static ApiResposta<T> Falha(string mensagem)
        {
            return new ApiResposta<T>
            {
                Sucesso = false,
                Mensagem = mensagem
            };
        }

        // Metodo estatico para retornar falha com lista de erros (ex: validacao de campos)
        public static ApiResposta<T> FalhaValidacao(List<string> erros, string mensagem = "Erro de validacao.")
        {
            return new ApiResposta<T>
            {
                Sucesso = false,
                Mensagem = mensagem,
                Erros = erros
            };
        }
    }
}
