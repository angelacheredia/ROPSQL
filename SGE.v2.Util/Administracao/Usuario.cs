//
//
//  Gerado com o plug-in C# no StarUML®
//
//  @ Projeto : SGE.v2
//  @ Nome do Arquivo : Usuario.cs
//  @ Data de Criação : 11/09/2012
//  @ Autor : Ponto Certo Consultoria
//
//

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SGE.Utilitarios.Conectividade;
using SGE.Seguranca.ControleAcesso;
using SGE.Utilitarios.Dominio;

namespace SGE.Utilitarios.Administracao
{
    [Serializable]
    [AssinaturaHash(CodigoHash = RelacaoEntidadeHash.Usuario)]
    [PerfilAcesso(CodigoPerfil = PerfilAcessoEntidade.Usuario)]
    [Tabela(NomeTabela = "SGE_USUARIO")]
    [Funcionalidade(GrupoFuncionalidade = GrupoFuncionalidade.Manutencao,
                    SubGrupoFuncionalidade = ModuloSistema.Administracao,
                    NomeExibicao = FuncionalidadeSistema.ManterUsuarios,
                    AcessoFuncionalidade = "~/Manutencao/Administracao/Usuario.aspx")]
	public class Usuario : AdaptadorDados
    {
        #region Declarações
        
        [ColunaDados(NomeColuna = "ID",
                     ChavePrimaria = true,
                     AutoNumeracao = true,
                     Requerida = true)]
        public int Id { get; set; }

        [ColunaDados(NomeColuna = "ID_COLABORADOR", Requerida = true)]
        [DisplayName(IdentificacaoAtributo.Identificador)]
        [Required(ErrorMessage = ValidacaoAtributo.ObrigatoriedadeIdColaborador)]
        public int IdColaborador { get; set; }

        [ColunaDados(NomeColuna = "DT_EXPIRA_ACESSO", Requerida = true)]
        [DisplayName(IdentificacaoAtributo.DataExpiracao)]
        [Required(ErrorMessage = ValidacaoAtributo.ObrigatoriedadeDataExpiracao)]
        public DateTime DataExpiracao { get; set; }

        [ColunaDados(NomeColuna = "TX_APELIDO", Requerida = true)]
        [DisplayName(IdentificacaoAtributo.Apelido)]
        [Required(ErrorMessage = ValidacaoAtributo.ObrigatoriedadeApelido)]
        [StringLength(30, ErrorMessage = "A")]
        public string Apelido { get; set; }

        [ColunaDados(NomeColuna = "TX_SENHA", Requerida = true)]
        [DisplayName(IdentificacaoAtributo.Senha)]
        [Required(ErrorMessage = ValidacaoAtributo.ObrigatoriedadeSenha)]
        public string Senha { get; set; }

        [ColunaDados(NomeColuna = "TX_CERTIFICADO")]
        [DisplayName(IdentificacaoAtributo.Certificado)]
        [StringLength(2048, ErrorMessage = "A")]
        public string Certificado { get; set; }

        [ColunaDados(NomeColuna = "BN_PERFIL_ACESSO")]
        [DisplayName(IdentificacaoAtributo.PerfilAcesso)]
        [Required(ErrorMessage = ValidacaoAtributo.ObrigatoriedadePerfilAcesso)]
        public string PerfilAcesso { get; set; }

        [ColunaDados(NomeColuna = "NR_TENTATIVAS_ACESSO", Requerida = true)]
        [DisplayName(IdentificacaoAtributo.TentativasAcesso)]
        public int TentativasAcesso { get; set; }

        #endregion
	}
}
