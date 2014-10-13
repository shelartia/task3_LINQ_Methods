using Combo.Properties;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Combo.Models
{
    
    public class StudentMongoRepository:IStudentsRepository
    {
        public MongoDatabase MongoDatabase;
        public MongoCollection StudentsCollection;
        private MongoCollection GroupsCollection;
        public bool ServerIsDown = false;

        // Constructor
        public StudentMongoRepository()
        {
            // Get the Mongo Client
            var mongoClient = new MongoClient(Settings.Default.StudentsConnectionString);
 
            // Get the Mongo Server from the Cliet Instance
            var server = mongoClient.GetServer();
 
            // Assign the database to mongoDatabase
            MongoDatabase = server.GetDatabase(Settings.Default.DB);
 
            // get the Students and groups collection (table) and assign to Collection
            StudentsCollection = MongoDatabase.GetCollection("StudentM");
            GroupsCollection = MongoDatabase.GetCollection("GroupM");    
 
            //test if server is up and running
            try
            {
                MongoDatabase.Server.Ping(); 
            // Ping() method throws exception if not able to connect
 
            }
            catch (Exception ex)
            {
                ServerIsDown = true;
            }
        }

        #region Test Data
        private StudentModel[] getTestData()
        {
            var Groups = GroupsCollection.FindAs(typeof(GroupModel), Query.NE("GroupName", "null"));
            IList<string> group_ids = new List<string>();
            if (Groups.Count() > 0)
            {
                
                foreach (GroupModel Group in Groups)
                {
                    group_ids.Add(Group.Id);
                }
            }
            return new StudentModel[] {
                new StudentModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Gerry",
                    LastName = "Ikonov",
                    Address = "address",
                    Group_Id = group_ids.First()
                
                },
                new StudentModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Berry",
                    LastName = "Dyakonov",
                    Address = "adress",
                    Group_Id = group_ids.First()
                
                },
                new StudentModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Serry",
                    LastName = "Donov",
                    Address = "adres",
                    Group_Id = group_ids.Last()
                
                }

            };
        }

        #endregion

        private List<StudentModel> _StudentsList = new List<StudentModel>();

        public IEnumerable<StudentModel> GetAllStudents()
        {
            if (ServerIsDown) return null;

            if (Convert.ToInt32(GroupsCollection.Count()) > 0)
            {
                _StudentsList.Clear();
                var Students = StudentsCollection.FindAs(typeof(StudentModel), Query.NE("FirstName", "null"));
                if (Students.Count() > 0)
                {
                    foreach (StudentModel model in Students)
                    {
                        model.GroupName = (from g in GroupsCollection.AsQueryable<GroupModel>()
                                           where g.Id == model.Group_Id
                                           select g.GroupName).SingleOrDefault();
                        _StudentsList.Add(model);
                    }
                }
            }
            else
            {
                #region add test data if DB is empty

                StudentsCollection.RemoveAll();
                foreach (var student in getTestData())
                {
                    _StudentsList.Add(student);

                    Add(student); // add data to mongo db also
                }

                #endregion
            }
            

            var result = _StudentsList.AsQueryable();
            return result;
        }

        public void PrepareGroup(StudentModel model)
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            var Groups = GroupsCollection.FindAs(typeof(GroupModel), Query.NE("GroupName", "null"));
            if (Groups.Count() > 0)
            {

                foreach (GroupModel Group in Groups)
                {
                    groups.Add(new SelectListItem
                    {
                        Text = Group.GroupName,
                        Value = Group.Id.ToString()
                    });
                }
            }
            model.Groups = groups;
        }

        public StudentModel GetStudentById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id", "User Id is empty!");
            }
            var model = (StudentModel)StudentsCollection.FindOneAs(typeof(StudentModel), Query.EQ("_id", id));
            model.GroupName = (from g in GroupsCollection.AsQueryable<GroupModel>()
                               where g.Id == model.Group_Id
                               select g.GroupName).SingleOrDefault();
            return model;
        }

        public StudentModel Add(StudentModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                model.Id = Guid.NewGuid().ToString();
            }
            StudentsCollection.Save(model);
            return model;
        }

        public bool Delete(string objectId)
        {
            StudentsCollection.Remove(Query.EQ("_id", objectId));
            return true;
        }

    }
}