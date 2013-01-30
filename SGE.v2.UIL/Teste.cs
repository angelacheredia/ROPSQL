using System;
using System.Text;
using SGE.Manutencao.Comum;
using SGE.Utilitarios.Administracao;
using SGE.Seguranca.ControleAcesso;
using SGE.Seguranca.Criptografia;
using SGE.Utilitarios.InfraEstrutura.Idioma;

namespace SGE.InterfaceUsuario
{
    public class Teste
    {
        public static void Main()
        {
            string usuario = string.Empty, senha = string.Empty;

            try
            {
                Console.WriteLine("SGE.v2 - Ingresso");
                Console.WriteLine("-----------------");
                Console.Write("Usuário : ");
                usuario = Console.ReadLine();
                Console.Write("Senha : ");
                senha = Console.ReadLine();
                Ingressar(usuario, senha);
                Console.WriteLine(string.Empty);
                Console.Write("Pressione qualquer tecla para sair...");
                Console.Read();

            }
            catch(Exception erro)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(erro.Message);
                Console.Read();
            }
        }

        private static void Ingressar(string usuario, string senha)
        {
            ControladorAcesso controleAcesso = new ControladorAcesso();
            Usuario usuarioIngressar = new Usuario();
            Usuario usuarioRegistrado = null;

            usuarioIngressar.Apelido = usuario;
            usuarioIngressar.Senha = senha;

            usuarioRegistrado = controleAcesso.validarAcesso(usuarioIngressar);

            if (usuarioRegistrado != null)
            {
                controleAcesso.listarFuncionalidades(usuarioRegistrado.PerfilAcesso);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Funcionalidades listadas.");
            }
            else
                throw new ExcecaoAcessoNegado(false);
        }
    }
}
