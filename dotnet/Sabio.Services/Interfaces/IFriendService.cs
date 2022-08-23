using Sabio.Models;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Requests.Friends;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IFriendService
    {
        int Add(FriendAddRequest addRequest, int userId);
        int AddV2(FriendAddRequestV2 addRequest, int userId);
        int AddV3(FriendAddRequestV3 addRequest, int userId);
        void Delete(int id);
        void DeleteV2(int id);
        void DeleteV3(int id);
        Friend Get(int id);
        List<Friend> GetAll();
        List<FriendV2> GetAllV2();
        List<FriendV3> GetAllV3();
        FriendV2 GetV2(int id);
        FriendV3 GetV3(int id);
        Paged<FriendV2> PaginationV2(int pageIndex, int pageSize);
        Paged<FriendV3> PaginationV3(int pageIndex, int pageSize);
        Paged<FriendV2> SearchV2(int pageIndex, int pageSize, string query);
        Paged<FriendV3> SearchV3(int pageIndex, int pageSize, string query);
        void Update(FriendUpdateRequest updateRequest, int userId);
        void UpdateV2(FriendUpdateRequestV2 updateReq, int userId);
        void UpdateV3(FriendUpdateRequestV3 updateReq, int userId);
    }
}