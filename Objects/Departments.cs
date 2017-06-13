// using System;
// using System.Collections.Generic;
// using System.Data.SqlClient;
// using Registrar;
//
// namespace Registrar.Objects
// {
//   public class Department
//   {
//     private int _id;
//     private string _name;
//
//     public Department(string Name, int Id = 0)
//     {
//       _id = Id;
//       _name = Name;
//     }
//
//     public override bool Equals(System.Object otherDepartment)
//     {
//       if (!(otherDepartment is Department))
//       {
//         return false;
//       }
//       else
//       {
//         Department newDepartment = (Department) otherDepartment;
//         bool idEquality = this.GetId() == newDepartment.GetId();
//         bool nameEquality = this.GetName() == newDepartment.GetName();
//         return (idEquality && nameEquality);
//       }
//     }
//
//     public int GetId()
//     {
//       return _id;
//     }
//     public void SetId(int id)
//     {
//       _id = id;
//     }
//     public string GetName()
//     {
//       return _name;
//     }
//
//     public static void DeleteAll()
//     {
//       SqlConnection conn = DB.Connection();
//       conn.Open();
//       SqlCommand cmd = new SqlCommand("DELETE FROM departments;", conn);
//       cmd.ExecuteNonQuery();
//       conn.Close();
//     }
//
//     public static List<Department> GetAll()
//     {
//       List<Department> allDepartments = new List<Department>{};
//
//       SqlConnection conn = DB.Connection();
//       conn.Open();
//
//       SqlCommand cmd = new SqlCommand("SELECT * FROM departments;", conn);
//       SqlDataReader rdr = cmd.ExecuteReader();
//
//       while(rdr.Read())
//       {
//         int departmentId = rdr.GetInt32(0);
//         string departmentName = rdr.GetString(1);
//         Department newDepartment = new Department(departmentName, departmentId);
//         allDepartments.Add(newDepartment);
//       }
//       if (rdr != null)
//       {
//         rdr.Close();
//       }
//       if (conn != null)
//       {
//         conn.Close();
//       }
//       return allDepartments;
//     }
//     public void Save()
//     {
//       SqlConnection conn = DB.Connection();
//       conn.Open();
//
//       SqlCommand cmd = new SqlCommand("INSERT INTO departments (name) OUTPUT INSERTED.id VALUES (@Name)", conn);
//
//       SqlParameter nameParameter = new SqlParameter();
//       nameParameter.ParameterName = "@Name";
//       nameParameter.Value = this.GetName();
//
//
//       cmd.Parameters.Add(nameParameter);
//
//       SqlDataReader rdr = cmd.ExecuteReader();
//
//       while(rdr.Read())
//       {
//         this._id = rdr.GetInt32(0);
//       }
//       if (rdr != null)
//       {
//         rdr.Close();
//       }
//       if(conn != null)
//       {
//         conn.Close();
//       }
//     }
//     public static Department Find(int id)
//     {
//       SqlConnection conn = DB.Connection();
//       conn.Open();
//
//       SqlCommand cmd = new SqlCommand("SELECT * FROM departments WHERE id = @DepartmentId", conn);
//       SqlParameter departmentIdParameter = new SqlParameter();
//       departmentIdParameter.ParameterName = "@DepartmentId";
//       departmentIdParameter.Value = id.ToString();
//       cmd.Parameters.Add(departmentIdParameter);
//       SqlDataReader rdr = cmd.ExecuteReader();
//
//       int foundDepartmentId = 0;
//       string foundDepartmentName = null;
//
//       while(rdr.Read())
//       {
//         foundDepartmentId = rdr.GetInt32(0);
//         foundDepartmentName = rdr.GetString(1);
//       }
//       Department foundDepartment = new Department(foundDepartmentName, foundDepartmentId);
//
//       if (rdr != null)
//      {
//        rdr.Close();
//      }
//      if (conn != null)
//      {
//        conn.Close();
//      }
//
//      return foundDepartment;
//     }
//     public void AddCourseAndStudent(Course newCourse, Student newStudent)
//     {
//       SqlConnection conn = DB.Connection();
//       conn.Open();
//
//       SqlCommand cmd = new SqlCommand("INSERT INTO majors (name, students_id, courses_id, departments_id) OUTPUT INSERTED.departments_id VALUES (@MajorName, @StudentId, @CourseId, @DepartmentId);", conn);
//
//       SqlParameter studentIdParameter = new SqlParameter();
//       studentIdParameter.ParameterName = "@StudentId";
//       studentIdParameter.Value = newStudent.GetId();
//       cmd.Parameters.Add(studentIdParameter);
//
//       SqlParameter courseIdParameter = new SqlParameter();
//       courseIdParameter.ParameterName = "@CourseId";
//       courseIdParameter.Value = newCourse.GetId();
//       cmd.Parameters.Add(courseIdParameter);
//
//       SqlParameter majorNameParameter = new SqlParameter();
//       majorNameParameter.ParameterName = "@MajorName";
//       majorNameParameter.Value = this.GetName();
//       cmd.Parameters.Add(majorNameParameter);
//
//       SqlParameter departmentIdParameter = new SqlParameter();
//       departmentIdParameter.ParameterName = "@DepartmentId";
//       departmentIdParameter.Value = this.GetId();
//       cmd.Parameters.Add(departmentIdParameter);
//
//       SqlDataReader rdr = cmd.ExecuteReader();
//       while (rdr.Read())
//       {
//         this._id = rdr.GetInt32(0);
//       }
//
//       if (rdr != null)
//       {
//         rdr.Close();
//       }
//       if (conn != null)
//       {
//         conn.Close();
//       }
//     }
//
//     public List<Course> GetCourses()
//     {
//       SqlConnection conn = DB.Connection();
//       conn.Open();
//
//       SqlCommand cmd = new SqlCommand("SELECT courses.* FROM departments JOIN majors ON (majors.departments_id = departments.id) JOIN courses ON (majors.courses_id = courses.id) JOIN students ON (majors.students_id = students.id) WHERE departments.id = @DepartmentId;", conn);
//       SqlParameter departmentIdParameter = new SqlParameter();
//       departmentIdParameter.ParameterName = "@DepartmentId";
//       departmentIdParameter.Value = this.GetId().ToString();
//
//       cmd.Parameters.Add(departmentIdParameter);
//
//       SqlDataReader rdr = cmd.ExecuteReader();
//
//       List<Course> courses = new List<Course>{};
//
//       while(rdr.Read())
//       {
//         int courseId = rdr.GetInt32(0);
//         string courseName = rdr.GetString(1);
//         string courseNumber = rdr.GetString(2);
//         DateTime courseStartDate = rdr.GetDateTime(3);
//         Course newCourse = new Course(courseName, courseNumber, courseStartDate, courseId);
//         courses.Add(newCourse);
//         Console.WriteLine("courses = {0}", courses[0].GetName());
//       }
//
//       if (rdr != null)
//       {
//         rdr.Close();
//       }
//       if (conn != null)
//       {
//         conn.Close();
//       }
//       return courses;
//     }
//   }
// }
