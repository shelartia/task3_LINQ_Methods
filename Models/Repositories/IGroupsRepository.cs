using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combo.Models
{
    interface IGroupsRepository
    {
        IEnumerable<GroupModel> GetAllGroups();
        GroupModel GetGroupById(string id);
        GroupModel Add(GroupModel group);
        bool Delete(string objectId);
    }
}
