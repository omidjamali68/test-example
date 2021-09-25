namespace Cooking.Infrastructure.Sms.Contracts
{
    public interface IBaseSmsService
    {
        string SendParameterizedMessage(ParameterizedSmsMessageDto dto);
    }
}