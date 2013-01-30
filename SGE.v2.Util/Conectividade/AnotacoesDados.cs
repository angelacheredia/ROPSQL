using System;

namespace SGE.Utilitarios.Conectividade
{
    public class AssinaturaHash : Attribute
    {
        public long CodigoHash;
    }

    public class Tabela : Attribute
    {
        #region Declarações

        public string NomeTabela;

        #endregion
    }

    public class ColunaDados : Attribute
    {
        #region Declarações

        public bool ChavePrimaria;
        public bool AutoNumeracao;
        public bool Requerida;
        public string NomeColuna;

        #endregion
    }

    public enum TipoJuncaoRelacional
    {
        #region Declarações

        Obrigatoria = 1,
        Opcional = 2

        #endregion
    }

    public class ColunaRelacional : Attribute
    {
        #region Declarações

        public string NomeTabela;
        public string NomeColuna;
        public string ColunaChave;
        public string ColunaChaveEstrangeira;
        public TipoJuncaoRelacional TipoJuncao;

        #endregion
    }

    public enum TipoAgregacaoDados
    {
        #region Declarações

        Soma = 1,
        Contagem = 2,
        Minimo = 3,
        Maximo = 4,
        Media = 5

        #endregion
    }

    public class ColunaDadosAgregados : Attribute
    {
        #region Declarações

        public string NomeColuna;
        public string ApelidoColuna;
        public TipoAgregacaoDados TipoAgregacao;

        #endregion
    }

    public class EntidadeRelacionada : Attribute 
    {
        #region Declarações

        public string AtributoChaveEstrangeira;

        #endregion
    }

    public class Funcionalidade : Attribute
    {
        #region Declaracoes

        public string GrupoFuncionalidade = string.Empty;
        public string SubGrupoFuncionalidade = string.Empty;
        public string NomeExibicao = string.Empty;
        public string AcessoFuncionalidade = string.Empty;
        public bool AcessivelUsuario = true;
        public bool DemonstraResultado = true;
        public bool ParticipaFluxo = false;
        public bool RealizaIntegracao = false;

        #endregion
    }
}
