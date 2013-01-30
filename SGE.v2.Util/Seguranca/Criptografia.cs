//
//
//  Gerado com o plug-in C# no StarUML®
//
//  @ Projeto : SGE.v2
//  @ Nome do Arquivo : Criptografia.cs
//  @ Data de Criação : 11/09/2012
//  @ Autor : Ponto Certo Consultoria
//
//

using System;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Security.Cryptography;

namespace SGE.Seguranca.Criptografia
{
	public class Criptografador
    {
        #region Declarações

        DESCryptoServiceProvider provedorCriptografia;
        
        #endregion

        #region Construtores

        public Criptografador()
        {
            provedorCriptografia = new DESCryptoServiceProvider();
            provedorCriptografia.KeySize = 64;
            
            provedorCriptografia.IV = Convert.FromBase64String(ConfigurationSettings.AppSettings["VetorCripto"]);
            provedorCriptografia.Key = Convert.FromBase64String(ConfigurationSettings.AppSettings["ChaveCripto"]);
        }
        
        #endregion

        #region Métodos Públicos

        public byte[] criptografarBinario(string textoCriptografar)
        {
            char[] conteudoConverter = textoCriptografar.ToCharArray();
            byte[] conteudoConvertido = new byte[conteudoConverter.Length];

            int indiceAC = 0;
            foreach (char simbolo in conteudoConverter)
            {
                conteudoConvertido[indiceAC] = Convert.ToByte(simbolo);
                indiceAC++;
            }


            return provedorCriptografia.CreateEncryptor().TransformFinalBlock(conteudoConvertido, 0, conteudoConvertido.Length); 
        }

        public byte[] criptografarBinario(byte[] arrayCriptografar)
        {
            byte[] conteudoConvertido = new byte[arrayCriptografar.Length];

            conteudoConvertido = provedorCriptografia.CreateEncryptor().TransformFinalBlock(arrayCriptografar, 0, arrayCriptografar.Length);

            return conteudoConvertido;
        }

        public byte[] criptografarBinario(BitArray arrayBinarioCriptografar, int tamanhoArray)
        {
            byte[] arrayCriptografar = new byte[tamanhoArray];

            arrayBinarioCriptografar.CopyTo(arrayCriptografar, 0);

            return provedorCriptografia.CreateEncryptor().TransformFinalBlock(arrayCriptografar, 0, arrayCriptografar.Length);
        }

        public string criptografarBinario(ref BitArray arrayBinarioCriptografar, int tamanhoArray)
        {
            byte[] arrayCriptografar = new byte[tamanhoArray];
            byte[] arrayCriptografado = new byte[tamanhoArray];

            arrayBinarioCriptografar.CopyTo(arrayCriptografar, 0);

            arrayCriptografado = provedorCriptografia.CreateEncryptor().TransformFinalBlock(arrayCriptografar, 0, arrayCriptografar.Length);

            return Convert.ToBase64String(arrayCriptografado);
        }

        public string decriptografarBinario(byte[] conteudoCriptografado)
        {
            byte[] conteudoConverter = provedorCriptografia.CreateDecryptor().TransformFinalBlock(conteudoCriptografado, 0, conteudoCriptografado.Length);
            StringBuilder conteudoConvertido = new StringBuilder();

            foreach (byte simbolo in conteudoConverter)
            {
                conteudoConvertido.Append(Convert.ToChar(simbolo));
            }

            return conteudoConvertido.ToString();
        }

        public byte[] decriptografarBinario(ref byte[] conteudoCriptografado)
        {
            return provedorCriptografia.CreateDecryptor().TransformFinalBlock(conteudoCriptografado, 0, conteudoCriptografado.Length);
        }

        public BitArray decriptografarBinario(byte[] conteudoCriptografado, int tamanhoArray)
        {
            BitArray arrayBinarioRetorno = new BitArray(provedorCriptografia.CreateDecryptor().TransformFinalBlock(conteudoCriptografado, 0, conteudoCriptografado.Length));

            arrayBinarioRetorno.Length = tamanhoArray;
            
            return arrayBinarioRetorno;
        }

        public string criptografarTexto(string textoCriptografar)
        {
            StringBuilder textoRetornar = new StringBuilder();

            char[] conteudoConverter = textoCriptografar.ToCharArray();
            byte[] conteudoConvertido = new byte[conteudoConverter.Length];

            int indiceAC = 0;
            foreach (char simbolo in conteudoConverter)
            {
                conteudoConvertido[indiceAC] = Convert.ToByte(simbolo);
                indiceAC++;
            }

            byte[] conteudoRetornar = provedorCriptografia.CreateEncryptor().TransformFinalBlock(conteudoConvertido, 0, conteudoConvertido.Length);

            return Convert.ToBase64String(conteudoRetornar);
        }

        public string decriptografarTexto(string textoCriptografado)
        {
            byte[] conteudoCriptografado = Convert.FromBase64String(textoCriptografado);

            byte[] conteudoConverter = provedorCriptografia.CreateDecryptor().TransformFinalBlock(conteudoCriptografado, 0, conteudoCriptografado.Length);
            StringBuilder conteudoConvertido = new StringBuilder();

            foreach (byte simbolo in conteudoConverter)
            {
                conteudoConvertido.Append(Convert.ToChar(simbolo));
            }

            return conteudoConvertido.ToString();
        }

        public byte[] decriptografarTexto(ref string textoCriptografado)
        {
            byte[] conteudoCriptografado = Convert.FromBase64String(textoCriptografado);

            byte[] conteudoDecripto = provedorCriptografia.CreateDecryptor().TransformFinalBlock(conteudoCriptografado, 0, conteudoCriptografado.Length);

            return conteudoDecripto;
        }

        #endregion
	}
}
