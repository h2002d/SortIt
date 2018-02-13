using SortItResearch.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class Progress
    {
        public int? Id { get; set; }
        public string StudentId { get; set; }
        public int LessonId { get; set; }
        public bool Passed { get; set; }
        public string Attachement { get; set; }
        static ProgressDAO DAO = new ProgressDAO();

        public static Progress GetProgressById(int id)
        {
            return DAO.getProgressById(id);
        }

        public static Progress GetProgressByLessonId(string studentId,int lessonId)
        {
            return DAO.getProgressByLessonId(studentId, lessonId);
        }

        public static bool IsPassed(int subjectId,string studentId)
        {
            return DAO.IsPassed(subjectId, studentId);
        }

        public static List<HomeworkViewModel> GetProgressByTeacher(string teacherId)
        {
            return DAO.getProgressByTeacherId(teacherId);
        }

        public static void SetProgressStatus(int id)
        {
            DAO.setProgressStatus(id);
        }

        public void Save()
        {
            DAO.saveProgress(this);
        }
    }
}