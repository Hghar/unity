namespace Infrastructure.Services.WindowService.MVVM
{
    public class LevelingItemViewModel : ViewModel<ItemCreationArgs>
    {
        private readonly int id;
        private readonly bool _isBlack;
        private readonly Alignment _alligment;
        public int Id => id;
        public Alignment Alignment => _alligment;
        public bool IsBlack => _isBlack;

        public LevelingItemViewModel(ItemCreationArgs model) : base(model)
        {
            id = model.Id;
            _isBlack = model.IsBlack;
            _alligment = model.Alligment;
        }
    }
}
