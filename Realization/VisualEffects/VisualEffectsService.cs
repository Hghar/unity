using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ModestTree.Util;
using Units;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Realization.VisualEffects
{
    public class VisualEffectsService : IVisualEffectService, IDisposable
    {
        private const string PATH = "VisualEffects";

        private readonly Dictionary<VisualEffectType, string> Prefabs = new()
        {
            {VisualEffectType.Damage, "SwordHitMiniYellow"},
            {VisualEffectType.CriticalDamage, "SwordHitMagicRed"},
            {VisualEffectType.SplashDamage, "SwordSlashThickYellow"},
            {VisualEffectType.Heal, "HealOnceBurst"},
            {VisualEffectType.Healing, "HealStream2"},
            {VisualEffectType.SplashHeal, "HealNova"},
            {VisualEffectType.AggressionUp, "PowerupActivateRed"},
            {VisualEffectType.AggressionDown, "PowerupActivateBlue"},
            {VisualEffectType.Lifesteal, "SwordHitMiniYellow"},
            {VisualEffectType.SplashLifesteal, "SwordSlashThickYellow"},
            {VisualEffectType.Stun, "StunExplosion2"},
            {VisualEffectType.Stunning, "StunnedCirclingStars"},
            {VisualEffectType.Silence, "HitDustExplosion"},
            {VisualEffectType.Silencing, "WaterBoiling"},
            {VisualEffectType.Shield, "SimplePortalGold"},
            {VisualEffectType.Immortality, "MagicChargeYellow"},
            {VisualEffectType.Reflection, "VortexPortalBlue"},
            {VisualEffectType.Buff, "MagicBuffYellow"},
            {VisualEffectType.Debuff, "MagicBuffYellow1"},
            {VisualEffectType.Summon, "MagicCircleSimpleYellow"},
            {VisualEffectType.Death, "PowerboxPickupSkull"},
            {VisualEffectType.Evade, "WindlinesSpeedy"},
            {VisualEffectType.Energy, "PowerboxPickupLightning1"},
            {VisualEffectType.EnergyDown, "PowerboxPickupLightning"},
        };

        private DiContainer _container;
        private Dictionary<IMinion, Dictionary<VisualEffectType, int>> _buffCounts = new();
        private Dictionary<IMinion, Dictionary<VisualEffectType, VisualEffect>>  _buffs = new();
        private List<VisualEffect> _allEffects = new();
        private bool _disposing;

        public VisualEffectsService(DiContainer container)
        {
            _container = container;
        }

        public void Create(VisualEffectType type, IMinion target)
        {
            if (_disposing)
                return;
            VisualEffect effect;
            effect = CreateEffectGameObject(Prefabs[type], target);
        }

        public void CreateAOE(VisualEffectType type, IMinion target, int radius)
        {
            if (_disposing)
                return;
            
            VisualEffect effect;
            effect = CreateEffectGameObject(Prefabs[type], target);
            if(effect == null)
                return;
            effect.Scale(radius);
            if (target.Flipper.Flipped)
            {
                effect.Invert();
            }
        }

        public void CreateEffectWithDuration(VisualEffectType type, IMinion target)
        {
            if (_disposing)
                return;
            
            _buffCounts.TryAdd(target, new Dictionary<VisualEffectType, int>());
            if(_buffCounts[target].ContainsKey(type) == false)
            {
                _buffCounts[target].Add(type, 0);
            }
            _buffCounts[target][type] += 1;
            if (_buffCounts[target][type] == 1)
            {
                var showingEffect = ShowingEffect(type, target);
                if (showingEffect == null)
                    return;
                
                if(_buffs.ContainsKey(target) == false)
                    _buffs.Add(target, new Dictionary<VisualEffectType, VisualEffect>());
                
                _buffs[target].TryAdd(type, showingEffect);
            }
        }

        public void EndEffectWithDuration(VisualEffectType type, IMinion target)
        {
            if(_buffCounts.ContainsKey(target) == false || 
               _buffCounts[target].ContainsKey(type) == false || 
               _disposing ||
               _buffs[target].ContainsKey(type) == false)
                return;
            
            _buffCounts[target][type] -= 1;
            if (_buffCounts[target][type] <= 0)
            {
                _allEffects.Remove(_buffs[target][type]);
                _buffs[target][type].DestroyEffect();
                _buffs[target].Remove(type);
                _buffCounts[target][type] = 0;
            }
        }

        public void EndEffectWithDurationDirty(VisualEffectType type, IMinion target)
        {
            if(_buffCounts.ContainsKey(target) == false || 
               _buffCounts[target].ContainsKey(type) == false || 
               _disposing ||
               _buffs[target].ContainsKey(type) == false)
                return;
            
            _buffCounts[target][type] = 0;
            if (_buffCounts[target][type] <= 0)
            {
                _allEffects.Remove(_buffs[target][type]);
                _buffs[target][type].DestroyEffect();
                _buffs[target].Remove(type);
            }
        }

        private VisualEffect CreateEffectGameObject(string prefabName, IMinion target)
        {
            if (target.GameObject == null)
                return null;
            var prefab = _container.InstantiatePrefabResourceForComponent<VisualEffect>($"{PATH}/{prefabName}");
            prefab?.Place(target.WorldPosition+(Vector2)prefab.transform.position);
            prefab?.SetParent(target);
            return prefab;
        }

        private VisualEffect ShowingEffect(VisualEffectType type, IMinion target)
        {
            VisualEffect effect;
            effect = CreateEffectGameObject(Prefabs[type], target);
            if (effect == null)
                return null;
            effect.DontDestroy();
            _allEffects.Add(effect);
            return effect;
        }

        public void Dispose()
        {
            _disposing = true;
            foreach (var visualEffect in _allEffects)
            {
                if (visualEffect != null)
                    Object.Destroy(visualEffect);
            }
            _allEffects.Clear();

            foreach (var buff in _buffs)
            {
                foreach (var effect in buff.Value)
                {
                    if(effect.Value != null)
                        Object.Destroy(effect.Value);
                }
                buff.Value.Clear();
            }
            _buffs.Clear();
        }
    }
}