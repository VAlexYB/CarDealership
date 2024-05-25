using CarDealership.Application.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Web.Api.Controllers
{
    //TODO: добавить ActionResult, обработку ошибок
    [ApiController]
    public abstract class BaseController <M, F, Req, Res> : ControllerBase
        where M: BaseModel
        where F: BaseFilter
        where Req: BaseRequest
        where Res: BaseResponse
    {
        protected readonly BaseService<M, F> _service;

        protected readonly IResponseBuilder<Res, M> _factory;
        protected readonly IModelBuilder<Req, M> _modelBuilder;
        protected readonly IModelBuilderAsync<Req, M> _modelBuilderAsync;
        private readonly bool _useAsyncBuilder;

        public BaseController(
            BaseService<M, F> service,
            IResponseBuilder<Res, M> factory
        )
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));

            _modelBuilder = factory as IModelBuilder<Req, M>;
            _modelBuilderAsync = factory as IModelBuilderAsync<Req, M>;

            _useAsyncBuilder = _modelBuilderAsync != null;
        }   
        
        
        [Route("getAll")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var models = await _service.GetAllAsync();
                var response = models.Select(model => _factory.CreateResponse(model)).ToList();
                return Ok(response);
            }
            catch (Exception)
            {

                return StatusCode(500, "Ошибки случаются");
            }
           
        }

        [Route("getByFilter")]
        [HttpPost]
        public async Task<IActionResult> GetByFilterAsync(F filter)
        {
            try
            {
                var models = await _service.GetFilteredAsync(filter);
                var response = models.Select(model => _factory.CreateResponse(model)).ToList();
                return Ok(response);
            }
            catch(Exception)
            {
                return StatusCode(500, "Ошибки случаются");
            }
        }

        [Route("getById/{itemId}")]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid itemId)
        {
            try
            {
                var model = await _service.GetByIdAsync(itemId);
                if (model == null)
                {
                    return NotFound();
                }
                var response = _factory.CreateResponse(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ошибки случаются");
            }
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(Req req)
        {
            try
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
                await _service.CreateOrEditAsync(model);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Ошибки случаются");
            }
        }


        [Route("deleteById/{itemId}")]
        [HttpGet]
        public async Task<IActionResult> DeleteById(Guid itemId)
        {
            try
            {
                await _service.DeleteAsync(itemId);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Ошибки случаются");
            }
        }
    }
}
