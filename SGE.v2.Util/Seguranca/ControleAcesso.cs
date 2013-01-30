//
//
//  Gerado com o plug-in C# no StarUML®
//
//  @ Projeto : SGE.v2
//  @ Nome do Arquivo : ControleAcesso.cs
//  @ Data de Criação : 11/09/2012
//  @ Autor : Ponto Certo Consultoria
//
//

using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Linq;
using SGE.Utilitarios;
using SGE.Interfaces;
using SGE.Utilitarios.Conectividade;
using SGE.Utilitarios.InfraEstrutura;
using SGE.Utilitarios.InfraEstrutura.Idioma;
using SGE.Utilitarios.Administracao;
using SGE.Seguranca.Criptografia;

namespace SGE.Seguranca.ControleAcesso
{
	public class ControladorAcesso
    {
        #region Declarações

        public const int tamanhoChavePerfil = 1024; // 1024 bits

        public static int UsuariosConectados;

        #endregion

        #region Métodos Públicos

        public Usuario validarAcesso(Usuario usuarioValidar)
        {
            Usuario usuarioConsulta = new Usuario();
            Criptografador cripto = new Criptografador();

            usuarioValidar.Senha = cripto.criptografarTexto(usuarioValidar.Senha);

            usuarioConsulta = (Usuario)usuarioConsulta.consultar(usuarioValidar, false);                
            
            return usuarioConsulta;
        }

        public static bool verificarPermissao(Type tipoEntidadeVerificar, string chavePerfilCripto)
        {
            bool resultadoVerificacao = false;
            BitArray arrayChaveDecripto = null;
            byte[] preChavePerfilDecripto = new byte[ControladorAcesso.tamanhoChavePerfil];
            bool[] chavePerfilDecripto = new bool[ControladorAcesso.tamanhoChavePerfil];
            Criptografador cripto = new Criptografador();

            preChavePerfilDecripto = cripto.decriptografarTexto(ref chavePerfilCripto);

            arrayChaveDecripto = new BitArray(preChavePerfilDecripto);
            arrayChaveDecripto.Length = ControladorAcesso.tamanhoChavePerfil;

            arrayChaveDecripto.CopyTo(chavePerfilDecripto, 0);
            
            foreach (object atributo in tipoEntidadeVerificar.GetCustomAttributes(true))
                if (atributo.GetType().Name.Equals("PerfilAcesso"))
                {
                    long CodigoPerfil = long.Parse(atributo.GetType().GetField("CodigoPerfil").
                                                                      GetValue(atributo).ToString());

                    resultadoVerificacao = chavePerfilDecripto[CodigoPerfil];
                    break;
                }

            return resultadoVerificacao;
        }

        public static bool verificarPermissao(object entidadeVerificar, string chavePerfilCripto)
        {
            return verificarPermissao(entidadeVerificar.GetType(), chavePerfilCripto);
        }

