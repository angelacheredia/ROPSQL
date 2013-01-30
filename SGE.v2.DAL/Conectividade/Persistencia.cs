//
//
//  Gerado com o plug-in C# no StarUML®
//
//  @ Projeto : SGE.v2
//  @ Nome do Arquivo : Persistencia.cs
//  @ Data de Criação : 11/09/2012
//  @ Autor : Ponto Certo Consultoria
//
//

using System;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using SGE.Interfaces;
using SGE.Utilitarios.Conectividade;
using SGE.Conectividade.BancoDados;
using SGE.Conectividade;
using System.Collections;

namespace SGE.Conectividade.Persistencia
{
    public class PersistenciaDados : ConexaoBancoDados, IPersistencia
    {
        #region Declarações

	    private readonly string[] _arrayVazio = new string[0];
        bool _manterConexao = false;

        #endregion

        #region Construtores

        public PersistenciaDados() { }

	    public PersistenciaDados(bool manterConexaoDados)
        {
            _manterConexao = manterConexaoDados;
            this.conectar();
        }

        #endregion

        #region Métodos Públicos

        public int incluir(object entidade, Type tipoEntidade)
        {
            string instrucaoSQL = string.Empty;
            Dictionary<string, object> parametrosComando;
            int codigoRegistroInserido = 0;

            if (_manterConexao || base.conectar())
            {
                instrucaoSQL = traduzirEntidade(Convert.ChangeType(entidade, tipoEntidade), 
                                                tipoEntidade,
                                               (int)AcaoPersistencia.Inclusao, 
                                               null, 
                                               _arrayVazio,
                                               out parametrosComando);
                
                codigoRegistroInserido = executarComando(instrucaoSQL, parametrosComando);
                
                if (!_manterConexao) base.desconectar();
            }

            return codigoRegistroInserido;
		}

        public int alterar(object entidade, object entidadeFiltro, Type tipoEntidade)
        {
            string instrucaoSQL = string.Empty;
            Dictionary<string, object> parametrosComando;
            int registrosAfetados = 0;

            if (_manterConexao || base.conectar())
            {
                instrucaoSQL = traduzirEntidade(Convert.ChangeType(entidade, tipoEntidade), tipoEntidade, (int)AcaoPersistencia.Alteracao, entidadeFiltro, _arrayVazio, out parametrosComando);

                registrosAfetados = executarComando(instrucaoSQL, parametrosComando);
            }
            
            if (!_manterConexao) base.desconectar();

            return registrosAfetados;
		}

        public int excluir(object entidadeFiltro, Type tipoEntidade)
        {
            string instrucaoSQL = string.Empty;
            Dictionary<string, object> parametrosComando;
            int registrosAfetados = 0;

            if (_manterConexao || base.conectar())
            {
                instrucaoSQL = traduzirEntidade(Convert.ChangeType(entidadeFiltro, tipoEntidade),
                                                   tipoEntidade,
                                                  (int)AcaoPersistencia.Exclusao,
                                                  Convert.ChangeType(entidadeFiltro, tipoEntidade),
                                                  _arrayVazio,
                                                  out parametrosComando);

                registrosAfetados = executarComando(instrucaoSQL, parametrosComando);
            }

            if (!_manterConexao) base.desconectar();

            return registrosAfetados;
		}

        public object consultar(object entidadeFiltro, Type tipoEntidade, bool cargaComposicao)
        {
            object entidadeRetorno = null;
            
            var listaConsulta = listar(entidadeFiltro, tipoEntidade, 0, string.Empty, string.Empty, string.Empty, false, cargaComposicao);

            if (listaConsulta.Count > 0) entidadeRetorno = listaConsulta[0];
            
            return entidadeRetorno;
		}

