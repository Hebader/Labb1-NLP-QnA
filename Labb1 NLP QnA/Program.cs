using Azure;
using Azure.AI.Language.QuestionAnswering;
using Azure.AI.TextAnalytics;
using System;

namespace AzureNlpQnA
{
    internal class Program
    {
        // Text Analytics 
        private static readonly string textAnalyticsEndpoint = "https://nlpqna.cognitiveservices.azure.com/";
        private static readonly string textAnalyticsKey = "bac9c7f9be4e46f2a5cbcb6cd4f890b5 ";

        // QnA 
        private static readonly string qnaEndpoint = "https://heba123.cognitiveservices.azure.com/";
        private static readonly string qnaKey = "2d6f8a80d1f24f43846d7e6b8db75667";
        private static readonly string projectName = "Cats";
        private static readonly string deploymentName = "production";

        static void Main(string[] args)
        {
            //Skapar instanser av klienter som för att kommunicera med QNA och Text Analystics
      
            var textAnalyticsClient = new TextAnalyticsClient(new Uri(textAnalyticsEndpoint), new AzureKeyCredential(textAnalyticsKey));
            var qnaClient = new QuestionAnsweringClient(new Uri(qnaEndpoint), new AzureKeyCredential(qnaKey));
            var project = new QuestionAnsweringProject(projectName, deploymentName);

            //skapar en meny där användaren får välja ett alternativ

            while (true)
            {
       
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Ask a question to QnA");
                Console.WriteLine("2. Detect language of a text");
                Console.WriteLine("3. Exit");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        AskQuestion(qnaClient, project);
                        break;
                    case "2":
                        DetectLanguageAndDisplay(textAnalyticsClient);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please select again.");
                        break;
                }
            }
        }
        // Metod för QNA
        static void AskQuestion(QuestionAnsweringClient client, QuestionAnsweringProject project)
        {
            Console.WriteLine("Enter your question:");
            string question = Console.ReadLine();

            try
            {
                var response = client.GetAnswers(question, project);
                Console.WriteLine($"QnA Answer: {(response.Value.Answers.Count > 0 ? response.Value.Answers[0].Answer : "No answer found.")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
        }

        //Metod för Text Analystics
        static void DetectLanguageAndDisplay(TextAnalyticsClient client)
        {
            Console.WriteLine("Enter the text to detect language:");
            string text = Console.ReadLine();

            try
            {
                var response = client.DetectLanguage(text);
             
                var detectedLanguage = response.Value;
                Console.WriteLine($"Detected Language: {detectedLanguage.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
        }
    }
}
