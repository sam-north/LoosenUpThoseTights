namespace LoosenItUp.Factories
{
    public class ObjectFactory
    {
        public static T Create<T>() where T : new()
        {
            return new T();
        }
    }
}
