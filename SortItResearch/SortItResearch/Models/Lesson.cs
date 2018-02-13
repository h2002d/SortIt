using SortItResearch.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class Lesson
    {
        public int? Id { get; set; }
        public int? DayId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public string Attachement { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public string Facilitator { get; set; }

        public bool IsMandatory { get; set; }

        public bool Type { get; set; }

        public bool Passed { get; set; }
        static LessonDAO DAO = new LessonDAO();

        public int Save()
        {
            return DAO.saveModule(this);
        }

        public static void Delete(int id)
        {
            DAO.deleteLesson(id);
        }

        public static void DeleteLessonFromDay(int DayId, int LessonId)
        {
            DAO.deleteLessonFromDay(DayId, LessonId);
        }

        public static void SaveLessonFromDay(int DayId, int LessonId)
        {
            DAO.saveLessonFromDay(DayId, LessonId);
        }

        public static List<Lesson> GetLesson(int? id, int? typeId)
        {
            return DAO.getLesson(id, typeId);
        }

        public static List<Lesson> GetLessonBySubjectId(int id)
        {
            return DAO.getLessonBySubjectId(id);
        }

        public static List<Lesson> GetLessonByDayId(int id)
        {
            return DAO.getLessonByDayId(id);
        }
    }
}