using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

class Program
{
    private static CloudTable studentTable;

    static async Task Main(string[] args)
    {
        string connectionString = ""; 
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
        CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

        studentTable = tableClient.GetTableReference("StudentTable");
        await studentTable.CreateIfNotExistsAsync();

        // Test CRUD operations
        await CreateStudentAsync(new StudentEntity("UniBucharest", "123") { Name = "Alice", Age = 21, Department = "IT" });
        await ReadStudentAsync("UniBucharest", "123");
        await UpdateStudentAsync("UniBucharest", "123", "Engineering");
        await DeleteStudentAsync("UniBucharest", "123");
    }

    private static async Task CreateStudentAsync(StudentEntity student)
    {
        TableOperation insertOperation = TableOperation.Insert(student);
        await studentTable.ExecuteAsync(insertOperation);
        Console.WriteLine("Student created.");
    }

    private static async Task ReadStudentAsync(string partitionKey, string rowKey)
    {
        TableOperation retrieveOperation = TableOperation.Retrieve<StudentEntity>(partitionKey, rowKey);
        TableResult result = await studentTable.ExecuteAsync(retrieveOperation);
        StudentEntity student = result.Result as StudentEntity;

        if (student != null)
        {
            Console.WriteLine($"Read Student: {student.Name}, Age: {student.Age}, Department: {student.Department}");
        }
        else
        {
            Console.WriteLine("Student not found.");
        }
    }

    private static async Task UpdateStudentAsync(string partitionKey, string rowKey, string newDepartment)
    {
        TableOperation retrieveOperation = TableOperation.Retrieve<StudentEntity>(partitionKey, rowKey);
        TableResult result = await studentTable.ExecuteAsync(retrieveOperation);
        StudentEntity student = result.Result as StudentEntity;

        if (student != null)
        {
            student.Department = newDepartment;
            TableOperation updateOperation = TableOperation.Replace(student);
            await studentTable.ExecuteAsync(updateOperation);
            Console.WriteLine("Student updated.");
        }
        else
        {
            Console.WriteLine("Student not found.");
        }
    }

    private static async Task DeleteStudentAsync(string partitionKey, string rowKey)
    {
        TableOperation retrieveOperation = TableOperation.Retrieve<StudentEntity>(partitionKey, rowKey);
        TableResult result = await studentTable.ExecuteAsync(retrieveOperation);
        StudentEntity student = result.Result as StudentEntity;

        if (student != null)
        {
            TableOperation deleteOperation = TableOperation.Delete(student);
            await studentTable.ExecuteAsync(deleteOperation);
            Console.WriteLine("Student deleted.");
        }
        else
        {
            Console.WriteLine("Student not found.");
        }
    }
}
