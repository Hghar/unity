using Infrastructure.Shared.Factories;
using Model.Economy.Numerics;
using UnityEngine;

namespace Model.Economy.Resources
{
    public class ResourceFactory : IFactory<Resource, Currency>
    {
        public Resource Create(Currency args)
        {
            int value = 0;
            /*if (PlayerPrefs.GetInt("level") != 0)
                value = PlayerPrefs.GetInt(args.ToString(), 300);
            else
                value = PlayerPrefs.GetInt(args.ToString());*/

            NaturalNumber number = new NaturalNumber(value);
            return new Resource(number, args);
        }
    }
}