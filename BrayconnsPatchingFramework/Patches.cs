using System;
using System.Xml.Serialization;

namespace BrayconnsPatchingFramework
{
    #region Interfaces
    public interface IPatchFootprint
    {
        ulong Address { get; }
        ulong Length { get; }
    }
    public interface IPatch : IPatchFootprint
    {
        byte[] Data { get; }
    }
    #endregion

    #region Implementations

    /// <summary>
    /// The remenants of a patch
    /// </summary>
    public class PatchFootprint : IPatchFootprint
    {
        [XmlAttribute(AttributeName = "address")]
        public ulong Address { get; set; } = 0;
        [XmlAttribute(AttributeName = "length")]
        public ulong Length { get; set; } = 0;

        public PatchFootprint(IPatchFootprint pf) : this(pf.Address, pf.Length)
        { }
        public PatchFootprint(ulong address, ulong length)
        {
            Address = address;
            Length = length;
        }
        private PatchFootprint()
        { }
    }

    /// <summary>
    /// Patch for an executable.
    /// </summary>
    public class Patch : IPatch, ICloneable
    {
        public ulong Address { get; }

        public ulong Length => (ulong)Data.Length;

        public byte[] Data { get; }

        public Patch(ulong address, params byte[] data)
        {
            Address = address;
            Data = data;
        }

        public object Clone()
        {
            return new Patch(Address, (byte[])Data.Clone());
        }
    }
    #endregion
}
