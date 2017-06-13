using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Registrar.Objects;

namespace Registrar
{
  [Collection("registrar_test")]
  public class CourseTest : IDisposable
  {
    public CourseTest()
    {
      DBConfiguration.ConnectionString  = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Course.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Course.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfCoursesAreTheSame()
    {
      Course firstCourse = new Course("Billiards", "B-125", default(DateTime));
      Course secondCourse = new Course("Billiards", "B-125", default(DateTime));
      Assert.Equal(firstCourse, secondCourse);
    }

    [Fact]
    public void Test_Save_ToCourseDatabase()
    {
      Course testCourse = new Course("Hand Fishing", "H-15", new DateTime (2017, 10, 8));
      testCourse.Save();

      List<Course> result = Course.GetAll();
      List<Course> testList = new List<Course>{testCourse};
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      Course testCourse = new Course("Hand Fishing", "H-15", new DateTime (2017, 10, 8));
      testCourse.Save();
      int testId = testCourse.GetId();
      int savedCourseId = Course.GetAll()[0].GetId();
      Assert.Equal(testId, savedCourseId);
    }

    [Fact]
    public void Test_Find_FindsCourseInDatabase()
    {
      Course testCourse = new Course("MeepMerp", "MM-00", new DateTime (2017, 10, 8));
      testCourse.Save();
      Course foundCourse = Course.Find(testCourse.GetId());
      Assert.Equal(testCourse, foundCourse);
    }
  }
}
