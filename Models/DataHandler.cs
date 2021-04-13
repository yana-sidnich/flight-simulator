using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FlightGearTestExec
{
    class DataHandler
    {
        private string timeseriesCSV;

        private string pathCSV;

        private string pathXML;

        private List<string> attributes;

        private Dictionary<string, List<float>> _dataByColumn;

        private Dictionary<string, FlightDataContainer> _dataDictionary;

        private List<string> dataByRow;

        public Dictionary<string, FlightDataContainer> DataDictionary
        {
            get { return _dataDictionary; }
            set { }
        }

        public Dictionary<string, List<float>> DataByColumn
        {
            get { return _dataByColumn; }
            set { }
        }

        public List<string> DataByRow
        {
            get { return dataByRow; }
            set { }
        }

        public DataHandler(string csv, string xml)

        {
            this.pathCSV = csv;

            this.pathXML = xml;

            timeseriesCSV = "myFlight.csv";

            attributes = new List<string>();

            _dataByColumn = new Dictionary<string, List<float>>();

            _dataDictionary = new Dictionary<string, FlightDataContainer>();

            dataByRow = new List<string>();

            this.parseXmlAttributes();
            Trace.Write(this.attributes);

            this.initData();
            this.createCSV();

        }



        private void parseXmlAttributes()

        {

            StreamReader sr = File.OpenText(pathXML);

            string text = sr.ReadToEnd();

            string[] list = text.Split(new string[] { "<output>" }, StringSplitOptions.None);

            string output = list[1].Split(new string[] { "</output>" }, StringSplitOptions.None)[0];

            string[] s = output.Split(new string[] { "<name>" }, StringSplitOptions.None);

            int i = 1;

            for (; i < s.Length; i++)

            {

                string name = s[i].Split(new string[] { "</name>" }, StringSplitOptions.None)[0];

                attributes.Add(name);

            }

            // deals with duplications in feature names
            string[] names = attributes.ToArray();

            Dictionary<string, int> count = new Dictionary<string, int>();

            foreach (string name in attributes)
            {
                if (!count.ContainsKey(name))
                    count.Add(name, 0);
                else if (count[name] == 0)
                    count[name] += 2;
                else
                    count[name]++;
            }
            for (int a = (names.Length - 1); a >= 0; a--)
            {
                if (count[names[a]] != 0)
                    names[a] += "_" + (count[names[a]]--).ToString();
            }
            attributes = names.ToList<string>();

            sr.Close();

        }



        public void initData()

        {

            foreach (string att in attributes)

            {
                if (!_dataByColumn.ContainsKey(att))
                {
                    _dataByColumn.Add(att, new List<float>());
                }

                if (!_dataDictionary.ContainsKey(att))
                {
                    _dataDictionary.Add(att, new FlightDataContainer());
                }                

            }

            StreamReader sr = File.OpenText(pathCSV);

            string line;

            while ((line = sr.ReadLine()) != null)

            {
                dataByRow.Add(line);

                string[] s_vals = line.Split(',');

                string last_attr = "";
                for (int i = 0; i < s_vals.Length; i++)

                {
                    // if first line contains column names - continue
                    if (!float.TryParse(s_vals[i], out _))
                        continue;
                    if (last_attr != attributes[i])
                    {
                        _dataByColumn[attributes[i]].Add(float.Parse(s_vals[i]));
                    }

                    if (last_attr != attributes[i])
                    {
                        _dataDictionary[attributes[i]].addValue(float.Parse(s_vals[i]));
                    }
                                        
                    last_attr = attributes[i];


                }

            }

        }



        private void createCSV()

        {

            StreamReader sr = File.OpenText(pathCSV);

            string text = sr.ReadToEnd();

            sr.Close();

            StreamWriter sw = File.CreateText(timeseriesCSV);

            string s = attributes[0];

            for (int i = 1; i < attributes.Count; i++)

            {

                s += "," + attributes[i];

            }

            sw.WriteLine(s);

            sw.Write(text);

            sw.Close();

        }


    }

}