using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using SGE.Interfaces;
using SGE.Utilitarios.Dominio;
using SGE.Utilitarios.InfraEstrutura;

namespace SGE.Utilitarios.Conectividade
{
    public class AdaptadorDados : ControleExcecao
    {
        #region Declarações

            IPersistencia _persistencia = null;

        #endregion

        #region Construtores

            public AdaptadorDados()
            {
                if (CacheInstanciaDAL.Persistencia == null)
                {
                    _persistencia = (IPersistencia)Activator.CreateInstanceFrom("..\\..\\..\\..\\..\\..\\WINDOWS\\SGE.v2.DAL.dll",
                                                                                "SGE.Conectividade.Persistencia.PersistenciaDados").Unwrap();
                    CacheInstanciaDAL.Persistencia = _persistencia;
                }
                else
                    _persistencia = (IPersistencia)CacheInstanciaDAL.Persistencia;
            }

        #endregion

        #region Métodos Públicos

            [DisplayName("SGE_AcaoEntidadePreliminar_Incluir")]
            public int incluir(object entidade)
            {
                return _persistencia.incluir(entidade, entidade.GetType());
            }

            [DisplayName("SGE_AcaoEntidadePreliminar_Alterar")]
            public int alterar(object entidade, object entidadeFiltro)
            {
                return _persistencia.alterar(entidade, entidadeFiltro, entidade.GetType());
            }

            [DisplayName("SGE_AcaoEntidadePreliminar_Excluir")]
            public int excluir(object entidadeFiltro)
            {
                return _persistencia.excluir(entidadeFiltro, entidadeFiltro.GetType());
            }

            [DisplayName("SGE_AcaoEntidadePreliminar_Consultar")]
            public object consultar(object entidadeFiltro, bool cargaComposicao)
            {
                return _persistencia.consultar(entidadeFiltro, entidadeFiltro.GetType(), cargaComposicao);
            }

            [DisplayName("SGE_AcaoEntidadePreliminar_Pesquisar")]
            public IList listar(object entidadeFiltro, bool cargaComposicao)
            {
                return _persistencia.listar(entidadeFiltro, entidadeFiltro.GetType(), 0, string.Empty, string.Empty, string.Empty, false, cargaComposicao);
            }

            public IList listar(object entidadeFiltro, int limiteRegistros, bool cargaComposicao)
            {
                return _persistencia.listar(entidadeFiltro, entidadeFiltro.GetType(), limiteRegistros, string.Empty, string.Empty, string.Empty, false, cargaComposicao);
            }

            public IList listar(object entidadeFiltro, string camposAgrupar, bool cargaComposicao)
            {
                return _persistencia.listar(entidadeFiltro, entidadeFiltro.GetType(), 0, string.Empty, camposAgrupar, string.Empty, false, cargaComposicao);
            }

            public IList listar(object entidadeFiltro, string camposClassificar, bool ordenacaoDecrescente, bool cargaComposicao)
            {
                return _persistencia.listar(entidadeFiltro, entidadeFiltro.GetType(), 0, string.Empty, string.Empty, camposClassificar, ordenacaoDecrescente, cargaComposicao);
            }

            public IList listar(object entidadeFiltro, string camposAgrupar, string camposClassificar, bool ordenacaoDecrescente, bool cargaComposicao)
            {
                return _persistencia.listar(entidadeFiltro, entidadeFiltro.GetType(), 0, string.Empty, string.Empty, camposClassificar, ordenacaoDecrescente, cargaComposicao);
            }

            public IList listar(object entidadeFiltro, string camposAgrupar, string camposClassificar, int limiteRegistros, bool ordenacaoDecrescente, bool cargaComposicao)
            {
                return _persistencia.listar(entidadeFiltro, entidadeFiltro.GetType(), 0, string.Empty, camposAgrupar, camposClassificar, ordenacaoDecrescente, cargaComposicao);
            }

            public IList listar(object entidadeFiltro, string camposExibir, string camposAgrupar, string camposClassificar, int limiteRegistros, bool ordenacaoDecrescente, bool cargaComposicao)
            {
                return _persistencia.listar(entidadeFiltro, entidadeFiltro.GetType(), limiteRegistros, camposExibir, camposAgrupar, camposClassificar, ordenacaoDecrescente, cargaComposicao);
            }

            public void iniciarTransacao()
            {
                if (CacheInstanciaDAL.Persistencia == null)
                {
                    _persistencia = (IPersistencia)Activator.CreateInstanceFrom("..\\..\\..\\..\\..\\..\\WINDOWS\\SGE.v2.DAL.dll",
                                                                                "SGE.Conectividade.Persistencia.PersistenciaDados", new object[] { true }).Unwrap();
                    CacheInstanciaDAL.Persistencia = _persistencia;
                }
                else
                    _persistencia = (IPersistencia)CacheInstanciaDAL.Persistencia;

                _persistencia.iniciarTransacao();
            }

            public void efetivarTransacao()
            {
                _persistencia.efetivarTransacao();
            }

            public void cancelarTransacao()
            {
                _persistencia.cancelarTransacao();
            }

        #endregion
    }

    static class CacheInstanciaDAL
    {
        public static object Persistencia = null;
    }
}
