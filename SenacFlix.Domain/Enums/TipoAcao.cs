// ============================================================
// Nome:         TipoAcao.cs
// Objetivo:     Enumera os tipos de acoes possiveis que podem ser
//               registradas no log de auditoria do SenacFlix.
//               Garante consistencia nos valores gravados no banco.
// Camada:       Domain (Enums)
// Participa em: Auditoria.cs usa este enum (convertido para string).
//               A camada de aplicacao referencia os valores ao registrar eventos.
// ============================================================

namespace SenacFlix.Domain.Enums
{
    public enum TipoAcao
    {
        ///<summary>
        ///Indica que um novo regitro foi criado no sistema.
        ///Exemplos: novo filme cadastrado, nova categoria criada.
        ///</summary>
        Criacao = 1, // Valor 1: explícito para evitar ambiguidade com valor padrão 0

        ///<summary>
        ///Indica que um registro existente foi alterado no sistema.
        ///Exemplos: titulo do filme editado, descrição 
        ///da categoria atualizada.
        ///</summary>
        Atualizacao = 2, // Valor 2: segunda operação mais comum após a criação

        ///<summary>
        ///Indica que um registro existente foi desativado no sistema.
        ///Exemplos: Um filme desativado do catálogo > O dado fica no banco mas não visivel para os usuários
        ///da categoria atualizada.
        ///</summary>
        ExclusaoLogica = 3, // Valor 3: Exclusão lógica (Ativo = false, DataExclusao preenchida).

        ///<summary>
        ///Indica que um registro existente foi excluído no sistema.
        ///Exemplos: Um filme excluido permanentemente do catálogo.
        ///</summary>
        ExclusaoPermanente = 4, // Valor 4: Exclusão Física(DELETE SQL), irreversível.

        ///<summary>
        ///Indica que um usuário realizou login no sistema.
        ///Registrado para monitoramento de acessos e detecção de anomalias.
        /// </summary>
        Login = 5, // Valor 5: evento de autenticação bem-sucedida.

        ///<summary>
        ///Indica que um usuário realizou logout(saiu) do sistema.
        ///Registrado para controle de sessões ativas e auditoria de acessos.
        /// </summary>
        Logout = 6, // Valor 6: evento de encerramento de sessão

    }
}
