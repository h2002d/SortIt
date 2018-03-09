using SortItResearch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SortItResearch.DAO
{
    public class SubjectDAO : DAO

    {
        public List<Subject> getSubjects(int? id)
        {

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminGetSubjects", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        if (id == null)
                            command.Parameters.AddWithValue("@Id", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Id", id);
                        SqlDataReader rdr = command.ExecuteReader();
                        List<Subject> newCategoryList = new List<Subject>();
                        while (rdr.Read())
                        {
                            Subject newCategory = new Subject();
                            newCategory.Id = Convert.ToInt32(rdr["Id"]);
                            newCategory.Name = rdr["Name"].ToString();
                            newCategory.Description = rdr["Description"].ToString();

                            newCategoryList.Add(newCategory);
                        }
                        return newCategoryList;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
        }

        public List<MySubjectViewModel> getSubjectByStudentId(string studentId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_StudentGetSubjectsByStudentId", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@StudentId", studentId);
                        SqlDataReader rdr = command.ExecuteReader();
                        List<MySubjectViewModel> newSubjectViewList = new List<MySubjectViewModel>();
                        
                        while (rdr.Read())
                        {
                            MySubjectViewModel newSubjectView = new MySubjectViewModel();
                            newSubjectView.Id = Convert.ToInt32(rdr["Id"]);
                            newSubjectView.Name = rdr["Name"].ToString();
                            newSubjectView.Description = rdr["Description"].ToString();

                            newSubjectView.isAccepted = Convert.ToBoolean(rdr["Accepted"]);
                            newSubjectView.TeacherId = rdr["TeacherId"].ToString();
                            newSubjectViewList.Add(newSubjectView);
                        }
                        return newSubjectViewList;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
        }

        public List<MySubjectViewModel> getSubjectByStudentIdById(string studentId,int subjectId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_StudentGetSubjectsByStudentIdById", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@StudentId", studentId);
                        command.Parameters.AddWithValue("@SubjectId", subjectId);


                        SqlDataReader rdr = command.ExecuteReader();
                        List<MySubjectViewModel> newSubjectViewList = new List<MySubjectViewModel>();
                        MySubjectViewModel newSubjectView = new MySubjectViewModel();
                        while (rdr.Read())
                        {
                            newSubjectView.Id = Convert.ToInt32(rdr["Id"]);
                            newSubjectView.Name = rdr["Name"].ToString();
                            newSubjectView.Description = rdr["Description"].ToString();
                           
                            newSubjectView.isAccepted = Convert.ToBoolean(rdr["Accepted"]);
                            newSubjectView.TeacherId = rdr["TeacherId"].ToString();
                            newSubjectViewList.Add(newSubjectView);
                        }
                        return newSubjectViewList;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
        }

        public void saveSubject(Subject subject)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminCreateSubject", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        if (subject.Id == null)
                            command.Parameters.AddWithValue("@Id", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Id", subject.Id);
                        command.Parameters.AddWithValue("@Name", subject.Name);
                        command.Parameters.AddWithValue("@Description", subject.Description);

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
        public void deleteSubject(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminDeleteSubject", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
    }
}