using System.Threading.Tasks;

namespace FoliownServices
{
    public interface IEnquiryService
    {
        Task<string> GetVrmDetails(string vrm, string manufacturer);
        Task<VesVehicleDetails> ParseResponse(string response);
    }


}