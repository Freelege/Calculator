using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using Calculator;

namespace Calculator.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void CalculationFunction_GivenCorrectInput_ShouldReturnCorrectResult()
        {

            // Invoke the lambda function and confirm the string was upper cased.
            var function = new CalculationFunction();
            var context = new TestLambdaContext();
           
           var dto = new CalculatorDto()
           {
               Operand = Operand.Add,
               Num1 = 100.5m,
               Num2 = 20.1m
           };

           var result = function.FunctionHandler(dto, context);
           Assert.Equal(dto.Num1 + dto.Num2, result);

           dto.Operand = Operand.Sub;
           result = function.FunctionHandler(dto, context);
           Assert.Equal(dto.Num1 - dto.Num2, result);

           dto.Operand = Operand.Mul;
           result = function.FunctionHandler(dto, context);
           Assert.Equal(dto.Num1 * dto.Num2, result);

           dto.Operand = Operand.Div;
           result = function.FunctionHandler(dto, context);
           Assert.Equal(dto.Num1 / dto.Num2, result);
        }

        [Fact]
        public void CalculationFunction_GivenZeroDividend_ShouldThrowException()
        {
            var function = new CalculationFunction();
            var context = new TestLambdaContext();

            var dto = new CalculatorDto()
            {
                Operand = Operand.Div,
                Num1 = 100.5m,
                Num2 = 0
            };

            var ex = Assert.Throws<Exception>(() => function.FunctionHandler(dto, context));

            Assert.Equal("the dividend is zero!", ex.Message);
        }
    }
}
