using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SortItResearch.Models;
using System.Data.SqlClient;
using System.Data;

namespace SortItResearch.DAO
{
    public class DissertationDAO : DAO
    {
        internal List<DissertationViewModel> getDissertationById(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UserGetDissertationById", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);

                        SqlDataReader rdr = command.ExecuteReader();
                        List<DissertationViewModel> dissertationList = new List<DissertationViewModel>();
                        while (rdr.Read())
                        {
                            DissertationViewModel dissertation = new DissertationViewModel();
                            dissertation.Id = Convert.ToInt32(rdr["Id"]);
                            dissertation.StudentId = rdr["StudentId"].ToString();
                            dissertation.Attachement = rdr["Attachement"].ToString();
                            dissertation.SubjectId = Convert.ToInt32(rdr["SubjectId"]);
                            dissertation.CreateDate = Convert.ToDateTime(rdr["CreateDate"]);
                            dissertation.Accepted = Convert.ToBoolean(rdr["Accepted"] == DBNull.Value ? false : rdr["Accepted"]);
                            //dissertation.Category = rdr["Category"].ToString();
                            dissertation.Design = rdr["Design"].ToString();
                            dissertation.Title = rdr["Title"].ToString();
                            dissertation.ShortDescription = rdr["ShortDescription"].ToString();
                            dissertation.Status = rdr["Status"].ToString();
                            dissertationList.Add(dissertation);
                        }
                        return dissertationList;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        internal List<DissertationViewModel> getDissertationBySubject(int subjectId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UserGetDissertationBySubject", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SubjectId", subjectId);

                        SqlDataReader rdr = command.ExecuteReader();
                        List<DissertationViewModel> dissertationList = new List<DissertationViewModel>();
                        while (rdr.Read())
                        {
                            DissertationViewModel dissertation = new DissertationViewModel();
                            dissertation.Id = Convert.ToInt32(rdr["Id"]);
                            dissertation.StudentId = rdr["StudentId"].ToString();
                            dissertation.Attachement = rdr["Attachement"].ToString();
                            dissertation.SubjectId = Convert.ToInt32(rdr["SubjectId"]);
                            dissertation.CreateDate = Convert.ToDateTime(rdr["CreateDate"]);
                            dissertation.Accepted = Convert.ToBoolean(rdr["Accepted"]==DBNull.Value? false:rdr["Accepted"]);
                            //dissertation.Category = rdr["Category"].ToString();
                            dissertation.Design = rdr["Design"].ToString();
                            dissertation.Title = rdr["Title"].ToString();
                            dissertation.ShortDescription = rdr["ShortDescription"].ToString();
                            dissertation.Status = rdr["Status"].ToString();
                            dissertationList.Add(dissertation);
                        }
                        return dissertationList;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        internal Dissertation getDissertationByStudent(int subjectId, string studentId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UserGetDissertationByStudent", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SubjectId", subjectId);
                        command.Parameters.AddWithValue("@StudentId", studentId);

                        SqlDataReader rdr = command.ExecuteReader();
                        Dissertation dissertation = new Dissertation();
                        while (rdr.Read())
                        {
                            dissertation.Id = Convert.ToInt32(rdr["Id"]);
                            dissertation.StudentId = rdr["StudentId"].ToString();
                            dissertation.Attachement = rdr["Attachement"].ToString();
                            dissertation.SubjectId = Convert.ToInt32(rdr["SubjectId"]);
                            dissertation.Accepted = Convert.ToBoolean(rdr["Accepted"] == DBNull.Value ? false : rdr["Accepted"]);
                            dissertation.Category = rdr["Category"].ToString();
                            dissertation.Design = rdr["Design"].ToString();
                            dissertation.Title = rdr["Title"].ToString();
                            dissertation.ShortDescription = rdr["ShortDescription"].ToString();
                            dissertation.Status= rdr["Status"].ToString();

                            dissertation.CreateDate = Convert.ToDateTime(rdr["CreateDate"]);

                        }
                        return dissertation;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        internal void saveDissertation(Dissertation dissertation)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_StudentCreateDissertation", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        if (dissertation.Id == null)
                            command.Parameters.AddWithValue("@Id", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Id", dissertation.Id);
                        command.Parameters.AddWithValue("@Attachement", dissertation.Attachement);
                        command.Parameters.AddWithValue("@StudentId", dissertation.StudentId);
                        command.Parameters.AddWithValue("@SubjectId", dissertation.SubjectId);
                        command.Parameters.AddWithValue("@Title", dissertation.Title);
                        command.Parameters.AddWithValue("@ShortDescription", dissertation.ShortDescription);
                        command.Parameters.AddWithValue("@Status", dissertation.Status);
                        command.Parameters.AddWithValue("@Category", dissertation.Category);
                        command.Parameters.AddWithValue("@Design", dissertation.Design);

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

        internal void saveDissertationStatus(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminSetDissertationStatus", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id",id);


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
    }
}