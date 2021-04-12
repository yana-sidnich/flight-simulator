using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightGearTestExec
{
    public class FlightDataContainer
    {
        public string correlatedFeatureName { get; set; }
        public List<float> values { get; set; }
        public float minValue { get; set; }
        public float maxValue { get; set; }

        public FlightDataContainer()
        {
            values = new List<float>();
            correlatedFeatureName = "";
            maxValue = float.MinValue;
            minValue = float.MaxValue;
        }

        public void addValue(float value)
        {
            values.Add(value);
            minValue = Math.Min(minValue, value);
            maxValue = Math.Max(maxValue, value);
        }
        
        // public static Dictionary<string, FlightDataContainer> get_should_be_data_context()
        // {
        //     if (result != null && result.Count > 0)
        //         return result;
        //     result = new Dictionary<string, FlightDataContainer>();
        //
        //     const int SIZE = 54;
        //     const int POINTS_SIZE = 2314;
        //
        //     Random rnd = new Random(1460);
        //     List<int> indexes = new List<int>();
        //     string[] featuresNames = new string[SIZE];
        //
        //     String baseName = "Feature";
        //     for (int i = 0; i < SIZE; i++)
        //     {
        //         indexes.Add(i);
        //         String featureName = baseName + "_" + i.ToString();
        //         featuresNames[i] = featureName;
        //     }
        //     int[] shuffeld_indexes = indexes.OrderBy((item) => rnd.Next()).ToArray();
        //
        //     for (int i = 0; i < SIZE; i++)
        //     {
        //         FlightDataContainer feature = new FlightDataContainer();
        //
        //         feature.correlatedFeatureName = featuresNames[shuffeld_indexes[i]];
        //         if (rnd.Next(0, 10) == 5)
        //         {
        //             feature.correlatedFeatureName = "";
        //         }
        //         feature.values = new double[POINTS_SIZE];
        //
        //         //Make Random Numbers
        //         double rand1 = rnd.NextDouble() + 1;
        //         double rand2 = rnd.NextDouble() + 2;
        //         double rand3 = rnd.NextDouble() + 3;
        //
        //         //Variables, Play with these for unique results!
        //         float peakheight = 80;
        //         float flatness = 300;
        //         int offset = 30;
        //         double localMin = 0;
        //         double localMax = 0;
        //
        //         //Generate basic terrain sine
        //         for (int x = 0; x < POINTS_SIZE; x++)
        //         {
        //
        //             double height = peakheight / rand1 * Math.Sin((float)x / flatness * rand1 + rand1);
        //             height += peakheight / rand2 * Math.Sin((float)x / flatness * rand2 + rand2);
        //             height += peakheight / rand3 * Math.Sin((float)x / flatness * rand3 + rand3);
        //
        //             height += offset;
        //             localMin = Math.Min(localMin, height);
        //             localMax = Math.Max(localMax, height);
        //
        //             feature.values[x] = height;
        //         }
        //
        //         feature.minValue = localMin;
        //         feature.maxValue = localMax;
        //
        //         result[featuresNames[i]] = feature;
        //     }
        //
        //     return result;
        // }
    }

}