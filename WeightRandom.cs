using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// 参考： https://stackoverflow.com/questions/1761626/weighted-random-numbers

namespace GraphGame.Logic
{
    public class WeightRandom<T>
    {
        private readonly int ChoiceCount = 0;
        private readonly int MaxWeight = 0;
        private readonly Dictionary<T, int> Weights = new Dictionary<T, int>();
        private readonly IDictionary<T, int> OriginWeights;
        public WeightRandom(Dictionary<T, int> weights)
        {
            this.OriginWeights = weights;
            foreach (var kvp in this.OriginWeights)
            {
                ++this.ChoiceCount;
                this.MaxWeight += kvp.Value;
                this.Weights.Add(kvp.Key, kvp.Value);
            }

            this.AutoReset = true;
        }

        private Random Random;
        public void InitRandom(int seed)
        {
            this.Random = new Random(seed);
        }

        public bool AutoReset { get; set; }

        public T Next()
        {
            var r = this.Random.Next(0, this.SumOfWeight);

            foreach (var kvp in this.Weights)
            {
                if (r < kvp.Value)
                    return kvp.Key;
                r -= this.Weights[kvp.Key];
            }

            return default(T);
        }

        private int SumOfWeight
        {
            get
            {
                var sum = 0;
                foreach (var kvp in this.Weights)
                    sum += kvp.Value;

                if (sum == 0 && this.AutoReset)
                {
                    sum = this.MaxWeight;
                    this.ResetWeight();
                }

                return sum;
            }
        }

        private void ResetWeight()
        {
            foreach (var kvp in this.OriginWeights)
                this.Weights.Add(kvp.Key, kvp.Value);
        }
    }

    public class WeightRandomTest
    {
        private static WeightRandomTest instance = new WeightRandomTest();
        public static WeightRandomTest Instance { get { return instance; } }
        private Dictionary<int, int> weights = new Dictionary<int, int> { { 1, 100 }, { 2, 100 }, { 3, 100 } };
        private Dictionary<int, int> result = new Dictionary<int, int>();

        private static int KeyCount = 10;
        private int TotalWeight = 0;
        private void InitData()
        {
            this.weights.Clear();
            this.result.Clear();

            var rnd = new Random();
            for (var i = 0; i < KeyCount; ++i)
            {
                // var key = rnd.Next(0, 100);
                var key = i;
                var w = rnd.Next(100, 300);

                this.weights.Add(key, w);
                this.result.Add(key, 0);

                this.TotalWeight += w;
            }

        }

        private static int RunTimes = 1000000;
        public void Run()
        {
            this.InitData();

            var weightRandom = new WeightRandom<int>(this.weights);
            weightRandom.InitRandom(0);

            for (var i = 0; i < RunTimes; ++i)
            {
                var v = weightRandom.Next();
                ++this.result[v];
            }

            this.Print();
        }

        private void Print()
        {
            Console.WriteLine("Config:");
            foreach (var kvp in this.weights)
                Console.WriteLine("\t{0}: {1}/{2}", kvp.Key, kvp.Value, kvp.Value * 1.0f / this.TotalWeight);

            Console.WriteLine("Result:");
            foreach (var kvp in this.result)
            {
                Console.WriteLine("\t{0}: {1}/{2}", kvp.Key, kvp.Value, kvp.Value * 1.0f / RunTimes);
            }
        }
    }
}
