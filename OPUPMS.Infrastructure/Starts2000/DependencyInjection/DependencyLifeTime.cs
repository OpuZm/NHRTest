namespace Starts2000.DependencyInjection
{
    /// <summary>
    /// Lifestyles of types used in dependency injection system.
    /// </summary>
    public enum DependencyLifeTime
    {
        /// <summary>
        /// Specifies that a single instance of the service will be created.
        /// </summary>        
        Singleton = 0,
        /// <summary>
        /// Specifies that a new instance of the service will be created for each scope.
        /// </summary>
        /// <remarks>
        /// In ASP.NET Core applications a scope is created around each server request.
        /// </remarks>
        Scoped = 1,
        /// <summary>
        /// Specifies that a new instance of the service will be created every time it is requested.
        /// </summary>
        Transient = 2
    }
}
