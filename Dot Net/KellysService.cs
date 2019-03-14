using Microsoft.Extensions.Caching.Memory;
using Kellys.Data;
using Kellys.Data.Providers;
using Kellys.Models;
using Kellys.Models.Domain;
using Kellys.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;

namespace Kellys.Services
{
    public class InfluencerService : IInfluencerService
    {
        private readonly IDataProvider _dataProvider;
        private readonly ICacheService _cacheService;
        private static readonly MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        private static readonly string key = "Influencer_CacheData_";

        public InfluencerService(IDataProvider dataProvider, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _dataProvider = dataProvider;
        }

        public List<Influencer> SelectAll()
        {
            List<Influencer> allInfluencers = null;

            _dataProvider.ExecuteCmd(
                "dbo.Influencer_SelectAll",
                null, (reader, recordSetIndex) =>
                {
                    if (allInfluencers == null)
                    {
                        allInfluencers = new List<Influencer>();
                    }
                    Influencer influencer = new Influencer();
                    Mapper(reader, influencer);
                    allInfluencers.Add(influencer);
                });
            return allInfluencers;
        }

        public Paged<Influencer> Pagination(int pageIndex, int pageSize)
        {
            Paged<Influencer> pagedList = null;
            List<Influencer> list = null;
            int totalCount = 0;

            _dataProvider.ExecuteCmd(
                "dbo.Influencer_SelectAllPaginated",
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                },
                (reader, recordSetIndex) =>
                {

                    Influencer influencer = PageMapper(reader);

                    totalCount = reader.GetSafeInt32(12);

                    if (list == null)
                    {
                        list = new List<Influencer>();
                    }
                    list.Add(influencer);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<Influencer>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Influencer> SearchPaginated(string search, int pageIndex, int pageSize)
        {
            Paged<Influencer> pagedList = null;
            List<Influencer> list = null;
            int totalCount = 0;

            _dataProvider.ExecuteCmd(
                "dbo.Influencer_SearchPaginated",
                (paramCol) =>
                {
                    paramCol.AddWithValue("@Search", search);
                    paramCol.AddWithValue("@PageIndex", pageIndex);
                    paramCol.AddWithValue("@PageSize", pageSize);
                },
                (reader, set) =>
                {
                    Influencer influencer = PageMapper(reader);

                    totalCount = reader.GetSafeInt32(12);

                    if (list == null)
                    {
                        list = new List<Influencer>();
                    }
                    list.Add(influencer);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<Influencer>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Influencer SelectById(int id)
        {
            Influencer influencer = null;

            _dataProvider.ExecuteCmd(
                "dbo.Influencer_SelectById",
                (param) =>
                {
                    param.AddWithValue("@id", id);
                },
                (reader, recordSetIndex) =>
                {
                    influencer = new Influencer();
                    int index = 0;
                    influencer.Id = reader.GetSafeInt32(index++);
                    influencer.UserId = reader.GetSafeInt32(index++);
                    influencer.Bio = reader.GetSafeString(index++);
                    influencer.FaqId = reader.GetSafeInt32(index++);
                    influencer.MilestoneId = reader.GetSafeInt32(index++);
                    influencer.IsActive = reader.GetSafeBool(index++);
                    influencer.FirstName = reader.GetSafeString(index++);
                    influencer.LastName = reader.GetSafeString(index++);
                });


            return influencer;
        }

        public Influencer SelectByUserId(int userId)
        {
            Influencer influencer = null;
            string cachedKey = key + userId.ToString();
            var cachedInfluencer = _cacheService.Get<Influencer>(cachedKey);

            if (cachedInfluencer == null)
            {
                _dataProvider.ExecuteCmd(
                "dbo.InfluencerUser_SelectByUserId",
                (param) =>
                {
                    param.AddWithValue("@UserId", userId);
                },
                (reader, recordSetIndex) =>
                {
                    influencer = new Influencer();
                    int index = 0;
                    influencer.Id = reader.GetSafeInt32(index++);
                    influencer.UserId = reader.GetSafeInt32(index++);
                    influencer.Bio = reader.GetSafeString(index++);
                    influencer.FaqId = reader.GetSafeInt32(index++);
                    influencer.MilestoneId = reader.GetSafeInt32(index++);
                    influencer.IsActive = reader.GetSafeBool(index++);
                    influencer.AvatarUrl = reader.GetSafeString(index++);
                    influencer.FirstName = reader.GetSafeString(index++);
                    influencer.LastName = reader.GetSafeString(index++);

                });
                DateTimeOffset expiration = DateTimeOffset.Now.AddDays(5);
                _cacheService.Add(cachedKey, influencer, expiration);
            }
            else
            {
                influencer = cachedInfluencer;
            }
            return influencer;
        }

        public int Insert(InfluencerAddRequest influencer, int userId)
        {
            int newInfluencerId = 0;
            _dataProvider.ExecuteNonQuery("dbo.Influencer_Insert", (param) =>
            {
                param.AddWithValue("@UserId", userId);
                if (influencer.Bio == null)
                {
                    param.AddWithValue("@Bio", "");
                }
                else
                {
                    param.AddWithValue("@Bio", influencer.Bio);
                }   
                param.AddWithValue("@FaqId", influencer.FaqId);
                param.AddWithValue("@MilestoneId", influencer.MilestoneId);
                param.AddWithValue("@Account", influencer.Account);
                param.AddWithValue("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
            },
                (param) =>
                {
                    Int32.TryParse(param["@Id"].Value.ToString(), out newInfluencerId);
                }
                );
            return newInfluencerId;
        }

        public void Update(InfluencerUpdateRequest influencer, int id)
        {
            _dataProvider.ExecuteNonQuery("dbo.Influencer_Update_v2", (param) =>
            {
                param.AddWithValue("@Id", influencer.Id);
                param.AddWithValue("@UserId", influencer.UserId);
                param.AddWithValue("@Bio", influencer.Bio);
                param.AddWithValue("@FaqId", influencer.FaqId);
                param.AddWithValue("@MilestoneId", influencer.MilestoneId);
                param.AddWithValue("@IsActive", influencer.IsActive);
            });

            string cachedKey = key + influencer.UserId.ToString();
            _cacheService.Remove(cachedKey);
        }

        public void Delete(int userId)
        {
            _dataProvider.ExecuteNonQuery("dbo.Influencer_Delete_v2", (param) =>
            {
                param.AddWithValue("@userId", userId);
            });

            string cachedKey = key + userId.ToString(); 
            _cacheService.Remove(cachedKey);
        }

        public bool ValidateResponse(int userId)
        {
            bool returnValidation = false;

            _dataProvider.ExecuteCmd(
                "dbo.UserSocialMediaAccounts_SelectByUserId",
                (param) =>
                {
                    param.AddWithValue("@UserId", userId);
                },
                (reader, recordSetIndex) =>
                {
                    int index = 0;
                    returnValidation = reader.GetBoolean(index++);
                });
            return returnValidation;
        }

        private static void Mapper(IDataReader reader, Influencer influencer)
        {
            int index = 0;
            influencer.Id = reader.GetSafeInt32(index++);
            influencer.UserId = reader.GetSafeInt32(index++);
            influencer.Bio = reader.GetSafeString(index++);
            influencer.FaqId = reader.GetSafeInt32(index++);
            influencer.MilestoneId = reader.GetSafeInt32(index++);
            influencer.IsActive = reader.GetSafeBool(index++);
        }

        private Influencer PageMapper(IDataReader reader)
        {
            int index = 0;
            Influencer influencer = new Influencer();
            influencer.Id = reader.GetSafeInt32(index++);
            influencer.UserId = reader.GetSafeInt32(index++);
            influencer.Bio = reader.GetSafeString(index++);
            influencer.FaqId = reader.GetSafeInt32(index++);
            influencer.MilestoneId = reader.GetSafeInt32(index++);
            influencer.IsActive = reader.GetSafeBool(index++);
            influencer.InstagramUsername = reader.GetSafeString(index++);
            influencer.InstagramFollowers = reader.GetSafeInt32(index++);
            influencer.InstagramBio = reader.GetSafeString(index++);
            influencer.InstagramAvatar = reader.GetSafeString(index++);
            influencer.FirstName = reader.GetSafeString(index++);
            influencer.LastName = reader.GetSafeString(index++);

            return influencer;
        }

    }
}