using Microsoft.Azure.Cosmos.Table;

public class StudentEntity : TableEntity
{
    public StudentEntity(string university, string studentId)
    {
        PartitionKey = university;
        RowKey = studentId;
    }

    public StudentEntity() { }

    public string Name { get; set; }
    public int Age { get; set; }
    public string Department { get; set; }
}
