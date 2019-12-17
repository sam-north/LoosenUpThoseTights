using LoosenItUp.Dtos;
using LoosenItUp.Factories;
using System;
using System.IO;

namespace LoosenItUp.FileProcessors
{
    public class CamdenDocXFileProcessor : IFileProcessor
    {
        string CustomerName = "Camden South llc";
        public ResultDto Process(string filename)
        {
            var result = ObjectFactory.Create<ResultDto>();

            var customerOccuranceDto = ObjectFactory.Create<OccuranceDto>();
            customerOccuranceDto.Name = CustomerName;
            customerOccuranceDto.Occurance += 1;
            result.CustomerOccurrances.Add(customerOccuranceDto);
            var customerBalanceDto = ObjectFactory.Create<BalanceOwedDto>();
            customerBalanceDto.Name = CustomerName;

            var patientNameSearchString = "Our records indicate that,";
            var amountSearchString = "owes us money in the sum of";
            foreach (var line in File.ReadLines(filename))
            {
                var patientSubstringIndex = line.IndexOf(patientNameSearchString);
                if (result.PatientOccurrances.Count == 0 && patientSubstringIndex != -1)
                {
                    var restOfLine = line.Substring(patientSubstringIndex + patientNameSearchString.Length);
                    var nextCommaIndex = restOfLine.IndexOf(",");
                    var patientOccuranceDto = ObjectFactory.Create<OccuranceDto>();
                    patientOccuranceDto.Name = restOfLine.Substring(0, nextCommaIndex).Trim();
                    patientOccuranceDto.Occurance += 1;
                    result.PatientOccurrances.Add(patientOccuranceDto);
                }

                var amountSubstringIndex = line.IndexOf(amountSearchString);
                if (result.PatientBalancesOwed.Count == 0 && amountSubstringIndex != -1)
                {
                    var restOfLine = line.Substring(amountSubstringIndex + amountSearchString.Length);
                    var decimalPointIndex = restOfLine.IndexOf(".");
                    var patientAmountOwed = ObjectFactory.Create<BalanceOwedDto>();
                    patientAmountOwed.Name = result.PatientOccurrances[0].Name;
                    var restOfLineAfterDecimal = restOfLine.Substring(decimalPointIndex);
                    var closingPeriodIndexAfterAmount = restOfLineAfterDecimal.IndexOf(".");
                    var amount = Convert.ToDecimal(restOfLine.Substring(0, closingPeriodIndexAfterAmount).Trim());
                    patientAmountOwed.Total += amount;
                    customerBalanceDto.Total += amount;
                    result.PatientBalancesOwed.Add(patientAmountOwed);
                }
            }
            result.BalancesOwedToCustomer.Add(customerBalanceDto);
            return result;
        }
    }
}
