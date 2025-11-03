using System.Linq;
using VelocipedeUtils.Ubp.Core.Models.Configurations;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;
using VelocipedeUtils.Ubp.Core.Models.Pipes;
using VelocipedeUtils.Ubp.Core.Pipes;
using VelocipedeUtils.Ubp.Core.Repositories;

namespace VelocipedeUtils.Ubp.Core.Domain.DatasetGenerators;

/// <summary>
/// Class for initializing datasets.
/// </summary>
public class DatasetGenerator
{
    private AppSettings _appSettings;
    private IUnitOfWork _unitOfWork;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public DatasetGenerator(
        AppSettings appSettings,
        IUnitOfWork unitOfWork)
    {
        _appSettings = appSettings;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Initializes datasets.
    /// </summary>
    public void Initialize()
    {
        var pipeParams = new PipeParams(_appSettings.EmployeeQty, _appSettings.VacationIntervals);
        var result = new PipeResult(pipeParams);
        var generatingPipe = new PipeBuilder(_appSettings, InsertIntoRepositories)
            .AddGenerating(typeof(EmployeePipe))
            .AddGenerating(typeof(VacationPipe))
            .Build();
        generatingPipe(result);
    }

    /// <summary>
    /// Inserts initial datasets into the repositories.
    /// </summary>
    private void InsertIntoRepositories(PipeResult result)
    {
        foreach (var employee in result.Employees)
        {
            _unitOfWork.EmployeeRepository.Insert(employee);
        }
        foreach (var vacation in result.Vacations)
        {
            _unitOfWork.VacationRepository.Insert(vacation);
        }

        System.Console.WriteLine("Data added into the repositories");
    }
}