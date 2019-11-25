namespace Resume.Data.Json
{
    public class Resume
    {
        public Basics Basics { get; set; }
        public Work[] Work { get; set; }
        public Education[] Education { get; set; }
        public Language[] Languages { get; set; }
        public Interest[] Interests { get; set; }
        public Skill[] Skills { get; set; }
    }
}
