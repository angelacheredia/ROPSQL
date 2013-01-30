//
//
//  Gerado com o plug-in C# no StarUML®
//
//  @ Projeto : SGE.v2
//  @ Nome do Arquivo : RegistroSistema.cs
//  @ Data de Criação : 11/09/2012
//  @ Autor : Ponto Certo Consultoria
//
//

using System;
using System.ComponentModel.DataAnnotations;
using SGE.Utilitarios.Dominio;
using SGE.Utilitarios.Conectividade;
using SGE.Utilitarios.InfraEstrutura;
using SGE.Seguranca.ControleAcesso;

namespace SGE.Utilitarios.Monitoramento
{
    [Serializable]
    [Tabela(NomeTabela = "SGE_REGISTRO")]
	public class RegistroSistema : AdaptadorDados
    {
        [ColunaDados(NomeColuna = "ID",
                     ChavePrimaria = true,
                     AutoNumeracao = true,
                     Requerida = true)]
		public long Id  { get; set; }

        [ColunaDados(NomeColuna = "ID_USUARIO", Requerida = true)]
        public int IdUsuario { get; set; }

        [ColunaDados(NomeColuna = "NR_HASH_ENTIDADE", Requerida = true)]
        public long HashEntidade { get; set; }

        [ColunaDados(NomeColuna = "DT_ACAO", Requerida = true)]
		public DateTime DataAcao  { get; set; }

        [ColunaDados(NomeColuna = "NR_ACAO", Requerida = true)]
		public int Acao  { get; set; }
        
        [ColunaDados(NomeColuna = "TX_DESCRICAO", Requerida = true)]
		public string Descricao  { get; set; }
	}

    public class AcaoRegistroSistema
    {
        #region Declarações

        public const int Consulta = 1;
        public const int Listagem = 2;
        public const int Inclusao = 3;
        public const int Alteracao = 4;
        public const int Exclusao = 5;
        public const int Impressao = 6;
        public const int Exportacao = 7;
        public const int Aprovacao = 8;
        public const int EnvioIntegracao = 9;
        public const int RecebimentoIntegracao = 10;
        public const int RegistroExcecao = 11;
        
        #endregion
    }
}
