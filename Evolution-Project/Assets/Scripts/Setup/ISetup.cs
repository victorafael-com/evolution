using System;

public interface ISetup<T>
{
	void Apply(T item);
}
