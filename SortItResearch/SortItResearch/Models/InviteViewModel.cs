using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SortItResearch.Models
{
    public class InviteViewModel:InviteModel
    {
        public UserProfile Student { get { return new UserProfile(StudentId); } }
        public Subject Subject { get { return Models.Subject.GetSubject(SubjectId).First(); } }
    }

}