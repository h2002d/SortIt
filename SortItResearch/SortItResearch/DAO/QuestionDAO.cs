using SortItResearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace SortItResearch.DAO
{
    public class QuestionDAO : DAO
    {
        public List<Question> getQuestion(int? id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminGetQuestion", sqlConnection))
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
                        List<Question> newQuestionList = new List<Question>();
                        while (rdr.Read())
                        {
                            Question newQuestion = new Question();
                            newQuestion.Id = Convert.ToInt32(rdr["Id"]);
                            newQuestion.Name = rdr["Content"].ToString();
                            newQuestion.LessonId = Convert.ToInt32(rdr["LessonId"]);
                            newQuestion.Type = Convert.ToInt32(rdr["Type"]);

                            SqlCommand cmd = new SqlCommand("sp_AdminGetAnswerByQuestion", sqlConnection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(newQuestion.Id));

                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                Answer answer = new Answer();

                                answer.Id = Convert.ToInt32(reader["Id"]);
                                answer.QuestionId = Convert.ToInt32(reader["QuestionId"]);
                                answer.Name = reader["Content"].ToString();
                                answer.IsRight = reader["IsRight"].ToString().ToLower().Equals("true");

                                newQuestion.Answers.Add(answer);
                            }
                            newQuestionList.Add(newQuestion);
                        }
                        return newQuestionList;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
        }

        public List<Question> getQuestionByLesson(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminGetQuestionByLessonId", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        SqlDataReader rdr = command.ExecuteReader();
                        List<Question> newQuestionList = new List<Question>();
                        while (rdr.Read())
                        {
                            Question newQuestion = new Question();
                            newQuestion.Id = Convert.ToInt32(rdr["Id"]);
                            newQuestion.Name = rdr["Content"].ToString();
                            newQuestion.LessonId = Convert.ToInt32(rdr["LessonId"]);
                            newQuestion.Type = Convert.ToInt32(rdr["Type"]);

                            SqlCommand cmd = new SqlCommand("sp_AdminGetAnswerByQuestion", sqlConnection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(newQuestion.Id));

                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                Answer answer = new Answer();

                                answer.Id = Convert.ToInt32(reader["Id"]);
                                answer.QuestionId = Convert.ToInt32(reader["QuestionId"]);
                                answer.Name = reader["Content"].ToString();
                                answer.IsRight = reader["IsRight"].ToString().ToLower().Equals("true");


                                newQuestion.Answers.Add(answer);
                            }
                            newQuestionList.Add(newQuestion);
                        }
                        return newQuestionList;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
        }

        public int saveQuestion(Question module)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminCreateQuestion", sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        if (module.Id == null)
                            command.Parameters.AddWithValue("@Id", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Id", module.Id);
                        command.Parameters.AddWithValue("@Content", module.Name);
                        command.Parameters.AddWithValue("@LessonId", module.LessonId);
                        command.Parameters.AddWithValue("@Type", module.Type);
                        int Id = Convert.ToInt32(command.ExecuteScalar());
                        if (module.Id == null)
                            module.Id = Id;
                      
                        foreach (var item in module.Answers)
                        {
                            SqlCommand cmd = new SqlCommand("sp_AdminCreateAnswer", sqlConnection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (item.Id == null)
                                cmd.Parameters.AddWithValue("@Id", DBNull.Value);
                            else
                                cmd.Parameters.AddWithValue("@Id", item.Id);
                            cmd.Parameters.AddWithValue("@Content", item.Name);
                            cmd.Parameters.AddWithValue("@QuestionId", module.Id);
                            cmd.Parameters.AddWithValue("@IsRight", item.IsRight);
                           cmd.ExecuteNonQuery();

                        }

                        return Id;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public void deleteQuestion(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminDeleteQuestion", sqlConnection))
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
        public void deleteAnswer(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AdminDeleteAnswer", sqlConnection))
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