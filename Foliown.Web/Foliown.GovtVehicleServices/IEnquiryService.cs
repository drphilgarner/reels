using System.Threading.Tasks;

namespace Foliown.GovtVehicleServices
{
    public interface IEnquiryService
    {
        Task<string> GetVrmDetails(string vrm, string manufacturer);
        Task<VesVehicleDetails> ParseResponse(string response);
    }


}