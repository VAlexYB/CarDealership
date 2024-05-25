using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;

namespace CarDealership.Web.Api.Factories
{
    public class TransmissionTypeRMFactory : IModelBuilder<TransmissionTypeRequest, TransmissionType>, IResponseBuilder<TransmissionTypeResponse, TransmissionType>
    {
        public TransmissionType CreateModel(TransmissionTypeRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));
            
            var transmissionTypeCreateResult = TransmissionType.Create(req.Id, req.Value, req.IsDeleted);
            if(transmissionTypeCreateResult.IsFailure)
            {
                throw new InvalidOperationException(transmissionTypeCreateResult.Error);
            }

            return transmissionTypeCreateResult.Value;
        }

        public TransmissionTypeResponse CreateResponse(TransmissionType model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            var response = new TransmissionTypeResponse(model.Id)
            {
                Value = model.Value
            };

            return response;
        }
    }
}
