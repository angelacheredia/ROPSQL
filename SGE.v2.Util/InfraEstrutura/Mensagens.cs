
namespace SGE.Utilitarios.Mensagens
{
    public static class MensagemInfraEstrutura
    {
        // Acesso ao banco de dados
        public const string AusenciaConfiguracaoConexao = "Configuração para conexão ao banco de dados não informada.";
        public const string CaminhoDadosNaoEncontrado = "Caminho do repositório de dados não encontrado.";
        public const string CredenciaisConexaoInvalidas = "As credenciais informadas para conexão ao banco de dados são inválidas.";
        public const string ConexaoDadosFechada = "A conexão ao banco de dados encontra-se fechada, é necessário conectar.";
        public const string InstrucaoSQLNaoInformada = "Instrução SQL não informada.";
        public const string AcaoPersistenciaInvalida = "Ação de persistência inválida.";
    }

    public static class MensagemSeguranca
    {
        // Controle de acesso
        public const string AcessoNegadoAusenciaPerfil = "Acesso não permitido.";
        public const string AcessoNegadoPerfilInsuficiente = "Permissão de acesso insuficiente para esta funcionalidade.";
    }
}