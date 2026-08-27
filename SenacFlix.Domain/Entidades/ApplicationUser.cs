// ============================================================
// Nome:         ApplicationUser.cs
// Objetivo:     Representa o usuario autenticado do sistema SenacFlix,
//               estendendo a classe base do ASP.NET Core Identity com
//               informacoes de perfil proprias da plataforma.
// Camada:       Domain (Entidades)
// Participa em: Identity, autenticacao, favoritos, auditoria.
//               E a entidade central que representa quem esta logado.
// ============================================================

using Microsoft.AspNetCore.Identity; // Importa a classe base IdentityUser do ASP.NET Core Identity

namespace SenacFlix.Domain.Entidades // Define o namespace da camada de dominio, pasta Entidades
{
    /// <summary>
    /// Entidade que representa o usuario da plataforma SenacFlix.
    /// Herda de IdentityUser, que ja fornece campos como Id, Email,
    /// UserName, PasswordHash, PhoneNumber, entre outros gerenciados pelo Identity.
    /// </summary>
    public class ApplicationUser : IdentityUser // Herda de IdentityUser para aproveitar toda a infraestrutura de autenticacao
    {
        // --------------------------------------------------------
        // Propriedades adicionais ao IdentityUser padrao
        // --------------------------------------------------------

        /// <summary>
        /// Nome completo do usuario, como aparecera na plataforma.
        /// Campo obrigatorio para identificacao visual do assinante.
        /// </summary>
        public required string NomeCompleto { get; set; } // required garante que o campo seja preenchido na criacao do objeto

        /// <summary>
        /// Data de nascimento do usuario.
        /// Usada para validar a classificacao indicativa dos filmes.
        /// DateOnly representa apenas a data, sem horario.
        /// </summary>
        public DateOnly DataNascimento { get; set; } // DateOnly e o tipo correto para datas sem componente de hora (.NET 6+)

        /// <summary>
        /// URL da foto de perfil do usuario (imagem de avatar).
        /// Pode ser nulo caso o usuario nao tenha configurado uma foto.
        /// </summary>
        public string? FotoPerfilUrl { get; set; } // string? (nullable) pois o campo e opcional

        /// <summary>
        /// Indica se o usuario esta ativo no sistema.
        /// Quando false, o usuario nao consegue se autenticar (exclusao logica).
        /// </summary>
        public bool Ativo { get; set; } = true; // Inicializado como true: todo usuario e ativo ao ser cadastrado

        /// <summary>
        /// Data e hora em que o cadastro foi realizado.
        /// Preenchido automaticamente no momento do registro.
        /// </summary>
        public DateTime DataCadastro { get; set; } // Armazena o instante exato do cadastro do usuario

        /// <summary>
        /// Data e hora da ultima atualizacao nos dados do usuario.
        /// Null caso nunca tenha sido atualizado apos o cadastro inicial.
        /// </summary>
        public DateTime? DataAtualizacao { get; set; } // Nullable pois so e preenchido quando houver atualizacao

        /// <summary>
        /// Data e hora da exclusao logica do usuario.
        /// Quando preenchida, indica que o usuario foi "removido" sem apagar do banco.
        /// </summary>
        public DateTime? DataExclusao { get; set; } // Nullable pois so e preenchido quando o usuario for desativado/excluido
    }
}
