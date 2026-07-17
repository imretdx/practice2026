using System; using System.IO; using System.Collections.Generic; using System.Text.Json; using System.Text.Json.Serialization;
namespace task13;
public class Subject { public string Name { get; set; } = string.Empty; public int Grade { get; set; } }
public class Student {
    public string FirstName { get; set; } = string.Empty; public string LastName { get; set; } = string.Empty; public DateTime BirthDate { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public List<Subject>? Grades { get; set; }
}
public class JsonStudentService {
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
    public string SerializeStudent(Student s) => JsonSerializer.Serialize(s, _options);
    public Student DeserializeStudent(string json) {
        var s = JsonSerializer.Deserialize<Student>(json, _options) ?? throw new InvalidOperationException();
        if (string.IsNullOrWhiteSpace(s.FirstName) || string.IsNullOrWhiteSpace(s.LastName)) throw new ArgumentException();
        return s;
    }
}
