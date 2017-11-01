namespace Assets.Scripts.Craiel.VFX
{
    public struct VFXTicket
    {
        public static readonly VFXTicket Invalid = new VFXTicket(0);

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public VFXTicket(uint id)
        {
            this.Id = id;
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public readonly uint Id;

        public static bool operator ==(VFXTicket value1, VFXTicket value2)
        {
            return value1.Equals(value2);
        }

        public static bool operator !=(VFXTicket value1, VFXTicket value2)
        {
            return !(value1 == value2);
        }

        public override bool Equals(object obj)
        {
            var typed = (VFXTicket) obj;
            return typed.Id == this.Id;
        }

        public bool Equals(VFXTicket other)
        {
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return (int) this.Id;
        }
    }
}
