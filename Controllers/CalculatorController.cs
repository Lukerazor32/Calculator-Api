using Microsoft.AspNetCore.Mvc;
using NCalc;
using System.Text.RegularExpressions;

namespace ApiTestApp.Controllers
{
    [ApiController]
    [Route("api/calculator")]
    public class CalculatorController : ControllerBase
    {

        public CalculatorController()
        {
        }

        [HttpGet("plus")]
        public IActionResult Plus([FromQuery] double a, [FromQuery] double b)
        {
            return Ok(new { Result = a + b });
        }

        [HttpGet("minus")]
        public IActionResult Minus([FromQuery] double a, [FromQuery] double b)
        {
            return Ok(new { Result = a - b });
        }

        [HttpGet("multiply")]
        public IActionResult Multiply([FromQuery] double a, [FromQuery] double b)
        {
            return Ok(new { Result = a * b });
        }

        [HttpGet("divide")]
        public IActionResult Divide(double a, double b)
        {
            if (a == 0 || b == 0)
            {
                return BadRequest("Ошибка при делении на ноль");
            }
            return Ok(new { Result = a / b });
        }

        [HttpGet("exponent")]
        public IActionResult Exponent(double a, double b)
        {
            return Ok(new { Result = Math.Pow(a, b) });
        }

        [HttpGet("extract")]
        public IActionResult Extract(double a, double b)
        {
            return Ok(new { Result = Math.Pow(a, 1.0 / b) });
        }

        [HttpPost("custom")]
        public IActionResult CustomExpression([FromBody] string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return BadRequest("Выражение не может быть пустым.");
            }

            try
            {
                Console.WriteLine(ReplacePowerSymbolWithPow(value));
                Expression expression = new Expression(ReplacePowerSymbolWithPow(value));
                object result = expression.Evaluate();
                
                return Ok(new { Result = result });
            }
            catch (Exception e)
            {
                return BadRequest($"Ошибка при вычислении выражения: {e}");
            }
        }

        static string ReplacePowerSymbolWithPow(string expression)
        {
            string correctExpr = expression.Replace(" ", "");
            string pattern = @"(\d+\.?\d*|\(.+?\))\^(\d+\.?\d*|\(.+?\))";
            string replacement = "Pow($1, $2)";

            return Regex.Replace(correctExpr, pattern, replacement);
        }
    }
}
