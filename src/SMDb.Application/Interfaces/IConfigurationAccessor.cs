namespace SMDb.Application.Interfaces;

public interface IConfigurationAccessor
{
    string GetVariable(string name);
}
