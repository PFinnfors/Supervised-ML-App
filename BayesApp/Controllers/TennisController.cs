using Accord;
using Accord.MachineLearning.Bayes;
using Accord.Math;
using Accord.Statistics.Filters;
using BayesApp.Models;
using System;
using System.Data;
using System.Web.Mvc;

namespace BayesApp.Controllers
{
    public class TennisController : Controller
    {
        public DataTable TennisDataSet { get; set; }
        public Codification TennisCodebook { get; set; }

        //Constructor initializes and prepares data
        public TennisController()
        {
            //
            TennisDataSet = InitTennisDataSet();
            TennisCodebook = GetTennisCodebook(TennisDataSet);
        }

        public string[] GetTennisAnswer(TennisModel model)
        {
            // Extract input and output pairs to train
            DataTable symbols = TennisCodebook.Apply(TennisDataSet);
            int[][] inputs = symbols.ToJagged<int>("Outlook", "Temperature", "Humidity", "Wind");
            int[] outputs = symbols.ToArray<int>("PlayTennis");

            //
            var learner = new NaiveBayesLearning();
            NaiveBayes nb = learner.Learn(inputs, outputs);

            //Run user input into accord.net bayes to get an integer value decision for "PlayTennis"
            int[] instance = TennisCodebook.Transform(model.Outlook, model.Temperature, model.Humidity, model.Wind);
            int c = nb.Decide(instance);

            //Get a Yes / No answer, as well as the "probabilities" calculated by the bayesian
            string result = TennisCodebook.Revert("PlayTennis", c);
            double[] probs = nb.Probabilities(instance); // { e.g. 0.795, 0.205 }

            //Convert to percentages
            probs[0] = probs[0] * 100;
            probs[1] = probs[1] * 100;
            
            //Return the answer in two formats
            string[] answerValues = new string[3] {result, probs[0].ToString("#.0"), probs[1].ToString("#.0")};
            return answerValues;
        }

        public DataTable InitTennisDataSet()
        {
            DataTable tData = new DataTable("Mitchell's Tennis Example");

            tData.Columns.Add("Day", "Outlook", "Temperature", "Humidity", "Wind", "PlayTennis");

            tData.Rows.Add("D1", "Sunny", "Mild", "Normal", "Medium", "Yes");
            tData.Rows.Add("D2", "Overcast", "Cool", "High", "Strong", "No");
            tData.Rows.Add("D3", "Overcast", "Mild", "High", "Strong", "No");
            tData.Rows.Add("D4", "Rain", "Hot", "Normal", "Strong", "No");
            tData.Rows.Add("D5", "Overcast", "Cool", "Normal", "Weak", "Yes");
            tData.Rows.Add("D6", "Overcast", "Mild", "Low", "Strong", "No");
            tData.Rows.Add("D7", "Overcast", "Hot", "Low", "Medium", "Yes");
            tData.Rows.Add("D8", "Overcast", "Mild", "Normal", "Medium", "Yes");
            tData.Rows.Add("D9", "Rain", "Mild", "Low", "Strong", "No");
            tData.Rows.Add("D10", "Sunny", "Hot", "High", "Weak", "Yes");
            tData.Rows.Add("D11", "Overcast", "Hot", "Normal", "Medium", "No");
            tData.Rows.Add("D12", "Overcast", "Cool", "Low", "Weak", "Yes");
            tData.Rows.Add("D13", "Rain", "Mild", "Low", "Medium", "No");
            tData.Rows.Add("D14", "Rain", "Hot", "High", "Weak", "Yes");
            tData.Rows.Add("D15", "Rain", "Hot", "Low", "Weak", "Yes");
            tData.Rows.Add("D16", "Overcast", "Hot", "Normal", "Weak", "Yes");
            tData.Rows.Add("D17", "Rain", "Cool", "Normal", "Weak", "No");
            tData.Rows.Add("D18", "Rain", "Hot", "High", "Weak", "Yes");
            tData.Rows.Add("D19", "Rain", "Cool", "Normal", "Strong", "No");
            tData.Rows.Add("D20", "Rain", "Cool", "High", "Strong", "No");
            tData.Rows.Add("D21", "Overcast", "Cool", "Low", "Weak", "No");
            tData.Rows.Add("D22", "Sunny", "Cool", "Low", "Weak", "Yes");
            tData.Rows.Add("D23", "Sunny", "Cool", "High", "Medium", "Yes");
            tData.Rows.Add("D24", "Rain", "Cool", "Low", "Medium", "No");
            tData.Rows.Add("D25", "Overcast", "Cool", "Normal", "Strong", "No");
            tData.Rows.Add("D26", "Rain", "Cool", "Normal", "Medium", "No");
            tData.Rows.Add("D27", "Rain", "Mild", "Low", "Strong", "No");
            tData.Rows.Add("D28", "Sunny", "Hot", "High", "Medium", "No");
            tData.Rows.Add("D29", "Sunny", "Cool", "Low", "Strong", "No");
            tData.Rows.Add("D30", "Sunny", "Mild", "High", "Strong", "Yes");

            return tData;
        }

        public Codification GetTennisCodebook(DataTable iData)
        {
            // Create a new codebook to convert strings into integers for accord.net
            Codification codebook = new Codification(iData,
                "Outlook", "Temperature", "Humidity", "Wind", "PlayTennis");

            return codebook;
        }

    }
}