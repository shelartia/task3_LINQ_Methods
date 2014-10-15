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
            var query = from Group in context.Groups
                        select Group;

            var Groups = query.ToList();
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
            GroupModel model = (from Group in context.Groups

                                where Group.Id == Convert.ToInt32(id)
                                select new GroupModel()
                                {
                                    Id = Group.Id.ToString(),
                                    GroupName = Group.GroupName,
                                    Speciality = Group.Speciality

                                }).FirstOrDefault();
            return model;
        }

        public GroupModel Add(GroupModel groupmodel)
        {
            try
            {
                Group Group = new Group()
                {
                    GroupName = groupmodel.GroupName,
                    Speciality = groupmodel.Speciality
                };
                context.Groups.InsertOnSubmit(Group);
                context.SubmitChanges();
                return groupmodel;
            }
            catch
            {
                return groupmodel;
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