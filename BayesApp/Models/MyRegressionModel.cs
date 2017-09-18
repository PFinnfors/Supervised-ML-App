using Accord.Statistics.Models.Regression.Linear;
using Newtonsoft.Json.Linq;

namespace BayesApp.Models
{
    public class MyRegressionModel
    {
        public double[] inputs;
        public double[] outputs;

        public OrdinaryLeastSquares ols;
        public SimpleLinearRegression regression;

        public MyRegressionModel(double[] inputs, double[] outputs)
        {
            this.inputs = inputs;
            this.outputs = outputs;

            this.ols = new OrdinaryLeastSquares();
            this.regression = ols.Learn(inputs, outputs);
        }

        public double Predict(double input)
        {
            return regression.Transform(input);
        }
    }
}