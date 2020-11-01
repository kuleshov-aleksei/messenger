namespace Messenger.Orchestrator
{
    internal class Service
    {
        internal string Address { get; set; }
        internal int Port { get; set; }
        internal string Name { get; set; }
        internal string Description { get; set; }

        public override string ToString()
        {
            return $"{Name, -20} ({Description, -25}) on {Address}:{Port}";
        }
    }
}
