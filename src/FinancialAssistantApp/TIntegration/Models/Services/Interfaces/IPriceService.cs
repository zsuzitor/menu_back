using TIntegration.Models.DTO;

namespace TIntegration.Models.Services.Interfaces
{
    public interface IPriceService
    {
        Task<PriceResponseDto> GetPrice(PriceRequestDto ticker);
        Task<List<PriceResponseDto>> GetPrice(List<PriceRequestDto> ticker);
    }
}
