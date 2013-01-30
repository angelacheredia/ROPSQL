using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Utilitarios.Dominio
{
    public class AcaoEntidade
    {
        public const string Pesquisar = "SGE_AcaoEntidadePreliminar_Pesquisar";
        public const string Consultar = "SGE_AcaoEntidadePreliminar_Consultar";
        public const string Incluir = "SGE_AcaoEntidadePreliminar_Incluir";
        public const string Alterar = "SGE_AcaoEntidadeSelecao_Alterar";
        public const string Excluir = "SGE_AcaoEntidadeSelecao_Excluir";
        public const string Gravar = "SGE_AcaoEntidadeEfetiva_Gravar";
        public const string CancelarEdicao = "SGE_AcaoEntidadeEfetiva_CancelarEdicao";
    }

    public class TipoAcaoEntidade
    {
        public const string Preliminar = "SGE_AcaoEntidadePreliminar";
        public const string Efetiva = "SGE_AcaoEntidadeEfetiva";
        public const string Selecao = "SGE_AcaoEntidadeSelecao";
    }
}
