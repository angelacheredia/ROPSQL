//
//
//  Gerado com o plug-in C# no StarUML®
//
//  @ Projeto : SGE.v2
//  @ Nome do Arquivo : ParChaveValor.cs
//  @ Data de Criação : 11/09/2012
//  @ Autor : Ponto Certo Consultoria
//
//

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SGE.Interfaces;
using SGE.Utilitarios;
using SGE.Utilitarios.Conectividade;
using SGE.Utilitarios.Dominio;

namespace SGE.Utilitarios.Manutencao
{
    [Serializable]
	public class ParChaveValor : AdaptadorDados
    {
        #region Construtores

	    public ParChaveValor() {}
        
        #endregion

        #region Declarações
        
        [ColunaDados(NomeColuna="ID",
                     ChavePrimaria=true,
                     AutoNumeracao=true)]
        [DisplayName(IdentificacaoAtributo.Identificador)]
        public int Id { get; set; }
        
        [ColunaDados(NomeColuna="TX_DESCRICAO")]
        [DisplayName(IdentificacaoAtributo.Descricao)]
        [Required(ErrorMessage = ValidacaoAtributo.ObrigatoriedadeDescricao)]
        public string Descricao { get; set; }

        #endregion
	}
}
