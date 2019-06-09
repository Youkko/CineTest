namespace CineTest.Entities
{
    public class Message<T> where T : IObject
    {
        public Itens<T> itens { get; set; }
    }
    
}
