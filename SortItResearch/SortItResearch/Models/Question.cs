using SortItResearch.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class Question
    {
        public int? Id { get; set; }
        [Required]
        public int LessonId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Type { get; set; }
        public List<Answer> Answers { get; set; }
        static QuestionDAO DAO = new QuestionDAO(); 
        public Question()
        {
            Answers = new List<Answer>();
        }

        public static List<Question> GetQuestions(int? id)
        {
            return DAO.getQuestion(id);
        }

        public static List<Question> GetQuestionByLessonId(int id)
        {
            return DAO.getQuestionByLesson(id);
        }

        public int Save()
        {
           return DAO.saveQuestion(this);
        }

        public static void Delete(int id)
        {
            DAO.deleteQuestion(id);
        }
    }
}