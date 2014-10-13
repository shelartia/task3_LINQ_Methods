using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Combo.Models
{
    public class GroupModel
    {
        [BsonId]
        public string Id { get; set; }
        [Required]
        [DisplayName("Group Name")]
        public string GroupName { get; set; }
        public string Speciality { get; set; }
        public IList<StudentModel> Students { get; set; }
    }
}