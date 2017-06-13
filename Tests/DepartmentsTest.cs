// using Xunit;
// using System;
// using System.Collections.Generic;
// using System.Data;
// using System.Data.SqlClient;
// using Registrar.Objects;
//
// namespace Registrar
// {
//   [Collection("Registrar")]
//   public class DepartmentTest : IDisposable
//   {
//     public DepartmentTest()
//     {
//       DBConfiguration.ConnectionString  = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
//     }
//
//     public void Dispose()
//     {
//       Course.DeleteAll();
//       Student.DeleteAll();
//       Department.DeleteAll();
//     }
//
//     [Fact]
//     public void Test_DatabaseEmptyAtFirst()
//     {
//       int result = Department.GetAll().Count;
//       Assert.Equal(0, result);
//     }
//
//     [Fact]
//     public void Test_Equal_ReturnsTrueIfDepartmentsAreTheSame()
//     {
//       Department firstDepartment = new Department("Arts and Stuff");
//       Department secondDepartment = new Department("Arts and Stuff");
//       Assert.Equal(firstDepartment, secondDepartment);
//     }
//
//     [Fact]
//     public void Test_Save_ToDepartmentDatabase()
//     {
//       Department testDepartment = new Department("Hand Fishing");
//       testDepartment.Save();
//
//       List<Department> result = Department.GetAll();
//       List<Department> testList = new List<Department>{testDepartment};
//       Assert.Equal(testList, result);
//     }
//
//     [Fact]
//     public void Test_Save_AssignsIdToObject()
//     {
//       Department testDepartment = new Department("Hand Fishing");
//       testDepartment.Save();
//       int testId = testDepartment.GetId();
//       int savedDepartmentId = Department.GetAll()[0].GetId();
//       Assert.Equal(testId, savedDepartmentId);
//     }
//
//     [Fact]
//     public void Test_Find_FindsDepartmentInDatabase()
//     {
//       Department testDepartment = new Department("MeepMerp");
//       testDepartment.Save();
//       Department foundDepartment = Department.Find(testDepartment.GetId());
//       Assert.Equal(testDepartment, foundDepartment);
//     }
//
//     [Fact]
//     public void GetCourses_ReturnsAllDepartments_Via_MajorsList()
//     {
//
//       Department testDepartment = new Department("Billiards");
//       testDepartment.Save();
//
//       Course testCourse1 = new Course("Trick-Shots", "B-310", new DateTime(2017, 1, 1), 2);
//       testCourse1.Save();
//       Course testCourse2 = new Course("Sharking", "B-215", new DateTime(2017, 1, 1), 5);
//       testCourse2.Save();
//
//       Student testStudent = new Student("Willie", "Nelson", new DateTime(1984, 10, 16));
//       testStudent.Save();
//
//       testCourse1.AddStudent(testStudent);
//       testCourse2.AddStudent(testStudent);
//
//       testDepartment.AddCourseAndStudent(testCourse1, testStudent);
//       testDepartment.AddCourseAndStudent(testCourse2, testStudent);
//       List<Course> savedCourses = testDepartment.GetCourses();
//       List<Course> testList = new List<Course> {testCourse1, testCourse2};
//       Console.WriteLine("savedCourses id = {0}, {1}", savedCourses[0].GetName(), savedCourses[1].GetName());
//       Console.WriteLine("testList id = {0}, {1}", testList[0].GetName(), testList[1].GetName());
//       Assert.Equal(testList, savedCourses);
//     }
//   }
// }
