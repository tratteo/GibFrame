namespace GibFrame
{
    /// <summary>
    ///   Override the custom null check for the <see cref="Optional{T}"/> method
    /// </summary>
    public interface IOptional
    {
        bool HasValue();
    }
}