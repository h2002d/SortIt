using SortItResearch.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class ModuleDay
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int ModuleId { get; set; }
        [Required]
        public int TrainingId { get; set; }
        public List<Lesson> Lessons { get; set; }
        static ModuleDayDAO DAO = new ModuleDayDAO();  

        public ModuleDay()
        {
            Lessons = new List<Lesson>();
        }
        public static List<ModuleDay> GetDay(int? id)
        {
            return DAO.getModuleDays(id);
        }
        public void Save()
        {
            DAO.saveModuleDay(this);
        }

        public static void Delete(int id)
        {
            DAO.deleteModuleDay(id);
        }
    }
}