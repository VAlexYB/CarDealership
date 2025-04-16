using System.Diagnostics;
using CarDealership.Application.Services;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Web.Api.Endpoints.Controllers
{
    //TODO: добавить ActionResult, обработку ошибок
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<M, F, Req, Res> : ControllerBase
        where M : BaseModel
        where F : BaseFilter
        where Req : BaseRequest
        where Res : BaseResponse
    {
        protected readonly IGenericService<M, F> _service;

        protected readonly IResponseBuilder<Res, M> _factory;
        protected readonly IModelBuilder<Req, M> _modelBuilder;
        protected readonly IModelBuilderAsync<Req, M> _modelBuilderAsync;

        protected readonly ILogger _logger;

        private readonly bool _useAsyncBuilder;

        public BaseController(
            IGenericService<M, F> service,
            IResponseBuilder<Res, M> factory,
            ILogger logger
        )
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));

            _modelBuilder = factory as IModelBuilder<Req, M>;
            _modelBuilderAsync = factory as IModelBuilderAsync<Req, M>;

            _logger = logger;
            _useAsyncBuilder = _modelBuilderAsync != null;
        }

        [Route("getAll")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var models = await _service.GetAllAsync();
            var response = models.Select(model => _factory.CreateResponse(model)).ToList();
            return Ok(response);
        }

        [Route("getByFilter")]
        [HttpPost]
        public async Task<IActionResult> GetByFilterAsync(F filter)
        {
            var models = await _service.GetFilteredAsync(filter);
            var response = models.Select(model => _factory.CreateResponse(model)).ToList();
            return Ok(response);
        }

        [Route("getById/{itemId}")]
        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(Guid itemId)
        {
            var model = await _service.GetByIdAsync(itemId);
            if (model == null)
            {
                return NotFound();
            }
            var response = _factory.CreateResponse(model);
            return Ok(response);
        }

        [Route("add")]
        [HttpPost]
        public virtual async Task<IActionResult> CreateOrEditAsync([FromBody] Req req)
        {
            M model;
            if (_useAsyncBuilder)
            {
                model = await _modelBuilderAsync.CreateModelAsync(req);
            }
            else
            {
                model = _modelBuilder.CreateModel(req);
            }
            Guid modelId = await _service.CreateOrEditAsync(model);
            return Ok(modelId);
        }


        [Route("deleteById/{itemId}")]
        [HttpGet]
        public async Task<IActionResult> DeleteByIdAsync(Guid itemId)
        {
            Guid deletedId = await _service.DeleteAsync(itemId);
            return Ok(deletedId);
        }
    }
}
