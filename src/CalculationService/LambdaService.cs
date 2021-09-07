using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using CalculationService.Dtos;
using CalculationService.Interfaces;
using Newtonsoft.Json;

namespace CalculationService
{
    class LambdaService : ILambdaService
    {
        private readonly AmazonLambdaClient _lambdaClient;

        public LambdaService(AmazonLambdaClient lambdaClient)
        {
            _lambdaClient = lambdaClient;
        }

        public async Task<string> GetLambdaResponse(CalculatorDto dto)
        {
            var lambdaRequest = new InvokeRequest
            {
                FunctionName = "Calculator",
                Payload = JsonConvert.SerializeObject(dto)
            };

            var response = await _lambdaClient.InvokeAsync(lambdaRequest);
            if (response != null)
            {
                using var sr = new StreamReader(response.Payload);
                return await sr.ReadToEndAsync();
            }
            return string.Empty;
        }
    }
}
