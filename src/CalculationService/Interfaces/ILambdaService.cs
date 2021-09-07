using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CalculationService.Dtos;

namespace CalculationService.Interfaces
{
    public interface ILambdaService
    {
        Task<string> GetLambdaResponse(CalculatorDto dto);
    }
}
