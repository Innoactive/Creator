using System.Collections;
using Innoactive.Creator.Core;

/// <summary>
/// A stage process that does nothing.
/// </summary>
public sealed class EmptyProcess : IProcess
{
    /// <inheritdoc />
    public void Start()
    {
    }

    /// <inheritdoc />
    public IEnumerator Update()
    {
        yield break;
    }

    /// <inheritdoc />
    public void End()
    {
    }

    /// <inheritdoc />
    public void FastForward()
    {
    }
}
