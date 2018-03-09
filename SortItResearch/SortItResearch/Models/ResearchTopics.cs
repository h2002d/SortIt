using SortItResearch.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class ResearchTopics
    {
        public int? Id { get; set; }
        public string Topic { get; set; }
        public string ShortDescription { get; set; }
        public string UserId { get; set; }
        public int SubjectId { get; set; }

        public List<int> ResearchIds { get; set; }
        static ResearchTopicDAO DAO = new ResearchTopicDAO();

        public static List<ResearchTopics> GetTopicById(int id)
        {
            return DAO.getTopicById(id);
        }

        public void Save()
        {
            DAO.saveTopic(this);
        }
        public static void SaveAreas(List<int> id,int topicId)
        {
            DAO.saveAreas(id, topicId);
        }

        public static void SaveProfileAreas(List<int> id,string userId)
        {
            DAO.deleteProfileAreas(userId);
            DAO.saveProfileAreas(id,userId);
        }
    }

    public class TopicArea
    {
        public int Id { get; set; }
        public string Name { get; set; }
        static ResearchTopicDAO DAO = new ResearchTopicDAO();

        public static List<TopicArea> GetTopicArea(int? id)
        {
            return DAO.getAreaById(id);
        }



    }
}