using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SortItResearch.Models;
using System.Data.SqlClient;
using System.Data;

namespace SortItResearch.DAO
{
    public class ProgressDAO : DAO
    {
        internal Progress getProgressById(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UserGetProgressById", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        SqlDataReader rdr = command.ExecuteReader();
                        Progress progress = new Progress();
                        while (rdr.Read())
                        {
                            progress.Id = Convert.ToInt32(rdr["Id"]);
                            progress.StudentId = rdr["StudentId"].ToString();
                            progress.Attachement = rdr["Attachement"].ToString();
                            progress.LessonId = Convert.ToInt32(rdr["LessonId"]);
                            progress.Passed = Convert.ToBoolean(rdr["Passed"]);
                        }
                        return progress;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        internal List<HomeworkViewModel> getProgressByTeacherId(string teacherId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_TeacherGetProgressByTeacher", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TeacherId", teacherId);
                        SqlDataReader rdr = command.ExecuteReader();
                        List<HomeworkViewModel> progressList = new List<HomeworkViewModel>();
                        while (rdr.Read())
                        {
                            HomeworkViewModel progress = new HomeworkViewModel();
                            progress.Id = Convert.ToInt32(rdr["Id"]);
                            progress.StudentId = rdr["StudentId"].ToString();
                            progress.Attachement = rdr["Attachement"].ToString();
                            progress.LessonId = Convert.ToInt32(rdr["LessonId"]);
                            progress.Passed = Convert.ToBoolean(rdr["Passed"]);
                            progressList.Add(progress);
                        }

                        return progressList;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        internal void setProgressStatus(int id)
        {

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_StudentSetProgressStatus", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", id);

                        //command.Parameters.AddWithValue("@DateBirth", user.DateBirth);
                        command.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        internal void saveProgress(Progress progress)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_StudentCreateProgress", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@LessonId", progress.LessonId);
                        command.Parameters.AddWithValue("@StudentId", progress.StudentId);
                        command.Parameters.AddWithValue("@Passed", progress.Passed);
                        command.Parameters.AddWithValue("@Attachement", progress.Attachement);

                        //command.Parameters.AddWithValue("@DateBirth", user.DateBirth);
                        command.ExecuteNonQuery();


                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        internal Progress getProgressByLessonId(string studentId, int lessonId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UserGetProgressByLessonIdd", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StudentId", studentId);
                        command.Parameters.AddWithValue("@LessonId", lessonId);

                        SqlDataReader rdr = command.ExecuteReader();
                        Progress progress = new Progress();
                        while (rdr.Read())
                        {
                            progress.Id = Convert.ToInt32(rdr["Id"]);
                            progress.StudentId = rdr["StudentId"].ToString();
                            progress.Attachement = rdr["Attachement"].ToString();
                            progress.LessonId = Convert.ToInt32(rdr["LessonId"]);
                            progress.Passed = Convert.ToBoolean(rdr["Passed"]);
                            if (progress.Passed)
                            {
                                break;
                            }
                        }
                        return progress;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
    }
}