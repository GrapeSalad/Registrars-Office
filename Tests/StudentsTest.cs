using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Registrar.Objects;

namespace Registrar
{
  [Collection("registrar_test")]
  public class StudentTest : IDisposable
  {
    public StudentTest()
    {
      DBConfiguration.ConnectionString  = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Student.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Student.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfStudentsAreTheSame()
    {
      Student firstStudent = new Student("Joe", "Mo", default(DateTime));
      Student secondStudent = new Student("Joe", "Mo", default(DateTime));
      Assert.Equal(firstStudent, secondStudent);
    }
    [Fact]
    public void Test_Save_ToStudentDatabase()
    {
      Student testStudent = new Student("Joe", "Mo", new DateTime(1999, 01, 01));
      testStudent.Save();

      List<Student> result = Student.GetAll();
      List<Student> testList = new List<Student>{testStudent};
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      Student testStudent = new Student("Joe", "Mo", new DateTime(1999, 01, 01));
      testStudent.Save();
      int testId = testStudent.GetId();
      int savedStudentId = Student.GetAll()[0].GetId();
      Assert.Equal(testId, savedStudentId);
    }
  }
}
