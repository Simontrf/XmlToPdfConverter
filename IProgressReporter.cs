namespace XmlToPdfConverter.Core.Interfaces
{
    //Rapport de progression d'une opération
    public interface IProgressReporter
    {
        //Rapporte l'état d'avancement) de 0 à 100% + message décrivant l'étape
        void Report(int percent, string message);
    }

}
