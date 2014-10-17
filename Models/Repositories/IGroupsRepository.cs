using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combo.Models
{
    public interface IGroupsRepository
    {
        IEnumerable<GroupModel> GetAllGroups();
        GroupModel GetGroupById(string id);
        void Add(GroupModel group);
        bool Delete(string objectId);
    }
}
