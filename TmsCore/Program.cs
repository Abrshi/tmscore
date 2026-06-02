
// Exercise 1 — Null Safety Basics


string? region = null;

// Null-conditional operator (?.)
// string? upperRegion = region?.ToUpper();
// Console.WriteLine($"Region (conditional): {upperRegion}");

// Null-coalescing operator (??)
// string displayRegion = region ?? "Unassigned";
// // Console.WriteLine($"Region (coalesced): {displayRegion}");
// // Null-coalescing assignment (??=)
// region ??= "Addis Ababa";
// Console.WriteLine($"Region (assigned): {region}");

// Console.WriteLine(); // spacing

// Step 3: Core TMS variables

// string studentName = "Abeba";
// string studentId = "STU-001";
// int enrollmentCount = 3;
// decimal grantAmount = 1999.99m;
// DateTime enrolledAt = DateTime.UtcNow;

// string? campusRegion = null;

// Console.WriteLine($"Student: {studentName} ({studentId})");
// Console.WriteLine($"Courses: {enrollmentCount}");
// Console.WriteLine($"Grant: {grantAmount:F2}");
// Console.WriteLine($"Enrolled: {enrolledAt:yyyy-MM-dd}");
// Console.WriteLine($"Campus: {campusRegion ?? "Not assigned"}");



// exercise 2
// // Legacy implementation — the bug that caused the audit failure 
// double grantPerStudent = 1999.99; 
// double totalAllocation = grantPerStudent * 100_000; 
// Console.WriteLine($"Total allocated (double): {totalAllocation}");
// Fixed implementation — exact financial math 
// decimal grantPerStudent = 1999.99m; 
// decimal totalAllocation = grantPerStudent * 100_000m; 
// Console.WriteLine($"Total allocated (decimal): {totalAllocation}"); 
// Console.WriteLine($"Total allocated (formatted): {totalAllocation:F2}");



// exercise 3

// Legacy implementation — what the logging service did to the data 
//  public readonly class Enrollment
// {
// public string StudentId { get; set; } = string.Empty;
// public string CourseCode { get; set; } = string.Empty;
// public DateTime ProcessedAt { get; set; }
// }


var enrollment = new EnrollmentRecord("STU-001", "CS-401", DateTime.UtcNow);
Console.WriteLine(enrollment);
// Try to mutate it — uncomment this line and see the compiler error:
// enrollment.CourseCode = "HACKED"; // ERROR: init-only property
// Non-destructive copy — creates a NEW record with one field changed
var corrected = enrollment with { CourseCode = "CS-402" };
Console.WriteLine(corrected);
// Value equality — two records with the same data are equal
var duplicate = new EnrollmentRecord("STU-001", "CS-401", enrollment.EnrolledAt);
Console.WriteLine($"Same data? {enrollment == duplicate}"); // True



// Legacy Pre-C# 14 Implementation (Verbose)
// public class Course
// {
// private int _capacity; // Manual backing field
// public int Capacity
// {
// get => _capacity;
// set
// {
// if (value <= 0)
// throw new ArgumentOutOfRangeException("Capacity must be positive.");
// _capacity = value;
// }
// }
// }


var course = new Course { Code = "CS-401", Title = "Advanced C#", Capacity = 30 };
// Console.WriteLine($"Course: {course.Title} (Capacity: {course.Capacity})");
// // Invalid capacity — should throw
// try
// {course.Capacity = -5;
// }
// catch (ArgumentOutOfRangeException ex)
// {
// Console.WriteLine($"Caught: {ex.Message}");
// }
// // Invalid title — should throw
// try
// {
// course.Title = "";
// }
// catch (ArgumentException ex)
// {
// Console.WriteLine($"Caught: {ex.Message}");
// }

var s = new Student { Id = "S1", Name = "Abeba", Age = 20, GPA = 3.8m };
Console.WriteLine($"Student: {s.Name}, GPA: {s.GPA}");


