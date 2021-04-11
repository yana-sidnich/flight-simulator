using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Utilities;
using LiveChartsCore.Collections;
using LiveChartsCore.SkiaSharpView.WPF;
using LiveChartsCore.Defaults;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System.Diagnostics;

namespace LinearRegression
{

    public partial class Graph : UserControl
    {
        public int POINT_SIZE = 8;
        const int MAX_SIZE = 30;
        const float defaultMin = -100;
        const float defaultMax = 100;
        public ObservableCollection<ISeries> seriesCollection { get; set; }
        Queue<bool> pointsIsAnomalyQueue = new Queue<bool>();
        public SolidColorPaintTask regularFill, regularStroke, anomalyFill, anomalyStroke;
        public ObservableCollection<ObservablePointF> pointsRegular;
        public ObservableCollection<ObservablePointF> pointsAnomalies;
        public ObservableCollection<ObservablePointF> GraphDim;
        public ObservableCollection<ObservablePointF> linearReg;
        public TimeSeries ts;
        public string trainCSV, testCSV;
        public LinearRegressionDetector detector;
        public float threshold;
        long currentFrame = -1;
        string feature1 = null;
        List<float> x, y;

        public Graph(string testCSV, string trainCSV)
        {
            this.testCSV = testCSV;
            this.trainCSV = trainCSV;
            pointsAnomalies = new ObservableCollection<ObservablePointF>();
            pointsRegular = new ObservableCollection<ObservablePointF>();
            linearReg = new ObservableCollection<ObservablePointF>();
            GraphDim = new ObservableCollection<ObservablePointF>();

            setColors();
            setDetector();
            defaultGraph();

            seriesCollection = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservablePointF>
                {
                Values = linearReg,
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

        public void setDetector()
        {
            detector = new LinearRegressionDetector();
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

        public void setNewLine()
        {
            lin_reg_cf c = detector.getCFbyFeature(feature1);
            x = ts.getAttributeData(feature1);
            y = ts.getAttributeData(c.feature2);
            DeleteAll(GraphDim);
            DeleteAll(linearReg);
            GraphDim.Add(new ObservablePointF(x.Min(), y.Min()));
            GraphDim.Add(new ObservablePointF(x.Max(), y.Max()));
            linearReg.Add(new ObservablePointF(x.Min(), c.lin_reg.f(x.Min())));
            linearReg.Add(new ObservablePointF(x.Max(), c.lin_reg.f(x.Max())));
        }

        public void moveOneFrame()
        {
            currentFrame++;
            int j = (int)currentFrame;
            bool isAnomaly = detector.isAnomalous(x[j], y[j], detector.getCFbyFeature(feature1));
            pointsIsAnomalyQueue.Enqueue(isAnomaly);
            if (isAnomaly) {
                pointsAnomalies.Add(new ObservablePointF(x[j], y[j]));
            } 
            else
            {
                pointsRegular.Add(new ObservablePointF(x[j], y[j]));
            }
            if (pointsIsAnomalyQueue.Count >= MAX_SIZE + 1) {
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
            if (feature1 != null && detector.getCFbyFeature(newFeature).feature2 != null)
            {
                setNewLine();
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
            if (newFrame < MAX_SIZE-1)
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




    // DLL Connector
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
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (lin_reg_cf c in graph.detector.getNormalData())
            {
                dict.Add(c.feature1, c.feature2);
            }
            return dict;
        }
    }

}
