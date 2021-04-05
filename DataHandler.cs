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

        private Dictionary<string, List<double>> dataByColumn;

        private List<string> dataByRow;

        public Dictionary<string, List<double>> DataByColumn
        {
            get { return dataByColumn; }
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

            dataByColumn = new Dictionary<string, List<double>>();

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

            sr.Close();

        }



        public void initData()

        {

            foreach (string att in attributes)

            {
                if(!dataByColumn.ContainsKey(att))
                {
                    dataByColumn.Add(att, new List<double>());
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
                    if (last_attr != attributes[i])
                    {
                        dataByColumn[attributes[i]].Add(Convert.ToDouble(s_vals[i]));
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

