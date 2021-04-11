using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Utilities
{
    public class TimeSeries
    {
        private Dictionary<string, List<float>> data;
        private List<string> attributes;
        public TimeSeries(string csv)
        {
            data = new Dictionary<string, List<float>>();
            attributes = new List<string>();
            StreamReader sr = File.OpenText(csv);
            string line = sr.ReadLine();
            string[] attr = line.Split(',');
            foreach (string att in attr)
            {
                data.Add(att, new List<float>());
                attributes.Add(att);
            }
            while ((line = sr.ReadLine()) != null)
            {
                attr = line.Split(',');
                for (int i= 0; i < attributes.Count; i++)
                {
                    data[attributes[i]].Add(float.Parse(attr[i]));
                }
            }
            sr.Close();
        }

        public List<string> getAttributes()
        {
            return attributes;
        }

        public List<float> getAttributeData(string att)
        {
            return data[att];
        }
    }
}
