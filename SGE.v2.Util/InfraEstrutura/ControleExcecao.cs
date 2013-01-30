using System;
using System.Text;
using System.Diagnostics;
using SGE.Utilitarios.Dominio;
using SGE.Utilitarios.Conectividade;
using SGE.Utilitarios.InfraEstrutura.Idioma;
using SGE.Utilitarios.Administracao;
using SGE.Utilitarios.InfraEstrutura.Serializacao;
using SGE.Utilitarios.Monitoramento;
using SGE.Seguranca.Criptografia;

namespace SGE.Utilitarios.InfraEstrutura
{
    [Serializable]
    public class Excecao
    {
        public string Mensagem { get; set; }
        public string Rastreamento { get; set; }

        public Excecao() { }
    }

    public class ControleExcecao : IDisposable
    {
        public ControleExcecao() { }

        public string registrarExcecao(Usuario usuarioRemetente, long hashEntidadeRemetente, Exception objetoExcecao, short tipoExcecao, int nivelRegistro, string siglaIdioma)
        {
            string retornoRegistro = string.Empty;

            if (nivelRegistro > 0)
            {
                if ((NivelRegistroExcecao.Notificacao & nivelRegistro) == NivelRegistroExcecao.Notificacao)
                    retornoRegistro = notificarExcecao(objetoExcecao, siglaIdioma);

                if ((NivelRegistroExcecao.Gravacao & nivelRegistro) == NivelRegistroExcecao.Gravacao)
                    gravarExcecao(usuarioRemetente, hashEntidadeRemetente, objetoExcecao, tipoExcecao, siglaIdioma);

                if ((NivelRegistroExcecao.Armazenamento & nivelRegistro) == NivelRegistroExcecao.Armazenamento)
                    armazenarExcecao(usuarioRemetente, hashEntidadeRemetente, objetoExcecao);

                //if ((NivelRegistroExcecao.EnvioPorEmail & nivelRegistro) == NivelRegistroExcecao.EnvioPorEmail)
                    //enviarExcecao(usuarioRemetente, hashEntidadeRemetente, objetoExcecao);

                //if ((NivelRegistroExcecao.AcionamentoSuporteTecnico & nivelRegistro) == NivelRegistroExcecao.AcionamentoSuporteTecnico)
                    //acionarSuporteTecnico(usuarioRemetente, hashEntidadeRemetente, objetoExcecao);
            }
            else
                throw new ExcecaoNivelRegistroExcecaoNaoInformado(siglaIdioma);

            return retornoRegistro;
        }

        protected string notificarExcecao(Exception objetoExcecao, string siglaIdioma)
        {
            string mensagemRetorno = string.Empty;
            TradutorIdioma tradutorIdioma = new TradutorIdioma(siglaIdioma);

            mensagemRetorno = string.Format(tradutorIdioma.traduzirMensagem("MensagemExcecaoUsuario"), objetoExcecao.Message);

            return mensagemRetorno;
        }

        protected void gravarExcecao(Usuario usuarioRemetente, long hashEntidadeRemetente, Exception objetoExcecao, short tipoExcecao, string siglaIdioma)
        {
            string tituloAplicacao = string.Empty;
            string mensagemRegistro = string.Empty;
            string origemErro = string.Empty;
            EventLog registroEventoSistema = new EventLog();
            EventLogEntryType tipoEntradaRegistro = EventLogEntryType.Information;
            TradutorIdioma tradutorIdioma = new TradutorIdioma(siglaIdioma);

            mensagemRegistro = string.Format(tradutorIdioma.traduzirMensagem("MensagemExcecaoSistema"), 
                                                                                 objetoExcecao.Message,
                                                                              usuarioRemetente.Apelido,
                                                                                 hashEntidadeRemetente,
                                                                              objetoExcecao.StackTrace);

            tituloAplicacao = tradutorIdioma.traduzirTexto("SGE_TituloAplicacao");

            origemErro = string.Concat("SGE.v2 : ", objetoExcecao.Message.Substring(0, objetoExcecao.Message.Length >= 203 ? 
                                                                                       203 : 
                                                                                       objetoExcecao.Message.Length));

            if (!EventLog.SourceExists(origemErro))
                EventLog.CreateEventSource(origemErro, tituloAplicacao);

            registroEventoSistema.Source = origemErro;
            registroEventoSistema.Log = tituloAplicacao;

            switch (tipoExcecao)
            {
                case TipoExcecao.Informacao:
                    tipoEntradaRegistro = EventLogEntryType.Information;
                    break;
                case TipoExcecao.Alerta:
                    tipoEntradaRegistro = EventLogEntryType.Warning;
                    break;
                case TipoExcecao.Erro:
                    tipoEntradaRegistro = EventLogEntryType.Error;
                    break;
            }
            
            registroEventoSistema.WriteEntry(mensagemRegistro, tipoEntradaRegistro);
            registroEventoSistema.Dispose();

            tradutorIdioma = null;
        }

