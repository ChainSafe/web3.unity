using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.InProcessSigner
{
    /// <summary>
    /// <see cref="InProcessSigner"/> extension methods.
    /// </summary>
    public static class InProcessSignerExtensions
    {
        /// <summary>
        /// Bind <see cref="InProcessSigner"/> as <see cref="ISigner"/> and a <see cref="configuration"/> to <see cref="collection"/>.
        /// </summary>
        /// <param name="collection">Service Collection to bind services to.</param>
        /// <param name="configuration">Config used by <see cref="InProcessSigner"/>.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseInProcessSigner(this IWeb3ServiceCollection collection, InProcessSignerConfig configuration)
        {
            collection.UseInProcessSigner();
            collection.ConfigureInProcessSigner(configuration);
            return collection;
        }

        /// <summary>
        /// Bind <see cref="InProcessSigner"/> as <see cref="ISigner"/> to <see cref="collection"/>.
        /// </summary>
        /// <param name="collection">Service Collection to bind services to.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseInProcessSigner(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ISigner>();
            collection.AddSingleton<ISigner, InProcessSigner>();
            return collection;
        }

        /// <summary>
        /// Binds a <see cref="configuration"/> to <see cref="collection"/>.
        /// </summary>
        /// <param name="collection">Service Collection to bind services to.</param>
        /// <param name="configuration">Config used by <see cref="InProcessSigner"/>.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection ConfigureInProcessSigner(this IWeb3ServiceCollection collection, InProcessSignerConfig configuration)
        {
            collection.Replace(ServiceDescriptor.Singleton(typeof(InProcessSignerConfig), configuration));
            return collection;
        }
    }
}
