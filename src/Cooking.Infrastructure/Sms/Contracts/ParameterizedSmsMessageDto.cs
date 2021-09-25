using System;
using System.Collections.Generic;
using Cooking.Entities.ApplicationIdentities;

namespace Cooking.Infrastructure.Sms.Contracts
{
    public class ParameterizedSmsMessageDto
    {
        public List<string> Receivers { get; } = new List<string>();

        public Dictionary<string, object> Parameters { get; } =
            new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

        public SMSMessageTemplate Template { get; private set; }

        public ParameterizedSmsMessageDto AddReceiver(params string[] phones)
        {
            Receivers.AddRange(phones);
            return this;
        }

        public ParameterizedSmsMessageDto SetParameter(string name, object value)
        {
            Parameters[name] = value;
            return this;
        }

        public ParameterizedSmsMessageDto WithTemplate(SMSMessageTemplate template)
        {
            Template = template;
            return this;
        }

        public static ParameterizedSmsMessageDto Create()
        {
            return new ParameterizedSmsMessageDto();
        }
    }
}