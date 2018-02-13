using SortItResearch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SortItResearch.DAO
{
    public class SkillDAO : DAO
    {
        public void saveModule(string teacherId, List<int> skillId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_TeacherCreateSkill", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        SqlCommand cmd = new SqlCommand("sp_TeacherDeleteSkill", sqlConnection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TeacherId", teacherId);
                        cmd.ExecuteNonQuery();
                        command.CommandType = CommandType.StoredProcedure;
                        foreach (int moduleId in skillId)
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@TeacherId", teacherId);
                            command.Parameters.AddWithValue("@ModuleId", moduleId);
                            command.ExecuteNonQuery();
                        }
                    }

                    catch (Exception ex)
                    {

                    }
                }
            }
        }
        public List<int> getSelectedSkills(string teacherId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_TeacherGetSkills", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        
                            command.Parameters.AddWithValue("@TeacherId", teacherId);
                        SqlDataReader rdr = command.ExecuteReader();
                        List<int> selectedModules = new List<int>();
                        while (rdr.Read())
                        {
                            selectedModules.Add(Convert.ToInt32(rdr["ModuleId"]));
                        }
                        return selectedModules;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public static List<UserProfile> getUserBySkill(int moduleId,int roleId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UserGetProfileByModuleId", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ModuleId", moduleId);
                        command.Parameters.AddWithValue("@RoleId", roleId);

                        SqlDataReader rdr = command.ExecuteReader();
                        List<UserProfile> skilledUsers = new List<UserProfile>();
                        while (rdr.Read())
                        {
                            UserProfile user = new UserProfile();
                            user.Id = rdr["UserId"].ToString();
                            user.Name = rdr["Name"].ToString();
                            user.SurName= rdr["SurName"].ToString();
                            user.Dissertation = rdr["Dissertation"].ToString();

                            skilledUsers.Add(user);
                        }
                        return skilledUsers;
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