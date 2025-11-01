using Mapster;
using Yandex.Cinema.Events.Messages;
using Yandex.Cinema.Events.Requests;

namespace Yandex.Cinema.Events;

public class MapsterConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<MovieRequest, MovieMessageDto>()
            .RequireDestinationMemberSource(true);

        config.NewConfig<PaymentRequest, PaymentMessageDto>()
            .RequireDestinationMemberSource(true);

        config.NewConfig<UserRequest, UserMessageDto>()
            .RequireDestinationMemberSource(true);
    }
}