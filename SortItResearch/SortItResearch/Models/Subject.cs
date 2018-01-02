using SortItResearch.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class Subject
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        private static SubjectDAO DAO= new SubjectDAO();
        
        public static List<Subject> GetSubject(int? id)
        {
            return DAO.getSubjects(id);
        }

       

        public void Save()
        {
            DAO.saveSubject(this);
        }
        public static void  Delete(int id)
        {
            DAO.deleteSubject(id);
        }
    }
}