void PrintGradeReport(IEnumerable<IGradable> assessments)
{
Console.WriteLine("--- Grade Report ---");
foreach (var item in assessments)
{
Console.WriteLine($"{item.Title}: {item.CalculateGrade():F2}%");
}
}
// Test it — one array holds two completely different types
IGradable[] cohortAssessments = [
new Quiz { Title = "C# Basics", CorrectAnswers = 18, TotalQuestions = 20 },
new LabAssignment { Title = "Registration API", FunctionalityScore = 90m, CodeQualityScore = 85m }
];
PrintGradeReport(cohortAssessments);








// s2


var service = new EnrollmentService();
// Test 1: Valid registration
var validStudent = new Student { Id = "S1", Name = "Abeba", Age = 20, GPA = 3.8m };
var validCourse = new Course { Code = "CS-401", Title = "Advanced C#", Capacity = 30 };
var result = service.ProcessRegistration(validStudent, validCourse);
Console.WriteLine($"Enrolled: {result.StudentId} in {result.CourseCode}");
// Test 2: Null student should throw
try
{
service.ProcessRegistration(null, validCourse);
}
catch (ArgumentNullException ex)
{
Console.WriteLine($"Guard caught: {ex.ParamName}");
}
// Test 3: Full course should throw
var fullCourse = new Course { Code = "CS-402", Title = "Full Course", Capacity = 1 };
fullCourse.EnrolledCount = 1;
try
{
service.ProcessRegistration(validStudent, fullCourse);
}
catch (InvalidOperationException ex)
{
Console.WriteLine($"Business rule: {ex.Message}");
}



// C# 12+ Collection Expressions the modern way to initialize lists
List<Student> students = [
new Student { Id = "S1", Name = "Abeba", Age = 22, GPA = 3.8m },
new Student { Id = "S2", Name = "Kidane", Age = 21, GPA = 2.4m },
new Student { Id = "S3", Name = "Dawit", Age = 20, GPA = 3.1m },
new Student { Id = "S4", Name = "Sara", Age = 23, GPA = 3.9m },
new Student { Id = "S5", Name = "Frehiwot", Age = 19, GPA = 2.0m },
new Student { Id = "S6", Name = "Yonas", Age = 24, GPA = 3.5m },
new Student { Id = "S7", Name = "Meron", Age = 22, GPA = 1.8m },
new Student { Id = "S8", Name = "Tesfaye", Age = 21, GPA = 2.9m }
];

var leaderboard = students
    // TODO 1: GPA >= 3.5
    .Where(student => student.GPA >= 3.5m)

    // TODO 2: Sort descending
    .OrderByDescending(student => student.GPA)

    // TODO 3: Keep only Name
    .Select(student => student.Name)

    // TODO 4: Convert to List
    .ToList();

Console.WriteLine($"Found {leaderboard.Count} Honors Students:");

foreach (var name in leaderboard)
{
    Console.WriteLine($"- {name}");
}

// TODO 5: Average GPA
decimal averageGpa = students.Average(student => student.GPA);

Console.WriteLine($"\nClass Average GPA: {averageGpa:F2}");


// TODO 6: GroupBy with switch expression
var standingGroups = students.GroupBy(student => student.GPA switch
{
    >= 3.5m => "Honors",
    >= 2.5m => "Good Standing",
    >= 2.0m => "Probation",
    _ => "Academic Warning"
});

Console.WriteLine("\n--- Academic Standing Report ---");

foreach (var group in standingGroups)
{
    Console.WriteLine($"\n{group.Key} ({group.Count()}):");

    foreach (var student in group)
    {
        Console.WriteLine($" {student.Name} GPA: {student.GPA}");
    }
}


// TODO 7: Spread operator
string[] backendCourses = ["C#", "ASP.NET Core"];
string[] frontendCourses = ["TypeScript", "Angular"];

string[] allCourses =
[
    ..backendCourses,
    ..frontendCourses,
    "Database Design"
];

Console.WriteLine($"\nFull curriculum: {string.Join(", ", allCourses)}");