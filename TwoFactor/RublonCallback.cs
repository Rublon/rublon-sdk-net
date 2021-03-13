using Rublon.Sdk.Core.Exception;

namespace Rublon.Sdk.TwoFactor
{
    public abstract class RublonCallback : Core.RublonHttpCallback
    {
        protected new IRublon rublon;
        protected Credentials credentials;

        public RublonCallback(IRublon rublon)
            : base(rublon)
        {
            this.rublon = rublon;
        }

        /// <summary>
        /// Finalize transaction for state "OK".
        /// </summary>
        protected override void FinalizeTransaction()
        {
            base.FinalizeTransaction();
            try
            {
                var accessToken = GetAccessToken();
                credentials = rublon.GetCredentials(accessToken);
                var userId = credentials.GetUserId();
                UserAuthenticated(userId);
            }
            catch (ConnectionException ex)
            {
                throw new CallbackException("Connection problem in the Rublon callback method when trying to get auth credentials.", ex);
            }
            catch (APIException ex)
            {
                throw new CallbackException("Rublon API error in the Rublon callback method when trying to get auth credentials.", ex);
            }
        }
    }
}
