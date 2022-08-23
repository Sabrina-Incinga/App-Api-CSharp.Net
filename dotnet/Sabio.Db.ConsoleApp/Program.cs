using Sabio.Data;
using Sabio.Models;
using Sabio.Models.Domain.Addresses;
using Sabio.Models.Domain.Companies;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Domain.Jobs;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Addresses;
using Sabio.Models.Requests.Companies;
using Sabio.Models.Requests.Friends;
using Sabio.Models.Requests.Jobs;
using Sabio.Models.Requests.Users;
using Sabio.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sabio.Db.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Here are two example connection strings. Please check with the wiki and video courses to help you pick an option

            //string connString = @"Data Source=ServerName_Or_IpAddress;Initial Catalog=DB_Name;User ID=SabioUser;Password=Sabiopass1!";
            string connString = @"";

            TestConnection(connString);
            TestJobService(connString);




            Console.ReadLine();//This waits for you to hit the enter key before closing window
        }
        private static void TestCompanyService(string conn)
        {
            SqlDataProvider provider = new SqlDataProvider(conn);
            CompanyService coService = new CompanyService(provider);

            CompanyAddRequest addRequest = new CompanyAddRequest();
            addRequest.Name = "MomCorp";
            addRequest.Profile = "Some profile";
            addRequest.Summary = "Some summary";
            addRequest.Headline = "A headline";
            addRequest.ContactInformation = "momcorp@corp.com";
            addRequest.Slug = "corp187654";
            addRequest.StatusId = "Active";
            addRequest.Tags = new List<string>() { "great co", "love it"};
            addRequest.Urls = new List<string>() { "www.momco.com" };
            CoImageAddRequest coImageAddRequest = new CoImageAddRequest();
            coImageAddRequest.ImageUrl = "some url";
            coImageAddRequest.EntityTypeId = 1;
            addRequest.Images = new List<CoImageAddRequest>() { coImageAddRequest };
            addRequest.FriendIds = new List<int>() { 17 };

            int userId = 6847;

            int newCompId = coService.Add(addRequest, userId);
            Company newCompany = coService.Get(newCompId);

            coService.UpdateStatusId(newCompId, "Deleted", userId);

            Company deletedCompany = coService.Get(newCompId);

            CompanyUpdateRequest updateRequest = new CompanyUpdateRequest();
            updateRequest.Id = newCompId;
            updateRequest.Name = "MomCorp";
            updateRequest.Profile = "Some new profile";
            updateRequest.Summary = "Some new summary";
            updateRequest.Headline = "A new headline";
            updateRequest.ContactInformation = "momcorp@corp.com";
            updateRequest.Slug = "corp187654";
            updateRequest.StatusId = "Active";
            updateRequest.Tags = new List<string>() { "great co", "love it" };
            updateRequest.Urls = new List<string>() { "www.momco.com", "www.newmomco.com" };
            CoImageAddRequest coNewImageAddRequest = new CoImageAddRequest();
            coImageAddRequest.ImageUrl = "some new url";
            coImageAddRequest.EntityTypeId = 2;
            updateRequest.Images = new List<CoImageAddRequest>() { coImageAddRequest };
            updateRequest.FriendIds = new List<int>() { 17 };

            coService.Update(updateRequest, userId);

            #region Getters Ok
            Company company = coService.Get(newCompId);
            Company company1 = coService.GetBySlug("acme2");
            List<Company> companies = coService.GetAll();
            Paged<Company> companyPage = coService.Pagination(0, 2);
            Paged<Company> companySearch = coService.Search(0, 2, "w");
            #endregion

            

        }

        private static void TestJobService(string conn)
        {
            SqlDataProvider provider = new SqlDataProvider(conn);
            JobService jobService = new JobService(provider);

            #region Getters Ok
            Job aJob = jobService.Get(37);
            List<Job> jobs = jobService.GetAll();
            Paged<Job> jobPage = jobService.Pagination(0, 5);
            Paged<Job> jobSearchPage = jobService.Search(0, 5, "w");
            #endregion


            JobAddRequest addReq = new JobAddRequest();
            addReq.Title = "New job";
            addReq.Description = "New job long description";
            addReq.Summary = "New job description summary";
            addReq.Pay = "10000";
            addReq.Slug = "job84178";
            addReq.StatusId = "Active";
            addReq.TechCompanyId = 4;
            addReq.Skills = new List<string>() { "Debugging", ".NET C#" };
            int UserId = 687914;

            int newJobId = jobService.Add(addReq, UserId);
            Job newJob = jobService.Get(newJobId);

            JobUpdateRequest updateReq = new JobUpdateRequest();
            updateReq.Id = newJobId;
            updateReq.Title = "Updated job";
            updateReq.Description = "Updated job long description";
            updateReq.Summary = "Updated job description summary";
            updateReq.Pay = "100000";
            updateReq.Slug = "job84178";
            updateReq.StatusId = "Active";
            updateReq.TechCompanyId = 4;
            updateReq.Skills = new List<string>() { "Debugging", ".NET C#" };

            jobService.Update(updateReq, UserId);
            Job updatedJob = jobService.Get(newJobId);

            jobService.Delete(newJobId);

            Job deletedJob = jobService.Get(newJobId);

            
        }
        
        private static void TestFriendService(string conn)
        {
            SqlDataProvider provider = new SqlDataProvider(conn);
            FriendService FService = new FriendService(provider);

            #region V1 methods test
            /*
            #region Getters
            Friend aFriend = FService.Get(16);
            List<Friend> allFriends = FService.GetAll();
            #endregion

            FriendAddRequest addRequest = new FriendAddRequest();
            addRequest.Title = "Anne";
            addRequest.Bio = "A bio";
            addRequest.Summary = "A summary";
            addRequest.Headline = "Some headline";
            addRequest.Slug = "friend6874";
            addRequest.StatusId = 1;
            addRequest.PrimaryImageUrl = "Some image";
            int UserId = 687914;

            int newFriendId = FService.Add(addRequest, UserId);
            Friend newFriend = FService.Get(newFriendId);

            FriendUpdateRequest updateRequest = new FriendUpdateRequest();
            updateRequest.Title = "Anita";
            updateRequest.Bio = "A new bio";
            updateRequest.Summary = "A new summary";
            updateRequest.Headline = "Some new headline";
            updateRequest.Slug = "friend654";
            updateRequest.StatusId = 2;
            updateRequest.PrimaryImageUrl = "Some image";
            updateRequest.Id = newFriendId;

            FService.Update(updateRequest, UserId);

            Friend updatedFriend = FService.Get(newFriendId);

            FService.Delete(newFriendId);

            Friend deletedFriend = FService.Get(newFriendId);*/
            #endregion

            #region V2 methods test
            #region Get Ok
            /*FriendV2 aFriend = FService.GetV2(16);
            List<FriendV2> allFriends = FService.GetAllV2();
            Paged<FriendV2> page1 = FService.PaginationV2(0, 5);*/
            #endregion

            /* FriendAddRequestV2 addRequest = new FriendAddRequestV2();
             addRequest.Title = "Anne";
             addRequest.Bio = "A bio";
             addRequest.Summary = "A summary";
             addRequest.Headline = "Some headline";
             addRequest.Slug = "friend354";
             addRequest.StatusId = 1;
             addRequest.ImageUrl = "Some image";
             addRequest.ImageTypeId = 1;
             int UserId = 687914;

             int newFriendId = FService.AddV2(addRequest, UserId);
             FriendV2 newFriend = FService.GetV2(newFriendId);

             FriendUpdateRequestV2 updateRequest = new FriendUpdateRequestV2();
             updateRequest.Title = "Anita";
             updateRequest.Bio = "A new bio";
             updateRequest.Summary = "A new summary";
             updateRequest.Headline = "Some new headline";
             updateRequest.Slug = "friend354";
             updateRequest.StatusId = 2;
             updateRequest.ImageUrl = "Some new image";
             updateRequest.ImageTypeId = 1;
             updateRequest.Id = newFriendId;

             FService.UpdateV2(updateRequest, UserId);

             FriendV2 updatedFriend = FService.GetV2(newFriendId);

             FService.DeleteV2(newFriendId);

             FriendV2 deletedFriend = FService.GetV2(newFriendId); */
            #endregion

            #region V3 methods test
            #region Get Ok
            FriendV3 aFriend = FService.GetV3(16);
            List<FriendV3> allFriends = FService.GetAllV3();
            Paged<FriendV3> pageOne = FService.PaginationV3(0, 5);
            Paged<FriendV3> searchPage = FService.SearchV3(0, 5, "w");
            #endregion

            FriendAddRequestV3 addRequest = new FriendAddRequestV3();
            addRequest.Title = "Anne";
            addRequest.Bio = "A bio";
            addRequest.Summary = "A summary";
            addRequest.Headline = "Some headline";
            addRequest.Slug = "friend3878";
            addRequest.StatusId = 1;
            addRequest.ImageUrl = "Some image";
            addRequest.ImageTypeId = 1;
            addRequest.Skills = new List<string> { "Java", "React" };
            int UserId = 687914;

            int newFriendId = FService.AddV3(addRequest, UserId);
            FriendV3 newFriend = FService.GetV3(newFriendId);

            FriendUpdateRequestV3 updateRequest = new FriendUpdateRequestV3();
            updateRequest.Id = newFriendId;
            updateRequest.Title = "Anne of Green Gables";
            updateRequest.Bio = "A bio";
            updateRequest.Summary = "A summary";
            updateRequest.Headline = "Some headline";
            updateRequest.Slug = "friend3878";
            updateRequest.StatusId = 1;
            updateRequest.ImageUrl = "Some image";
            updateRequest.ImageTypeId = 1;
            updateRequest.Skills = new List<string> { "Java", "Angular", "SQL" };

            FService.UpdateV3(updateRequest, UserId);
            
            FriendV3 updatedFriend = FService.GetV3(newFriendId);

            FService.DeleteV3(newFriendId);

            FriendV3 deletedFriend = FService.GetV3(newFriendId);
            #endregion



            Console.WriteLine("");
        }

        private static void TestUserService(string connection)
        {
            SqlDataProvider provider = new SqlDataProvider(connection);
            UserServiceV1 UService = new UserServiceV1(provider);

            #region Getters
            User aUser = UService.Get(1);
            List<User> users = UService.GetAll();
            #endregion

            #region Add request
            UserAddRequest aRequest = new UserAddRequest();

            aRequest.FirstName = "Peter";
            aRequest.LastName = "Parker";
            aRequest.Email = "peter@parker.com";
            aRequest.AvatarUrl = "Some url";
            aRequest.TenantId = "LKJ6575A";
            aRequest.Password = "password";
            aRequest.PasswordConfirm = "password1";

            int newUserId = UService.Add(aRequest);

            User addedUser = UService.Get(newUserId);
            #endregion

            #region Update request
            UserUpdateRequest updateRequest = new UserUpdateRequest();

            updateRequest.FirstName = "Peter Joseph";
            updateRequest.LastName = "Parker";
            updateRequest.Email = "peter458@parker.com";
            updateRequest.AvatarUrl = "Some other url";
            updateRequest.TenantId = "LKJ6575A";
            updateRequest.Password = "password";
            updateRequest.PasswordConfirm = "password1";
            updateRequest.Id = newUserId;

            UService.Update(updateRequest);

            User updatedUser = UService.Get(newUserId);
            #endregion

            #region Delete request
            UService.Delete(newUserId);

            User deletedUser = UService.Get(newUserId); 
            #endregion

            Console.WriteLine("");
        }

        private static void TestAddressService(string aConn)
        {

            #region Constructor calls - OK
            SqlDataProvider provider = new SqlDataProvider(aConn);
            AddressService AService = new AddressService(provider);
            #endregion

            #region Getters - OK
            Address anAddress = AService.Get(9);
            Address anotherAddress = AService.Get(99999);

            List<Address> addresses = AService.GetRandomAddresses();
            #endregion

            #region Add request - OK
            AddressAddRequest request = new AddressAddRequest();
            request.LineOne = "A line one";
            request.SuiteNumber = 152;
            request.City = "Some city";
            request.State = "Some state";
            request.PostalCode = "AT6847";
            request.IsActive = true;
            request.Lat = 12.68786;
            request.Long = -13.59781;

            int UserId = 687914;

            int newAddressId = AService.Add(request, UserId);
            #endregion

            #region Update request - OK
            AddressUpdateRequest updateRequest = new AddressUpdateRequest();
            updateRequest.LineOne = "A new line one";
            updateRequest.SuiteNumber = 1552;
            updateRequest.City = "Some new city";
            updateRequest.State = "Some new state";
            updateRequest.PostalCode = "AT6847";
            updateRequest.IsActive = true;
            updateRequest.Lat = 12.68786;
            updateRequest.Long = -13.59781;
            updateRequest.Id = newAddressId;

            AService.Update(updateRequest, UserId);

            Address updatedAddress = AService.Get(newAddressId); 
            #endregion

            Console.WriteLine(updatedAddress.Id.ToString());

            AService.Delete(newAddressId);

            Address deletedAddress = AService.Get(newAddressId);

            Console.WriteLine("");

        }

        private static void TestConnection(string connString)
        {
            bool isConnected = IsServerConnected(connString);
            Console.WriteLine("DB isConnected = {0}", isConnected);
        }
         
        private static bool IsServerConnected(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
    }
}
