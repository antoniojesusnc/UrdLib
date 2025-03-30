namespace RubberDuck.Editor
{
    public class GoogleSheetCollection
    {
        public string Collection { get; private set; }
        public string Subcollection { get; private set; }
        public string Carpeta { get; private set; }
        public string Stars { get; private set; }
        public string Name { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        public GoogleSheetCollection(string collection, string subcollection, string carpeta, string stars, string name,
            string title, string description)
        {
            Collection = collection;
            Subcollection = subcollection;
            Carpeta = carpeta;
            Stars = stars;
            Name = name;
            Title = title;
            Description = description;
        }
    }
}
