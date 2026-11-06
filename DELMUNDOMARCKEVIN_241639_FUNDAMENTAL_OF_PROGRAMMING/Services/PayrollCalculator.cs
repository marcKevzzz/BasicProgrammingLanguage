namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

public sealed record PayrollBreakdown(decimal Salary, decimal SalaryCredit, decimal Sss,
    decimal PhilHealth, decimal PagIbig, decimal TaxableIncome, decimal WithholdingTax)
{
    public decimal Contributions => Sss + PhilHealth + PagIbig;
    public decimal TotalDeductions => Contributions + WithholdingTax;
    public decimal NetPay => Salary - TotalDeductions;
}

public static class PayrollCalculator
{
    public static decimal Money(decimal value) => decimal.Round(value, 2, MidpointRounding.AwayFromZero);

    // SSS employee schedule effective January 2025; PhilHealth 5%; HDMF Circular 460.
    public static PayrollBreakdown Calculate(decimal salary)
    {
        if (salary > 0)
        {
            if (salary == Money(salary))
            {
                decimal credit = Math.Clamp(decimal.Floor((Math.Min(salary, 35000m) + 250m) / 500m) * 500m, 5000m, 35000m);
                decimal sss = Money(credit * 0.05m);
                decimal philHealth = Money(Math.Clamp(salary, 10000m, 100000m) * 0.05m / 2m);
                decimal pagIbigRate;
                if (salary <= 1500m) pagIbigRate = 0.01m;
                else pagIbigRate = 0.02m;
                decimal pagIbig = Money(Math.Min(salary, 10000m) * pagIbigRate);
                decimal taxable = Math.Max(0m, salary - sss - philHealth - pagIbig);
                return new(salary, credit, sss, philHealth, pagIbig, taxable, MonthlyTax(taxable));
            }
            else throw new ArgumentException("Salary must use whole centavos.", nameof(salary));
        }
        else throw new ArgumentOutOfRangeException(nameof(salary));
    }

    // BIR RR 11-2018 Annex E: monthly table effective January 2023 onwards.
    public static decimal MonthlyTax(decimal taxable)
    {
        decimal tax;
        if (taxable <= 20833m) tax = 0m;
        else if (taxable < 33333m) tax = (taxable - 20833m) * 0.15m;
        else if (taxable < 66667m) tax = 1875m + (taxable - 33333m) * 0.20m;
        else if (taxable < 166667m) tax = 8541.80m + (taxable - 66667m) * 0.25m;
        else if (taxable < 666667m) tax = 33541.80m + (taxable - 166667m) * 0.30m;
        else tax = 183541.80m + (taxable - 666667m) * 0.35m;
        return Money(tax);
    }
}
