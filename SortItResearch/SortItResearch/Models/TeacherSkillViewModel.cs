using SortItResearch.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class TeacherSkillViewModel
    {
        public Subject Subject { get; set; }
        public List<Module> Modules {get; set;}
        public static SkillDAO DAO = new SkillDAO();
        public TeacherSkillViewModel()
        {
            Modules = new List<Module>();
        }

        public void Save(string teacherId,List<int> moduleIds)
        {
            DAO.saveModule(teacherId, moduleIds);
        }

        public static List<int> GetSelectedSkills(string teacherId)
        {
           return DAO.getSelectedSkills(teacherId);
        }
       
    }
}