using SortItResearch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SortItResearch.DAO
{
    public class CertificateDAO:DAO
    {

        internal List<Certificate> getDissertationById(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UserGetCertificateById", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);

                        SqlDataReader rdr = command.ExecuteReader();
                        List<Certificate> certificateList = new List<Certificate>();
                        while (rdr.Read())
                        {
                            Certificate certificate = new Certificate();
                            certificate.Id = Convert.ToInt32(rdr["Id"]);
                            certificate.StudentId = rdr["UserId"].ToString();
                            certificate.SubjectId = Convert.ToInt32(rdr["SubjectId"]);
                            certificate.CreateDate = Convert.ToDateTime(rdr["CreateDate"]);
                            certificateList.Add(certificate);
                        }
                        return certificateList;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        internal List<Certificate> getDissertationByStudentId(string id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UserGetCertificateByStudentId", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StudentId", id);

                        SqlDataReader rdr = command.ExecuteReader();
                        List<Certificate> certificateList = new List<Certificate>();
                        while (rdr.Read())
                        {
                            Certificate certificate = new Certificate();
                            certificate.Id = Convert.ToInt32(rdr["Id"]);
                            certificate.StudentId = rdr["UserId"].ToString();
                            certificate.SubjectId = Convert.ToInt32(rdr["SubjectId"]);
                            certificate.CreateDate = Convert.ToDateTime(rdr["CreateDate"]);
                            certificateList.Add(certificate);
                        }
                        return certificateList;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        internal int Save(Certificate certificate)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminCreateCertificate", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StudentId", certificate.StudentId);
                        command.Parameters.AddWithValue("@SubjectId", certificate.SubjectId);

                        //command.Parameters.AddWithValue("@DateBirth", user.DateBirth);
                       return Convert.ToInt32(command.ExecuteScalar());


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