namespace Model.Maps
{
    public class UnitCounter
    {
        private int _amount;
        public int Amount => _amount;

        public void Add()
        {
            _amount++;
        }

        public void Remove()
        {
            _amount--;
        }
    }
}