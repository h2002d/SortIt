using SortItResearch.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class Dissertation
    {
        public int? Id { get; set; }

        public string Attachement { get; set; }
        public DateTime CreateDate { get; set; }
        public string StudentId { get; set; }
        public int SubjectId { get; set; }
        public bool Accepted { get; set; }

        static DissertationDAO DAO = new DissertationDAO();

        internal static List<DissertationViewModel> GetDissertationBySubjectId(int subjectId)
        {
            return DAO.getDissertationBySubject(subjectId);
        }

        internal static Dissertation GetDissertationByStudentId(int subjectId,string studentId)
        {
            return DAO.getDissertationByStudent(subjectId,studentId);
        }

        internal static DissertationViewModel GetDissertationById(int id)
        {
            return DAO.getDissertationById(id).FirstOrDefault();
        }
        internal void Save()
        {
            DAO.saveDissertation(this);
        }

        internal void SaveStatus()
        {
            DAO.saveDissertationStatus(Convert.ToInt32(Id));
        }
    }
}