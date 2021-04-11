using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;

namespace Utilities
{
    
    public class Line
    {
        public float a;
        public float b;
        public Line() { a = 0;b = 0; }
        public Line(float a, float b)
        {
            this.a = a;
            this.b = b;
        }
        public float f(float x)
        {
            return a * x + b;
        }
    }

    public class Point
    {
        public float x;
        public float y;
        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class Circle
    {
        public Point center;
        public float radius;
        public Circle(Point p, float r)
        {
            center = p;
            radius = r;
        }
    }

    public class calculate
    { 
        public static float avg(float[] x, int size)
        {
            float sum = 0;
            for (int i = 0; i < size; sum += x[i], i++) ;
            return sum / size;
        }

        public static float var(float[] x, int size)
        {
            float av = avg(x, size);
            float sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += x[i] * x[i];
            }
            return sum / size - av * av;
        }

        // returns the covariance of X and Y
        public static float cov(float[] x, float[] y, int size)
        {
            float sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += x[i] * y[i];
            }
            sum /= size;

            return sum - avg(x, size) * avg(y, size);
        }


        // returns the Pearson correlation coefficient of X and Y
        public static float pearson(float[] x, float[] y, int size)
        {
            return (float)(cov(x, y, size) / (Math.Sqrt(var(x, size)) * Math.Sqrt(var(y, size))));
        }

        // performs a linear regression and returns the line equation
        public static Line linear_reg(Point[] points, int size)
        {
            float[] x = new float[size]; 
            float[] y = new float[size];
            for (int i = 0; i < size; i++)
            {
                x[i] = points[i].x;
                y[i] = points[i].y;
            }
            float a = cov(x, y, size) / var(x, size);
            float b = avg(y, size) - a * (avg(x, size));

            return new Line(a, b);
        }

        // returns the deviation between point p and the line equation of the points
        public static float dev(Point p, Point[] points, int size)
        {
            Line l = linear_reg(points, size);
            return dev(p, l);
        }

        // returns the deviation between point p and the line
        public static float dev(Point p, Line l)
        {
            return Math.Abs(p.y - l.f(p.x));
        }

        public static float[] listToArray(List<float> list)
        {
            float[] arr = new float[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                arr[i] = list[i];
            }
            return arr;
        }

        public static Point[] toPoints(List<float> l1, List<float> l2)
        {
            Point[] points = new Point[l1.Count];
            for (int i = 0; i < l1.Count; i++)
            {
                points[i] = new Point(l1[i], l2[i]);
            }
            return points;
        }
    }
    
}
