﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5456
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SGE.Conectividade {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class RepositorioSQL {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RepositorioSQL() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SGE.Conectividade.RepositorioSQL", typeof(RepositorioSQL).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GROUP BY {0}.
        /// </summary>
        internal static string PersistenciaDados_Acao_Agrupar {
            get {
                return ResourceManager.GetString("PersistenciaDados_Acao_Agrupar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE {0} SET {1} WHERE {2}.
        /// </summary>
        internal static string PersistenciaDados_Acao_Alterar {
            get {
                return ResourceManager.GetString("PersistenciaDados_Acao_Alterar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT {0} {1} FROM {2} {3} WHERE {4} {5} {6}.
        /// </summary>
        internal static string PersistenciaDados_Acao_Consultar {
            get {
                return ResourceManager.GetString("PersistenciaDados_Acao_Consultar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM {0} WHERE {1}.
        /// </summary>
        internal static string PersistenciaDados_Acao_Excluir {
            get {
                return ResourceManager.GetString("PersistenciaDados_Acao_Excluir", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO {0} ({1}) VALUES ({2}).
        /// </summary>
        internal static string PersistenciaDados_Acao_Inserir {
            get {
                return ResourceManager.GetString("PersistenciaDados_Acao_Inserir", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TOP {0}.
        /// </summary>
        internal static string PersistenciaDados_Acao_Limitar {
            get {
                return ResourceManager.GetString("PersistenciaDados_Acao_Limitar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ORDER BY {0} {1}.
        /// </summary>
        internal static string PersistenciaDados_Acao_Ordenar {
            get {
                return ResourceManager.GetString("PersistenciaDados_Acao_Ordenar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INNER JOIN {0} ON {1} = {2}.
        /// </summary>
        internal static string PersistenciaDados_Acao_RelacionarObrigatoriamente {
            get {
                return ResourceManager.GetString("PersistenciaDados_Acao_RelacionarObrigatoriamente", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to LEFT JOIN {0} ON {1} = {2}.
        /// </summary>
        internal static string PersistenciaDados_Acao_RelacionarOpcionalmente {
            get {
                return ResourceManager.GetString("PersistenciaDados_Acao_RelacionarOpcionalmente", resourceCulture);
            }
        }
    }
}
