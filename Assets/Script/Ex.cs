public static class Ex 
{
	public static bool IsValidIndex<T>(this T[] array, int index)
	{
		return 0 <= index && index < array.Length;
	}

	public static bool IsEmpty<T>(this T[] array)
	{
		return array.Length == 0;
	}

	public static T GetElementOrNullByIndex<T>(this T[] array, int index)
	{
		return array.IsValidIndex(index) ? array[index] : default(T);
	}
}