        public IList listar(object entidadeFiltro, Type tipoEntidade, int limiteRegistros, string atributosExibir, string atributosAgrupar, string atributosOrdenar, bool ordenacaoDecrescente, bool cargaComposicao)
        {
            string instrucaoSQL = string.Empty;
            string[] atributosAgrupamento = new string[0];
            Dictionary<string, object> relacaoAtributosCampos = null;
            Dictionary<string, object> parametrosComando;
            string listaCampos = string.Empty;

            Type tipoDinamicoLista = typeof(List<>).MakeGenericType(new Type[] { tipoEntidade });
            object listaRetorno = Activator.CreateInstance(tipoDinamicoLista);

            // Montando instrução de consulta
            
            instrucaoSQL = traduzirEntidade(Convert.ChangeType(entidadeFiltro, tipoEntidade), 
                                            tipoEntidade,
                                           (int)AcaoPersistencia.Listagem, 
                                           Convert.ChangeType(entidadeFiltro, tipoEntidade), 
                                           atributosExibir != string.Empty ? atributosExibir.Split(',') : _arrayVazio,
                                           out parametrosComando);

            instrucaoSQL = string.Format(instrucaoSQL, limiteRegistros > 0 ? string.Format(RepositorioSQL.PersistenciaDados_Acao_Limitar, limiteRegistros) : string.Empty, "{0}", "{1}");

            if (!string.IsNullOrEmpty(atributosAgrupar))
            {
                string listaCamposComplementar = string.Empty;

                atributosAgrupamento = atributosAgrupar.Split(',');

                for (int vC = 0; vC < atributosAgrupamento.Length; vC++)
                    atributosAgrupamento[vC] = atributosAgrupamento[vC].Trim();

                relacaoAtributosCampos = obterListaAnotacoesValores(Convert.ChangeType(entidadeFiltro, tipoEntidade), tipoEntidade, (int)AcaoPersistencia.Listagem, out parametrosComando);

                foreach (var relacao in relacaoAtributosCampos)
                    if (Array.IndexOf(atributosAgrupamento, relacao.Key) > -1)
                        listaCampos += string.Format("{0}, ", ((KeyValuePair<string, object>)relacao.Value).Key);
                    else
                        if ((relacao.Key != "Classe") && (relacao.Key != "Tabela"))
                            listaCamposComplementar += string.Format("{0}, ", ((KeyValuePair<string, object>)relacao.Value).Key);

                if (!String.IsNullOrEmpty(listaCampos) && Convert.ToInt32(listaCampos) > 2)
                    listaCampos = listaCampos.Substring(0, listaCampos.Length - 2);
                if (!String.IsNullOrEmpty(listaCamposComplementar) && Convert.ToInt32(listaCamposComplementar) > 2)
                    listaCamposComplementar = listaCamposComplementar.Substring(0, listaCamposComplementar.Length - 2);

                instrucaoSQL = string.Format(instrucaoSQL,
                                             string.Format(RepositorioSQL.PersistenciaDados_Acao_Agrupar,
                                                          listaCampos + ", " + listaCamposComplementar),
                                                          "{0}");
            }
            else
                instrucaoSQL = string.Format(instrucaoSQL, string.Empty, "{0}");

            if (!string.IsNullOrEmpty(atributosOrdenar))
            {
                atributosAgrupamento = atributosOrdenar.Split(',');

                for (int vC = 0; vC < atributosAgrupamento.Length; vC++)
                    atributosAgrupamento[vC] = atributosAgrupamento[vC].Trim();

                relacaoAtributosCampos = obterListaAnotacoesValores(Convert.ChangeType(entidadeFiltro, tipoEntidade), tipoEntidade, (int)AcaoPersistencia.Listagem, out parametrosComando);

                listaCampos = relacaoAtributosCampos.Where(relacao => Array.IndexOf(atributosAgrupamento, relacao.Key) > -1).
                                                     Aggregate(listaCampos, (current, relacao) => string.Format("{0}{1}", current, (((KeyValuePair<string, object>) relacao.Value).Key + ", ")));

                listaCampos = listaCampos.Substring(0, listaCampos.Length - 2);

                instrucaoSQL = string.Format(instrucaoSQL,
                                             string.Format(RepositorioSQL.PersistenciaDados_Acao_Ordenar,
                                                          listaCampos),
                                                          ordenacaoDecrescente ? "DESC" : "ASC");
            }
            else
                instrucaoSQL = string.Format(instrucaoSQL, string.Empty, "{0}");

            if (_manterConexao || base.conectar())
            {
                // Tratando retorno do banco

                XmlDocument retornoPesquisa = efetuarPesquisa_SQL(instrucaoSQL);

                listaRetorno = traduzirRetornoBancoDados(retornoPesquisa, entidadeFiltro.GetType());
            }

            if(!_manterConexao) base.desconectar();

            // Efetuando carga da composição quando existente (Lazy Loading)

            if (cargaComposicao && (((IList)listaRetorno).Count > 0))
                for (int inC = 0; inC < ((IList)listaRetorno).Count; inC++)
                    carregarComposicao(((IList)listaRetorno)[inC], ((IList)listaRetorno)[inC].GetType());

            return (IList)listaRetorno;
		}

