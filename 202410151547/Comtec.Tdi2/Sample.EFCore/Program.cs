using Microsoft.EntityFrameworkCore;
using Sample.EFCore.DL.Context;
using Sample.EFCore.DL.Repository.Samples;

namespace Sample.EFCore {
    internal class Program {
        static void Main(string[] args) {
            //GenderTypeModel genderType = new GenderTypeModel();
            //genderType.Name = "T";
            //genderType.Description = "Ts";
            //DLEFCoreContext context = new DLEFCoreContext();
            //context.GenderType.Add(genderType);
            //context.SaveChanges();

            //foreach (var gender in context.GenderType.Where(x => x.Disabled == false).OrderBy(x => x.Name)) {
            //    Console.WriteLine(gender.Name);
            //}

            //// without repository
            //AuthorModel author = new AuthorModel();
            //author.Name = "John Grisham";
            //author.CreatedDateTime = DateTime.Now;

            //var dlContext = new DLContext();
            //dlContext.Authors.Add(author);
            //dlContext.SaveChanges();

            //foreach (AuthorModel a in dlContext.Authors)
            //{
            //    Console.WriteLine(a.Name);
            //}
            //Console.ReadKey();

            //var users = dlContext.Users.Include(x => x.DepartmentID).ToList();

            //var user = dlContext.Users.FirstOrDefault(x => x.Id == 1);
            //dlContext.Departments.Where(x => x.Id == user.DepartmentID).Load();

            // with repository
            var dbContext = new DlefCoreContext();
            GenderTypeModel genderType = new GenderTypeModel();
            genderType.Name = "Q";
            genderType.Description = "Q";
            dbContext.GenderType.Add(genderType);
            dbContext.SaveChanges();

            foreach (var item in dbContext.GenderType.ToList()) {
                Console.WriteLine(item.Id);
            }

            dbContext.GenderType.Where(x => x.Name == "Q").ExecuteDelete();
            dbContext.SaveChanges();

            Console.WriteLine();
            Console.ReadKey();
        }
    }
}