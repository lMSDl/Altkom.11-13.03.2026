namespace Services.Bogus.Fakers
{
    public class PersonFaker : EntityFaker<Models.Person>
    {
        public PersonFaker() : base()
        {
            RuleFor(p => p.FirstName, f => f.Name.FirstName());
            RuleFor(p => p.LastName, f => f.Name.LastName());

        }
    }
}
