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

    public Student(int Id, string FirstName, string LastName, DateTime DateOfEnrollment)
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
  }
}
