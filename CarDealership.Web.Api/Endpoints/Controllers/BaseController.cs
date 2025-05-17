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

        protected readonly IReqResModelFactory<Req, Res, M> _factory;

        protected readonly ILogger _logger;

        private readonly bool _useAsyncBuilder;

        public BaseController(
            IGenericService<M, F> service,
            IReqResModelFactory<Req, Res, M> factory,
            ILogger logger
        )
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _logger = logger;
        }

        [Route("getAll")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var models = await _service.GetAllAsync();

            IEnumerable<Task<Res>> tasks = models.Select(model => _factory.CreateResponse(model));
            var response = (await Task.WhenAll(tasks)).ToList();
            return Ok(response);
        }

        [Route("getByFilter")]
        [HttpPost]
        public async Task<IActionResult> GetByFilterAsync(F filter)
        {
            var models = await _service.GetFilteredAsync(filter);

            IEnumerable<Task<Res>> tasks = models.Select(model => _factory.CreateResponse(model));
            var response = (await Task.WhenAll(tasks)).ToList();
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
            var response = await _factory.CreateResponse(model);
            return Ok(response);
        }

        [Route("add")]
        [HttpPost]
        public virtual async Task<IActionResult> CreateOrEditAsync([FromBody] Req req)
        {

            M model = await _factory.CreateModel(req);
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
