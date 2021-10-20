using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;

namespace ServerChat
{
    class Program
    {
        public static int k;
        static void Main(string[] args)
        {
            Thread_Do();
        }
        public static void Thread_Do()
        {
            k++;
            NamedPipeServerStream pipeServerStream = new NamedPipeServerStream("mypipe", PipeDirection.InOut, 5, PipeTransmissionMode.Message);
            pipeServerStream.WaitForConnection();
            Console.WriteLine($"Клиент №{k} подключился!");
            Thread T = new Thread(Thread_Do);
            T.Start();
            byte[] bytes = new byte[256];
            while(true)
            {
                pipeServerStream.Read(bytes, 0, bytes.Length);
                string s = Encoding.ASCII.GetString(bytes);
                Console.WriteLine(s);
                pipeServerStream.Write(bytes, 0, bytes.Length);
                pipeServerStream.Flush();
            }
            
        }
    }
}
