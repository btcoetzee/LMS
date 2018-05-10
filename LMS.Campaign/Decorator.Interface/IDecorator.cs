using System;
namespace Decorator.Interface
{
    using System.IO;
    public interface IDecorator
    {
        // Decorate the Lead
        void DecorateLead(Stream lead);
    }
}
