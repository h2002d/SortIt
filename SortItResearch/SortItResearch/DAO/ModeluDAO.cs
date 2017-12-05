using SortItResearch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SortItResearch.DAO
{
    public class ModeluDAO : DAO
    {

        public List<Module> getModules(int? id)
        {

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminGetModule", sqlConnection))
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
                        List<Module> newCategoryList = new List<Module>();
                        while (rdr.Read())
                        {
                            Module newModule = new Module();
                            newModule.Id = Convert.ToInt32(rdr["Id"]);
                            newModule.Name = rdr["Name"].ToString();
                            newModule.SubjectId = Convert.ToInt32(rdr["SubjectId"]);
                            SqlCommand cmd = new SqlCommand("sp_AdminGetModuleDayByModuleId", sqlConnection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(newModule.Id));

                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                ModuleDay day = new ModuleDay();
                                day.Id = Convert.ToInt32(reader["Id"]);
                                day.ModuleId = Convert.ToInt32(reader["ModuleId"]);
                                day.Name = reader["Name"].ToString();
                                day.TrainingId = Convert.ToInt32(reader["TrainingId"]);
                                newModule.ModuleDays.Add(day);

                            }
                            newCategoryList.Add(newModule);
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

        public List<Module> getModulesBySubjectId(int? id)
        {

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminGetModuleBySubjectId", sqlConnection))
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
                        List<Module> newCategoryList = new List<Module>();
                        while (rdr.Read())
                        {
                            Module newModule = new Module();
                            newModule.Id = Convert.ToInt32(rdr["Id"]);
                            newModule.Name = rdr["Name"].ToString();
                            newModule.SubjectId = Convert.ToInt32(rdr["SubjectId"]);

                            SqlCommand cmd = new SqlCommand("sp_AdminGetModuleDayByModuleId", sqlConnection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(newModule.Id));

                            SqlDataReader reader = cmd.ExecuteReader();
                            while(reader.Read())
                            {
                                ModuleDay day = new ModuleDay();
                                day.Id = Convert.ToInt32(reader["Id"]);
                                day.ModuleId = Convert.ToInt32(reader["ModuleId"]);
                                day.Name = reader["Name"].ToString();
                                day.TrainingId= Convert.ToInt32(reader["TrainingId"]);
                                var list = Lesson.GetLessonByDayId(Convert.ToInt32(day.Id));
                                foreach (var item in list)
                                {
                                    day.Lessons.Add(item);
                                }
                                newModule.ModuleDays.Add(day);
                               
                            }
                            newCategoryList.Add(newModule);
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

        public void saveModule(Module module)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminCreateModule", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        if (module.Id == null)
                            command.Parameters.AddWithValue("@Id", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Id", module.Id);
                        command.Parameters.AddWithValue("@Name", module.Name);
                        command.Parameters.AddWithValue("@SubjectId", module.SubjectId);

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        public void deleteModule(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminDeleteModule", sqlConnection))
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
                        throw ex;
                    }
                }
            }
        }
    }
}