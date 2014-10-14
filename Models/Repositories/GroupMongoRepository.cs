using Combo.Properties;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Combo.Models
{
    public class GroupMongoRepository:IGroupsRepository
    {
        public MongoDatabase MongoDatabase;
        public MongoCollection GroupsCollection;
        public bool ServerIsDown = false;
 
        // Constructor
        public GroupMongoRepository()
        {
            // Get the Mongo Client
            var mongoClient = new MongoClient(Settings.Default.StudentsConnectionString);
 
            // Get the Mongo Server from the Cliet Instance
            var server = mongoClient.GetServer();
 
            // Assign the database to mongoDatabase
            MongoDatabase = server.GetDatabase(Settings.Default.DB);
 
            // get the Groups collection (table) and assign to GroupsCollection
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

        private GroupModel[] _testGroupData = new GroupModel[]
        {
            new GroupModel()
            {
                Id = Guid.NewGuid().ToString(),
                GroupName = "KM01",
                Speciality = "IT",
                
            },
            new GroupModel()
            {
                Id = Guid.NewGuid().ToString(),
                GroupName = "KM02",
                Speciality = "IT",
                
            },
            new GroupModel()
            {
                Id = Guid.NewGuid().ToString(),
                GroupName = "GIS01",
                Speciality = "Geoinformation",
                
            },
            new GroupModel()
            {
                Id = Guid.NewGuid().ToString(),
                GroupName = "GIS02",
                Speciality = "Geoinformation",
                
            },
            new GroupModel()
            {
                Id = Guid.NewGuid().ToString(),
                GroupName = "IS02",
                Speciality = "Information",
                
            }
            
        };
 
        #endregion

        

        public IEnumerable<GroupModel> GetAllGroups()
        {
            if (ServerIsDown) return null;
            List<GroupModel> _GroupsList = new List<GroupModel>();
 
            if (Convert.ToInt32(GroupsCollection.Count()) > 0)
            {
                _GroupsList.Clear();
                
                var _Groups = GroupsCollection.FindAllAs<GroupModel>();
                if (_Groups.Count() > 0)
                {
                    foreach (GroupModel _Group in _Groups)
                    {
                        _GroupsList.Add(_Group);
                    }
                }
            }
            else
            {
                #region add test data if DB is empty
 
                GroupsCollection.RemoveAll();
                foreach (var Group in _testGroupData)
                {
                    _GroupsList.Add(Group);

                    Add(Group); // add data to mongo db also
                }
 
                #endregion
            }
 
            return _GroupsList;
        }


        public GroupModel GetGroupById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id", "User Id is empty!");
            }
            var Group = (GroupModel)GroupsCollection.FindOneAs(typeof(GroupModel), Query.EQ("_id", id));
            return Group;
        }


        public GroupModel Add(GroupModel Group)
        {
            if (string.IsNullOrEmpty(Group.Id))
            {
                Group.Id = Guid.NewGuid().ToString();
            }
            GroupsCollection.Save(Group);
            return Group;
        }

        /*public bool Update(string objectId, GroupModel Group)
        {
            UpdateBuilder updateBuilder = MongoDB.Driver.Builders.Update
                .Set("GroupName", Group.GroupName)
                .Set("Speciality", Group.Speciality);
            GroupsCollection.Update(Query.EQ("_id", objectId), updateBuilder);
 
            return true;
        }*/
 
        public bool Delete(string objectId)
        {
            GroupsCollection.Remove(Query.EQ("_id", objectId));
            return true;
        }
    }

}