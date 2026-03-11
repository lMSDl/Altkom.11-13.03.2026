using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IPeopleService : IGenericService<Person>
    {
        Task<IEnumerable<Person>> GetByNameAsync(string name);
    }
}
