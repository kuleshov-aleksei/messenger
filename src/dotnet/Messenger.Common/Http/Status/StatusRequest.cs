using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.Common.Http.Status
{
    class StatusRequest : RequestBase
    {
        public override bool Validate()
        {
            return true;
        }
    }
}
