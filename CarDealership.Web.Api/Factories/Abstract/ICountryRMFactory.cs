using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;

namespace CarDealership.Web.Api.Factories.Abstract
{
    public interface ICountryRMFactory : IReqResModelFactory<CountryRequest, CountryResponse, Country>
    {
    }
}
