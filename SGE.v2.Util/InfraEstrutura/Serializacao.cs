//
//
//  Gerado com o plug-in C# no StarUML®
//
//  @ Projeto : SGE.v2
//  @ Nome do Arquivo : Serializacao.cs
//  @ Data de Criação : 11/09/2012
//  @ Autor : Ponto Certo Consultoria
//
//

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SGE.Utilitarios.InfraEstrutura.Serializacao
{
    public class Serializador
    {
        #region Métodos Públicos

        public string serializarBinario(object entidade)
        {
            BinaryFormatter serializadorBinario = new BinaryFormatter();
            MemoryStream fluxoMemoria = new MemoryStream();

            serializadorBinario.Serialize(fluxoMemoria, entidade);

            fluxoMemoria.Flush();
            fluxoMemoria.Seek(0, SeekOrigin.Begin);

            return Convert.ToBase64String(fluxoMemoria.ToArray());
        }

        public object deserializarBinario(string entidadeSerializada, Type tipoEntidade)
        {
            BinaryFormatter serializadorBinario = new BinaryFormatter();
            MemoryStream fluxoMemoria = new MemoryStream(Convert.FromBase64String(entidadeSerializada));

            return serializadorBinario.Deserialize(fluxoMemoria);
        }

        public string serializarXML(object entidade)
        {
            StringBuilder textoGerado = new StringBuilder();
            XmlWriter geradorXML = XmlWriter.Create(textoGerado);

            new XmlSerializer(entidade.GetType()).
                Serialize(geradorXML, entidade);

            return textoGerado.ToString();
        }

        public object deserializarXML(string entidadeSerializada, Type tipoEntidade)
        {
            XmlReader leitorXML = XmlReader.Create(entidadeSerializada);

            new XmlSerializer(tipoEntidade).Deserialize(leitorXML);

            return leitorXML.ReadContentAsObject();
        }

        #endregion
	}
}
