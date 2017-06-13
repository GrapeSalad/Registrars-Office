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
  }
}
