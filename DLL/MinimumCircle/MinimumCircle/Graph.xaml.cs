using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Utilities;

namespace MinimumCircle
{
    /// <summary>
    /// Interaction logic for Graph.xaml
    /// </summary>
    public partial class Graph : UserControl
    {
        const int POINT_SIZE = 8;
        const int RESOLUTION = 2000;
        const float defaultMin = -100;
        const float defaultMax = 100;
        const int MAX_SIZE = 30;
        public ObservableCollection<ISeries> seriesCollection { get; set; }
        public SolidColorPaintTask regularFill, regularStroke, anomalyFill, anomalyStroke;
        public ObservableCollection<ObservablePointF> pointsRegular;
        public ObservableCollection<ObservablePointF> pointsAnomalies;
        public ObservableCollection<ObservablePointF> GraphDim;
        public static ObservableCollection<ObservablePointF> circle;
        public TimeSeries ts;
        public string trainCSV, testCSV;
        public MinimumCircleDetector detector;
        public float threshold;
        Queue<bool> pointsIsAnomalyQueue = new Queue<bool>();
        long currentFrame = -1;
        string feature1 = null;
        List<float> x, y;

        public Graph(string testCSV, string trainCSV)
        {
            this.trainCSV = trainCSV;
            this.testCSV = testCSV;
            pointsAnomalies = new ObservableCollection<ObservablePointF>();
            pointsRegular = new ObservableCollection<ObservablePointF>();
            circle = new ObservableCollection<ObservablePointF>();
            GraphDim = new ObservableCollection<ObservablePointF>();

            setColors();
            setDetector(testCSV, trainCSV);
            defaultGraph();
            

            seriesCollection = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservablePointF>
                {
                Values = circle,
                GeometrySize = 0,
                Fill = null,
                },
                new ScatterSeries<ObservablePointF>
                {
                Values = GraphDim,
                GeometrySize = 0,
                Fill = null,
                Stroke = null,
                },
                new ScatterSeries<ObservablePointF>
                {
                Values = pointsRegular,
                Fill = regularFill,
                Stroke = regularStroke,
                GeometrySize = POINT_SIZE
                },
                new ScatterSeries<ObservablePointF>
                {
                Values = pointsAnomalies,
                Fill = anomalyFill,
                Stroke = anomalyStroke,
                GeometrySize = POINT_SIZE
                },
            };
            InitializeComponent();
            graph.DataContext = this;
        }

        public void setDetector(string testCSV, string trainCSV)
        {
            detector = new MinimumCircleDetector();
            detector.learnNormal(new TimeSeries(trainCSV));
            ts = new TimeSeries(testCSV);
        }
        public void setColors()
        {
            regularFill = new SolidColorPaintTask() { Color = SKColors.LightGray };
            regularStroke = new SolidColorPaintTask() { Color = SKColors.Gray };
            anomalyFill = new SolidColorPaintTask() { Color = SKColors.Red };
            anomalyStroke = new SolidColorPaintTask() { Color = SKColors.DarkRed };
        }

        public void defaultGraph()
        {
            GraphDim.Add(new ObservablePointF(defaultMax, defaultMax));
            GraphDim.Add(new ObservablePointF(defaultMin, defaultMin));
        }

        public void setNewCircle()
        {
            MinimumCircle_cf c = detector.getCFbyFeature(feature1);
            x = ts.getAttributeData(feature1);
            y = ts.getAttributeData(c.feature2);
            DeleteAll(GraphDim);
            DeleteAll(circle);
            float absMin = x.Min() >= y.Min() ? x.Min() : y.Min();
            float absMax = x.Max() >= y.Max() ? x.Max() : y.Max();
            GraphDim.Add(new ObservablePointF(absMin, absMin));
            GraphDim.Add(new ObservablePointF(absMax, absMax));
            generateCircle(c.minCircle.c.x, c.minCircle.c.y, c.minCircle.r); 
        }

