using Framework.ObjectModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var xmlPath = Directory.GetCurrentDirectory() + "\\..\\..\\Database.xml";
            ConnectionProviderBase.Init(xmlPath);
            var tableXml1 = Directory.GetCurrentDirectory() + "\\..\\..\\Table1.xml";
            var tableXml2 = Directory.GetCurrentDirectory() + "\\..\\..\\Table2.xml";
            EntityManager.Assemble("Default", tableXml1);
            EntityManager.Assemble("Hz", tableXml2);
            EntityManager.Assemble("Hz2", tableXml1);
            EntityManager.Assemble("Hz2", tableXml2);
            EntityManager.Init();




            //Single Insert
            //Student student = new Student { Id = Guid.NewGuid().ToString(), Name = "XiaoQiang", Age = 12};
            //SqlCondition condition = new SqlCondition("Student");
            //using (DbContext context = new DbContext(condition))
            //{
            //    var jj = context.Insert(student);
            //}


            //Batch Insert
            //List<Student> students = new List<Student>();
            //students.Add(new Student { Id = Guid.NewGuid().ToString(), Name = "XiaoQiang2", Age = 12 });
            //students.Add(new Student { Id = Guid.NewGuid().ToString(), Name = "XiaoQiang3", Age = 13 });
            //students.Add(new Student { Id = Guid.NewGuid().ToString(), Name = "XiaoQiang4", Age = 14 });
            //students.Add(new Student { Id = Guid.NewGuid().ToString(), Name = "XiaoQiang5", Age = 15 });
            //SqlCondition condition = new SqlCondition("Student");
            //using (DbContext context = new DbContext(condition))
            //{
            //    var jj = context.Insert(students);
            //}

            //Query
            //SqlCondition condition = new SqlCondition("Student")
            //.SetFilter(FilterWay.FilterOut, "Id")
            //.SetOrderBy(OrderBy.DESC, "Name")
            //.SetContraints(new Expression().Add("Age", 13, Operator.GreaterThan));
            //using (DbContext context = new DbContext(condition))
            //{
            //    var jj = context.Query<Student>();
            //}


            //Delete
            //SqlCondition condition = new SqlCondition("Student")
            //.SetContraints(new Expression().Add("Age", 13));
            //using (DbContext context = new DbContext(condition))
            //{
            //    var jj = context.Delete();
            //}


            //Update
            Student student = new Student { Id = Guid.NewGuid().ToString(), Name = "DaWang3", Age = 80, Weight = 150 };
            SqlCondition condition = new SqlCondition("Student")
            .SetFilter(FilterWay.FilterOut, "Id")
            .SetContraints(new Expression().Add("Age", 79, Operator.GreaterThan));
            using (DbContext context = new DbContext(condition))
            {
                var jj = context.Update<Student>(student);
            }
        }
    }
}
