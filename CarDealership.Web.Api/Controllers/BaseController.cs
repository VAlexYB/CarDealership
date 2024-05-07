using CarDealership.Application.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Web.Api.Controllers
{
    [ApiController]
    public class BaseController <M, F, Req, Res> : ControllerBase
        where M: BaseModel
        where F: BaseFilter
        where Req: BaseRequest
        where Res: BaseResponse
    {
        protected readonly BaseService<M, F> _service;

        protected readonly IReqResModelFactory<Req, Res, M> _factory;
        public BaseController(BaseService<M, F> service, IReqResModelFactory<Req, Res, M> factory)
        {
            _service = service;
            _factory = factory;
        }
        
        
        [Route("getAll")]
        [HttpGet]
        public async Task<List<Res>> GetAllAsync()
        {
            var models = await _service.GetAllAsync();
            return models.Select(model => _factory.CreateResponse(model)).ToList();
        }

        [Route("getByFilter")]
        [HttpPost]
        public async Task<List<Res>> GetByFilterAsync(F filter)
        {
            var models = await _service.GetFilteredAsync(filter);
            return models.Select(model => _factory.CreateResponse(model)).ToList();
        }

        [Route("getById/{itemId}")]
        [HttpGet]
        public async Task<Res> GetById(Guid itemId)
        {
            var model = await _service.GetByIdAsync(itemId);
            return _factory.CreateResponse(model);
        }

        [Route("add")]
        [HttpPost]
        public async Task CreateOrEdit(Req req)
        {
            var model = _factory.CreateModel(req);
            await _service.CreateOrEditAsync(model);
        }


        [Route("deleteById/{itemId}")]
        [HttpGet]
        public async Task DeleteById(Guid itemId)
        {
            await _service.DeleteAsync(itemId);
        }

    }
}
