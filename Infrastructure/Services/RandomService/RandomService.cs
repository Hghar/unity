using UnityEngine;

namespace Infrastructure.Services.RandomService
{
    public class RandomService:IRandomService
    {
        public int Next(int minValue, int maxValue) => Random.Range(minValue, maxValue);
        public float Next(float minValue, float maxValue)=> Random.Range(minValue, maxValue);
    }
}