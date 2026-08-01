namespace APIEnterprise.Pagination
{
    public class EmployeesBirthDateFilter : PaginationParams
    {
        public DateOnly? BirthDate { get; set; }
        public String? Filter { get; set; }
    }
}
