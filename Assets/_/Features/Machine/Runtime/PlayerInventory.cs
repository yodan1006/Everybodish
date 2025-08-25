namespace Machine.Runtime
{
    public class PlayerInventory
    {
        public Food currentFood;

        public bool HasFood => currentFood != null;

        public Food TakeFood()
        {
            Food food = currentFood;
            currentFood = null;
            return food;
        }

        public void GiveFood(Food food)
        {
            currentFood = food;
        }
    }
}