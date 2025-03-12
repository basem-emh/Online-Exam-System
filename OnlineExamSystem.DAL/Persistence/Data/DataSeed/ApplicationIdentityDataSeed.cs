using Microsoft.AspNetCore.Identity;
using OnlineExamSystem.DAL.Entities.Identity;

namespace OnlineExamSystem.DAL.Persistence.Data.DataSeed
{
    public static class ApplicationIdentityDataSeed
    {
        public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
        {
            if (! userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    FirstName = "Basem",
                    LastName = "Emad",
                    Email = "basememad1907@gmail.com",
                    UserName = "Basem.Emad",
                    PhoneNumber = "01118914149",
                };
                await userManager.CreateAsync(user, "P@sw0rd");
            }
        } 
    }
}
