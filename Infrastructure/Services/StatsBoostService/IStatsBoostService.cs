using Units;

namespace Infrastructure.Services.StatsBoostService
{
    public interface IStatsBoostService
    {
        void SetUpdateLevel(int level, ClassParent id);
        void UpdateStats(ClassParent id);
        void UpdateStatsAll();
        void ResetStats(ClassParent id);
        bool GeneralLevelIsMax { get; }
        bool LevelIsMax(ClassParent id);
    }
}