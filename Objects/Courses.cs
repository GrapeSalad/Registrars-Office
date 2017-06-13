using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Registrar;

namespace Registrar.Objects
{
  public class Course
  {
    private int _id;
    private string _name;
    private string _courseNumber;
    private DateTime _startDate;

    public Course(string Name, string CourseNumber, DateTime StartDate, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _courseNumber = CourseNumber;
      _startDate = StartDate;
    }

    public override bool Equals(System.Object otherCourse)
    {
      if (!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        bool idEquality = this.GetId() == newCourse.GetId();
        bool nameEquality = this.GetName() == newCourse.GetName();
        bool courseNumberEquality = this.GetCourseNumber() == newCourse.GetCourseNumber();
        bool startDateEquality = this.GetStartDate() == newCourse.GetStartDate();
        return (idEquality && nameEquality && courseNumberEquality && startDateEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public string GetCourseNumber()
    {
      return _courseNumber;
    }
    public DateTime GetStartDate()
    {
      return _startDate;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM courses;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Course> GetAll()
    {
      List<Course> allCourses = new List<Course>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        string courseName = rdr.GetString(1);
        string courseCourseNumber = rdr.GetString(2);
        DateTime courseStartDate = rdr.GetDateTime(3);
        Course newCourse = new Course(courseName, courseCourseNumber, courseStartDate, courseId);
        allCourses.Add(newCourse);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allCourses;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO courses (name, courseNumber, startDate) OUTPUT INSERTED.id VALUES (@Name, @CourseNumber, @StartDate)", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@Name";
      nameParameter.Value = this.GetName();
      SqlParameter courseNumberParameter = new SqlParameter();
      courseNumberParameter.ParameterName = "@CourseNumber";
      courseNumberParameter.Value = this.GetCourseNumber();
      SqlParameter startDateParameter = new SqlParameter();
      startDateParameter.ParameterName = "@StartDate";
      startDateParameter.Value = this.GetStartDate();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(courseNumberParameter);
      cmd.Parameters.Add(startDateParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }
  }
}
