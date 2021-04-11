using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using static Utilities.calculate;

namespace LinearRegression
{
    public class lin_reg_cf
    {
        public string feature1;
        public string feature2;
        public float correlation;
        public Line lin_reg;
        public float threshold;
    }
    public class LinearRegressionDetector
    {
        private List<lin_reg_cf> cf;
        private float threshold;

        public LinearRegressionDetector()
        {
            cf = new List<lin_reg_cf>();
            this.threshold = 0.9f;
        }

        public List<lin_reg_cf> getNormalData()
        {
            return cf;
        }
        public void setThreshold(float newThreshold)
        {
            this.threshold = newThreshold;
        }
        public bool isAnomalous(float x, float y, lin_reg_cf c)
        {
            return (Math.Abs(y - c.lin_reg.f(x)) > c.threshold);
        }

        private void addToCF(string f1, string f2, float correlation, TimeSeries ts)
        {
            lin_reg_cf c = new lin_reg_cf();
            c.feature1 = f1;
            if (f2 == null)
            {
                c.feature2 = null;
                c.correlation = -1;
                c.lin_reg = null;
                c.threshold = -1;
            }
            else
            {
                c.correlation = correlation;
                c.feature1 = f1;
                c.feature2 = f2;
                Point[] points = toPoints(ts.getAttributeData(f1), ts.getAttributeData(f2));
                c.lin_reg = linear_reg(points, points.Length);
                c.threshold = findThreshold(c.lin_reg, points) * 1.1f;
            }
            cf.Add(c);
        }

        public string findCorrelated(string f1)
        {
            foreach(lin_reg_cf c in cf)
            {
                if (c.feature1 == f1)
                    return c.feature2;
            }
            return null;
        }

        public lin_reg_cf getCFbyFeature(string feature1)
        {
            foreach (lin_reg_cf c in cf)
            {
                if (c.feature1 == feature1)
                    return c;
            }
            return null;
        }


        private float findThreshold(Line l, Point[] points)
        {
            float max = 0;
            foreach (Point p in points)
            {
                float d = dev(p, l);
                if (d > max)
                    max = d;
            }
            return max;

        }
        public Line get_reg(string f1)
        {
            foreach (lin_reg_cf c in cf)
            {
                if (c.feature1 == f1)
                    return c.lin_reg;
            }
            return null;
        }


        

        
        
        public void learnNormal(TimeSeries ts)
        {
            this.cf.Clear();
            List<string> attributes = ts.getAttributes();
            for (int i = 0; i < attributes.Count; i++)
            {
                string f1 = attributes[i];
                string f2 = null;
                float max = 0;
                for (int j = 0; j < attributes.Count; j++)
                {
                    if (j == i) continue;
                    float[] vals1 = listToArray(ts.getAttributeData(f1));
                    float[] vals2 = listToArray(ts.getAttributeData(attributes[j]));
                    float p = pearson(vals1, vals2, vals1.Length);
                    if (p > max && p >= this.threshold)
                    {
                        f2 = attributes[j];
                        max = p;
                    }
                }
                addToCF(f1, f2, max, ts);
            }

        }
        
    }
}
