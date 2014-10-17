using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Combo.Models
{
    public class StudentMSRepository:IStudentsRepository
    {
        private OperationDataContext context;
        // Constructor
        public StudentMSRepository()
        {
            context = new OperationDataContext();
        }

        private List<StudentModel> StudentList = new List<StudentModel>();

        public IEnumerable<StudentModel> GetAllStudents()
        {
            var query = from Student in context.Students
                        join Group in context.Groups
                        on Student.Group_Id equals Group.Id
                        select new StudentModel
                        {
                            Id = Student.Id.ToString(),
                            FirstName = Student.FirstName,
                            LastName = Student.LastName,
                            Address = Student.Address,
                            GroupName = Group.GroupName

                        };
            StudentList = query.ToList();
            return StudentList;
        }

        public void PrepareGroup(StudentModel model)
        {
            model.Groups = context.Groups.AsQueryable<Group>().Select(x =>
                    new SelectListItem()
                    {
                        Text = x.GroupName,
                        Value = x.Id.ToString()
                    });
        }

        

        public StudentModel GetStudentById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id", "User Id is empty!");
            }
            StudentModel model = (from Student in context.Students
                                  join Group in context.Groups
                                  on Student.Group_Id equals Group.Id
                                  where Student.Id == Convert.ToInt32(id)
                                  select new StudentModel()
                                  {
                                      Id = Student.Id.ToString(),
                                      FirstName = Student.FirstName,
                                      LastName = Student.LastName,
                                      Address = Student.Address,
                                      GroupName = Group.GroupName
                                  }).FirstOrDefault();
            return model;
        }

        public void Add(StudentModel model)
        {
            try
            {
                Student Student = new Student()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    Group_Id = Convert.ToInt32(model.Group_Id)
                };
                context.Students.InsertOnSubmit(Student);
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
                Student Student = context.Students.Where(x => x.Id == Convert.ToInt32(objectId)).Single<Student>();
                context.Students.DeleteOnSubmit(Student);
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