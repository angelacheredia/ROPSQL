//
//
//  Gerado com o plug-in C# no StarUML®
//
//  @ Projeto : SGE.v2
//  @ Nome do Arquivo : BancoDados.cs
//  @ Data de Criação : 11/09/2012
//  @ Autor : Ponto Certo Consultoria
//
//

using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Xml;
using System.Text;
using SGE.Seguranca.Criptografia;
using SGE.Utilitarios.InfraEstrutura;
using SGE.Utilitarios.InfraEstrutura.Idioma;

namespace SGE.Conectividade.BancoDados
{
	public abstract class ConexaoBancoDados : ControleExcecao
    {
        #region Declarações

	    readonly string _configConexao;
        readonly string _siglaIdioma;

        //readonly SqlCeConnection _conexaoC;
        //SqlCeTransaction _controleTransacaoC;

        private readonly SqlConnection _conexao;
        SqlTransaction _controleTransacao;
        
        #endregion

        #region Construtores

	    protected ConexaoBancoDados()
        {
            _configConexao = new Criptografador().decriptografarTexto(
                        ConfigurationManager.ConnectionStrings["SGE_ConnStr"].ConnectionString);

            _siglaIdioma = ConfigurationManager.AppSettings["Idioma"];

            //_conexaoC = new SqlCeConnection(_configConexao);
            _conexao = new SqlConnection(_configConexao);

            _controleTransacao = null;
        }

        #endregion

        #region Métodos Públicos

        protected bool conectar()
        {
            if (!string.IsNullOrEmpty(this._configConexao))
            {
                if (_conexao.State != ConnectionState.Open)
                {
                    _conexao.ConnectionString = this._configConexao;

                    _conexao.Open();
                }
            }
            else
                throw new ExcecaoAusenciaConfiguracaoConexao(_siglaIdioma);

            return (_conexao.State == ConnectionState.Open);
		}

		protected bool desconectar()
        {
            if (_conexao.State == System.Data.ConnectionState.Open)
                _conexao.Close();

            return (_conexao.State == ConnectionState.Closed);
		}

		public void iniciarTransacao()
        {
            if (_conexao.State == System.Data.ConnectionState.Open)
                this._controleTransacao = _conexao.BeginTransaction();
		}

		public void efetivarTransacao()
        {
            if ((_conexao.State == ConnectionState.Open)
                && (this._controleTransacao != null))
                this._controleTransacao.Commit();
		}

		public void cancelarTransacao()
        {
            if ((_conexao.State == ConnectionState.Open)
                && (this._controleTransacao != null))
                this._controleTransacao.Rollback();
        }

        protected int executarComando(string instrucaoSQL, Dictionary<string, object> parametros)
        {
            //SqlCeCommand comandoSQLC;
            SqlCommand comandoSQL;

            int registrosAfetados = 0;

            if (_conexao.State == ConnectionState.Open)
            {
                comandoSQL = _conexao.CreateCommand();
                comandoSQL.CommandText = instrucaoSQL;

                comandoSQL.Parameters.Clear();
                foreach (var parametro in parametros)
                {
                    SqlParameter novoParametroSQL = new SqlParameter(parametro.Key, parametro.Value);
                    comandoSQL.Parameters.Add(novoParametroSQL);
                }

                if (_controleTransacao != null)
                    comandoSQL.Transaction = _controleTransacao;

                registrosAfetados = comandoSQL.ExecuteNonQuery();
                comandoSQL = null;
            }

            return registrosAfetados;
        }

        protected XmlDocument efetuarPesquisa_SQLC(string instrucaoSQL)
        {
            SqlCeCommand comandoSQLC = null;
            SqlCeDataAdapter adaptadorSQLC = null;

            DataSet tabelasDadosC = new DataSet();
            StringBuilder textoXMLC = new StringBuilder();
            XmlDocument estruturaRetornoC = new XmlDocument();

            if (_conexao.State == System.Data.ConnectionState.Open)
            {
                XmlWriter gravadorXMLC = XmlWriter.Create(textoXMLC);

                //comandoSQL = _conexao.CreateCommand();
                comandoSQLC.CommandText = instrucaoSQL;
                adaptadorSQLC = new SqlCeDataAdapter(comandoSQLC);

                adaptadorSQLC.Fill(tabelasDadosC);

                tabelasDadosC.WriteXml(gravadorXMLC);

                estruturaRetornoC.LoadXml(textoXMLC.ToString());

                comandoSQLC = null;
            }

            return estruturaRetornoC;
        }

        protected XmlDocument efetuarPesquisa_SQL(string instrucaoSQL)
        {
            SqlCommand comandoSQL = null;
            SqlDataAdapter adaptadorSQL = null;

            DataSet tabelasDados = new DataSet();
            StringBuilder textoXML = new StringBuilder();
            XmlDocument estruturaRetorno = new XmlDocument();

            if (_conexao.State == System.Data.ConnectionState.Open)
            {
                XmlWriter gravadorXML = XmlWriter.Create(textoXML);

                comandoSQL = _conexao.CreateCommand();
                comandoSQL.CommandText = instrucaoSQL;

                adaptadorSQL = new SqlDataAdapter(comandoSQL);

                adaptadorSQL.Fill(tabelasDados);

                tabelasDados.WriteXml(gravadorXML);

                estruturaRetorno.LoadXml(textoXML.ToString());

                comandoSQL = null;
            }

            return estruturaRetorno;
        }

        #endregion
    }
}
