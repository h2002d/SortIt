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
        public string Affiliation { get; set; }
        public int Country { get; set; }
        public string Profession { get; set; }
        public string Degree { get; set; }
        public string Publicants { get; set; }
        public string Phone { get; set; }

        public string Email { get; set; }
        public bool isTeacher { get; set; }
        public bool isMale { get; set; }
        public DateTime DateBirth { get; set; }
        static UserProfileDAO DAO = new UserProfileDAO();

        public UserProfile(string id)
        {
            var user = DAO.getUserById(id);
            this.Id = user.Id;
            this.Name = user.Name;
            this.SurName = user.SurName;
            this.Email = user.Email;
            this.Dissertation = user.Dissertation;
            this.Affiliation = user.Affiliation;
            this.Degree = user.Degree;
            this.Profession = user.Profession;
            this.Phone = user.Phone;
            this.Publicants = user.Publicants;
            this.Country = user.Country;
            this.isTeacher = user.isTeacher;
        }

        public static UserProfile GetTeacherByStudentLessonId(string student, int lessonId)
        {
            return DAO.getUserById(DAO.getTeacherByProgress(student, lessonId));
        }

        public UserProfile()
        {

        }

        public static List<UserProfile> GetSkilledUsers(int moduleId, int roleId)
        {
            return SkillDAO.getUserBySkill(moduleId, roleId);
        }

        public static List<UserProfile> GetUserByGender(bool isMale,string role)
        {
            return UserProfileDAO.getUserByGender(isMale,role);
        }
        public static List<UserProfile> GetUserByInterest(int id)
        {
            return UserProfileDAO.getUserByInterestId(id);
        }
        public void Save()
        {
            DAO.saveUser(this);
        }
    }
}