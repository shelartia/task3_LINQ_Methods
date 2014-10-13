using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Combo.Models
{
    public class StudentModel
    {
        [BsonId]
        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Address { get; set; }

        [DisplayName("Group")]
        public string Group_Id { get; set; }
        public string GroupName { get; set; }
        public IEnumerable<SelectListItem> Groups { get; set; }
    }
}