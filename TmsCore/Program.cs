
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