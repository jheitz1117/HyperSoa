using HyperSoa.Contracts;
using HyperSoa.Service.CommandModules;
using HyperSoa.Service.Serialization;
using HostingTest.Contracts;
using HostingTest.Modules.Serialization;

namespace HostingTest.Modules.CommandModules
{
    internal class ComplexCommandModule: ICommandModule, IServiceContractSerializerFactory
    {
        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            context.Activity.Track("In Complex command module...");

            if (context.Request is not ComplexCommandRequest request)
            {
                context.Activity.Track("Request was null. Throw tragic exception!");
                throw new InvalidOperationException("Request was null. Invalid deserialization!");
            }
            
            context.Activity.Track("Request was not null. Proceeding as planned.");
            context.Activity.Track($"{nameof(request.MyDateTime)} = '{request.MyDateTime}'");
            context.Activity.Track($"{nameof(request.MyInt32)} = '{request.MyInt32}'");
            context.Activity.Track($"{nameof(request.MyTimeSpan)} = '{request.MyTimeSpan}'");
            context.Activity.Track($"{nameof(request.MyDateTime)} = '{request.MyDateTime}'");

            return new ComplexCommandResponse
            {
                EightDaysLaterThanRequest = request.MyDateTime.AddDays(8),
                FiveHundredLessThanRequest = request.MyInt32 - 500,
                ProcessStatusFlags = MessageProcessStatusFlags.Success,
                FortyYearTimeSpan = (new DateTime(2040,1,1) - new DateTime(2000, 1,1)),
                ResponseStringNotTheSame = request.MyString + " not the same!!!"
            };
        }

        public IServiceContractSerializer Create()
        {
            return new DataContractJsonSerializer<ComplexCommandRequest, ComplexCommandResponse>();
        }
    }
}
