using System;
using System.Collections.Generic;
using System.Linq;

namespace task02;

public class Student
{
    public string Name { get; set; } = string.Empty;
    public string Faculty { get; set; } = string.Empty;
    public List<int> Grades { get; set; } = new();
}

public class StudentService
{
    private readonly List<Student> _students;

    public StudentService(List<Student> students)
    {
        _students = students;
    }

    public IEnumerable<Student> GetStudentsByFaculty(string faculty)
    {
        return _students.Where(s => s.Faculty == faculty);
    }

    public IEnumerable<Student> GetStudentsWithMinAverageGrade(double minAverageGrade)
    {
        return _students.Where(s => s.Grades.Any() && s.Grades.Average() >= minAverageGrade);
    }

    public IEnumerable<Student> GetStudentsOrderedByName()
    {
        return _students.OrderBy(s => s.Name);
    }

    public ILookup<string, Student> GroupStudentsByFaculty()
    {
        return _students.ToLookup(s => s.Faculty);
    }

    public string GetFacultyWithHighestAverageGrade()
    {
        var validStudents = _students.Where(s => s.Grades.Any());
        
        var groupedFaculties = validStudents.GroupBy(s => s.Faculty);
        
        var ratedFaculties = groupedFaculties.Select(g => new 
        { 
            FacultyName = g.Key, 
            AvgGrade = g.Average(s => s.Grades.Average()) 
        });
        
        var bestFaculty = ratedFaculties.OrderByDescending(x => x.AvgGrade).FirstOrDefault();
        
        if (bestFaculty == null)
        {
            return string.Empty;
        }
        
        return bestFaculty.FacultyName;
    }
}
