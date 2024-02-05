using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.SharedSchemas;
using System.Linq;

namespace Core.Modules.HumanResources.Application.Http.SharedValidators;

internal static class ContractValidators
{ 
    //TODO: Reduce Resource Usage in this method
    public static bool HaveValidContributionYears(List<ContractRequestSchema> contracts)
    { 
        var contributionYears = contracts.SelectMany(c => c.ContributionsYears, (_, cy) => cy.Year).ToList();
        var uniqueContributionYears = contributionYears.ToHashSet();

        if (uniqueContributionYears.Count < contributionYears.Count)
        {
            return false;
        }
        
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
                    if (workHour.Date.Year != contributionYear.Year)
                    {
                        return false; 
                    }

                    if (workHour.End.Subtract(workHour.Start).TotalHours > 12)
                    {
                        return false;
                    }
                    
                }
            }
        }
        return true;
    }
    
        public static bool HaveOnlyOneContractForASector(List<ContractRequestSchema> contracts)
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