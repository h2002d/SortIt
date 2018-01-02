using SortItResearch.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class MySubjectViewModel : Subject
    {
        public bool isAccepted { get; set; }
        public string TeacherId { get; set; }

        private static SubjectDAO DAO = new SubjectDAO();

        public static List<MySubjectViewModel> GetSubjectByStudentId(string studentId)
        {
            return DAO.getSubjectByStudentId(studentId);
        }

        public static MySubjectViewModel GetSubjectByStudentIdById(string studentId,int subjectId)
        {
            return DAO.getSubjectByStudentIdById(studentId,subjectId).First();
        }
    }
}