        #endregion

        #region Métodos Auxiliares

        private string traduzirEntidade(object entidade, Type tipoEntidade, int acao, object entidadeFiltro, string[] atributosExibir, out Dictionary<string, object> parametrosComando)
        {
            string instrucaoSQL = string.Empty;
            Dictionary<string, object> dadosSQLFiltro;

            Dictionary<string, object> dadosSQLEntidade = obterListaAnotacoesValores(Convert.ChangeType(entidade, tipoEntidade), tipoEntidade, acao, out parametrosComando);

            if (entidadeFiltro != null)
                dadosSQLFiltro = obterListaAnotacoesValores(Convert.ChangeType(entidadeFiltro, tipoEntidade), tipoEntidade, acao, out parametrosComando);
            else
                dadosSQLFiltro = null;

            Dictionary<string, string> parametrosSQL = obterParametrosSQL(dadosSQLEntidade, acao, dadosSQLFiltro, atributosExibir);

            switch (acao)
            {
                case (int)AcaoPersistencia.Listagem :

                    instrucaoSQL = String.Format(RepositorioSQL.PersistenciaDados_Acao_Consultar,
                                                 "{0}",
                                                 parametrosSQL["listaCampos"],
                                                 parametrosSQL["Tabela"],
                                                 parametrosSQL["listaRelacionamentos"],
                                                 parametrosSQL["listaCamposFiltro"],
                                                 "{1}", "{2}");

                    break;

                case (int)AcaoPersistencia.Inclusao:

                    instrucaoSQL = String.Format(RepositorioSQL.PersistenciaDados_Acao_Inserir,
                                                 parametrosSQL["Tabela"], 
                                                 parametrosSQL["listaCampos"],
                                                 parametrosSQL["listaValores"]);

                    break;

                case (int)AcaoPersistencia.Alteracao:

                    instrucaoSQL = String.Format(RepositorioSQL.PersistenciaDados_Acao_Alterar,
                                                 parametrosSQL["Tabela"], 
                                                 parametrosSQL["listaCamposComValores"],
                                                 parametrosSQL["listaCamposFiltro"]);
                    
                    break;

                case (int)AcaoPersistencia.Exclusao:

                    instrucaoSQL = String.Format(RepositorioSQL.PersistenciaDados_Acao_Excluir,
                                                 parametrosSQL["Tabela"], 
                                                 parametrosSQL["listaCamposFiltro"]);

                    break;
            }

            return instrucaoSQL;
        }

        private IList traduzirRetornoBancoDados(XmlDocument retornoBancoDados, Type tipoEntidade)
        {
            Type tipoDinamicoLista = typeof(List<>).MakeGenericType(new Type[] { tipoEntidade });
            object listaRetorno = Activator.CreateInstance(tipoDinamicoLista);
            object entidadeRetorno = null;
            XmlNodeList listaElementos;
            
            listaElementos = retornoBancoDados.GetElementsByTagName("Table");

            foreach (XmlNode elementoEntidade in listaElementos)
            {
                entidadeRetorno = Activator.CreateInstance(tipoEntidade, true);

                foreach (XmlNode elementoAtributo in elementoEntidade.ChildNodes)
                {
                    foreach (PropertyInfo campo in tipoEntidade.GetProperties())
                    {
                        if (campo.GetCustomAttributes(true).FirstOrDefault(ca => (ca.GetType().Name.Equals("ColunaDados")
                                                                                  && ((ColunaDados)ca).NomeColuna.Equals(elementoAtributo.Name))
                                                                                  || (ca.GetType().Name.Equals("ColunaRelacional"))
                                                                                  && (((ColunaRelacional)ca).NomeColuna.Equals(elementoAtributo.Name))) != null)
                            campo.SetValue(entidadeRetorno, formatarValorSaidaCampoSQL(elementoAtributo.InnerText, campo.PropertyType), null);
                    }
                }

                ((IList)listaRetorno).Add(entidadeRetorno);
            }

            return (IList)listaRetorno;
        }

