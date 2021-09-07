using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Model;
using Amazon.Runtime;
using CalculationService.Dtos;
using CalculationService.Enums;
using CalculationService.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CalculationService
{
    public class MainFunction
    {
        // Configuration Service
        public IConfiguration Configuration { get; set; }
        public ILambdaService LambdaService { get; set; }

        public MainFunction()
        {
            // Set up Dependency Injection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Get Configuration from DI system
            Configuration = serviceProvider.GetService<IConfigurationService>()?.GetConfiguration();
            LambdaService = serviceProvider.GetService<ILambdaService>();
        }
        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IConfigurationService, ConfigurationService>();

            var awsAccessKey = "Your access key";  
            var awsSecretKey = "Your secret key";
            var awsCredentials = new BasicAWSCredentials(awsAccessKey, awsSecretKey);
            var lambdaConfig = new AmazonLambdaConfig() { RegionEndpoint = RegionEndpoint.APSoutheast2 };
            serviceCollection.AddSingleton<AmazonLambdaClient>(sp =>
                new AmazonLambdaClient(awsCredentials, lambdaConfig));

            serviceCollection.AddSingleton<ILambdaService, LambdaService>();
        }

        /// <summary>
        /// A simple function that is deployed to API Gateway and take query parameters as input, and do simple calculation
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var env = Configuration.GetSection("App")["EnvironmentName"];
            var dto = new CalculatorDto();

            if (env == "development")
            {
                dto.Operand = Operand.Sub;
                dto.Num1 = 100.5m;
                dto.Num2 = 80.1m;
            }
            else  //Get input from API query string
            {
                var opr = request.QueryStringParameters["operand"];
                dto.Operand = (Operand)Enum.ToObject(typeof(Operand), Convert.ToInt16(opr));
                dto.Num1 = Convert.ToDecimal(request.QueryStringParameters["num1"]);
                dto.Num2 = Convert.ToDecimal(request.QueryStringParameters["num2"]);
            };

            var response = await LambdaService.GetLambdaResponse(dto);

            Console.WriteLine($" {JsonConvert.SerializeObject(dto)}\r\n result: {response}");
            LambdaLogger.Log($" {JsonConvert.SerializeObject(dto)}\r\n result: {response}");

            return new APIGatewayProxyResponse()
            {
                StatusCode = 200,
                Body = response
            };
        }
    }
}
