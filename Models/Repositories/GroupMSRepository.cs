using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace Combo.Models
{
    public class GroupMSRepository:IGroupsRepository
    {
        private OperationDataContext context;
        // Constructor
        public GroupMSRepository()
        {
            
            context = new OperationDataContext();
        }
        

        public IEnumerable<GroupModel> GetAllGroups()
        {
            List<GroupModel> GroupsList = new List<GroupModel>();
            var querym = context.Groups.AsEnumerable().OrderBy(Group => Group.GroupName); 
            var Groups = querym.ToList();
            foreach (var GroupData in Groups)
            {
                GroupsList.Add(new GroupModel()
                {
                    Id = GroupData.Id.ToString(),
                    GroupName = GroupData.GroupName,
                    Speciality = GroupData.Speciality
                });
            }
            return GroupsList;
        }

        public GroupModel GetGroupById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id", "User Id is empty!");
            }
            GroupModel model = context.Groups.AsQueryable<Group>().Where(Group => Group.Id == Convert.ToInt32(id)).
                               Select(x => new GroupModel()
                                {
                                    Id = x.Id.ToString(),
                                    GroupName = x.GroupName,
                                    Speciality = x.Speciality

                                }).FirstOrDefault();
            return model;
        }

        public void Add(GroupModel groupmodel)
        {
            try
            {
                Group group = new Group()
                {
                    GroupName = groupmodel.GroupName,
                    Speciality = groupmodel.Speciality
                };
                context.Groups.InsertOnSubmit(group);
                context.SubmitChanges();

                
            }
            catch
            {
                //
            }
        }

        public bool Delete(string objectId)
        {
            try
            {
                Group group = context.Groups.Where(x => x.Id == Convert.ToInt32(objectId)).Single<Group>();
                context.Groups.DeleteOnSubmit(group);
                context.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

    }
}