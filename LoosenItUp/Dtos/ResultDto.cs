using LoosenItUp.Factories;
using System.Collections.Generic;

namespace LoosenItUp.Dtos
{
    public class ResultDto
    {
        public IList<BalanceOwedDto> PatientBalancesOwed { get; set; } = ObjectFactory.Create<List<BalanceOwedDto>>();
        public IList<BalanceOwedDto> BalancesOwedToCustomer { get; set; } = ObjectFactory.Create<List<BalanceOwedDto>>();
        public IList<OccuranceDto> PatientOccurrances { get; set; } = ObjectFactory.Create<List<OccuranceDto>>();
        public IList<OccuranceDto> CustomerOccurrances { get; set; } = ObjectFactory.Create<List<OccuranceDto>>();
    }
}
