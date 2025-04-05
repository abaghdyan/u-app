namespace VistaLOS.Application.Common.Interfaces;

public interface IIdentifiable<out T>
{
    public T Id { get; }
}