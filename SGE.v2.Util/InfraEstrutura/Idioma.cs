//
//
//  Gerado com o plug-in C# no StarUML®
//
//  @ Projeto : SGE.v2
//  @ Nome do Arquivo : Execucao.cs
//  @ Data de Criação : 11/09/2012
//  @ Autor : Ponto Certo Consultoria
//
//

using System;
using System.Collections;
using System.Reflection;
using System.Globalization;
using SGE.Utilitarios;

namespace SGE.Utilitarios.InfraEstrutura.Idioma
{
	public class TradutorIdioma
    {
        private string _siglaIdioma;

        public TradutorIdioma(string siglaIdioma)
        {
            _siglaIdioma = siglaIdioma;
        }

        public string traduzirMensagem(string chaveRepositorio)
        {
            return traduzirMensagem(chaveRepositorio, _siglaIdioma);
        }

        private string traduzirMensagem(string chaveRepositorio, string siglaIdioma)
        {
            var repositorioMensagens = RepositorioMensagem.ResourceManager.GetResourceSet(new CultureInfo("pt-BR"), true, true);
            string mensagemRetorno = string.Empty;

            foreach (DictionaryEntry mensagem in repositorioMensagens)
                if (mensagem.Key.ToString().Contains(string.Concat(chaveRepositorio, siglaIdioma)))
                {
                    mensagemRetorno = mensagem.Value.ToString();
                    break;
                }

            return mensagemRetorno;
        }

        public string traduzirTexto(string chaveRepositorio)
        {
            return traduzirTexto(chaveRepositorio, _siglaIdioma);
        }

        private string traduzirTexto(string chaveRepositorio, string siglaIdioma)
        {
            var repositorioTextos = RepositorioTexto.ResourceManager.GetResourceSet(new CultureInfo("pt-BR"), true, true);
            string textoRetorno = string.Empty;

            foreach (DictionaryEntry texto in repositorioTextos)
                if (texto.Key.ToString().Contains(string.Concat(chaveRepositorio, siglaIdioma)))
                {
                    textoRetorno = texto.Value.ToString();
                    break;
                }

            return textoRetorno;
        }

        public string traduzirAtributo(string chaveRepositorio)
        {
            return traduzirAtributo(chaveRepositorio, _siglaIdioma);
        }

        private string traduzirAtributo(string chaveRepositorio, string siglaIdioma)
        {
            var repositorioAtributos = RepositorioAtributo.ResourceManager.GetResourceSet(new CultureInfo("pt-BR"), true, true);
            string atributoRetorno = string.Empty;

            foreach (DictionaryEntry atributo in repositorioAtributos)
                if (atributo.Key.ToString().Contains(string.Concat(chaveRepositorio, siglaIdioma)))
                {
                    atributoRetorno = atributo.Value.ToString();
                    break;
                }

            return atributoRetorno;
        }
	}

    public class Idioma
    {
        public const string PortuguesBrasil = "_BR";
        public const string InglesAmericano = "_US";
        public const string Espanhol = "_ES";
        public const string Frances = "_FR";
        public const string Alemao = "_DE";
    }
}
