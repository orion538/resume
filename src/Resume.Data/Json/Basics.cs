namespace Resume.Data.Json
{
    public class Basics
    {
        public string Name { get; set; }
        public Location Location { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
        public Profile[] Profiles { get; set; }
        public string Summary { get; set; }
    }
}