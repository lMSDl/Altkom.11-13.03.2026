namespace Services.Bogus.Fakers
{
    public class PersonFaker : EntityFaker<Models.Person>
    {
        public PersonFaker(string language) : base(language)
        {
            RuleFor(p => p.FirstName, f => f.Name.FirstName());
            RuleFor(p => p.LastName, f => f.Name.LastName());
        }
        public PersonFaker(BogusConfig config) : this(config.Language)
        {
            

        }
    }
}
