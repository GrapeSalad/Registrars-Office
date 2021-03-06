using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Registrar;

namespace Registrar.Objects
{
  public class Student
  {
    private int _id;
    private string _firstName;
    private string _lastName;
    private DateTime _dateOfEnrollment;

    public Student(string FirstName, string LastName, DateTime DateOfEnrollment, int Id = 0)
    {
      _id = Id;
      _firstName = FirstName;
      _lastName = LastName;
      _dateOfEnrollment = DateOfEnrollment;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = this.GetId() == newStudent.GetId();
        bool firstNameEquality = this.GetFirstName() == newStudent.GetFirstName();
        bool lastNameEquality = this.GetLastName() == newStudent.GetLastName();
        bool dateOfEnrollmentEquality = this.GetDateOfEnrollment() == newStudent.GetDateOfEnrollment();
        return (idEquality && firstNameEquality && lastNameEquality && dateOfEnrollmentEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetFirstName()
    {
      return _firstName;
    }
    public string GetLastName()
    {
      return _lastName;
    }
    public DateTime GetDateOfEnrollment()
    {
      return _dateOfEnrollment;
    }

    public void SetId(int id)
    {
        _id = id;
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM students;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentFirstName = rdr.GetString(1);
        string studentLastName = rdr.GetString(2);
        DateTime studentDateOfEnrollment = rdr.GetDateTime(3);
        Student newStudent = new Student(studentFirstName, studentLastName, studentDateOfEnrollment, studentId);
        allStudents.Add(newStudent);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allStudents;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students (firstName, lastName, dateOfEnrollment) OUTPUT INSERTED.id VALUES (@FirstName, @LastName, @DateOfEnrollment)", conn);

      SqlParameter firstNameParameter = new SqlParameter();
      firstNameParameter.ParameterName = "@FirstName";
      firstNameParameter.Value = this.GetFirstName();

      SqlParameter lastNameParameter = new SqlParameter();
      lastNameParameter.ParameterName = "@LastName";
      lastNameParameter.Value = this.GetLastName();

      SqlParameter dateOfEnrollmentParameter = new SqlParameter();
      dateOfEnrollmentParameter.ParameterName = "@DateOfEnrollment";
      dateOfEnrollmentParameter.Value = this.GetDateOfEnrollment();

      cmd.Parameters.Add(firstNameParameter);
      cmd.Parameters.Add(lastNameParameter);
      cmd.Parameters.Add(dateOfEnrollmentParameter);

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
    public static Student Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id = @StudentId", conn);
      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@StudentId";
      courseIdParameter.Value = id.ToString();
      cmd.Parameters.Add(courseIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundStudentId = 0;
      string foundStudentFirstName = null;
      string foundStudentLastName = null;
      DateTime foundStudentDateOfEnrollment = default(DateTime);

      while(rdr.Read())
      {
        foundStudentId = rdr.GetInt32(0);
        foundStudentFirstName = rdr.GetString(1);
        foundStudentLastName = rdr.GetString(2);
        foundStudentDateOfEnrollment = rdr.GetDateTime(3);
      }
      Student foundStudent = new Student(foundStudentFirstName, foundStudentLastName, foundStudentDateOfEnrollment, foundStudentId);

      if (rdr != null)
     {
       rdr.Close();
     }
     if (conn != null)
     {
       conn.Close();
     }

     return foundStudent;
    }
  }
}
