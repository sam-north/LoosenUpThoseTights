using LoosenItUp.Dtos;
using LoosenItUp.Factories;
using LoosenItUp.FileProcessors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LoosenItUp
{
    class Program
    {
        static void Main(string[] args)
        {
            //get files
            var directory = GetDirectory();
            var filenames = Directory.GetFiles(directory + "\\files");
            //loop
            var results = ObjectFactory.Create<List<ResultDto>>();
            foreach (var filename in filenames)
            {
                var fileProcessor = FileProcessorProvider.GetFileProcessor(filename);
                results.Add(fileProcessor.Process(filename));
            }

            //display data
            var aggregatedResults = AggregateResults(results);
            PrintResults(aggregatedResults);
        }

        private static string GetDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var pathDirectory = Path.GetDirectoryName(path);
            var searchTermIndex = pathDirectory.IndexOf(".vs\\");
            if (searchTermIndex == -1)
                searchTermIndex = pathDirectory.IndexOf("\\bin");
            var result = pathDirectory.Substring(0, searchTermIndex);
            return result;
        }

        private static void PrintResults(ResultDto aggregatedResults)
        {
            Console.WriteLine("*****Customer Occurances*****");
            foreach (var item in aggregatedResults.CustomerOccurrances)
                Console.WriteLine(item.Name + " - " + item.Occurance);

            Console.WriteLine();
            Console.WriteLine("*****Customer Receivables*****");
            foreach (var item in aggregatedResults.BalancesOwedToCustomer)
                Console.WriteLine(item.Name + " - " + item.Total);

            Console.WriteLine();
            Console.WriteLine("*****Patient Occurances*****");
            foreach (var item in aggregatedResults.PatientOccurrances)
                Console.WriteLine(item.Name + " - " + item.Occurance);

            Console.WriteLine();
            Console.WriteLine("*****Patient Balances*****");
            foreach (var item in aggregatedResults.PatientBalancesOwed)
                Console.WriteLine(item.Name + " - " + item.Total);
        }

        private static ResultDto AggregateResults(IList<ResultDto> results)
        {
            var aggregatedResults = ObjectFactory.Create<ResultDto>();
            foreach (var item in results)
            {
                foreach (var customerOccurance in item.CustomerOccurrances)
                {
                    var existingCustomerOccurance = aggregatedResults.CustomerOccurrances.SingleOrDefault(x => x.Name == customerOccurance.Name);
                    if (existingCustomerOccurance == null)
                    {
                        existingCustomerOccurance = ObjectFactory.Create<OccuranceDto>();
                        existingCustomerOccurance.Name = customerOccurance.Name;
                        aggregatedResults.CustomerOccurrances.Add(existingCustomerOccurance);
                    }
                    existingCustomerOccurance.Occurance += customerOccurance.Occurance;
                }

                foreach (var customerBalance in item.BalancesOwedToCustomer)
                {
                    var existingCustomerBalance = aggregatedResults.BalancesOwedToCustomer.SingleOrDefault(x => x.Name == customerBalance.Name);
                    if (existingCustomerBalance == null)
                    {
                        existingCustomerBalance = ObjectFactory.Create<BalanceOwedDto>();
                        existingCustomerBalance.Name = customerBalance.Name;
                        aggregatedResults.BalancesOwedToCustomer.Add(existingCustomerBalance);
                    }
                    existingCustomerBalance.Total += customerBalance.Total;
                }

                foreach (var patientOccurance in item.PatientOccurrances)
                {
                    var existingPatientOccurance = aggregatedResults.PatientOccurrances.SingleOrDefault(x => x.Name == patientOccurance.Name);
                    if (existingPatientOccurance == null)
                    {
                        existingPatientOccurance = ObjectFactory.Create<OccuranceDto>();
                        existingPatientOccurance.Name = patientOccurance.Name;
                        aggregatedResults.PatientOccurrances.Add(existingPatientOccurance);
                    }
                    existingPatientOccurance.Occurance += patientOccurance.Occurance;
                }

                foreach (var customerBalance in item.PatientBalancesOwed)
                {
                    var existingPatientBalance = aggregatedResults.PatientBalancesOwed.SingleOrDefault(x => x.Name == customerBalance.Name);
                    if (existingPatientBalance == null)
                    {
                        existingPatientBalance = ObjectFactory.Create<BalanceOwedDto>();
                        existingPatientBalance.Name = customerBalance.Name;
                        aggregatedResults.PatientBalancesOwed.Add(existingPatientBalance);
                    }
                    existingPatientBalance.Total += customerBalance.Total;
                }
            }
            return aggregatedResults;
        }
    }
}
