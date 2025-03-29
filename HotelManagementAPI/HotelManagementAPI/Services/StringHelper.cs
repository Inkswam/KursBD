namespace HotelManagementAPI.Services;

public class StringHelper
{
    public string ToUpper(string stringValue, int index)
    {
        Span<char> stringContent = stringValue.ToCharArray();
        stringContent[index] = char.ToUpper(stringContent[index]);
        return stringContent.ToString();
    }
}