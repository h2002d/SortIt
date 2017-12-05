using SortItResearch.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public DateTime DateBirth { get; set; }
        static UserProfileDAO DAO = new UserProfileDAO();


       public UserProfile(string id)
        {
           var user= DAO.getUserById(id);
            this.Id = user.Id;
            this.Name = user.Name;
            this.SurName = user.SurName;
            this.Email = user.Email;
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