using System; using Xunit; using task13;
namespace task13tests;
public class JsonStudentTests {
    private readonly JsonStudentService _service = new();
    [Fact] public void Serialize_WithNullGrades_ShouldIgnoreGradesProperty() {
        var json = _service.SerializeStudent(new Student { FirstName = "А", LastName = "Б", Grades = null });
        Assert.DoesNotContain("Grades", json);
    }
    [Fact] public void Deserialize_EmptyName_ShouldThrowArgumentException() {
        Assert.Throws<ArgumentException>(() => _service.DeserializeStudent("{\"FirstName\":\"\",\"LastName\":\"Б\"}"));
    }
}
