//
//
//  Gerado com o plug-in C# no StarUML®
//
//  @ Projeto : SGE.v2
//  @ Nome do Arquivo : PessoaEmpresa.cs
//  @ Data de Criação : 11/09/2012
//  @ Autor : Ponto Certo Consultoria
//
//

using System;
using SGE.Interfaces;
using SGE.Utilitarios.Conectividade;
using SGE.Seguranca.ControleAcesso;
using SGE.Utilitarios.Dominio;
using SGE.Manutencao.Comum;
using SGE.Utilitarios.InfraEstrutura.Idioma;

namespace SGE.Manutencao.Administracao
{
    [Serializable]
    [AssinaturaHash(CodigoHash = RelacaoEntidadeHash.PessoaEmpresa)]
    [PerfilAcesso(CodigoPerfil = PerfilAcessoEntidade.PessoaEmpresa)]
    [Tabela(NomeTabela = "SGE_EMPRESA")]
    [Funcionalidade(GrupoFuncionalidade = GrupoFuncionalidade.Manutencao,
                    SubGrupoFuncionalidade = ModuloSistema.Administracao,
                    NomeExibicao = FuncionalidadeSistema.ManterEmpresas,
                    AcessoFuncionalidade = "~/Manutencao/Administracao/Empresa.aspx")]
    public class PessoaEmpresa : PessoaJuridica
    {
        #region Declaracoes

        bool permissao = false;

        #endregion

        #region Construtores

        public PessoaEmpresa(string chavePerfilAcesso)
        {
            permissao = ControladorAcesso.verificarPermissao(this, chavePerfilAcesso);

            if (!permissao)
                throw new ExcecaoAcessoNegado(permissao);
        }

        protected PessoaEmpresa() { }

        #endregion
    }
}