        public Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>> listarFuncionalidades(string chavePerfilCripto)
        {
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>> grupoFuncionalidades =
                            new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>>();
            
            Assembly bibliotecaNegocios = Assembly.LoadFrom("..\\..\\..\\..\\..\\..\\WINDOWS\\SGE.v2.BOL.dll");
            Assembly bibliotecaAuxiliar = Assembly.LoadFrom("..\\..\\..\\..\\..\\..\\WINDOWS\\SGE.v2.Util.dll");

            IEnumerable<Type> entidades = bibliotecaNegocios.GetTypes().Union(bibliotecaAuxiliar.GetTypes()).
                                          Where(et => et.GetCustomAttributes(true).
                                          Any(ant => ant.GetType().Name.Equals("Funcionalidade")));
            
            foreach (Type entidade in entidades)
            {
                if (verificarPermissao(entidade, chavePerfilCripto))
                {
                    var funcionalidadeEntidade = entidade.GetCustomAttributes(true).
                                                     Where(ant => ant.GetType().
                                                     Name.Equals("Funcionalidade")).
                                                     FirstOrDefault();

                    string grupoFuncionalidade = funcionalidadeEntidade.GetType().GetField("GrupoFuncionalidade").
                                                                        GetValue(funcionalidadeEntidade).ToString();

                    string subGrupoFuncionalidade = funcionalidadeEntidade.GetType().GetField("SubGrupoFuncionalidade").
                                                                           GetValue(funcionalidadeEntidade).ToString();

                    List<string> descricoesCaracteristicas = new List<string>();

                    if (!grupoFuncionalidades.Keys.Any(key => key.Equals(grupoFuncionalidade)))
                        grupoFuncionalidades.Add(grupoFuncionalidade,
                                                 new Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>());

                    if (!grupoFuncionalidades[grupoFuncionalidade].
                                             Any(sbg => sbg.Key.Equals(subGrupoFuncionalidade)))
                        grupoFuncionalidades[grupoFuncionalidade].Add(subGrupoFuncionalidade,
                                                                      new Dictionary<string, Dictionary<string, List<string>>>());

                    var subGrupos = grupoFuncionalidades[grupoFuncionalidade];

                    string nomeExibicao  = funcionalidadeEntidade.GetType().GetField("NomeExibicao").
                                                                  GetValue(funcionalidadeEntidade).ToString();

                    if (!subGrupos.Keys.Any(key => key.Equals(subGrupoFuncionalidade)))
                        subGrupos.Add(subGrupoFuncionalidade, new Dictionary<string, Dictionary<string, List<string>>>());
                    
                    subGrupos[subGrupoFuncionalidade].Add(nomeExibicao, new Dictionary<string, List<string>>());

                    var atributosEntidade = entidade.GetProperties().Where(ent => ent.GetCustomAttributes(true).
                                                                     Any(atb => atb.GetType().Name.Contains("DisplayName")));

                    var metodosEntidade = entidade.GetMethods().Where(ent => ent.GetCustomAttributes(true).
                                                                     Any(atb => atb.GetType().Name.Contains("DisplayName")));

                    Dictionary<string, List<string>> caracteristicasFuncionalidade;
                    string acessoFuncionalidade = string.Empty;

                    caracteristicasFuncionalidade = subGrupos[subGrupoFuncionalidade][nomeExibicao];

                    acessoFuncionalidade = funcionalidadeEntidade.
                                           GetType().GetField("AcessoFuncionalidade").
                                           GetValue(funcionalidadeEntidade).ToString();

                    if (atributosEntidade.Count() > 0)
                    {
                        foreach (var atributo in atributosEntidade)
                        {
                            string descricaoAtributo = ((DisplayNameAttribute)atributo.GetCustomAttributes(true).
                                                                                       Where(atb => atb.GetType().
                                                                                       Name.Contains("DisplayName")).
                                                                                       FirstOrDefault()).DisplayName;
                            if (!descricoesCaracteristicas.Contains(descricaoAtributo))
                                descricoesCaracteristicas.Add(descricaoAtributo);
                        }

                        foreach (var metodo in metodosEntidade)
                        {
                            string descricaoMetodo = ((DisplayNameAttribute)metodo.GetCustomAttributes(true).
                                                                                       Where(atb => atb.GetType().
                                                                                       Name.Contains("DisplayName")).
                                                                                       FirstOrDefault()).DisplayName;
                            if (!descricoesCaracteristicas.Contains(descricaoMetodo))
                                descricoesCaracteristicas.Add(descricaoMetodo);
                        }

                        if (!caracteristicasFuncionalidade.Keys.Any(key => key.Equals(acessoFuncionalidade)))
                            caracteristicasFuncionalidade.Add(acessoFuncionalidade, descricoesCaracteristicas);
                    }
                    else
                    {
                        if (!caracteristicasFuncionalidade.Keys.Any(key => key.Equals(acessoFuncionalidade)))
                            caracteristicasFuncionalidade.Add(acessoFuncionalidade, new List<string>(0));
                    }
                 }
            }

            return grupoFuncionalidades;
        }

        #endregion
	}

    public class PerfilAcesso : Attribute
    {
        #region Declarações

        public long CodigoPerfil;
        public bool ExigeAutenticacao = true;

        #endregion
    }

    public class ExcecaoAcessoNegado : Exception
    {
        #region Construtores

        public ExcecaoAcessoNegado(bool acessoSistema)
        {
            TradutorIdioma tradutor = new TradutorIdioma(ConfigurationSettings.AppSettings["Idioma"]);
            string mensagem = string.Empty;

            if (!acessoSistema)
                mensagem = tradutor.traduzirMensagem("AcessoNegadoAusenciaPerfil");
            else
                mensagem = tradutor.traduzirMensagem("AcessoNegadoPerfilInsuficiente");

            throw new ExcecaoAcessoNegado(mensagem);
        }

        protected ExcecaoAcessoNegado(string mensagemExcecao) : base(mensagemExcecao)
        {

        }

        #endregion
    }
}
