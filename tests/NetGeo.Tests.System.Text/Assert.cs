using System.Text.Json;

namespace NetGeo.SharedTests;

public class Assert : Xunit.Assert
{
    public static void Json(string expected, string actual, string message = null)
    {
        var expectedDoc = JsonDocument.Parse(expected).RootElement;
        var actualDoc = JsonDocument.Parse(actual).RootElement;
        // deep equals
        var assert = expectedDoc.Equals(actualDoc);

        if (message is null)
            True(assert);
        else
            True(assert, message);
    }

    public static void Void(Action act) =>
        Fail(act);

    public static async Task TaskAsync(Func<Task> act, string msg = null) =>
        await Fail(act, msg);

    public static Task TaskAsync<T>(Func<T, Task> act, T obj, string msg = null) =>
        TaskAsync(() => act.Invoke(obj), msg);

    public static Task TaskAsync<T, TE>(Func<T, TE, Task> act, T obj, TE obj2, string msg = null) =>
        TaskAsync(() => act.Invoke(obj, obj2), msg);

    public static void Fail(Action act, string msg = null)
    {
        try
        {
            act.Invoke();
            True(true);
        }
        catch (Exception ex)
        {
            Fail(msg is null ? ex.Message : $"{msg}: {ex.Message}");
        }
    }

    public static async Task Fail(Func<Task> act, string msg = null)
    {
        try
        {
            await act.Invoke();
            True(true);
        }
        catch (Exception ex)
        {
            Fail(msg is null ? ex.Message : $"{msg}: {ex.Message}");
        }
    }
}
