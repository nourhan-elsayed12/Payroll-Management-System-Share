using PayrollManagementSystem.DTOs.Common;
using PayrollManagementSystem.DTOs.Reports;

namespace PayrollManagementSystem.Business.Interfaces
{
    public interface IReportService
    {
        Task<List<AttendanceReportRowDto>> GetAttendanceAsync(DateOnly month, int? departmentId, CancellationToken cancellationToken);

        Task<List<EmployeesReportRowDto>> GetEmployeesAsync(int departmentId, CancellationToken cancellationToken);

        // month = first day of the month the incentives and deductions are calculated for
        Task<List<IncentivesReportRowDto>> GetIncentivesAsync(DateOnly month, int departmentId, CancellationToken cancellationToken);

        Task<List<LookupItemDto>> GetDepartmentsLookupAsync(CancellationToken cancellationToken);

        Task<string?> GetDepartmentNameAsync(int departmentId, CancellationToken cancellationToken);

    }
}
