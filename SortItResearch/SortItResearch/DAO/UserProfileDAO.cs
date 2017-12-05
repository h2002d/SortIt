﻿using SortItResearch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SortItResearch.DAO
{
    public class UserProfileDAO:DAO
    {
        public UserProfile getUserById(string userId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UserGetUserProfileById", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", userId);
                        SqlDataReader rdr = command.ExecuteReader();
                        UserProfile user = new UserProfile();
                        while (rdr.Read())
                        {
                            user.Id = rdr["UserId"].ToString();
                            user.Name = rdr["Name"].ToString();
                            user.SurName = rdr["SurName"].ToString();
                            user.Email = rdr["Email"].ToString();
                            //user.DateBirth = Convert.ToDateTime(rdr["DateBirth"]);
                        }
                        return user;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public void saveUser(UserProfile user)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UserCreateProfile", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", user.Id);
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@SurName", user.SurName);
                        //command.Parameters.AddWithValue("@DateBirth", user.DateBirth);

                        SqlDataReader rdr = command.ExecuteReader();
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