        private Dictionary<string, object> obterListaAnotacoesValores(object entidade, Type tipoEntidade, int acao, out Dictionary<string, object> parametrosComando)
        {
            var associacaoDadosObjetoSQL = new Dictionary<string, object>();
            parametrosComando = new Dictionary<string, object>();

            associacaoDadosObjetoSQL.Add("Classe", tipoEntidade.Name);

            object[] anotacoesClasse = tipoEntidade.GetCustomAttributes(true);

            foreach (object anotacao in anotacoesClasse)
            {
                if (anotacao.GetType().Name.Equals("Tabela"))
                    associacaoDadosObjetoSQL.Add("Tabela", ((Tabela)anotacao).NomeTabela);
            }

            PropertyInfo atributoChavePrimaria = tipoEntidade.GetProperties().FirstOrDefault(fd =>
                                                 (fd.GetCustomAttributes(true).Any(ca =>
                                                                                  (ca.GetType().Name.Equals("ColunaDados")
                                                                                   && ((ColunaDados)ca).ChavePrimaria))));

            PropertyInfo[] listaAtributos = tipoEntidade.GetProperties();

            foreach (var atributo in listaAtributos)
            {
                object[] anotacoesAtributo = atributo.GetCustomAttributes(true);

                foreach (object anotacao in anotacoesAtributo)
                {
                    if (anotacao.GetType().Name.Equals("ColunaDados"))
                    {
                        object valorCampo = atributo.GetValue(Convert.ChangeType(entidade, tipoEntidade), null);

                        valorCampo = formatarValorEntradaCampoSQL(valorCampo, acao, (ColunaDados)anotacao, parametrosComando);

                        object campoValorSQL = new KeyValuePair<string, object>(((ColunaDados)anotacao).NomeColuna, valorCampo);

                        if (!((((ColunaDados)anotacao).ChavePrimaria)
                            && (acao == (int)AcaoPersistencia.Inclusao)))
                            associacaoDadosObjetoSQL.Add(atributo.Name, campoValorSQL);
                    }
                    else if(anotacao.GetType().Name.Equals("ColunaRelacional")
                            && ((acao == (int)AcaoPersistencia.Listagem) || (acao == (int)AcaoPersistencia.Consulta)))
                    {
                        ColunaRelacional configRelacao = (ColunaRelacional)anotacao;
                        configRelacao.ColunaChave = 
                            ((ColunaDados)atributoChavePrimaria.GetCustomAttributes(true).FirstOrDefault(ca => ca.GetType().Name.Equals("ColunaDados"))).NomeColuna;

                        associacaoDadosObjetoSQL.Add("Relacao", configRelacao);
                    }
                }
            }

            return associacaoDadosObjetoSQL;
        }

