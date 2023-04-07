namespace ProducerConsumerProblem
{
    internal class Program
    {
        static Queue<int> buffer = new Queue<int>();
        private readonly static object o = new object();

        static void Produce()
        {
            Random rand = new Random();
            int number = rand.Next(0, 100);

            lock (o)
            {
                buffer.Enqueue(number);

                if (buffer.Count > 100)
                {
                    Monitor.Pulse(o);
                    Monitor.Wait(o);
                }
            }

            Thread.Sleep(rand.Next(0, 200));
        }

        static void Consume()
        {
            Random rand = new Random();

            lock (o)
            {
                if(buffer.Count < 90)
                {
                    Monitor.Pulse(o);
                }

                if(buffer.Count == 0)
                {
                    Monitor.Pulse(o);
                    Monitor.Wait(o);
                }

                int num = buffer.Dequeue();
                Console.WriteLine(num);

            }

            Thread.Sleep(rand.Next(0, 200));
        }

        static void Main()
        {
            Thread producer = new Thread(() => Produce());
            Thread consumer = new Thread(() => Consume());

            producer.Start();
            consumer.Start();

            producer.Join();
            consumer.Join();
        }
    }
}