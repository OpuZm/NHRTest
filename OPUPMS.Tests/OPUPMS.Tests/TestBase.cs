using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace OPUPMS.Tests
{
    public abstract class TestBase<TInterface>
    {
        public IKernel Kernel
        {
            get;
            private set;
        }

        protected TestBase()
        {
            Kernel = TestSetup.Setup();
        }

        public TInterface Get()
        {
            return Kernel.Get<TInterface>();
        }
    }
}