        private Dictionary<string, string> obterParametrosSQL(Dictionary<string, object> dadosSQLEntidade, int acao, Dictionary<string, object> dadosSQLFiltro, string[] atributosExibir)
        {
            var dicionarioRetorno = new Dictionary<string, string>();
            var dicionarioRelacionamentos = new Dictionary<string, string>();

            string _nomeTabela = string.Empty;
            string _listaCampos = string.Empty;
            string _listaValores = string.Empty;
            string _listaCamposComValores = string.Empty;
            string _listaCamposFiltro = string.Empty;
            string _listaRelacionamentos = string.Empty;

            foreach (var item in dadosSQLEntidade.Where(item => !item.Key.Equals("Classe")))
            {
                if (item.Key.Equals("Tabela"))
                {
                    dicionarioRetorno.Add(item.Key, item.Value.ToString());
                    _nomeTabela = item.Value.ToString();
                }
                else if (item.Key.Equals("Relacao"))
                {
                    ColunaRelacional configRelacao = item.Value as ColunaRelacional;

                    _listaCampos += string.Format("{0}.{1}, ", configRelacao.NomeTabela, configRelacao.NomeColuna);

                    if (configRelacao.TipoJuncao == TipoJuncaoRelacional.Obrigatoria)
                        _listaRelacionamentos += string.Format(RepositorioSQL.PersistenciaDados_Acao_RelacionarObrigatoriamente,
                                                             configRelacao.NomeTabela,
                                                             string.Concat(_nomeTabela, ".", configRelacao.ColunaChave),
                                                             string.Concat(configRelacao.NomeTabela, ".", configRelacao.ColunaChaveEstrangeira));
                    else
                        _listaRelacionamentos += string.Format(RepositorioSQL.PersistenciaDados_Acao_RelacionarOpcionalmente,
                                                             configRelacao.NomeTabela,
                                                             string.Concat(_nomeTabela, ".", configRelacao.ColunaChaveEstrangeira),
                                                             string.Concat(configRelacao.NomeTabela, ".", configRelacao.ColunaChave));
                }
                else if (item.Key.Equals("EntidadeRelacionada"))
                {

                }
                else
                {
                    string nomeAtributoEntidade = item.Key;
                    object nomeCampoEntidade = ((KeyValuePair<string, object>)item.Value).Key;
                    object valorCampoEntidade = ((KeyValuePair<string, object>)item.Value).Value;

                    switch (acao)
                    {
                        case (int)AcaoPersistencia.Inclusao:
                            _listaCampos += string.Format("{0}, ", nomeCampoEntidade);
                            _listaValores += string.Format("{0}, ", valorCampoEntidade);
                            
                            break;
                        case (int)AcaoPersistencia.Listagem:
                            if (atributosExibir.Length > 0)
                                for (int vC = 0; vC < atributosExibir.Length; vC++)
                                    atributosExibir[vC] = atributosExibir[vC].Trim();
                            if ((atributosExibir.Length == 0)
                                || atributosExibir.Length > 0 && Array.IndexOf(atributosExibir, nomeAtributoEntidade) > -1)
                                _listaCampos += string.Format("{0}.{1}, ", _nomeTabela, nomeCampoEntidade);
                            
                            break;
                        default:
                            if (valorCampoEntidade != null)
                                _listaCamposComValores += string.Format("{0} = {1}, ", nomeCampoEntidade, valorCampoEntidade);
                            
                            break;
                    }
                }
            }
            if (dadosSQLFiltro != null)
            {
                foreach (var itemFiltro in dadosSQLFiltro)
                {
                    if ((!itemFiltro.Key.Equals("Classe")) && (!itemFiltro.Key.Equals("Tabela"))
                        && (!itemFiltro.Key.Equals("Relacao")) && (!itemFiltro.Key.Equals("EntidadeRelacionada")))
                    {
                        object nomeCampoFiltro = ((KeyValuePair<string, object>)itemFiltro.Value).Key;
                        object valorCampoFiltro = ((KeyValuePair<string, object>)itemFiltro.Value).Value;

                        if ((valorCampoFiltro != null) && (valorCampoFiltro.ToString() != ValoresPadraoCamposSql.Nulo))
                            _listaCamposFiltro += nomeCampoFiltro + OperadoresSql.Igual + valorCampoFiltro + OperadoresSql.E;
                    }
                }
            }

            if (acao == (int)AcaoPersistencia.Inclusao)
            {
                _listaCampos = _listaCampos.Substring(0, _listaCampos.Length - 2);
                _listaValores = _listaValores.Substring(0, _listaValores.Length - 2);

                dicionarioRetorno.Add("listaCampos", _listaCampos);
                dicionarioRetorno.Add("listaValores", _listaValores);
            }
            else
            {
                if (acao == (int)AcaoPersistencia.Listagem)
                {
                    _listaCampos = _listaCampos.Substring(0, _listaCampos.Length - 2);
                    dicionarioRetorno.Add("listaCampos", _listaCampos);
                    dicionarioRetorno.Add("listaRelacionamentos", _listaRelacionamentos);
                }
                else
                    if (!string.IsNullOrEmpty(_listaCamposComValores))
                    {
                        _listaCamposComValores = _listaCamposComValores.Substring(0, _listaCamposComValores.Length - 2);

                        dicionarioRetorno.Add("listaCamposComValores", _listaCamposComValores);
                    }

                if (!string.IsNullOrEmpty(_listaCamposFiltro))
                {
                    _listaCamposFiltro = _listaCamposFiltro.Substring(0, _listaCamposFiltro.Length - 5);

                    dicionarioRetorno.Add("listaCamposFiltro", _listaCamposFiltro);
                }
                else
                    dicionarioRetorno.Add("listaCamposFiltro", "1 = 1");
            }

            return dicionarioRetorno;
        }

