using System.Text;
using System.Linq;
using System.Globalization;

using Microsoft.EntityFrameworkCore;

using SoftUni.Data;
using SoftUni.Models;
using System.Security.Cryptography.X509Certificates;

namespace SoftUni
{
    public class StartUp
    {
        static void Main()
        {
            SoftUniContext context = new SoftUniContext();

            // Problem 03: Employees Full Information
            Console.WriteLine(GetEmployeesFullInformation(context));

            // Problem 04: Employees with Salary Over 50000
            Console.WriteLine(GetEmployeesWithSalaryOver50000(context));

            // Problem 05: Employees from Research and Development
            Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));

            // Problem 06: Adding a New Address and Updating Employees
            Console.WriteLine(AddNewAddressToEmployee(context));

            // Problem 07: Employees and Projects
            Console.WriteLine(GetEmployeesInPeriod(context));

            // Problem 08: Addressed by Town
            Console.WriteLine(GetAddressesByTown(context));

            // Problem 09: Employee 147
            Console.WriteLine(GetEmployee147(context));

            // Problem 10: Departments with More than 5 Employees
            Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));

            // Problem 11: Find Latest 10 Projects
            Console.WriteLine(GetLatestProjects(context));

            // Problem 12: Increase Salaries
            Console.WriteLine(IncreaseSalaries(context));

            // Problem 13: Find Employees by First Name Starting with Sa
            Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));

            // Problem 14: Delete Project by Id
            Console.WriteLine(DeleteProjectById(context));

            // Problem 15: Remove Town
            Console.WriteLine(RemoveTown(context));
        }

        // Problem 03: Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context
                .Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.MiddleName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.EmployeeId)
                .ToList();

            var sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }
            return sb.ToString().Trim();
        }

        // Problem 04: Employees with Salary Over 50000
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context
                .Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .ToList();

            var sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }
            return sb.ToString().Trim();
        }

        // Problem 05: Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context
                .Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .Where(e => e.DepartmentName == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList();

            var sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.DepartmentName} - ${employee.Salary:f2}");
            }
            return sb.ToString().Trim();
        }

        // Problem 06: Adding a New Address and Updating Employee
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var newAddress = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };
            var employee = context
                .Employees
                .FirstOrDefault(e => e.LastName == "Nakov");
            context.Addresses.Add(newAddress);
            employee.Address = newAddress;
            context.SaveChanges();

            var addresses = context
                .Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => e.Address)
                .Take(10)
                .ToList();

            var sb = new StringBuilder();
            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}");
            }
            return sb.ToString().Trim();
        }

        // Problem 07: Employees and Projects
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context
                .Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.Projects
                        .Select(p => new
                        {
                            p.Name,
                            p.StartDate,
                            p.EndDate
                        })
                        .Where(p => p.StartDate.Year >= 2001 && p.StartDate.Year <= 2003)
                })
                .Take(10)
                .ToList();

            var sb = new StringBuilder();
            string patternDateTime = "M/d/yyyy h:mm:ss tt";
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");
                foreach (var project in employee.Projects)
                {
                    object projectEndDate = project.EndDate == null
                        ? "not finished"
                        : $"{((DateTime)project.EndDate).ToString(patternDateTime, CultureInfo.InvariantCulture)}";
                    sb.AppendLine($"--{project.Name} - {project.StartDate.ToString(patternDateTime, CultureInfo.InvariantCulture)} - {projectEndDate}");
                }
            }
            return sb.ToString().Trim();
        }

        // Problem 08: Addresses by Town
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context
                .Addresses
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    a.Employees
                })
                .OrderByDescending(a => a.Employees.Count())
                .ThenBy(a => a.TownName)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .ToList();

            var sb = new StringBuilder();
            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.Employees.Count} employees");
            }
            return sb.ToString().Trim();
        }

        // Problem 09: Employee 147
        public static string GetEmployee147(SoftUniContext context)
        {

            var employee = context
                .Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Projects
                })
                .FirstOrDefault(x => x.EmployeeId == 147);

            var sb = new StringBuilder();
            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            foreach (var project in employee.Projects.OrderBy(p => p.Name))
            {
                sb.AppendLine(project.Name);
            }
            return sb.ToString().Trim();
        }

        // Problem 10: Departments with More Than 5 Employees
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context
                .Departments
                .Where(d => d.Employees.Count > 5)
                .Select(d => new
                {
                    d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    d.Employees
                })
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .ToList();

            var sb = new StringBuilder();
            foreach (var department in departments)
            {
                sb.AppendLine($"{department.Name} - {department.ManagerFirstName} {department.ManagerLastName}");
                foreach (var employee in department.Employees.OrderBy(e => e.FirstName).ThenByDescending(e => e.LastName))
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }
            return sb.ToString().Trim();
        }

        // Problem 11: Find Latest 10 Projects
        public static string GetLatestProjects(SoftUniContext context)
        {
            var patternDate = "M/d/yyyy h:mm:ss tt";
            var projects = context
                .Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .OrderBy(p => p.Name)
                .ToList();

            var sb = new StringBuilder();
            foreach (var project in projects)
            {
                sb.AppendLine($"{project.Name}");
                sb.AppendLine($"{project.Description}");
                sb.AppendLine($"{project.StartDate.ToString(patternDate, CultureInfo.InvariantCulture)}");
            }
            return sb.ToString().Trim();
        }

        // Problem 12: Increase Salaries
        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context
                .Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" || e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            var sb = new StringBuilder();
            foreach (var employee in employees)
            {
                employee.Salary += employee.Salary * 0.12m;
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})");
            }
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        // Problem 13: Find Employees by First Name Starting with "Sa"
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context
                .Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            var sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:f2})");
            }
            return sb.ToString().Trim();
        }

        // Problem  14: Delete Project by Id
        public static string DeleteProjectById(SoftUniContext context)
        {
            var projectToRemove = context.Projects.Find(2);

            var employees = context.Employees
                .Include(e => e.Projects)
                .Where(e => e.Projects.Any(p => p.ProjectId == 2))
                .ToList();

            foreach (var employee in employees)
            {
                employee.Projects.Remove(projectToRemove);
                context.SaveChanges();
            }

            context.Projects.Remove(projectToRemove);
            context.SaveChanges();

            var projectsToPrint = context
                .Projects
                .Take(10)
                .ToList();
            var sb = new StringBuilder();

            foreach (var project in projectsToPrint)
            {
                sb.AppendLine(project.Name);
            }
            return sb.ToString().Trim();
        }

        // Problem 15: Remove Town
        public static string RemoveTown(SoftUniContext context)
        {
            var addressesToDelete = context
                .Addresses
                .Where(a => a.Town.Name == "Seattle")
                .Select(a => a.AddressId)
                .ToList();

            var employees = context
                .Employees
                .Where(e => e.AddressId.HasValue && addressesToDelete.Contains(e.AddressId.Value))
                .Select(e => e.EmployeeId)
                .ToList();

            foreach (var employee in context.Employees)
            {
                if (employees.Contains(employee.EmployeeId))
                {
                    employee.AddressId = null;
                }
            }

            var townToDelete = context
                .Towns
                .FirstOrDefault(t => t.Name == "Seattle");
            context.Towns.Remove(townToDelete);
            context.Addresses.RemoveRange(context.Addresses.Where(a => a.Town.Name == "Seattle"));
            context.SaveChanges();

            return $"{addressesToDelete.Count} addresses in Seattle were deleted";
        }
    }
}