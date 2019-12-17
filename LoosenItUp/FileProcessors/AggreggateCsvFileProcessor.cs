using LoosenItUp.Dtos;
using LoosenItUp.Factories;
using System;
using System.IO;
using System.Linq;

namespace LoosenItUp.FileProcessors
{
    public class AggreggateCsvFileProcessor : IFileProcessor
    {
        public ResultDto Process(string filename)
        {
            var results = ObjectFactory.Create<ResultDto>();
            foreach (var line in File.ReadLines(filename))
            {
                var splitLine = line.Split(",");

                var customerName = splitLine[0];
                var patientName = splitLine[1];
                var amount = Convert.ToDecimal(splitLine[2]);

                var existingCustomerOccurance = results.CustomerOccurrances.SingleOrDefault(x => x.Name == customerName);
                if (existingCustomerOccurance == null)
                {
                    existingCustomerOccurance = ObjectFactory.Create<OccuranceDto>();
                    existingCustomerOccurance.Name = customerName;
                    results.CustomerOccurrances.Add(existingCustomerOccurance);
                }
                existingCustomerOccurance.Occurance += 1;

                var existingCustomerBalance = results.BalancesOwedToCustomer.SingleOrDefault(x => x.Name == customerName);
                if (existingCustomerBalance == null)
                {
                    existingCustomerBalance = ObjectFactory.Create<BalanceOwedDto>();
                    existingCustomerBalance.Name = customerName;
                    results.BalancesOwedToCustomer.Add(existingCustomerBalance);
                }
                existingCustomerBalance.Total += amount;

                var existingPatientOccurance = results.PatientOccurrances.SingleOrDefault(x => x.Name == patientName);
                if (existingPatientOccurance == null)
                {
                    existingPatientOccurance = ObjectFactory.Create<OccuranceDto>();
                    existingPatientOccurance.Name = patientName;
                    results.PatientOccurrances.Add(existingPatientOccurance);
                }
                existingPatientOccurance.Occurance += 1;

                var existingPatientBalance = results.PatientBalancesOwed.SingleOrDefault(x => x.Name == patientName);
                if (existingPatientBalance == null)
                {
                    existingPatientBalance = ObjectFactory.Create<BalanceOwedDto>();
                    existingPatientBalance.Name = patientName;
                    results.PatientBalancesOwed.Add(existingPatientBalance);
                }
                existingPatientBalance.Total += amount;
            }
            return results;
        }
    }
}
