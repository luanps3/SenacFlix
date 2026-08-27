// ============================================================
// Nome:         TipoAcao.cs
// Objetivo:     Enumera os tipos de acoes possiveis que podem ser
//               registradas no log de auditoria do SenacFlix.
//               Garante consistencia nos valores gravados no banco.
// Camada:       Domain (Enums)
// Participa em: Auditoria.cs usa este enum (convertido para string).
//               A camada de aplicacao referencia os valores ao registrar eventos.
// ============================================================

namespace SenacFlix.Domain.Enums // Define o namespace da camada de dominio, pasta Enums
{
    /// <summary>
    /// Enumeracao que define os tipos de acoes auditaveis no sistema SenacFlix.
    /// Cada valor representa uma operacao distinta que pode ser monitorada.
    /// </summary>
    public enum TipoAcao // enum: tipo por valor que define constantes nomeadas
    {
        /// <summary>
        /// Indica que um novo registro foi criado no sistema.
        /// Exemplos: novo filme cadastrado, nova categoria criada.
        /// </summary>
        Criacao = 1, // Valor 1 explicito para evitar ambiguidade com o valor padrao 0

        /// <summary>
        /// Indica que um registro existente foi alterado.
        /// Exemplos: titulo do filme editado, descricao da categoria atualizada.
        /// </summary>
        Atualizacao = 2, // Valor 2: segunda operacao mais comum apos a criacao

        /// <summary>
        /// Indica que um registro foi desativado (soft delete / exclusao logica).
        /// O dado permanece no banco, mas fica invisivel para os usuarios.
        /// </summary>
        ExclusaoLogica = 3, // Valor 3: exclusao logica (Ativo = false, DataExclusao preenchida)

        /// <summary>
        /// Indica que um registro foi removido fisicamente do banco de dados.
        /// Operacao irreversivel; usada apenas por administradores em casos especificos.
        /// </summary>
        ExclusaoPermanente = 4, // Valor 4: exclusao fisica (DELETE SQL); irreversivel

        /// <summary>
        /// Indica que um usuario realizou login com sucesso na plataforma.
        /// Registrado para monitoramento de acessos e deteccao de anomalias.
        /// </summary>
        Login = 5, // Valor 5: evento de autenticacao bem-sucedida

        /// <summary>
        /// Indica que um usuario encerrou a sessao na plataforma (logout).
        /// Registrado para controle de sessoes ativas e auditoria de acesso.
        /// </summary>
        Logout = 6 // Valor 6: evento de encerramento de sessao
    }
}
