using System;

namespace SwipeType.Example
{
    public struct PointPatternMatchResult
    {
        public PointPatternMatchResult(string name, double probability, int pointPatternSetCount, float distance, double averageDistance) : this()
        {
            if (probability > 100 || probability < 0)
                throw new OverflowException("Proability must be between zero (0) and one hundred (100)");

            Name = name;
            Probability = probability;
            PointPatternSetCount = pointPatternSetCount;
            Distance = distance;
            AverageDistance = averageDistance;
        }

        public string Name { get; set; }

        public double Probability { get; set; }

        public int PointPatternSetCount { get; set; }

        public float Distance
        {
            get; set;
        }

        public double AverageDistance
        {
            get; set;
        }
    }
}