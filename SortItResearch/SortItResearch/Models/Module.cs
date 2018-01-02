using SortItResearch.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class Module
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int SubjectId { get; set; }

        public List<ModuleDay> ModuleDays { get; set; }

        static ModeluDAO DAO = new ModeluDAO();
        public Module()
        {
            ModuleDays = new List<ModuleDay>();
        }

        public static List<Module> GetModules(int? id)
        {
            return DAO.getModules(id);
        }
        public static List<Module> GetModulesBySubjectId(int? id)
        {
            return DAO.getModulesBySubjectId(id);
        }

        public static void Delete(int id)
        {
            DAO.deleteModule(id);
        }

        public void Save()
        {
            DAO.saveModule(this);
        }
    }

    internal class ModuleProgress
    {
        public Module Module { get; set; }
        public bool Passed { get; set; }
    }
}