        public void carregarComposicao(object entidadeCarregada, Type tipoEntidade)
        {
            EntidadeRelacionada configRelacao = null;
            object instanciaAtributo = null;

            PropertyInfo[] listaAtributos = tipoEntidade.GetProperties();

            foreach (var atributo in listaAtributos)
            {
                IEnumerable<object> anotacoesAtributo = atributo.GetCustomAttributes(true)
                                                        .Where(an => an.GetType().Name.Equals("EntidadeRelacionada"));

                foreach (object anotacao in anotacoesAtributo)
                {
                    configRelacao = (EntidadeRelacionada)anotacao;

                    if (!atributo.PropertyType.Name.Contains("List"))
                    {
                        instanciaAtributo = Activator.CreateInstance(atributo.PropertyType, true);

                        FieldInfo colunaChaveEstrangeira = entidadeCarregada.GetType().GetField(configRelacao.AtributoChaveEstrangeira);

                        if (int.Parse(colunaChaveEstrangeira.GetValue(entidadeCarregada).ToString()) > 0)
                        {
                            PropertyInfo colunaChaveAtributo = instanciaAtributo.GetType().GetProperties().FirstOrDefault(fd =>
                                                               (fd.GetCustomAttributes(true).Any(ca =>
                                                                                                (ca.GetType().Name.Equals("ColunaDados")
                                                                                                 && ((ColunaDados)ca).ChavePrimaria))));

                            ColunaDados anotacaoChaveAtributo = colunaChaveAtributo.GetCustomAttributes(true)
                                                                .FirstOrDefault(ca => ca.GetType().Name.Equals("ColunaDados")) as ColunaDados;

                            colunaChaveAtributo.SetValue(instanciaAtributo, colunaChaveEstrangeira.GetValue(entidadeCarregada), null);

                            instanciaAtributo = consultar(instanciaAtributo, instanciaAtributo.GetType(), false);
                        }
                    }
                    else
                    {
                        instanciaAtributo = Activator.CreateInstance(atributo.PropertyType.GetGenericArguments()[0], true);

                        FieldInfo colunaChavePrimaria = entidadeCarregada.GetType().GetFields().FirstOrDefault(fd =>
                                                                             (fd.GetCustomAttributes(true).Any(ca =>
                                                                                                              (ca.GetType().Name.Equals("ColunaDados")
                                                                                                               && ((ColunaDados)ca).ChavePrimaria))));

                        FieldInfo colunaChaveEstrangeira = instanciaAtributo.GetType().GetField(configRelacao.AtributoChaveEstrangeira);
                        colunaChaveEstrangeira.SetValue(instanciaAtributo, int.Parse(colunaChavePrimaria.GetValue(entidadeCarregada).ToString()));

                        instanciaAtributo = listar(instanciaAtributo, instanciaAtributo.GetType(), 0, string.Empty, string.Empty, string.Empty, false, false);
                    }
                }

                if ((instanciaAtributo != null) && (instanciaAtributo.GetType().Name.Equals(atributo.PropertyType.Name)))
                    if (!atributo.PropertyType.Name.Contains("List"))
                        atributo.SetValue(entidadeCarregada, instanciaAtributo, null);
                    else
                        atributo.SetValue(entidadeCarregada, (IList)instanciaAtributo, null);
            }
        }

