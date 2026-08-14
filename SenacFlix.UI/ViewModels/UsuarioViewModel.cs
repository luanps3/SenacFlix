// Nome do arquivo: UsuarioViewModel.cs
// Objetivo: ViewModel para exibir dados do usuario (perfil e admin)
// Camada: UI

using System;
using System.Collections.Generic;

namespace SenacFlix.UI.ViewModels
{
    public class UsuarioViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string NomeCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateOnly DataNascimento { get; set; }
        public string? FotoPerfilUrl { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public List<string> Perfis { get; set; } = new List<string>();
        
        public string PerfisFormatados => string.Join(", ", Perfis);
    }
}
