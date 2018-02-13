using SortItResearch.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class UserProfile
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string SurName { get; set; }
        public string Dissertation { get; set; }
        public string Email { get; set; }
        public bool isTeacher { get; set; }
        public DateTime DateBirth { get; set; }
        static UserProfileDAO DAO = new UserProfileDAO();


       public UserProfile(string id)
        {
           var user= DAO.getUserById(id);
            this.Id = user.Id;
            this.Name = user.Name;
            this.SurName = user.SurName;
            this.Email = user.Email;
            this.Dissertation = user.Dissertation;

        }

        public static UserProfile GetTeacherByStudentLessonId(string student,int lessonId)
        {
            return  DAO.getUserById(DAO.getTeacherByProgress(student, lessonId));

        }
        public UserProfile()
        {

        }

        public static List<UserProfile> GetSkilledUsers(int moduleId, int roleId)
        {
            return SkillDAO.getUserBySkill(moduleId, roleId);
        }

        public void Save()
        {
            DAO.saveUser(this);
        }
    }
}