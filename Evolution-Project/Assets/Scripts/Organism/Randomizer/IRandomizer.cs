using System;

public interface IRandomizer<T>
{
	T Randomize(T original);
	T FullRandomize();
}

