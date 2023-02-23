using MiniORM.App.Data.Entities;
using MiniORM.App.Data;

var connectionString = "Server=.;Database=MiniORM;Integrated Security=True;Encrypt=False";

var context = new SoftUniDbContext(connectionString);

context.Employees.Add(new Employee
{
    FirstName = "Gosho",
    LastName = "Inserted",
    DepartmentId = context.Departments.First().Id,
    IsEmployed = true,
});

var employee = context.Employees.Last();
employee.FirstName = "Modified";

context.SaveChanges();