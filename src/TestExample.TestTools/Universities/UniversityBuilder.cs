using TestExample.Entities.Universities;

namespace TestExample.TestTools.Universities
{
    public class UniversityBuilder
    {
        private readonly University _university;

        public UniversityBuilder()
        {
            _university = new University(
                "dummy_name", "dummy_address", "dummy_email");
        }

        public UniversityBuilder WithName(string name)
        {
            _university.Name = name;
            return this;
        }

        public University Build()
        {
            return _university;
        }
    }
}