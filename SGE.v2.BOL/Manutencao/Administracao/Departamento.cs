//
//
//  Gerado com o plug-in C# no StarUML®
//
//  @ Projeto : SGE.v2
//  @ Nome do Arquivo : Departamento.cs
//  @ Data de Criação : 11/09/2012
//  @ Autor : Ponto Certo Consultoria
//
//

using System;
using SGE.Utilitarios.Conectividade;
using SGE.Seguranca.ControleAcesso;
using SGE.Utilitarios.Dominio;

namespace SGE.Manutencao.Administracao
{
    [Serializable]
    [AssinaturaHash(CodigoHash = RelacaoEntidadeHash.Departamento)]
    [PerfilAcesso(CodigoPerfil = PerfilAcessoEntidade.Departamento)]
    [Tabela(NomeTabela = "SGE_DEPARTAMENTO")]
    [Funcionalidade(GrupoFuncionalidade = GrupoFuncionalidade.Manutencao,
                    SubGrupoFuncionalidade = ModuloSistema.Administracao,
                    NomeExibicao = FuncionalidadeSistema.ManterDepartamentos,
                    AcessoFuncionalidade = "~/Manutencao/Administracao/Departamento.aspx")]
	public class Departamento : AdaptadorDados
    {
        [ColunaDados(NomeColuna = "ID",
                     ChavePrimaria = true,
                     AutoNumeracao = true,
                     Requerida = true)]
		public int Id ;

        [ColunaDados(NomeColuna = "ID_EMPRESA", Requerida = true)]
        public int IdEmpresa;

        [ColunaDados(NomeColuna = "TX_NOME", Requerida = true)]
		public string Nome ;
	}
}
