using SortItResearch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SortItResearch.DAO
{
    public class LessonDAO : DAO
    {
        public int saveModule(Lesson lesson)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminCreateLesson", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        if (lesson.Id == null)
                            command.Parameters.AddWithValue("@Id", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Id", lesson.Id);
                        command.Parameters.AddWithValue("@Name", lesson.Name);
                        command.Parameters.AddWithValue("@Description", lesson.Description);
                        command.Parameters.AddWithValue("@SubjectId", lesson.SubjectId);
                        command.Parameters.AddWithValue("@Duration", lesson.Duration);
                        command.Parameters.AddWithValue("@Facilitator", lesson.Facilitator);

                        command.Parameters.AddWithValue("@IsMandatory", lesson.IsMandatory);
                        command.Parameters.AddWithValue("@Attachement", lesson.Attachement);
                        command.Parameters.AddWithValue("@Type", lesson.Type ? 1 : 0);

                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        internal void saveLessonFromDay(int dayId, int lessonId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminCreateLessonForDay", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@DayId", dayId);
                        command.Parameters.AddWithValue("@LessonId", lessonId);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        internal void deleteLessonFromDay(int dayId, int lessonId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminDeleteLessonFromDay", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@DayId", dayId);
                        command.Parameters.AddWithValue("@LessonId", lessonId);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public List<Lesson> getLesson(int? id, int? typeId)
        {

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminGetLesson", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        if (id == null)
                        {
                            command.Parameters.AddWithValue("@Id", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Id", id);
                        }
                        if (typeId != null)
                            command.Parameters.AddWithValue("@TypeId", typeId);
                        else
                            command.Parameters.AddWithValue("@TypeId", DBNull.Value);


                        SqlDataReader rdr = command.ExecuteReader();
                        List<Lesson> newLessonList = new List<Lesson>();
                        while (rdr.Read())
                        {
                            Lesson newModule = new Lesson();
                            newModule.Id = Convert.ToInt32(rdr["Id"]);
                            newModule.Name = rdr["Name"].ToString();
                            newModule.Description = rdr["Description"].ToString();
                            newModule.Type = Convert.ToInt32(rdr["Type"]) == 1;//1 =homework
                            newModule.SubjectId = Convert.ToInt32(rdr["SubjectId"]);
                            newModule.Attachement = rdr["Attachement"].ToString();
                            newModule.IsMandatory = Convert.ToBoolean(rdr["IsMandatory"]);
                            //SqlCommand cmd = new SqlCommand("sp_AdminGetModuleDayByModuleId", sqlConnection);
                            //cmd.CommandType = CommandType.StoredProcedure;
                            //cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(newModule.Id));

                            //SqlDataReader reader = cmd.ExecuteReader();
                            //while (reader.Read())
                            //{
                            //    ModuleDay day = new ModuleDay();
                            //    day.Id = Convert.ToInt32(reader["Id"]);
                            //    day.ModuleId = Convert.ToInt32(reader["ModuleId"]);
                            //    day.Name = reader["Name"].ToString();
                            //    day.TrainingId = Convert.ToInt32(reader["TrainingId"]);
                            //    newModule.ModuleDays.Add(day);

                            //}
                            newLessonList.Add(newModule);
                        }
                        return newLessonList;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
        }

        public List<Lesson> getLessonByDayId(int? id)
        {

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminGetLessonByDayId", sqlConnection))
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
                        List<Lesson> newLessonList = new List<Lesson>();
                        while (rdr.Read())
                        {
                            Lesson newModule = new Lesson();
                            newModule.Id = Convert.ToInt32(rdr["Id"]);
                            newModule.Name = rdr["Name"].ToString();
                            newModule.Description = rdr["Description"].ToString();
                            newModule.Type = Convert.ToInt32(rdr["Type"]) == 1;//1 =homework
                            newModule.SubjectId = Convert.ToInt32(rdr["SubjectId"]);
                            newModule.Attachement = rdr["Attachement"].ToString();
                            newModule.Duration = rdr["Duration"].ToString();
                            newModule.Facilitator = rdr["Facilitator"].ToString();
                            newModule.IsMandatory = Convert.ToBoolean(rdr["IsMandatory"]);

                            //SqlCommand cmd = new SqlCommand("sp_AdminGetModuleDayByModuleId", sqlConnection);
                            //cmd.CommandType = CommandType.StoredProcedure;
                            //cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(newModule.Id));

                            //SqlDataReader reader = cmd.ExecuteReader();
                            //while (reader.Read())
                            //{
                            //    ModuleDay day = new ModuleDay();
                            //    day.Id = Convert.ToInt32(reader["Id"]);
                            //    day.ModuleId = Convert.ToInt32(reader["ModuleId"]);
                            //    day.Name = reader["Name"].ToString();
                            //    day.TrainingId = Convert.ToInt32(reader["TrainingId"]);
                            //    newModule.ModuleDays.Add(day);

                            //}
                            newLessonList.Add(newModule);
                        }
                        return newLessonList;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
        }

        public List<Lesson> getLessonBySubjectId(int? id)
        {

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminGetLessonBySubjectId", sqlConnection))
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
                        List<Lesson> newLessonList = new List<Lesson>();
                        while (rdr.Read())
                        {
                            Lesson newModule = new Lesson();
                            newModule.Id = Convert.ToInt32(rdr["Id"]);
                            newModule.Name = rdr["Name"].ToString();
                            newModule.Description = rdr["Description"].ToString();
                            newModule.Type = Convert.ToInt32(rdr["Type"]) == 1;//1 =homework
                            newModule.SubjectId = Convert.ToInt32(rdr["SubjectId"]);
                            newModule.Attachement = rdr["Attachement"].ToString();
                            newModule.IsMandatory = Convert.ToBoolean(rdr["IsMandatory"]);
                            newModule.Duration = rdr["Duration"].ToString();
                            newModule.Facilitator = rdr["Facilitator"].ToString();

                            //SqlCommand cmd = new SqlCommand("sp_AdminGetModuleDayByModuleId", sqlConnection);
                            //cmd.CommandType = CommandType.StoredProcedure;
                            //cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(newModule.Id));

                            //SqlDataReader reader = cmd.ExecuteReader();
                            //while (reader.Read())
                            //{
                            //    ModuleDay day = new ModuleDay();
                            //    day.Id = Convert.ToInt32(reader["Id"]);
                            //    day.ModuleId = Convert.ToInt32(reader["ModuleId"]);
                            //    day.Name = reader["Name"].ToString();
                            //    day.TrainingId = Convert.ToInt32(reader["TrainingId"]);
                            //    newModule.ModuleDays.Add(day);

                            //}
                            newLessonList.Add(newModule);
                        }
                        return newLessonList;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
        }
        public void deleteLesson(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminDeleteLesson", sqlConnection))
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