

namespace EventFilter.Container
{
    public class BaseContainer
    {
        protected BaseContainer Container { get; }

        public object GetInstance()
        {
            return this;
        }

        public void CreateInstanceWithParams()
        {

        }
    }
}
