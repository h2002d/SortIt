using SortItResearch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SortItResearch.DAO
{
    public class UserProfileDAO : DAO
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
                            user.Dissertation = rdr["Dissertation"].ToString();
                            user.Affiliation = rdr["Affiliation"].ToString();
                            user.Country = Convert.ToInt32(rdr["Country"]);
                            user.Degree = rdr["Degree"].ToString();
                            user.Phone = rdr["Phone"].ToString();
                            user.Profession = rdr["Profession"].ToString();
                            user.isMale = Convert.ToBoolean(rdr["isMale"]==null?0 :rdr["isMale"]);

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

        internal static List<UserProfile> getUserByGender(bool isMale,string role)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UserGetUserProfileByGender", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@isMale", isMale);
                        command.Parameters.AddWithValue("@RoleId", role);

                        SqlDataReader rdr = command.ExecuteReader();
                        List<UserProfile> userList = new List<UserProfile>();
                        while (rdr.Read())
                        {
                            UserProfile user = new UserProfile();
                            user.Id = rdr["UserId"].ToString();
                            user.Name = rdr["Name"].ToString();
                            user.SurName = rdr["SurName"].ToString();
                            user.Email = rdr["Email"].ToString();
                            user.Dissertation = rdr["Dissertation"].ToString();
                            user.Affiliation = rdr["Affiliation"].ToString();
                            user.Country = Convert.ToInt32(rdr["Country"]);
                            user.Degree = rdr["Degree"].ToString();
                            user.Phone = rdr["Phone"].ToString();
                            user.Profession = rdr["Profession"].ToString();
                            user.isMale = Convert.ToBoolean(rdr["isMale"]);
                            userList.Add(user);
                            //user.DateBirth = Convert.ToDateTime(rdr["DateBirth"]);
                        }
                        return userList;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
        internal static List<UserProfile> getUserByInterestId(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UserGetUserProfileByInterests", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);

                        SqlDataReader rdr = command.ExecuteReader();
                        List<UserProfile> userList = new List<UserProfile>();
                        while (rdr.Read())
                        {
                            UserProfile user = new UserProfile();
                            user.Id = rdr["UserId"].ToString();
                            user.Name = rdr["Name"].ToString();
                            user.SurName = rdr["SurName"].ToString();
                            user.Email = rdr["Email"].ToString();
                            user.Dissertation = rdr["Dissertation"].ToString();
                            user.Affiliation = rdr["Affiliation"].ToString();
                            user.Country = Convert.ToInt32(rdr["Country"]);
                            user.Degree = rdr["Degree"].ToString();
                            user.Phone = rdr["Phone"].ToString();
                            user.Profession = rdr["Profession"].ToString();
                            user.isMale = Convert.ToBoolean(rdr["isMale"]);
                            userList.Add(user);
                            //user.DateBirth = Convert.ToDateTime(rdr["DateBirth"]);
                        }
                        return userList;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        internal static List<Country> getCountries()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetCountry", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataReader rdr = command.ExecuteReader();
                        List<Country> countryList = new List<Country>();
                        while (rdr.Read())
                        {
                            Country country = new Country();
                            country.Id = Convert.ToInt32(rdr["Id"]);
                            country.Name = rdr["Name"].ToString();
                            countryList.Add(country);
                        }
                        return countryList;
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
                        if (user.isTeacher)
                        {
                            command.Parameters.AddWithValue("@Dissertation", user.Dissertation);
                            command.Parameters.AddWithValue("@Publicants", user.Publicants);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Publicants", DBNull.Value);
                            command.Parameters.AddWithValue("@Dissertation", DBNull.Value);
                        }
                        //command.Parameters.AddWithValue("@DateBirth", user.DateBirth);
                        command.Parameters.AddWithValue("@Degree", user.Degree);
                        command.Parameters.AddWithValue("@Profession", user.Profession);
                        command.Parameters.AddWithValue("@Affiliation", user.Affiliation);
                        command.Parameters.AddWithValue("@Country", user.Country);
                        command.Parameters.AddWithValue("@Phone", user.Phone);
                        command.Parameters.AddWithValue("@isMale", user.isMale);

                        SqlDataReader rdr = command.ExecuteReader();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public string getTeacherByProgress(string studentId, int lessonId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UserGetTeacherByLessonId", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StudentId", studentId);
                        command.Parameters.AddWithValue("@LessonId", lessonId);
                        return command.ExecuteScalar().ToString();
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