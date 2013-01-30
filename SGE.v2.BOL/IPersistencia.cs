using System;
using System.Collections.Generic;

namespace SGE.Interfaces
{
    public interface IPersistencia<T> where T : class
    {
        int inserir(T entidade);
        int alterar(T entidade, T entidadeFiltro);
        int excluir(T entidadeFiltro);
        List<T> listar(T entidadeFiltro);
        List<T> listar(T entidadeFiltro, int limiteRegistros);
        List<T> listar(T entidadeFiltro, string camposAgrupar);
        List<T> listar(T entidadeFiltro, string camposAgrupar, string camposClassificar);
        List<T> listar(T entidadeFiltro, string camposAgrupar, string camposClassificar, int limiteRegistros);
        List<T> listarClassificando(T entidadeFiltro, string camposClassificar);
    }
}
