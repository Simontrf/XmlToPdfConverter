using System;

namespace XmlToPdfConverter.Core.Interfaces
{
    //Niveau de sévérité des logs 
    public enum LogLevel
    {
        Debug, //Info de logs détaillés
        Info,//Info générales
        Warning, //Avertissements (non bloquants)
        Error //Erreurs critiques
    }

    public interface ILogger
    {
        //Message de débogage
        void Log(string message, LogLevel level = LogLevel.Info);
    }
}

