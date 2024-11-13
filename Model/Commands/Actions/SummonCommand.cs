using System.Collections.Generic;
using System.Linq;
using Fight.Damaging;
using Model.Commands.Creation;
using Realization.States.CharacterSheet;
using Realization.VisualEffects;
using Units;

namespace Model.Commands.Actions
{
    public class SummonCommand : IMinionCommand
    {
        private string _uid;
        private MinionFactory _factory;
        private Character[] _characters;
        private List<IMinion> _minions = new();
        private int _count;
        private IVisualEffectService _visualEffectsService;

        public SummonCommand(string uid, int count, MinionFactory factory, Character[] characters,
            IVisualEffectService visualEffectsService)
        {
            _visualEffectsService = visualEffectsService;
            _count = count;
            _characters = characters;
            _factory = factory;
            _uid = uid;
        }

        public void Perform(IMinion _)
        {
            for (int i = 0; i < _count; i++)
            {
                if(_factory.CanCreate() == false)
                    return;
                
                var minionClass = _characters.First((character => character.Uid == _uid));
                var minion = _factory.CreateAndPlace(minionClass, true);
                minion.Fight();
                _minions.Add(minion);
                _visualEffectsService.Create(VisualEffectType.Summon, minion);
            }
        }

        public void Undo(IMinion _)
        {
            foreach (var minion in _minions)
            {
                minion.Unimmortal();
                minion.Damage(new Damage(float.MaxValue));
            }
            _minions.Clear();
        }
    }
}