        private object formatarValorEntradaCampoSQL(object valorCampoSQL, int acao, ColunaDados configCampo, Dictionary<string, object> parametrosComando)
        {
            if (valorCampoSQL != null)
            {
                switch (valorCampoSQL.GetType().ToString())
                {
                    case TiposDeDados.InteiroCurto:
                        if (((short)valorCampoSQL == 0) && (!configCampo.Requerida))
                            valorCampoSQL = ValoresPadraoCamposSql.Nulo;
                        break;
                    case TiposDeDados.Inteiro:
                        if (((int)valorCampoSQL == 0) && ((!configCampo.Requerida) || (acao == (int)AcaoPersistencia.Listagem)))
                            valorCampoSQL = ValoresPadraoCamposSql.Nulo;
                        break;
                    case TiposDeDados.InteiroLongo:
                        if (((long)valorCampoSQL == 0) && (!configCampo.Requerida))
                            valorCampoSQL = ValoresPadraoCamposSql.Nulo;
                        break;
                    case TiposDeDados.Texto:
                        valorCampoSQL = "'" + valorCampoSQL + "'";
                        break;
                    case TiposDeDados.DataHora:
                        if (!(((DateTime)valorCampoSQL).Equals(DateTime.MinValue)))
                            valorCampoSQL = "'" + ((DateTime)valorCampoSQL).ToString(FormatoDatas.DataHoraCompleta) + "'";
                        else
                            valorCampoSQL = ValoresPadraoCamposSql.Nulo;
                        break;
                    case TiposDeDados.Binario :
                        parametrosComando.Add(string.Concat("@", configCampo.NomeColuna),
                                              (byte[])valorCampoSQL);

                        valorCampoSQL = string.Concat("@", configCampo.NomeColuna);
                        
                        break;
                }
            }
            else
            {
                valorCampoSQL = (acao == (int)AcaoPersistencia.Inclusao) ? ValoresPadraoCamposSql.Nulo : null;
            }

            return valorCampoSQL;
        }

        private object formatarValorSaidaCampoSQL(string valorCampoSQL, Type tipoDadoCampo)
        {
            object retorno = null;

            if (!string.IsNullOrEmpty(valorCampoSQL))
                retorno = Convert.ChangeType(valorCampoSQL, tipoDadoCampo);

            return retorno;
        }

        #endregion
    }

    public static class TiposDeDados
    {
        #region Declarações

        public const string InteiroCurto = "System.Int16";
        public const string Inteiro = "System.Int32";
        public const string InteiroLongo = "System.Int64";
        public const string Texto = "System.String";
        public const string DataHora = "System.DateTime";
        public const string Binario = "System.Byte[]";

        #endregion
    }

    public static class ValoresPadraoCamposSql
    {
        #region Declarações

        public const string Nulo = "NULL";
        public const string Zerado = "0";

        #endregion
    }

    public static class  OperadoresSql
    {
        #region Declarações

        public const string Igual = " = ";
        public const string E = " AND ";
        public const string Ou  = " OR ";
        public const string Maior = " > ";
        public const string MaiorOuIgual = " >= ";
        public const string Menor = " < ";
        public const string MenorOuIgual = " <= ";
        public const string EhNulo = " IS NULL ";
        public const string Entre = "BETWEEN {0} AND {1}";

        #endregion
    }
    public static class  FormatoDatas
    {
        #region Declarações

        public const string DataHoraCompleta = "yyyy-MM-dd hh:mm:ss";
        public const string DataNormal = "yyyy-MM-dd";
        public const string DataReduzida = "yy-MM-dd";

        #endregion
    }

    public enum AcaoPersistencia
    {
        #region Declarações

        Inclusao = 1,
        Alteracao = 2,
        Exclusao = 3,
        Listagem = 4,
        Consulta = 5

        #endregion
    }
}
