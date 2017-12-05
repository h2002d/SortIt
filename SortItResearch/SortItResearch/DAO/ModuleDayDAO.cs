using SortItResearch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SortItResearch.DAO
{
    public class ModuleDayDAO: DAO
    {
        public List<ModuleDay> getModuleDays(int? id)
        {

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminGetModuleDay", sqlConnection))
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
                        List<ModuleDay> newCategoryList = new List<ModuleDay>();
                        while (rdr.Read())
                        {
                            ModuleDay newModule = new ModuleDay();
                            newModule.Id = Convert.ToInt32(rdr["Id"]);
                            newModule.Name = rdr["Name"].ToString();
                            newModule.ModuleId = Convert.ToInt32(rdr["ModuleId"]);
                            newModule.TrainingId = Convert.ToInt32(rdr["TrainingId"]);
                            var list = Lesson.GetLessonByDayId(Convert.ToInt32(newModule.Id));
                            foreach (var item in list)
                            {
                                newModule.Lessons.Add(item);
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

        public void saveModuleDay(ModuleDay moduleDay)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminCreateModuleDay", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        if (moduleDay.Id == null)
                            command.Parameters.AddWithValue("@Id", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Id", moduleDay.Id);
                        command.Parameters.AddWithValue("@Name", moduleDay.Name);
                        command.Parameters.AddWithValue("@ModuleId", moduleDay.ModuleId);
                        command.Parameters.AddWithValue("@TrainingId", moduleDay.TrainingId);

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public void deleteModuleDay(int id)
        {

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminDeleteModuleDay", sqlConnection))
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