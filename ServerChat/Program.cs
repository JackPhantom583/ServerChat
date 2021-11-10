using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace ServerChat
{
    class Program
    {
        public static int k;
        public static List<string> ls = new List<string>();
        static void Main(string[] args)
        {
            
            Thread_Do();
        }
        public static void Thread_Do()
        {
            k++;
            NamedPipeServerStream pipeServerStream = new NamedPipeServerStream("mypipe", PipeDirection.InOut, 5, PipeTransmissionMode.Message,PipeOptions.Asynchronous);
            pipeServerStream.WaitForConnection();
            Console.WriteLine($"Клиент №{k} подключился!");
            Thread T = new Thread(Thread_Do);
            T.Start();
            byte[] bytes = new byte[256];
            IAsyncResult asyncResult;
            asyncResult = pipeServerStream.BeginRead(bytes, 0, bytes.Length, null, null);
            int count = 0;
            while (true)
            {
                if (asyncResult.IsCompleted)
                {
                    int a = pipeServerStream.EndRead(asyncResult);
                    string s = Encoding.ASCII.GetString(bytes, 0, a);
                    Console.WriteLine(s);
                    ls.Add(s);
                    asyncResult = pipeServerStream.BeginRead(bytes, 0, bytes.Length, null, null);
                }
                if (count <= ls.Count)
                {
                    for (int i = count; i < ls.Count; i++)
                    {
                        byte[] bs = Encoding.ASCII.GetBytes(ls[i]);
                        pipeServerStream.Write(bs, 0, bs.Length);
                        pipeServerStream.Flush();
                        count++;
                    }
                }
            }
            
        }
    }
}
