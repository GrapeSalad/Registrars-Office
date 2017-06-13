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
    private int _credits;

    public Course(string Name, string CourseNumber, DateTime StartDate, int Credits, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _courseNumber = CourseNumber;
      _startDate = StartDate;
      _credits = Credits;
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
        bool creditEquality = this.GetCredits() == newCourse.GetCredits();
        return (idEquality && nameEquality && courseNumberEquality && startDateEquality && creditEquality);
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
    public int GetCredits()
    {
      return _credits;
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
        int credits = rdr.GetInt32(4);
        Course newCourse = new Course(courseName, courseCourseNumber, courseStartDate, credits, courseId);
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

      SqlCommand cmd = new SqlCommand("INSERT INTO courses (name, courseNumber, startDate, credits) OUTPUT INSERTED.id VALUES (@Name, @CourseNumber, @StartDate, @Credits)", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@Name";
      nameParameter.Value = this.GetName();

      SqlParameter courseNumberParameter = new SqlParameter();
      courseNumberParameter.ParameterName = "@CourseNumber";
      courseNumberParameter.Value = this.GetCourseNumber();

      SqlParameter startDateParameter = new SqlParameter();
      startDateParameter.ParameterName = "@StartDate";
      startDateParameter.Value = this.GetStartDate();

      SqlParameter creditsParameter = new SqlParameter();
      creditsParameter.ParameterName = "@Credits";
      creditsParameter.Value = this.GetCredits();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(courseNumberParameter);
      cmd.Parameters.Add(startDateParameter);
      cmd.Parameters.Add(creditsParameter);

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

    public static Course Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses WHERE id = @CourseId", conn);
      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = id.ToString();
      cmd.Parameters.Add(courseIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCourseId = 0;
      string foundCourseName = null;
      string foundCourseNumber = null;
      DateTime foundCourseStartDate = default(DateTime);
      int foundCredits = 0;

      while(rdr.Read())
      {
        foundCourseId = rdr.GetInt32(0);
        foundCourseName = rdr.GetString(1);
        foundCourseNumber = rdr.GetString(2);
        foundCourseStartDate = rdr.GetDateTime(3);
        foundCredits = rdr.GetInt32(4);
      }
      Course foundCourse = new Course(foundCourseName, foundCourseNumber, foundCourseStartDate, foundCredits, foundCourseId);

      if (rdr != null)
     {
       rdr.Close();
     }
     if (conn != null)
     {
       conn.Close();
     }

     return foundCourse;
    }

    public void AddStudent(Student newStudent)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students_courses (students_id, courses_id, credits_earned) OUTPUT INSERTED.students_id VALUES (@StudentId, @CourseId, @CreditsEarned);", conn);

      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = newStudent.GetId();
      cmd.Parameters.Add(studentIdParameter);

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = this.GetId();
      cmd.Parameters.Add(courseIdParameter);

      SqlParameter creditsEarnedParameter = new SqlParameter();
      creditsEarnedParameter.ParameterName = "@CreditsEarned";
      creditsEarnedParameter.Value = this.GetCredits();
      cmd.Parameters.Add(creditsEarnedParameter);

      SqlDataReader rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        newStudent.SetId(rdr.GetInt32(0));
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Student> GetStudents()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT students.* FROM courses JOIN students_courses ON (courses.id = students_courses.courses_id) JOIN students ON (students_courses.students_id = students.id) WHERE courses.id = @CourseId;", conn);
      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = this.GetId().ToString();

      cmd.Parameters.Add(courseIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Student> students = new List<Student>{};

      while(rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentFirstName = rdr.GetString(1);
        string studentLastName = rdr.GetString(2);
        DateTime studentDateOfEnrollment = rdr.GetDateTime(3);
        Student newStudent = new Student(studentFirstName, studentLastName, studentDateOfEnrollment, studentId);
        students.Add(newStudent);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return students;
    }
  }
}
