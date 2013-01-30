using System;
using System.Collections;
using System.Collections.Generic;

namespace SGE.Interfaces
{
    public interface IPersistencia
    {
        int incluir(object entidade, Type tipoEntidade);
        int alterar(object entidade, object entidadeFiltro, Type tipoEntidade);
        int excluir(object entidadeFiltro, Type tipoEntidade);
        object consultar(object entidadeFiltro, Type tipoEntidade, bool cargaComposicao);
        IList listar(object entidadeFiltro, Type tipoEntidade, int limiteRegistros, string atributosExibir, string atributosAgrupar, string atributosOrdenar, bool ordenacaoDecrescente, bool cargaComposicao);
        void iniciarTransacao();
        void efetivarTransacao();
        void cancelarTransacao();
    }
}
