using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Ubp.Core.Domain.DatasetGenerators;

public interface IVacationGenerator
{
    List<Absense> GenerateVacations(
        Employee employee,
        List<int> vacationIntervals,
        System.Func<System.DateTime, System.DateTime, System.DateTime> generateDate);
}