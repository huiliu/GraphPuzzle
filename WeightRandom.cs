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
}
