using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SortItResearch.Models;
using System.Data.SqlClient;
using System.Data;

namespace SortItResearch.DAO
{
    public class ResearchTopicDAO : DAO
    {
        internal List<ResearchTopics> getTopicById(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetTopic", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        SqlDataReader rdr = command.ExecuteReader();
                        List<ResearchTopics> topicList = new List<ResearchTopics>();
                        while (rdr.Read())
                        {
                            ResearchTopics topic = new ResearchTopics();

                            topic.Id = Convert.ToInt32(rdr["Id"]);
                            topic.ShortDescription = rdr["ShortDescription"].ToString();
                            topic.Topic = rdr["Topic"].ToString();
                            topic.UserId = rdr["UserId"].ToString();
                            topic.SubjectId = Convert.ToInt32(rdr["SubjectId"]);
                            topicList.Add(topic);
                        }
                        return topicList;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        internal void saveProfileAreas(List<int> ids, string userId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UserCreateProfileArea", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        foreach (int id in ids)
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@AreaId", id);
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
        internal void deleteProfileAreas(string userId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UserDeleteProfileArea", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        internal List<TopicArea> getAreaById(int? id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetArea", sqlConnection))
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
                        List<TopicArea> topicList = new List<TopicArea>();
                        while (rdr.Read())
                        {
                            TopicArea topic = new TopicArea();

                            topic.Id = Convert.ToInt32(rdr["Id"]);
                            topic.Name = rdr["Name"].ToString();
                            topicList.Add(topic);
                        }
                        return topicList;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        internal void saveTopic(ResearchTopics researchTopics)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_SaveTopic", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        if (researchTopics.Id == null)
                            command.Parameters.AddWithValue("@Id", DBNull.Value);

                        else
                            command.Parameters.AddWithValue("@Id", researchTopics.Id);
                        command.Parameters.AddWithValue("@ShortDescription", researchTopics.ShortDescription);
                        command.Parameters.AddWithValue("@Topic", researchTopics.Topic);
                        command.Parameters.AddWithValue("@UserId", researchTopics.UserId);
                        command.Parameters.AddWithValue("@SubjectId", researchTopics.SubjectId);
                        researchTopics.Id = Convert.ToInt32(command.ExecuteScalar());

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            saveAreas(researchTopics.ResearchIds, Convert.ToInt32(researchTopics.Id));
        }

        internal void saveAreas(List<int> ids, int topicId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_SaveTopicAreas", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        foreach (int id in ids)
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@TopicId", topicId);
                            command.Parameters.AddWithValue("@AreaId", id);
                            command.ExecuteNonQuery();
                        }
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