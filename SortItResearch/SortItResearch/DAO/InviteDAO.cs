﻿using SortItResearch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SortItResearch.DAO
{
    public class InviteDAO : DAO
    {
        public InviteModel getInvite(int token)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UserGetInvitesByToken", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Token", token);
                        SqlDataReader rdr = command.ExecuteReader();
                        InviteModel invite = new InviteModel();
                        while (rdr.Read())
                        {
                            invite.Id = Convert.ToInt32(rdr["Id"]);
                            invite.StudentId = rdr["StudentId"].ToString();
                            invite.TeacherId= rdr["TeacherId"].ToString();
                            invite.SubjectId= Convert.ToInt32(rdr["SubjectId"]);
                            invite.Accepted = Convert.ToBoolean(rdr["Accepted"]);
                            invite.CreateDate = Convert.ToDateTime(rdr["CreateDate"]);
                        }
                        return invite;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public int saveInvite(InviteModel invite)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_StudentCreateRequest", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TeacherId", invite.TeacherId);
                        command.Parameters.AddWithValue("@StudentId", invite.StudentId);
                        command.Parameters.AddWithValue("@Type", invite.Type);
                        command.Parameters.AddWithValue("@SubjectId", invite.SubjectId);

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

        public int saveInviteStatus(int token,bool accepted)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UserChangeInviteStatus", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Token", token);
                        command.Parameters.AddWithValue("@Accepted", accepted);

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