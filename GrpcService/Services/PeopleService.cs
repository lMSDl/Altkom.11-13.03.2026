using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcService.Protos.PeopleProto;
using Services.Interfaces;
using Void = GrpcService.Protos.PeopleProto.Void;

namespace GrpcService.Services
{
    public class PeopleService : GrpcService.Protos.PeopleProto.PeopleGrpcService.PeopleGrpcServiceBase
    {

        private readonly IPeopleService _peopleService;

        public PeopleService(IPeopleService peopleService)
        {
            _peopleService = peopleService;
        }

        public override async Task<Id> Create(Person request, ServerCallContext context)
        {

            var person = new Models.Person
            {
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var id = await _peopleService.CreateAsync(person);

            return new Id { Id_ = id };
        }

        public override async Task<People> Read(Void request, ServerCallContext context)
        {
            var people = await _peopleService.ReadAsync();
            var response = new People();
            /*foreach (var person in people)
            {
                response.Collection.Add(new Person { Id = person.Id , FirstName = person.FirstName, LastName = person.LastName });
            }*/

            response.Collection.AddRange(people.Select(MapPerson));

            return response;
        }

        public override async Task<OptionalPerson> ReadById(Id request, ServerCallContext context)
        {
            var person = await _peopleService.ReadAsync(request.Id_);
            if (person is null)
                return new OptionalPerson { Empty = new Protos.PeopleProto.Void() };
            return new OptionalPerson { Person = MapPerson(person) };
        }

        private Person MapPerson(Models.Person person)
        {
            return new Person { Id = person.Id, FirstName = person.FirstName, LastName = person.LastName };
        }

        public override async Task<Empty> Update(Person request, ServerCallContext context)
        {
            var person = await _peopleService.ReadAsync(request.Id);

            if (person is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Person with Id={request.Id} not found"));
            }

            person.FirstName = request.FirstName;
            person.LastName = request.LastName;
            await _peopleService.UpdateAsync(request.Id, person);
            return new Empty();
        }

        public override async Task<Void> Delete(Id request, ServerCallContext context)
        {
            if (await _peopleService.ReadAsync(request.Id_) is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Person with Id={request.Id_} not found"));
            }

            await _peopleService.DeleteAsync(request.Id_);
            return new Void();
        }
    }
}
