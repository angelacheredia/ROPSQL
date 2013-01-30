//
//
//  Gerado com o plug-in C# no StarUML®
//
//  @ Projeto : SGE.v2
//  @ Nome do Arquivo : Parametro.cs
//  @ Data de Criação : 11/09/2012
//  @ Autor : Ponto Certo Consultoria
//
//

using System;
using SGE.Utilitarios.Conectividade;
using SGE.Utilitarios.Manutencao;
using SGE.Utilitarios.Dominio;
using SGE.Seguranca.ControleAcesso;

namespace SGE.Utilitarios.Administracao
{
    [Tabela(NomeTabela = "SGE_PARAMETRO")]
    [PerfilAcesso(CodigoPerfil = PerfilAcessoEntidade.Usuario)]
    [Funcionalidade(GrupoFuncionalidade = GrupoFuncionalidade.Manutencao,
                    SubGrupoFuncionalidade = ModuloSistema.Administracao,
                    NomeExibicao = FuncionalidadeSistema.ManterParametros,
                    AcessoFuncionalidade = "~/Manutencao/Administracao/Parametro.aspx")]
	public class Parametro : ParChaveValor
    {
	}
}
