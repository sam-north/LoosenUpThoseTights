using LoosenItUp.Dtos;
using LoosenItUp.Factories;
using System;
using System.IO;

namespace LoosenItUp.FileProcessors
{
    public class BossmanDocXFileProcessor : IFileProcessor
    {
        string CustomerName = "Dr. Boss Man ltd.";
        public ResultDto Process(string filename)
        {
            var result = ObjectFactory.Create<ResultDto>();

            var customerOccuranceDto = ObjectFactory.Create<OccuranceDto>();
            customerOccuranceDto.Name = CustomerName;
            customerOccuranceDto.Occurance += 1;
            result.CustomerOccurrances.Add(customerOccuranceDto);
            var customerBalanceDto = ObjectFactory.Create<BalanceOwedDto>();
            customerBalanceDto.Name = CustomerName;

            var patientNameSearchString = "Patient:";
            var amountSearchString = "Amount:";
            foreach (var line in File.ReadLines(filename))
            {
                var patientSubstringIndex = line.IndexOf(patientNameSearchString);
                if (result.PatientOccurrances.Count == 0 && patientSubstringIndex != -1)
                {
                    var restOfLine = line.Substring(patientSubstringIndex + patientNameSearchString.Length);
                    var patientOccuranceDto = ObjectFactory.Create<OccuranceDto>();
                    patientOccuranceDto.Name = restOfLine.Substring(0).Trim();
                    patientOccuranceDto.Occurance += 1;
                    result.PatientOccurrances.Add(patientOccuranceDto);
                }

                var amountSubstringIndex = line.IndexOf(amountSearchString);
                if (result.PatientBalancesOwed.Count == 0 && amountSubstringIndex != -1)
                {
                    var restOfLine = line.Substring(amountSubstringIndex + amountSearchString.Length);
                    var patientAmountOwed = ObjectFactory.Create<BalanceOwedDto>();
                    patientAmountOwed.Name = result.PatientOccurrances[0].Name;
                    var amount = Convert.ToDecimal(restOfLine.Substring(0).Trim());
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
