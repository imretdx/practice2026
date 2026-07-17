using System.Collections.Generic;
using System.Linq;
using Xunit;
using task02;

namespace task02tests;

public class StudentServiceTests
{
    private readonly List<Student> _testStudents;
    private readonly StudentService _service;

    public StudentServiceTests()
    {
        _testStudents = new List<Student>
        {
            new() { Name = "Иван", Faculty = "ФИТ", Grades = new List<int> { 5, 4, 5 } },
            new() { Name = "Анна", Faculty = "ФИТ", Grades = new List<int> { 3, 4, 3 } },
            new() { Name = "Петр", Faculty = "Экономика", Grades = new List<int> { 5, 5, 5 } }
        };
        _service = new StudentService(_testStudents);
    }

    [Fact] public void GetStudentsByFaculty_ReturnsCorrectStudents() => Assert.Equal(2, _service.GetStudentsByFaculty("ФИТ").Count());
    [Fact] public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty() => Assert.Equal("Экономика", _service.GetFacultyWithHighestAverageGrade());
    [Fact] public void GetStudentsWithMinAverageGrade_ReturnsCorrectStudents() => Assert.Equal("Петр", _service.GetStudentsWithMinAverageGrade(4.8).First().Name);
    [Fact] public void GetStudentsOrderedByName_ReturnsSortedStudents() => Assert.Equal("Анна", _service.GetStudentsOrderedByName().First().Name);
    [Fact] public void GroupStudentsByFaculty_ReturnsCorrectGroups() => Assert.Equal(2, _service.GroupStudentsByFaculty()["ФИТ"].Count());
}
