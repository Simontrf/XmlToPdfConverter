using System;
using System.Threading;

namespace XmlToPdfConverter.CLI
{
    public class ConsoleSpinner
    {
        private int counter = 0;
        private bool active;
        private Thread spinnerThread;
        private readonly string[] sequence = new[] {"#"};

        public void Start()
        {
            active = true;
            spinnerThread = new Thread(Spin);
            spinnerThread.Start();
        }

        public void Stop()
        {
            active = false;
            spinnerThread.Join();
            Console.Write("\r "); // Efface
        }

        private void Spin()
        {
            while (active)
            {
                Console.Write($"\r{sequence[counter % sequence.Length]} ");
                counter++;
                Thread.Sleep(100);
            }
        }
    }
}
