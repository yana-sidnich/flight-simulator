using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using static Utilities.calculate;


namespace MinimumCircle
{
    public class MinimumCircle_cf
    {
        public string feature1;
        public string feature2;
        public float correlation;
        public Circle minCircle;
    }
    /*****************************    CODE FOR MINIMUM CIRCLE - DO NOT CHANGE     **********************/
    public class SmallestEnclosingCircle
    {
        public static Circle MakeCircle(List<Point> points)
        {
            // Clone list to preserve the caller's data, do Durstenfeld shuffle
            List<Point> shuffled = new List<Point>(points);
            Random rand = new Random();
            for (int i = shuffled.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                Point temp = shuffled[i];
                shuffled[i] = shuffled[j];
                shuffled[j] = temp;
            }

            // Progressively add points to circle or recompute circle
            Circle c = Circle.INVALID;
            for (int i = 0; i < shuffled.Count; i++)
            {
                Point p = shuffled[i];
                if (c.r < 0 || !c.Contains(p))
                    c = MakeCircleOnePoint(shuffled.GetRange(0, i + 1), p);
            }
            return c;
        }


        // One boundary point known
        private static Circle MakeCircleOnePoint(List<Point> points, Point p)
        {
            Circle c = new Circle(p, 0);
            for (int i = 0; i < points.Count; i++)
            {
                Point q = points[i];
                if (!c.Contains(q))
                {
                    if (c.r == 0)
                        c = MakeDiameter(p, q);
                    else
                        c = MakeCircleTwoPoints(points.GetRange(0, i + 1), p, q);
                }
            }
            return c;
        }


        // Two boundary points known
        private static Circle MakeCircleTwoPoints(List<Point> points, Point p, Point q)
        {
            Circle circ = MakeDiameter(p, q);
            Circle left = Circle.INVALID;
            Circle right = Circle.INVALID;

            // For each point not in the two-point circle
            Point pq = q.Subtract(p);
            foreach (Point r in points)
            {
                if (circ.Contains(r))
                    continue;

                // Form a circumcircle and classify it on left or right side
                float cross = pq.Cross(r.Subtract(p));
                Circle c = MakeCircumcircle(p, q, r);
                if (c.r < 0)
                    continue;
                else if (cross > 0 && (left.r < 0 || pq.Cross(c.c.Subtract(p)) > pq.Cross(left.c.Subtract(p))))
                    left = c;
                else if (cross < 0 && (right.r < 0 || pq.Cross(c.c.Subtract(p)) < pq.Cross(right.c.Subtract(p))))
                    right = c;
            }

            // Select which circle to return
            if (left.r < 0 && right.r < 0)
                return circ;
            else if (left.r < 0)
                return right;
            else if (right.r < 0)
                return left;
            else
                return left.r <= right.r ? left : right;
        }


        public static Circle MakeDiameter(Point a, Point b)
        {
            Point c = new Point((a.x + b.x) / 2, (a.y + b.y) / 2);
            return new Circle(c, (float)Math.Max(c.Distance(a), c.Distance(b)));
        }


        public static Circle MakeCircumcircle(Point a, Point b, Point c)
        {
            // Mathematical algorithm from Wikipedia: Circumscribed circle
            float ox = (Math.Min(Math.Min(a.x, b.x), c.x) + Math.Max(Math.Max(a.x, b.x), c.x)) / 2;
            float oy = (Math.Min(Math.Min(a.y, b.y), c.y) + Math.Max(Math.Max(a.y, b.y), c.y)) / 2;
            float ax = a.x - ox, ay = a.y - oy;
            float bx = b.x - ox, by = b.y - oy;
            float cx = c.x - ox, cy = c.y - oy;
            float d = (ax * (by - cy) + bx * (cy - ay) + cx * (ay - by)) * 2;
            if (d == 0)
                return Circle.INVALID;
            float x = ((ax * ax + ay * ay) * (by - cy) + (bx * bx + by * by) * (cy - ay) + (cx * cx + cy * cy) * (ay - by)) / d;
            float y = ((ax * ax + ay * ay) * (cx - bx) + (bx * bx + by * by) * (ax - cx) + (cx * cx + cy * cy) * (bx - ax)) / d;
            Point p = new Point(ox + x, oy + y);
            float r = (float)Math.Max(Math.Max(p.Distance(a), p.Distance(b)), p.Distance(c));
            return new Circle(p, r);
        }

    }

    public struct Circle
    {

        public static Circle INVALID = new Circle(new Point(0, 0), -1);

        private const float MULTIPLICATIVE_EPSILON = (float)(1 + 1e-14);


        public Point c;   // Center
        public float r;  // Radius


        public Circle(Point c, float r)
        {
            this.c = c;
            this.r = r;
        }


        public bool Contains(Point p)
        {
            return c.Distance(p) <= r * MULTIPLICATIVE_EPSILON;
        }


        public bool Contains(ICollection<Point> ps)
        {
            foreach (Point p in ps)
            {
                if (!Contains(p))
                    return false;
            }
            return true;
        }

    }

    public struct Point
    {

        public float x;
        public float y;


        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }


        public Point Subtract(Point p)
        {
            return new Point(x - p.x, y - p.y);
        }


        public float Distance(Point p)
        {
            float dx = x - p.x;
            float dy = y - p.y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }


        // Signed area / determinant thing
        public float Cross(Point p)
        {
            return x * p.y - y * p.x;
        }

    }

    /************************************************************************************/


    public class MinimumCircleDetector
    {
        List<MinimumCircle_cf> cf;
        float threshold;

        public MinimumCircleDetector()
        {
            cf = new List<MinimumCircle_cf>();
            this.threshold = 0.5f;
        }

        public List<MinimumCircle_cf> getNormalData()
        {
            return cf;
        }
        public void setThreshold(float newThreshold)
        {
            this.threshold = newThreshold;
        }
        public bool isAnomalous(float x, float y, MinimumCircle_cf c)
        {
            return (new Point(x, y)).Distance(c.minCircle.c) > c.minCircle.r;
        }

        public void addToCF(string f1, string f2, float correlation, TimeSeries ts)
        {
            MinimumCircle_cf c = new MinimumCircle_cf();
            c.feature1 = f1;
            if (f2 == null)
            {
                c.feature2 = null;
                c.correlation = -1;
                c.minCircle = Circle.INVALID;
            }
            else
            {
                c.feature2 = f2;
                c.correlation = correlation;
                List<Point> pts = new List<Point>();
                List<float> v1 = ts.getAttributeData(f1);
                List<float> v2 = ts.getAttributeData(f2);
                for (int i = 0; i < v1.Count; i++)
                {
                    pts.Add(new Point(v1[i], v2[i]));
                }
                c.minCircle = SmallestEnclosingCircle.MakeCircle(pts);
            }
            cf.Add(c);
        }

        public MinimumCircle_cf getCFbyFeature(string feature1)
        {
            foreach (MinimumCircle_cf c in cf)
            {
                if (c.feature1 == feature1)
                    return c;
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
