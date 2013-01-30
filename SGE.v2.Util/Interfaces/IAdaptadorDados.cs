using System;
using System.Collections.Generic;

namespace SGE.Interfaces
{
    public interface IAdaptadorDados
    {
        int incluir<T>(T entidade);
        int alterar<T>(T entidade, T entidadeFiltro);
        int excluir<T>(T entidadeFiltro);
        List<object> listar<T>(T entidadeFiltro);
        List<object> listar<T>(T entidadeFiltro, int limiteRegistros);
        List<object> listar<T>(T entidadeFiltro, string camposAgrupar);
        List<object> listar<T>(T entidadeFiltro, string camposClassificar, bool ordenacaoDecrescente);
        List<object> listar<T>(T entidadeFiltro, string camposAgrupar, string camposClassificar, bool ordenacaoDecrescente);
        List<object> listar<T>(T entidadeFiltro, string camposAgrupar, string camposClassificar, int limiteRegistros, bool ordenacaoDecrescente);
        List<object> listar<T>(T entidadeFiltro, string camposExibir, string camposAgrupar, string camposClassificar, int limiteRegistros, bool ordenacaoDecrescente);
        object consultar<T>(T entidadeFiltro);
        void iniciarTransacao();
        void efetivarTransacao();
        void cancelarTransacao();
    }
}
