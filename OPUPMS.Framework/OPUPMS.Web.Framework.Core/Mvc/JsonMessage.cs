using System;

namespace OPUPMS.Web.Framework.Core.Mvc
{
    [Serializable]
    public class JsonMessage<TStatus, TMessage>
    {
        public TStatus Status
        {
            get;
            set;
        }

        public TMessage Message
        {
            get;
            set;
        }
    }
}
