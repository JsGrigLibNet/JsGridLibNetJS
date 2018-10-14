namespace SampleHttpsServer
{
    public class MyData: IJsGridEntity
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public Country Country { get; set; }

        public bool Married { get; set; }

        public string Id { get; set; }
    }
}