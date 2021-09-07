using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using CalculationService;
using Amazon.Lambda.APIGatewayEvents;
using CalculationService.Dtos;
using CalculationService.Enums;
using CalculationService.Interfaces;
using NSubstitute;

namespace CalculationService.Tests
{
    public class FunctionTest
    {
        private readonly ILambdaService _lambdaService;
        private decimal _result;
        public FunctionTest()
        {
            _lambdaService = Substitute.For<ILambdaService>();

           _lambdaService.When(obj => obj.GetLambdaResponse(Arg.Is<CalculatorDto>(x => x.Operand == Operand.Add))).Do(
               info =>
               {
                   _result = info.Arg<CalculatorDto>().Num1 + info.Arg<CalculatorDto>().Num2;
               });

           _lambdaService.When(obj => obj.GetLambdaResponse(Arg.Is<CalculatorDto>(x => x.Operand == Operand.Sub))).Do(
               info =>
               {
                   _result = info.Arg<CalculatorDto>().Num1 - info.Arg<CalculatorDto>().Num2;
               });

           _lambdaService.When(obj => obj.GetLambdaResponse(Arg.Is<CalculatorDto>(x => x.Operand == Operand.Mul))).Do(
               info =>
               {
                   _result = info.Arg<CalculatorDto>().Num1 * info.Arg<CalculatorDto>().Num2;
               });

           _lambdaService.When(obj => obj.GetLambdaResponse(Arg.Is<CalculatorDto>(x => x.Operand == Operand.Div))).Do(
               info =>
               {
                   _result = info.Arg<CalculatorDto>().Num1 / info.Arg<CalculatorDto>().Num2;
               });
        }

        [Fact]
        public async Task CalculationService_GivenCorrectInput_ShouldReturnResultSuccessfully()
        {

            // Invoke the lambda function and confirm the string was upper cased.
            var function = new MainFunction {LambdaService = _lambdaService};
            var context = new TestLambdaContext();
            var request = new APIGatewayProxyRequest()
            {
                QueryStringParameters = new Dictionary<string, string>()
                {
                    {"operand", "0"},
                    {"num1", "100.5"},
                    {"num2", "80.2"}
                }
            };
             var response = await function.FunctionHandler(request, context);
             Assert.Equal(180.7m,_result);
             Assert.Equal(200, response.StatusCode);

            request.QueryStringParameters = new Dictionary<string, string>()
             {
                 {"operand", "1"},
                 {"num1", "100.5"},
                 {"num2", "80.2"}
             };
             response = await function.FunctionHandler(request, context);
             Assert.Equal(20.3m, _result);
             Assert.Equal(200, response.StatusCode);

            request.QueryStringParameters = new Dictionary<string, string>()
             {
                 {"operand", "2"},
                 {"num1", "10.5"},
                 {"num2", "2.0"}
             };
             response = await function.FunctionHandler(request, context);
             Assert.Equal(21m, _result);
             Assert.Equal(200, response.StatusCode);

            request.QueryStringParameters = new Dictionary<string, string>()
             {
                 {"operand", "3"},
                 {"num1", "10.5"},
                 {"num2", "2.0"}
             };
             response = await function.FunctionHandler(request, context);
             Assert.Equal(5.25m, _result);
             Assert.Equal(200, response.StatusCode);
        }
    }
}