        protected void armazenarExcecao(Usuario usuarioRemetente, long hashEntidadeRemetente, Exception objetoExcecao)
        {
            Excecao novaExcecao = new Excecao();
            RegistroSistema registroExcecao = new RegistroSistema();
            Serializador serializador = new Serializador();

            novaExcecao.Mensagem = objetoExcecao.Message;
            novaExcecao.Rastreamento = objetoExcecao.StackTrace;

            registroExcecao.IdUsuario = usuarioRemetente.Id;
            registroExcecao.HashEntidade = hashEntidadeRemetente;
            registroExcecao.DataAcao = DateTime.Now;
            registroExcecao.Acao = AcaoRegistroSistema.RegistroExcecao;
            registroExcecao.Descricao = serializador.serializarBinario(novaExcecao);

            registroExcecao.incluir(registroExcecao);
        }

        public void Dispose() 
        {
            GC.ReRegisterForFinalize(this);
        }
    }

    public class TipoExcecao
    {
        public const short Informacao = 1;
        public const short Alerta = 2;
        public const short Erro = 3;
    }

    public class NivelRegistroExcecao
    {
        public const int Notificacao = 2;
        public const int Gravacao = 4;
        public const int Armazenamento = 8;
        public const int EnvioPorEmail = 16;
        public const int AcionamentoSuporteTecnico = 32;
    }

    public sealed class ExcecaoAusenciaConfiguracaoConexao : Exception
    {
        public ExcecaoAusenciaConfiguracaoConexao(string siglaIdioma)
        {
            TradutorIdioma tradutor = new TradutorIdioma(string.Empty);

            throw new Exception(tradutor.traduzirMensagem("AusenciaConfiguracaoConexao"));
        }
    }

    public sealed class ExcecaoCaminhoDadosNaoEncontrado : Exception
    {
        public ExcecaoCaminhoDadosNaoEncontrado(string siglaIdioma)
        {
            TradutorIdioma tradutor = new TradutorIdioma(string.Empty);

            throw new Exception(tradutor.traduzirMensagem("CaminhoDadosNaoEncontrado"));
        }
    }

    public sealed class ExcecaoCredenciaisConexaoInvalidas : Exception
    {
        public ExcecaoCredenciaisConexaoInvalidas(string siglaIdioma)
        {
            TradutorIdioma tradutor = new TradutorIdioma(string.Empty);

            throw new Exception(tradutor.traduzirMensagem("CredenciaisConexaoInvalidas"));
        }
    }

    public sealed class ExcecaoInstrucaoSQLNaoInformada : Exception
    {
        public ExcecaoInstrucaoSQLNaoInformada(string siglaIdioma)
        {
            TradutorIdioma tradutor = new TradutorIdioma(string.Empty);

            throw new Exception(tradutor.traduzirMensagem("InstrucaoSQLNaoInformada"));
        }
    }

    public sealed class ExcecaoAcaoPersistenciaInvalida : Exception
    {
        public ExcecaoAcaoPersistenciaInvalida(string siglaIdioma)
        {
            TradutorIdioma tradutor = new TradutorIdioma(string.Empty);

            throw new Exception(tradutor.traduzirMensagem("AcaoPersistenciaInvalida"));
        }
    }

    public sealed class ExcecaoNivelRegistroExcecaoNaoInformado : Exception
    {
        public ExcecaoNivelRegistroExcecaoNaoInformado(string siglaIdioma)
        {
            TradutorIdioma tradutor = new TradutorIdioma(string.Empty);

            throw new Exception(tradutor.traduzirMensagem("NivelRegistroExcecaoNaoInformado"));
        }
    }
}
