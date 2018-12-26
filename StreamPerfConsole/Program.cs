using System;

namespace StreamPerfConsole
{
    static class Program
    {
        static void Main(string[] args)
        {
        
            bool continu = true;
            while (continu)
            {
                Console.WriteLine("Choose your test:");
                Console.WriteLine(" -> perf test press '1',");
                Console.WriteLine(" -> thread test press '2',");
                Console.WriteLine(" -> quit press 'q'.");
                var k = Console.ReadKey();
                Console.WriteLine(Environment.NewLine);
                switch (k.KeyChar)
                {
                    case '1':
                        StreamPT.DoPT();
                        break;
                    case '2':
                        pStreamPT.ThreadTest();
                        break;
                    case 'q':
                        continu = false;
                        break;
                }
            }
        }
    }
}
