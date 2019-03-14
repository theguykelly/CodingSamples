using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Kellys.Models;
using Kellys.Models.Domain;
using Kellys.Models.Requests;
using Kellys.Services;
using Kellys.Web.Controllers;
using Kellys.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Kellys.Web.Api.Controllers
{
    [Route("api/influencers/")]
    [ApiController]
    public class InfluencerController : BaseApiController
    {
        private readonly IInfluencerService _influencerService;
        private IAuthenticationService<int> _authService;

        public InfluencerController(
            ILogger<InfluencerController> logger,
            IInfluencerService influencerService,
            IAuthenticationService<int> authService) : base(logger)
        {
            _influencerService = influencerService;
            _authService = authService;
        }

        [HttpGet]
        public ActionResult<ItemsResponse<Influencer>> GetAll()
        {
            ActionResult result = null;
            try
            {
                List<Influencer> list = _influencerService.SelectAll();

                if (list == null)
                {
                    result = NotFound404(new ErrorResponse("There are no influencers."));
                }
                else
                {
                    ItemsResponse<Influencer> response = new ItemsResponse<Influencer>();
                    response.Items = list;
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, "Internal Server Error");
            }
            return result;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Influencer>> SelectById(int id)
        {
            ItemResponse<Influencer> response = null;
            ActionResult result = null;

            try
            {
                Influencer influencer = _influencerService.SelectById(id);
                if (influencer == null)
                {
                    result = NotFound404(new ErrorResponse("This influencer does not exist"));
                }
                else
                {
                    response = new ItemResponse<Influencer>();
                    response.Item = influencer;

                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
            }
            return result;
        }

        [HttpGet("userId/{userId:int}")]
        public ActionResult<ItemResponse<Influencer>> SelectByUserId(int userId)
        {
            ItemResponse<Influencer> response = null;
            ActionResult result = null;

            try
            {
                Influencer influencer = _influencerService.SelectByUserId(userId);
                if (influencer == null)
                {
                    result = NotFound404(new ErrorResponse("This user is not an Influencer."));
                }
                else
                {
                    response = new ItemResponse<Influencer>();
                    response.Item = influencer;
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, "Internal Server Error");
            }
            return result;
        }

        [HttpGet("userId2/{userId:int}")]
        public ActionResult<ItemResponse<Influencer>> SelectByLoggedInUserId(int userId)
        {
            ItemResponse<Influencer> response = null;
            ActionResult result = null;
            int currentUserId = _authService.GetCurrentUserId();

            try
            {
                Influencer influencer = _influencerService.SelectByUserId(currentUserId);
                if (influencer == null)
                {
                    result = NotFound404(new ErrorResponse("This user is not an Influencer."));
                }
                else
                {
                    response = new ItemResponse<Influencer>();
                    response.Item = influencer;
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, "Internal Server Error");
            }
            return result;
        }

        [HttpPost]
        public ActionResult<ItemResponse<Influencer>> Insert(InfluencerAddRequest influencer)
        {
            ItemResponse<int> response = null;
            ActionResult result = null;
            int currentUserId = _authService.GetCurrentUserId();

            try
            {
                int newId = _influencerService.Insert(influencer, currentUserId);

                if (newId > 0)
                {
                    response = new ItemResponse<int>();
                    response.Item = newId;
                    result = Created201(response);
                }
                else
                {
                    result = NotFound404(new ErrorResponse("You must be logged in to use this feature."));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, "Internal Server Error");
            }
            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Put(InfluencerUpdateRequest influencer, int id)
        {
            ActionResult result = null;
            int currentUserId = _authService.GetCurrentUserId();

            try
            {
                if (influencer.UserId == currentUserId && influencer.Id == id)
                {
                    _influencerService.Update(influencer, id);
                    SuccessResponse response = new SuccessResponse();
                    result = Ok200(response);
                }
                else
                {
                    result = NotFound404(new ErrorResponse("URL Id does not match body id."));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, "Internal Server Error");
            }
            return result;
        }

        [HttpDelete]
        public ActionResult<SuccessResponse> Delete(int userId)
        {
            ItemResponse<int> response = null;
            ActionResult result = null;
            int currentUserId = _authService.GetCurrentUserId();

            try
            {
                _influencerService.Delete(currentUserId);
                if (currentUserId == 0)
                {
                    result = NotFound404(new ErrorResponse("This influencer does not exist"));
                }
                else
                {
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, "Internal Server Error");
            }
            return result;
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Influencer>>> Pagination(int pageIndex, int pageSize)
        {
            ActionResult result = null;
            try
            {
                Paged<Influencer> list = _influencerService.Pagination(pageIndex, pageSize);
                if (list.PagedItems == null)
                {
                    result = NotFound404(new ErrorResponse("There are no influencers."));
                }
                else
                {
                    ItemResponse<Paged<Influencer>> response = new ItemResponse<Paged<Influencer>>();
                    response.Item = list;
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
            }
            return result;
        }

        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<Influencer>>> SearchPaginated(string search, int pageIndex, int pageSize)
        {
            ActionResult result = null;
            try
            {
                Paged<Influencer> list = _influencerService.SearchPaginated(search, pageIndex, pageSize);
                if (list.PagedItems == null)
                {
                    result = NotFound404(new ErrorResponse("Nothing was Found"));
                }
                else
                {
                    ItemResponse<Paged<Influencer>> response = new ItemResponse<Paged<Influencer>>();
                    response.Item = list;
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
            }
            return result;
        }
    }
}