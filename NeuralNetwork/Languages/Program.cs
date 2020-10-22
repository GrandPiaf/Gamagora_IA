using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using FANNCSharp;
using FANNCSharp.Float;

namespace NN_Languages
{
    public class Program
    {
        static void Main(string[] args)
        {

            string trainingFilename = "xor.data";
            string resultFilename = "xor_float.net";

            /** TRAINING **/
            {
                // Connectivity & layers
                const float connectivity = 1f;
                const int num_layers = 3;
                const int num_input = 2;
                const int num_output = 1;
                const int num_neurons_hidden = 3;

                // Training variables
                const int max_epochs = 500000;
                const int epochs_betwen_reports = 1000;
                const float desired_error = 0.0001f;


                // Creating NN
                Console.WriteLine("Creating Neural Network");
                NeuralNet net = new NeuralNet(connectivity, num_layers, num_input, num_neurons_hidden, num_output);

                net.ActivationFunctionHidden = ActivationFunction.SIGMOID_SYMMETRIC;
                net.ActivationFunctionOutput = ActivationFunction.SIGMOID_SYMMETRIC;

                Console.WriteLine("Training NN on : " + trainingFilename);
                net.TrainOnFile(trainingFilename, max_epochs, epochs_betwen_reports, desired_error);

                Console.WriteLine("Saving NN on : " + resultFilename);
                net.Save(resultFilename);
            }

            Console.WriteLine("Training complete. Press ENTER to run test.");
            Console.ReadLine();


            /** RUNNING TEST **/
            {
                NeuralNet nnUse = new NeuralNet(resultFilename);

                float[] input = new float[2];
                input[0] = -1;
                input[1] = 1;

                float[] results = nnUse.Run(input);

                Console.WriteLine("xor test (" + input[0] + "," + input[1] +") -> " + results[0]);
            }

            Console.ReadLine();
        }
    }
}
