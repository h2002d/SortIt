using SortItResearch.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class Certificate
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public UserProfile Student { get { return new UserProfile(StudentId); } }
        public DateTime CreateDate { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get { return Subject.GetSubject(SubjectId).First(); } }
        static CertificateDAO DAO = new CertificateDAO();

        public static List<Certificate> GetCertificate(int id)
        {
            return DAO.getDissertationById(id);
        }

        public static List<Certificate> GetCertificateByStudentId(string studentId)
        {
            return DAO.getDissertationByStudentId(studentId);
        }

        public int Save()
        {
           return DAO.Save(this);
        }

    }
}