using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.SharedCommandParts;

namespace Core.Modules.HumanResources.Application.Http.SharedValidators;

internal static class ContractValidators
{
      //TODO: Reduce Resource Usage in this method
        public static bool HaveValidContributionYears(List<ContractCommand> contracts)
        {
            foreach (var contract in contracts)
            {
                foreach (var contributionYear in contract.ContributionsYears)
                {
                    if (contributionYear.Year < contract.StartDate.Year || contributionYear.Year > contract.EndDate.Year)
                    {
                        return false;
                    }
    
                    foreach (var workHour in contributionYear.WorkHours)
                    {
                        if (workHour.Date.Year < contract.StartDate.Year || workHour.Date.Year > contract.EndDate.Year)
                        {
                            return false;
                        }
    
                        if (workHour.Start.Subtract(workHour.End).TotalHours >= 18)
                        {
                            return false;
                        }
                    }
                }
            }
    
            return true;
        }
    
        public static bool HaveOnlyOneContractForASector(List<ContractCommand> contracts)
        {
            var sectors = contracts.Select(c => c.Sector).ToList();
            var numberOfSectors = sectors.Count;
            var distinctSectors = sectors.Distinct().Count();
    
            if (numberOfSectors > distinctSectors)
            {
                return false;
            }
            
            return true;
        }
}