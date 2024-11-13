using Parameters;
using Units;

namespace Infrastructure.Services.CharacteristicSetupService
{
    public interface ICharacteristicSetupService
    {
        void SetupCharacteristic(IUnitParameters parameters,ClassParent id);
        void SetupGeneralCharacteristic(IUnitParameters parameters);
    }
}