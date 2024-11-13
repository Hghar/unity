using System;
using System.Collections.Generic;
using System.Linq;
using Parameters;
using Plugins.Ship.Sheets.InfoSheet;
using UnityEngine;

namespace Realization.States.CharacterSheet
{
    public class DungeonsConfigInfoBuilder : InfoBuilder<ConfigBiom>
    {
        private string _uid;
        private string _biomName;
        private int _stageNumber;
        private int coin_Quantity_Modifier;
        private string _prefabName;
        private string _fractionEnemyTags;

        private Room[] _rooms = new[]
        {
                new Room(), //1
                new Room(), //2
                new Room(), //3
                new Room(), //4
                new Room(), //5
                new Room(), //6
                new Room(), //7
                new Room(), //8
                new Room(), //9
                new Room(), //10
        };

        protected override void SetQueue(Queue<Action<string>> queue)
        {
            queue.Enqueue((s =>
            {
                string[] words = s.Split('_');
                _biomName = words[0];
                _prefabName = words[1];
                //int.TryParse(words[2], out _stageNumber);
                string output = string.Concat( words[2].Where( Char.IsDigit ) );
                int.TryParse(output, out _stageNumber);
                _uid = s;
                
            }));
            queue.Enqueue((s => { }));
            queue.Enqueue((s => { }));
            queue.Enqueue((s => { _fractionEnemyTags = s; }));
            queue.Enqueue((s =>
            {
                int.TryParse(s, out coin_Quantity_Modifier);
            }));


            for (var i = 0; i < _rooms.Length; i++)
            {
                int index = i;
                queue.Enqueue((s => int.TryParse(s, out _rooms[index].Lvl_1)));
                queue.Enqueue((s => int.TryParse(s, out _rooms[index].Lvl_2)));
                queue.Enqueue((s => int.TryParse(s, out _rooms[index].Lvl_3)));
                queue.Enqueue((s => int.TryParse(s, out _rooms[index].Lvl_4)));
                queue.Enqueue((s => int.TryParse(s, out _rooms[index].Lvl_5)));
                queue.Enqueue((s => int.TryParse(s, out _rooms[index].Might)));
                queue.Enqueue((s => int.TryParse(s, out _rooms[index].Coins)));
            }
        }


        protected override IInfo<ConfigBiom> GetInternal()
        {
            var config = new ConfigBiom();
            config.Uid = _uid;
            config.BiomName = _biomName;
            config.PrefabName = _prefabName;
            config.StageNumber = _stageNumber;
            config.FractionEnemyTags = _fractionEnemyTags;
            config.Coin_Quantity_Modifier = coin_Quantity_Modifier;
            foreach (var room in _rooms.Where(RoomIsNotEmpty)) 
                config._rooms.Add(room);

            return config;
        }

        private bool RoomIsNotEmpty(Room room) =>
                room.Coins + room.Lvl_1 + room.Lvl_2 + room.Lvl_3 + room.Lvl_4 + room.Lvl_5 + room.Might != 0;
    }
}