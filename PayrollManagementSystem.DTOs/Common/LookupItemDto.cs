namespace PayrollManagementSystem.DTOs.Common
{
    // Used for dropdown lists (departments, job grades, ...)
    public class LookupItemDto
    {
        public int Id { get; init; }

        public string Name { get; init; } = string.Empty;
    }
}
