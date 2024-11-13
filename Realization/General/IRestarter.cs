using System;

namespace Realization.General
{
    public interface IRestarter
    {
        public event Action Restarting;
    }
}