        public ObservableCollection<ObservablePointF> generateCircle(float cx, float cy, float r)
        {
            ObservableCollection<ObservablePointF> c = new ObservableCollection<ObservablePointF>();
            float num_theta = RESOLUTION;
            float dtheta = (float)(2 * Math.PI / num_theta);
            float theta = 0;
            float x = 0;
            float y = 0;

            for (; theta <= 2 * Math.PI; theta += dtheta)
            {
                x = (float)(cx + r * Math.Cos(theta));
                y = (float)(cy + r * Math.Sin(theta));
                circle.Add(new ObservablePointF(x, y));
            }
            return circle;
        }



        public void moveOneFrame()
        {
            currentFrame++;
            int j = (int)currentFrame;
            bool isAnomaly = detector.isAnomalous(x[j], y[j], detector.getCFbyFeature(feature1));
            pointsIsAnomalyQueue.Enqueue(isAnomaly);
            if (isAnomaly)
            {
                pointsAnomalies.Add(new ObservablePointF(x[j], y[j]));
            }
            else
            {
                pointsRegular.Add(new ObservablePointF(x[j], y[j]));
            }
            if (pointsIsAnomalyQueue.Count >= MAX_SIZE + 1)
            {
                bool isFirstPointAnomaly = pointsIsAnomalyQueue.Dequeue();
                if (isFirstPointAnomaly)
                {
                    pointsAnomalies.RemoveAt(0);
                }
                else
                {
                    pointsRegular.RemoveAt(0);
                }
            }
        }

        public void updateThreshold(float newThreshold)
        {
            detector.setThreshold(newThreshold);
            detector.learnNormal(new TimeSeries(this.trainCSV));
            if (feature1 != null)
                updateChosenFeature(feature1);
        }

        public void updateChosenFeature(string newFeature)
        {
            feature1 = newFeature;
            if (feature1 != null && !(detector.getCFbyFeature(newFeature).feature2 == null))
            {
                setNewCircle();
                changePoints(currentFrame);
            }
        }

        public void DeleteAll(ObservableCollection<ObservablePointF> o)
        {
            while (o.Count > 0)
                o.RemoveAt(o.Count - 1);
        }

        public void changePoints(long newFrame)
        {
            if (newFrame < MAX_SIZE - 1)
            {
                currentFrame = -1;
                DeleteAll(pointsRegular);
                DeleteAll(pointsAnomalies);
                pointsIsAnomalyQueue.Clear();
            }
            else
            {
                currentFrame = newFrame - MAX_SIZE;
            }
            while (currentFrame < newFrame)
                moveOneFrame();
        }

        public void updateFrame(long newFrame)
        {
            if (feature1 != null && detector.getCFbyFeature(feature1).feature2 != null)
            {
                if (newFrame == currentFrame + 1)
                    moveOneFrame();
                else
                    changePoints(newFrame);
            }
            else
            {
                currentFrame = newFrame;
            }
        }

    }


    // DLL Adapter
    public class UCExported
    {
        Graph graph;
        public Graph CreateUC(string testCSV, string trainCSV)
        {
            graph = new Graph(testCSV, trainCSV);
            return graph;
        }
        public void UpdateFrame(long timeStep)
        {
            graph.updateFrame(timeStep);
        }
        public void UpdateChosenFeature(string feature)
        {
            graph.updateChosenFeature(feature);
        }
        public Dictionary<string, string> UpdateThreshold(float newThreshold)
        {
            graph.updateThreshold(newThreshold);
            return GetCorrelatedFeatures();
        }
        public Dictionary<string, string> GetCorrelatedFeatures()
        {
            Dictionary<string, string> cf = new Dictionary<string, string>();
            foreach (MinimumCircle_cf c in graph.detector.getNormalData())
            {
                cf.Add(c.feature1, c.feature2);
            }
            return cf;
        }
        
    }

}
