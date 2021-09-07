using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Calculator
{
    public class CalculationFunction
    {
        /// <summary>
        /// A simple function that does basic arithmetic calculation
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public decimal FunctionHandler(CalculatorDto dto, ILambdaContext context)
        {
            var result = 0m;
            switch (dto.Operand)
            {
                case Operand.Add:
                    result = dto.Num1 + dto.Num2;
                    break;
                case Operand.Sub:
                    result = dto.Num1 - dto.Num2;
                    break;
                case Operand.Mul:
                    result = dto.Num1 * dto.Num2;
                    break;
                case Operand.Div:
                    if (dto.Num2 == 0)
                        throw new Exception("the dividend is zero!");

                    result = dto.Num1 / dto.Num2;
                    break;
                default:
                    throw new Exception("operand type is invalid!");
            }

            return result;
        }
    }
}
