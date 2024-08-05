namespace Bimil.Engine.Models
{
    public class FixtureUserData
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public object AdditionalData { get; set; }

        public FixtureUserData(string name = "", string tag = "", object additionalData = null)
        {
            Name = name;
            Tag = tag;
            AdditionalData = additionalData;
        }
    }
}