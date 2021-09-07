using System;
using System.Collections.Generic;
using System.Text;
using CalculationService.Enums;

namespace CalculationService.Dtos
{
    public class CalculatorDto
    {
        public Operand Operand { get; set; }
        public decimal Num1 { get; set; }
        public decimal Num2 { get; set; }
    }
}
